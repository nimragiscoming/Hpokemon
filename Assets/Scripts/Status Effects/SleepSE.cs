using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepSE : StatusEffect
{
    int TurnsLeft;

    public override StatusAction action => StatusAction.Skip;

    public override string DisplayName => "Sleep";

    public override void OnAdd(MonsterGirl girl)
    {
        TurnsLeft = 2;
    }

    public override IEnumerator PerTurn(BattleManager bm)
    {
        TurnsLeft--;


        if(TurnsLeft<= 0)
        {
            remove = true;
        }

        yield break;
    }
}
