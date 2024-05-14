using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Unity.VisualScripting.Member;

public static class CombatHelper
{
    public static int CalcBaseDamage(MonsterGirl Source, MonsterGirl Target, CombatMove Move)
    {
        float AtkDfs;
        if (Move.MoveType == MoveType.Magical)
        {

            AtkDfs = (Source.Monster.MagicAttack * GetStatEffectMultiplier(Source,Stat.MagicAttack) * GetStatStageMultiplier(Source.MAtkStage)) / (Target.Monster.MagicDefense * GetStatEffectMultiplier(Target, Stat.MagicDefense) * GetStatStageMultiplier(Target.MDfsStage));
        }
        else
        {
            AtkDfs = (Source.Monster.Attack * GetStatEffectMultiplier(Source, Stat.Attack) * GetStatStageMultiplier(Source.AtkStage)) / (Target.Monster.Defense * GetStatEffectMultiplier(Target, Stat.Defense) * GetStatStageMultiplier(Target.DfsStage));
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

        if (UnityEngine.Random.Range(0, 100) < Source.Monster.Precision)
        {

            //hacky way of disabling crits altogether for certain status effects
            bool flag = false;
            foreach (StatusEffect effect in Source.StatusEffects)
            {
                if(effect.action == StatusAction.NoCrits)
                {
                    flag= true;
                    break;
                }
            }

            if(!flag)
            {
                Crit = 2;
                isCrit = true;
            }
            else
            {
                isCrit = false;
            }
        }
        else
        {
            isCrit = false;
        }

        float RandomMod = UnityEngine.Random.Range(0.9f, 1);

        return (int)(BaseDamage * Crit * RandomMod);
    }

    //returns true if monster died, false if not
    public static bool DoDamage(MonsterGirl Target, int Damage)
    {
        Target.Health -= Damage;

        if (Target.Health < 0)
        {
            return true;
        }
        else
        {
            return false;
        }

    }


    public static float GetStatEffectMultiplier(MonsterGirl Target, Stat stat)
    {
        float total = 1;

        foreach (StatusEffect effect in Target.StatusEffects)
        {
            foreach (StatMultiplier multiplier in effect.multipliers)
            {
                if (multiplier.stat == stat)
                {
                    total += multiplier.multiplier;
                }
            }
        }

        return total;
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

    public static string ParseArg(MonsterGirl source, MonsterGirl target, string str)
    {
        string[] parts = str.Split(".");

        int bodyindex = 1;

        MonsterGirl girl = target;

        if (parts[1] == "self")
        {
            bodyindex = 2;
            girl = source;
        }

        string str1 = string.Join(".", parts.Skip(bodyindex));

        switch(parts[0])
        {
            case "Stage":
                return ParseStageChange(girl, str1);
            case "Effect":
                return ParseStatusEffect(girl, str1);
            case "Set":
                return ParseSetValue(girl, str1);
            case "Add":
                return ParseAddValue(girl, str1);
        }

        return null;
    }

    public static string ParseStageChange(MonsterGirl target, string str)
    {
        string[] parts = str.Split("~");

        int value = int.Parse(parts[1]);

        switch (parts[0])
        {
            case "Attack":
                target.AtkStage = Math.Clamp(target.AtkStage + value, -3, 4);
                break;
            case "Defense":
                target.DfsStage = Math.Clamp(target.DfsStage + value, -3, 4);
                break;
            case "MagicAttack":
                target.MAtkStage = Math.Clamp(target.MAtkStage + value, -3, 4);
                break;
            case "MagicDefense":
                target.MDfsStage = Math.Clamp(target.MDfsStage + value, -3, 4);
                break;
        }

        return target.Title + "'s " + parts[0] + " was changed by " + parts[1] + "!";
    }

    public static string ParseStatusEffect(MonsterGirl target, string str)
    {
        float chance;
        StatusEffect effect = StatusEffect.FromString(str, out chance);


        //if the chance is not rolled, continue
        if (UnityEngine.Random.Range(0f, 0.9999f) > chance)
        {
            return null;
        }

        bool flag = false;

        foreach (StatusEffect e in target.StatusEffects)
        {
            if (e.DisplayName == effect.DisplayName)
            {
                flag = true;
                break;
            }
        }

        if (flag == true) { return null; }

        target.StatusEffects.Add(effect);
        effect.OnAdd(target);

        return target.Title + " was given the " + effect.DisplayName + " effect!";

    }

    public static string ParseSetValue(MonsterGirl target, string str)
    {
        string[] parts = str.Split("~");

        int value = (int)GetFloat(target, parts[1]);

        switch (parts[0])
        {
            case "Health":
                target.Health = Math.Min(value, target.BaseHealth);
                break;
            case "Energy":
                target.Energy = Math.Min(value, target.BaseEnergy);
                break;
        }

        string adjective = value > 0 ? "replenished" : "drained";

        return target.Title + "'s " + parts[0] + " was " + adjective + " to " + value + "!";
    }

    public static string ParseAddValue(MonsterGirl target, string str)
    {
        Debug.Log(str);

        string[] parts = str.Split("~");

        int value = (int)GetFloat(target, parts[1]);

        switch (parts[0])
        {
            case "Health":
                target.Health = Math.Min(target.Health+value,target.BaseHealth);
                break;
            case "Energy":
                target.Energy = Math.Min(target.Energy + value, target.BaseEnergy);
                break;
        }

        string adjective = value > 0 ? "replenished" : "drained";

        return target.Title + "'s " + parts[0] + " was " + adjective + " by " + value + "!";
    }

    public static float GetFloat(MonsterGirl target, string str)
    {
        string[] parts = str.Split("*");

        float total = 1;

        foreach (string s in parts)
        {
            if(float.TryParse(s, out float res))
            {
                total += res;
            }
            else
            {
                total *= GetVal(target, s);
            }
        }
        return total;
    }

    public static int GetVal(MonsterGirl target, string str)
    {
        switch (str)
        {
            case "BaseHealth":
                return target.BaseHealth;
            case "BaseEnergy":
                return target.BaseEnergy;
            case "Health":
                return target.Health;
            case "Energy":
                return target.Energy;
            default:
                Debug.LogError("No value found for "+ str);
                return 1;
        }
    }
}
