using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect
{
    public bool remove = true;

    public virtual StatMultiplier[] multipliers { get; } = new StatMultiplier[0];

    public virtual StatusAction action { get; protected set; } = StatusAction.None;

    public abstract void OnAdd(MonsterGirl girl);

    public abstract IEnumerator PerTurn(BattleManager bm);

    public abstract string DisplayName { get; }


    //im not going to do anything fancy here
    public static StatusEffect FromString(string str)
    {
        switch (str)
        {
            case "Sleep":
                return new SleepSE();
            case "Dazzled":
                return new DazzledSE();
            case "Bleed":
                return new BleedSE();
            case "Cursed":
                return new CursedSE();
            case "Poison":
                return new PoisonSE();
            case "Trapped":
                return new TrappedSE();
            default:
                Debug.LogError("Tried to find unknown status effect: "+ str);
                return null;
        }
    }
}

public enum StatusAction
{
    None,
    Skip,
    Trap,
    NoCrits
}

public struct StatMultiplier
{
    public Stat stat;
    public float multiplier;

    public StatMultiplier(Stat stat, float multiplier)
    {
        this.stat = stat;
        this.multiplier = multiplier;
    }
}
