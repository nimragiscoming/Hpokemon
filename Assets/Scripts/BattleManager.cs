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

    MonsterGirl CurPlayerMG;

    MonsterGirl CurEnemyMG;

    public CombatMoveButton CombatMoveButton;

    public GameObject CombatMoveButtonGroup;

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
        }

        enemy.ResetStats();


        SetDialogue("A " + enemy.Monster.MonsterName + " appears!");

        CurEnemyMG = enemy;

        EnemyStats.Girl = CurEnemyMG;

        CurPlayerMG = Player.Girls[0];

        PlayerStats.Girl = CurPlayerMG;

        CreateModels();

        UpdateStats();

        StartCoroutine(BattleCycle());
        //TODO: Initialise combat, make buttons etc
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

            StartPlayerTurn();

            while(State == BattleState.PlayerTurn)
            {
                yield return null;
            }

            EndPlayerTurn();

            if(State == BattleState.EnemyTurn)
            {
                yield return EnemyTurn();
            }
            
            if(State == BattleState.Win || State == BattleState.Lose)
            {
                yield break;
            }


            yield return UpdateStatusEffects();
        }
    }

    public void StartPlayerTurn()
    {
        TurnNumber++;

        foreach (CombatMove move in CurPlayerMG.Monster.Moveset)
        {
            CombatMoveButton Mv = Instantiate(CombatMoveButton, CombatMoveButtonGroup.transform);
            Mv.BattleManager= this;
            Mv.Move= move;
        }

        State = BattleState.PlayerTurn;
    }

    public void EndPlayerTurn()
    {
        //redundant for now, but potentially useful in future
    }


    //called from clicking action button directly
    public IEnumerator DoPlayerTurn(CombatMove Move)
    {
        if(State != BattleState.PlayerTurn) { yield break; }

        foreach (Transform child in CombatMoveButtonGroup.transform)
        {
            Destroy(child.gameObject);
        }

        SwitchCam(CurPlayerMG.Model.frontCam);



        //do damage with chosen move
        bool isCrit;
        int Damage = CombatHelper.GetDamage(CurPlayerMG, CurEnemyMG, Move, out isCrit);

        SetDialogue("Using " + Move.MoveName + "...");

        //wait a few second before showing damage text, and going to the enemy turn
        yield return new WaitForSeconds(1f);

        bool win = CombatHelper.DoDamage(CurEnemyMG, Damage);

        UpdateStats();

        string text1 = string.Empty;
        if(isCrit)
        {
            text1 += "Critical Hit! ";
        }
        text1 += "You hit for " + Damage + " damage!";
        SetDialogue(text1);

        yield return new WaitForSeconds(1f);

        if(Move.StatusEffects.Count > 0)
        {
            yield return AddEffects(Move, CurEnemyMG, CurEnemyMG.Monster.MonsterName);
        }

        SwitchCam(WideCam);



        if (win)
        {
            PlayerWin();
            yield break;
        }

        State = BattleState.EnemyTurn;
    }

    IEnumerator EnemyTurn()
    {
        SetDialogue("The " + CurEnemyMG.Monster.MonsterName + " is preparing to attack... ");
        yield return new WaitForSeconds(1f);

        SwitchCam(CurEnemyMG.Model.frontCam);



        //TODO: change monstergirl when she gets to 20% health




        //Selects random move out of X best

        int selectLength = Mathf.Min(2, CurEnemyMG.Monster.Moveset.Count);

        //Sort list by base damage
        List<CombatMove> moves = CurEnemyMG.Monster.Moveset.OrderByDescending(o => CombatHelper.CalcBaseDamage(CurEnemyMG, CurPlayerMG, o)).ToList();

        //randomly select one based on select length
        int rand = Random.Range(0, selectLength);

        CombatMove finalMove = moves[rand];



        //actually do the damage

        bool isCrit;
        int Damage = CombatHelper.GetDamage(CurEnemyMG,CurPlayerMG, finalMove, out isCrit);

        SetDialogue("The " + CurEnemyMG.Monster.MonsterName + " is using " + finalMove.MoveName + "...");

        yield return new WaitForSeconds(1f);

        bool win = CombatHelper.DoDamage(CurPlayerMG, Damage);

        UpdateStats();

        string text1 = string.Empty;
        if (isCrit)
        {
            text1 += "Critical Hit! ";
        }
        text1 += "The "+ CurEnemyMG.Monster.MonsterName + " hit for " + Damage + " damage!";

        SetDialogue(text1);

        yield return new WaitForSeconds(1f);

        if (finalMove.StatusEffects.Count > 0)
        {
            yield return AddEffects(finalMove, CurPlayerMG, "Your "+ CurPlayerMG.Monster.MonsterName);
            
        }


        SwitchCam(WideCam);



        if (win)
        {
            PlayerLose();
            yield break;
        }

    }

    IEnumerator UpdateStatusEffects()
    {
        foreach (StatusEffect item in CurEnemyMG.StatusEffects)
        {
            yield return item.PerTurn(this);
        }

        foreach (StatusEffect item in CurPlayerMG.StatusEffects)
        {
            yield return item.PerTurn(this);
        }
    }

    IEnumerator AddEffects(CombatMove Move, MonsterGirl Target, string DialogueName)
    {
        foreach (string str in Move.StatusEffects)
        {
            StatusEffect effect = StatusEffect.FromString(str);

            bool flag = false;

            foreach (StatusEffect e in Target.StatusEffects)
            {
                if (e.DisplayName == effect.DisplayName)
                {
                    flag = true;
                    break;
                }
            }

            if (flag == true) { continue; }

            Target.StatusEffects.Add(effect);
            effect.OnAdd(Target);

            SetDialogue(DialogueName + " was given the " + effect.DisplayName + " effect!");

            yield return new WaitForSeconds(1f);

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
        SetDialogue("You have defeated the " + CurEnemyMG.Monster.MonsterName + ".");
    }

    void PlayerLose()
    {
        State = BattleState.Lose;
        SetDialogue("You have been defeated by the " + CurEnemyMG.Monster.MonsterName + ".");
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
