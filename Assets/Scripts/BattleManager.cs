using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TMPro;
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

        foreach (MonsterGirl girl in Player.Girls)
        {
            girl.ResetStats();
        }

        enemy.ResetStats();


        DialogueBox.text = "A " + enemy.Monster.MonsterName + " appears!";

        CurEnemyMG = enemy;

        EnemyStats.Girl = CurEnemyMG;

        CurPlayerMG = Player.Girls[0];

        PlayerStats.Girl = CurPlayerMG;

        UpdateStats();

        StartPlayerTurn();
        //TODO: Initialise combat, make buttons etc
    }

    void UpdateStats()
    {
        PlayerStats.UpdateText();
        EnemyStats.UpdateText();
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

        State = BattleState.EnemyTurn;

        StartCoroutine(EnemyTurn());
    }

    public IEnumerator DoPlayerTurn(CombatMove Move)
    {
        if(State != BattleState.PlayerTurn) { yield break; }

        foreach (Transform child in CombatMoveButtonGroup.transform)
        {
            Destroy(child.gameObject);
        }



        //do damage with chosen move
        bool isCrit;
        int Damage = GetDamage(CurPlayerMG, CurEnemyMG, Move, out isCrit);

        DialogueBox.text = "Using " + Move.MoveName + "...";

        //wait a few second before showing damage text, and going to the enemy turn
        yield return new WaitForSeconds(1f);

        bool win = DoDamage(CurEnemyMG, Damage);

        UpdateStats();

        string text1 = string.Empty;
        if(isCrit)
        {
            text1 += "Critical Hit! ";
        }
        text1 += "You hit for " + Damage + " damage!";
        DialogueBox.text = text1;

        yield return new WaitForSeconds(1f);

        if (win)
        {
            PlayerWin();
            yield break;
        }

        EndPlayerTurn();
    }

    void PlayerWin()
    {
        State = BattleState.Win;
        DialogueBox.text = "You have defeated the " + CurEnemyMG.Monster.MonsterName + ".";
    }

    void PlayerLose()
    {
        State = BattleState.Lose;
        DialogueBox.text = "You have been defeated by the " + CurEnemyMG.Monster.MonsterName + ".";
    }

    IEnumerator EnemyTurn()
    {
        //TODO: change monstergirl when she gets to 20% health

        //Selects random move out of X best

        int selectLength = Mathf.Min(2, CurEnemyMG.Monster.Moveset.Count);

        //Sort list by base damage
        List<CombatMove> moves = CurEnemyMG.Monster.Moveset.OrderByDescending(o => CalcBaseDamage(CurEnemyMG, CurPlayerMG, o)).ToList();

        //randomly select one based on select length
        int rand = Random.Range(0, selectLength);

        CombatMove finalMove = moves[rand];


        //actually do the damage

        bool isCrit;
        int Damage = GetDamage(CurEnemyMG,CurPlayerMG, finalMove, out isCrit);


        DialogueBox.text = "The " + CurEnemyMG.Monster.MonsterName + " is using " + finalMove.MoveName + "...";

        yield return new WaitForSeconds(1f);

        bool win = DoDamage(CurPlayerMG, Damage);

        UpdateStats();

        string text1 = string.Empty;
        if (isCrit)
        {
            text1 += "Critical Hit! ";
        }
        text1 += "The "+ CurEnemyMG.Monster.MonsterName + " hit for " + Damage + " damage!";
        DialogueBox.text = text1;

        yield return new WaitForSeconds(1f);

        if(win)
        {
            PlayerLose();
            yield break;
        }

        //end turn
        StartPlayerTurn();
    }

    int CalcBaseDamage(MonsterGirl Source, MonsterGirl Target, CombatMove Move)
    {
        int AtkDfs;
        if (Move.IsMagic)
        {
            AtkDfs = Source.Monster.MagicAttack / Target.Monster.MagicDefense;
        }
        else
        {
            AtkDfs = Source.Monster.Attack / Target.Monster.Defense;
        }

        float STAB = Source.Monster.Type == Move.Type ? 1.5f : 1;

        float TE = MonsterTypes.GetTypeBonus(Move.Type,Target.Monster.Type);

        if (TE < 0)
        {
            TE = 0.5f;
        }
        else
        {
            TE += 1;
        }

        return (int)(Move.Power * AtkDfs * STAB * TE);
    }

    int GetDamage(MonsterGirl Source, MonsterGirl Target, CombatMove Move, out bool isCrit)
    {
        int BaseDamage = CalcBaseDamage(Source, Target, Move);


        int Crit =  1;

        if(Random.Range(0, 100) < Source.Monster.Precision)
        {
            Crit = 2;
            isCrit= true;
        }
        else
        {
            isCrit= false;
        }

        float RandomMod = Random.Range(0.9f, 1);

        return (int)(BaseDamage * Crit * RandomMod);
    }

    //returns true if monster died, false if not
    public bool DoDamage(MonsterGirl Target, int Damage)
    {
        Target.Health -= Damage;

        if(Target.Health < 0)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    float GetStatStageMultiplier(int stage)
    {
        switch(stage)
        {
            case -3:
                return 0.25f;
            case -2:
                return 0.50f;
            case -1:
                return 0.75f;
            case 0:
                return 1f;
            case 1:
                return 1.25f;
            case 2:
                return 1.50f;
            case 3:
                return 1.75f;
            case 4:
                return 2f;
            default:
                Debug.Log("Unexpected Stage Multiplier found, value was: "+ stage);
                return 1f;

        }
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
