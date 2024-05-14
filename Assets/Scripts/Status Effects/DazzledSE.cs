using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DazzledSE : StatusEffect
{
    int TurnCounter;

    public override StatMultiplier[] multipliers => new StatMultiplier[]
    {
        new StatMultiplier(Stat.Attack,-0.25f),
        new StatMultiplier(Stat.MagicAttack,-0.25f),
    };

    public override string DisplayName => "Dazzled";
    public override void OnAdd(MonsterGirl girl)
    {
        TurnCounter = 0;
    }

    public override IEnumerator PerTurn(BattleManager bm)
    {
        TurnCounter++;

        if(TurnCounter%4 == 0)
        {
            action = StatusAction.Skip;
        }
        else
        {
            action = StatusAction.None;
        }

        yield break;
    }

    //TODO: Reduce attacking stats by 25%
}
