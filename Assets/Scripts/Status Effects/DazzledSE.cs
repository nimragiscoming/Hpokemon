using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DazzledSE : StatusEffect
{
    int TurnCounter;
    public override void OnAdd(MonsterGirl girl)
    {
        TurnCounter = 0;
    }

    public override StatusAction PerTurn()
    {
        TurnCounter++;

        if(TurnCounter%4 == 0)
        {
            return StatusAction.Skip;
        }

        return StatusAction.None;
    }

    //TODO: Reduce attacking stats by 25%
}
