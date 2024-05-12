using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CombatHelper
{
    public static int CalcBaseDamage(MonsterGirl Source, MonsterGirl Target, CombatMove Move)
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

        float TE = MonsterTypes.GetTypeBonus(Move.Type, Target.Monster.Type);

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

    public static int GetDamage(MonsterGirl Source, MonsterGirl Target, CombatMove Move, out bool isCrit)
    {
        int BaseDamage = CalcBaseDamage(Source, Target, Move);


        int Crit = 1;

        if (Random.Range(0, 100) < Source.Monster.Precision)
        {
            Crit = 2;
            isCrit = true;
        }
        else
        {
            isCrit = false;
        }

        float RandomMod = Random.Range(0.9f, 1);

        return (int)(BaseDamage * Crit * RandomMod);
    }

    public static float GetStatStageMultiplier(int stage)
    {
        switch (stage)
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
                Debug.Log("Unexpected Stage Multiplier found, value was: " + stage);
                return 1f;

        }
    }
}
