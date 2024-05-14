using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursedSE : StatusEffect
{
    public override StatMultiplier[] multipliers => new StatMultiplier[]
    {
        new StatMultiplier(Stat.Speed,-0.25f),
    };

    public override StatusAction action => StatusAction.NoCrits;

    public override string DisplayName => "Cursed";
    public override void OnAdd(MonsterGirl girl)
    {
    }

    public override IEnumerator PerTurn(BattleManager bm)
    {
        yield break;
    }
}
