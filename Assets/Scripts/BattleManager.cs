using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    BattleState State;

    public int TurnNumber;

    public Player Player;

    public TMP_Text DialogueBox;

    public MGBase type;

    public MonsterGirl CurPlayerMG;

    public MonsterGirl CurEnemyMG;


    public ActionChoice ActionChoice;

    public Camera WideCam;

    Camera curCam;

    public StatDisplayPanel PlayerStats;

    public StatDisplayPanel EnemyStats;

    private void Start()
    {
        MonsterGirl mg = new MonsterGirl();
        mg.Monster = type;

        Startbattle(mg);
    }

    public void Startbattle(MonsterGirl enemy)
    {
        State = BattleState.Start;
        TurnNumber = 0;
        curCam = WideCam;

        foreach (MonsterGirl girl in Player.Girls)
        {
            girl.ResetStats();

            girl.Title = "Your " + girl.Monster.MonsterName;
        }

        enemy.ResetStats();

        enemy.Title = "Wild " + enemy.Monster.MonsterName;

        SetDialogue("A " + enemy.Title + " appears!");

        CurEnemyMG = enemy;

        EnemyStats.Girl = CurEnemyMG;


        SwitchPlayerGirl(Player.Girls[0]);

        StartCoroutine(BattleCycle());
        //TODO: Initialise combat, make buttons etc
    }

    public void SwitchPlayerGirl(MonsterGirl Girl)
    {
        CurPlayerMG = Girl;

        PlayerStats.Girl = CurPlayerMG;

        CreateModels();

        UpdateStats();
    }

    public void UpdateStats()
    {
        PlayerStats.UpdateText();
        EnemyStats.UpdateText();
    }

    void SwitchCam(Camera cam)
    {
        cam.enabled = true;
        curCam.enabled = false;
        curCam = cam;
    }

    public IEnumerator BattleCycle()
    {
        while(true)
        {
            //both skipplayerturn and skipenemyturn are bools set below in the updatestatuseffects function
            if (!SkipPlayerTurn)
            {
                StartPlayerTurn();

                while(State == BattleState.PlayerTurn)
                {
                    yield return null;
                }

                EndPlayerTurn();
            }
            else
            {
                State = BattleState.EnemyTurn;
            }

            if (!SkipEnemyTurn)
            {
                if(State == BattleState.EnemyTurn)
                {
                    yield return EnemyTurn();
                }
            }
            else
            {
                State = BattleState.PlayerTurn;
            }

            
            if(State == BattleState.Win || State == BattleState.Lose)
            {
                yield break;
            }


            yield return UpdateStatusEffects();

            RegenEnergyForAll();
        }
    }

    public void StartPlayerTurn()
    {
        TurnNumber++;

        ActionChoice.gameObject.SetActive(true);

        State = BattleState.PlayerTurn;
    }

    public void EndPlayerTurn()
    {
        //redundant for now, but potentially useful in future
    }


    //called from clicking action button directly
    public IEnumerator DoPlayerAttack(CombatMove Move)
    {
        if(State != BattleState.PlayerTurn) { yield break; }


        ActionChoice.ClearBox();


        SwitchCam(CurPlayerMG.Model.frontCam);



        SetDialogue("Using " + Move.MoveName + "...");



        //wait a few second before showing damage text, and going to the enemy turn
        yield return new WaitForSeconds(1f);

        CurPlayerMG.Energy -= Move.Cost;


        bool win = false;
        if(Move.MoveType != MoveType.Status)
        {

            //do damage with chosen move
            bool isCrit;
            int Damage = CombatHelper.GetDamage(CurPlayerMG, CurEnemyMG, Move, out isCrit);

            win = CombatHelper.DoDamage(CurEnemyMG, Damage);


            UpdateStats();

            string text1 = string.Empty;
            if(isCrit)
            {
                text1 += "Critical Hit! ";
            }
            text1 += CurPlayerMG.Title + " hit for " + Damage + " damage!";
            SetDialogue(text1);

            yield return new WaitForSeconds(1f);
        }






        yield return DoMoveArgs(CurPlayerMG,CurEnemyMG, Move);



        //switch back to spinning wide cam
        SwitchCam(WideCam);



        if (win)
        {
            PlayerWin();
            yield break;
        }

        State = BattleState.EnemyTurn;
    }

    public IEnumerator DoPlayerSwitch(MonsterGirl girl)
    {
        if (State != BattleState.PlayerTurn) { yield break; }

        SwitchCam(CurPlayerMG.Model.frontCam);

        ActionChoice.ClearBox();


        SetDialogue(CurPlayerMG.Title + " retreats.");

        yield return new WaitForSeconds(1f);

        SwitchPlayerGirl(girl);

        SetDialogue("Go forth, " + CurPlayerMG.Title + "!");

        yield return new WaitForSeconds(1f);


        //switch back to spinning wide cam
        SwitchCam(WideCam);

        State = BattleState.EnemyTurn;
    }

    public IEnumerator DoPlayerRest()
    {
        if (State != BattleState.PlayerTurn) { yield break; }

        SwitchCam(CurPlayerMG.Model.frontCam);

        ActionChoice.ClearBox();


        SetDialogue(CurPlayerMG.Title + " rests.");

        yield return new WaitForSeconds(1f);

        CurPlayerMG.Energy += 20;

        UpdateStats();

        SetDialogue(CurPlayerMG.Title + " recovers 20 energy!");

        yield return new WaitForSeconds(1f);


        //switch back to spinning wide cam
        SwitchCam(WideCam);

        State = BattleState.EnemyTurn;
    }

    IEnumerator EnemyTurn()
    {
        //TODO: change monstergirl when she gets to 20% health


        //Get all valid moves
        List<CombatMove> moves = GetValidEnemyMoves();


        //try and attack above all else
        if (moves.Count > 0)
        {
            yield return DoEnemyAttack(moves);
            yield break;
        }

        //then try and switch

        //then try and rest

        yield return DoEnemyRest();

    }

    List<CombatMove> GetValidEnemyMoves()
    {
        List<CombatMove> moves = CurEnemyMG.Monster.Moveset.OrderByDescending(o => CombatHelper.CalcBaseDamage(CurEnemyMG, CurPlayerMG, o)).ToList();

        List<CombatMove> moves1 = new List<CombatMove>();

        foreach (CombatMove m in moves)
        {
            if (m.Cost < CurEnemyMG.Energy)
            {
                if (m.MoveType == MoveType.Status)
                {
                    moves1.Insert(Random.Range(1, moves1.Count), m);
                }
                else
                {
                    moves1.Add(m);
                }

            }

        }

        return moves1;
    }

    public IEnumerator DoEnemyAttack(List<CombatMove> moves)
    {
        SetDialogue("The " + CurEnemyMG.Title + " is preparing to attack... ");
        yield return new WaitForSeconds(1f);

        SwitchCam(CurEnemyMG.Model.frontCam);

        //Selects random move out of X best

        int selectLength = Mathf.Min(2, CurEnemyMG.Monster.Moveset.Count);

        //randomly select one based on select length
        int rand = Random.Range(0, selectLength);

        CombatMove finalMove = moves[rand];


        SetDialogue("The " + CurEnemyMG.Title + " is using " + finalMove.MoveName + "...");


        yield return new WaitForSeconds(1f);

        CurEnemyMG.Energy -= finalMove.Cost;

        bool win = false;
        if (finalMove.MoveType != MoveType.Status)
        {
            //actually do the damage

            bool isCrit;
            int Damage = CombatHelper.GetDamage(CurEnemyMG, CurPlayerMG, finalMove, out isCrit);

            win = CombatHelper.DoDamage(CurPlayerMG, Damage);

            UpdateStats();

            string text1 = string.Empty;
            if (isCrit)
            {
                text1 += "Critical Hit! ";
            }
            text1 += "The " + CurEnemyMG.Title + " hit for " + Damage + " damage!";

            SetDialogue(text1);

            yield return new WaitForSeconds(1f);
        }





        yield return DoMoveArgs(CurEnemyMG, CurPlayerMG, finalMove);



        //switch back to spinning wide cam
        SwitchCam(WideCam);



        if (win)
        {
            PlayerLose();
            yield break;
        }
    }

    public IEnumerator DoEnemyRest()
    {

        SwitchCam(CurEnemyMG.Model.frontCam);

        ActionChoice.ClearBox();


        SetDialogue(CurEnemyMG.Title + " rests.");

        yield return new WaitForSeconds(1f);

        CurEnemyMG.Energy += 20;

        UpdateStats();

        SetDialogue(CurEnemyMG.Title + " recovers 20 energy!");

        yield return new WaitForSeconds(1f);


        //switch back to spinning wide cam
        SwitchCam(WideCam);
    }


    //not very elegant, but gets the job done

    bool SkipPlayerTurn;

    bool SkipEnemyTurn;

    IEnumerator UpdateStatusEffects()
    {
        SkipPlayerTurn = false;
        SkipEnemyTurn = false;
        foreach (StatusEffect item in CurEnemyMG.StatusEffects)
        {
            yield return item.PerTurn(this);
            if(item.action == StatusAction.Skip)
            {
                SkipEnemyTurn = true;
            }
        }

        foreach (StatusEffect item in CurPlayerMG.StatusEffects)
        {
            yield return item.PerTurn(this);
            if (item.action == StatusAction.Skip)
            {
                SkipPlayerTurn = true;
            }
        }
    }

    void RegenEnergyForAll()
    {
        foreach (MonsterGirl girl in Player.Girls)
        {
            if (girl == CurPlayerMG) { continue; }
            girl.Energy += 20;
        }
    }

    IEnumerator DoMoveArgs(MonsterGirl source, MonsterGirl target, CombatMove move)
    {
        foreach (string str in move.Args)
        {
            string dialogue = CombatHelper.ParseArg(source, target, str);

            UpdateStats();

            if(dialogue != null)
            {
                SetDialogue(dialogue);
                yield return new WaitForSeconds(1);
            }
        }
    }

    public void SetDialogue(string text)
    {
        StartCoroutine(TypewriterEffect());
        IEnumerator TypewriterEffect()
        {
            DialogueBox.text = string.Empty;
            float TypeTime = 0.4f;

            float interval = TypeTime/text.Length;
            foreach (char ch in text)
            {
                DialogueBox.text += ch;
                yield return new WaitForSeconds(interval);
            }
        }
    }

    void PlayerWin()
    {
        State = BattleState.Win;
        SetDialogue("You have defeated the " + CurEnemyMG.Title + ".");
    }

    void PlayerLose()
    {
        State = BattleState.Lose;
        SetDialogue("You have been defeated by the " + CurEnemyMG.Title + ".");
    }


    void CreateModels()
    {
        float dist = 20;

        Vector3 PlayerPos = transform.position + new Vector3(-dist/2, 0);

        Vector3 EnemyPos = transform.position + new Vector3(dist / 2, 0);

        CurPlayerMG.Model = Instantiate(CurPlayerMG.Monster.Model, PlayerPos, Quaternion.identity);

        CurEnemyMG.Model = Instantiate(CurEnemyMG.Monster.Model, EnemyPos, Quaternion.identity);
    }
}

enum BattleState
{
    Start,
    PlayerTurn,
    EnemyTurn,
    Win,
    Lose
}
