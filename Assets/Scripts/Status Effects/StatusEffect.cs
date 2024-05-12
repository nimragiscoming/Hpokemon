using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect
{
    public bool remove = true;
    public abstract void OnAdd(MonsterGirl girl);

    public abstract StatusAction PerTurn();
}

public enum StatusAction
{
    None,
    Skip,
}
