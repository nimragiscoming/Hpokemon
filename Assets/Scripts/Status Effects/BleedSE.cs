using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BleedSE : StatusEffect
{
    public override void OnAdd(MonsterGirl girl)
    {
    }

    public override StatusAction PerTurn()
    {
        return StatusAction.None;
    }
}
