using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    bool IsPlayerTurn = true;

    int TurnNumber;

    public PlayerCollection PlayerCollection;

    public void Startbattle()
    {
        TurnNumber = 0;

        foreach (MonsterGirl girl in PlayerCollection.Girls)
        {
            girl.ResetStats();
        }
    }

    void PlayerTurn()
    {
        IsPlayerTurn= true;

        //wait for input - use coroutine

        //do attack

        IsPlayerTurn= false;
    }

    void EnemyTurn()
    {
        // for now just randomly choose a move

        // change monstergirl when she gets to 20% health

        //in future calculate damage of moves, pick randomly from top few highest damaging
    }

    int GetDamage(MonsterGirl Source, MonsterGirl Target, CombatMove Move)
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

        int Crit = Random.Range(0, 100) < Source.Monster.Precision ? 2 : 1;

        float RandomMod = Random.Range(0.9f, 1);

        float STAB = Source.Monster.Type == Move.Type ? 1.5f : 1;

        float TE = MonsterTypes.GetTypeBonus(Source.Monster.Type, Move.Type);

        if(TE < 0)
        {
            TE = 0.5f;
        }
        else
        {
            TE += 1;
        }

        return (int)(Move.Power * AtkDfs * Crit * RandomMod * STAB * TE);
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
