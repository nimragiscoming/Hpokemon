using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepSE : StatusEffect
{
    int TurnsLeft;
    public override void OnAdd(MonsterGirl girl)
    {
        TurnsLeft = 2;
    }

    public override StatusAction PerTurn()
    {
        TurnsLeft--;

        if(TurnsLeft<= 0)
        {
            remove = true;
        }

        return StatusAction.Skip;
    }
}
