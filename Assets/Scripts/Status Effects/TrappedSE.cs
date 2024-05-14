using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrappedSE : StatusEffect
{
    public override StatusAction action => StatusAction.Trap;

    public override string DisplayName => "Trapped";
    public override void OnAdd(MonsterGirl girl)
    {
    }

    public override IEnumerator PerTurn(BattleManager bm)
    {
        yield break;
    }
}
