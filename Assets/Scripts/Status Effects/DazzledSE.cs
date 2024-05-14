using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DazzledSE : StatusEffect
{
    int TurnCounter;

    MonsterGirl monster;

    public override StatMultiplier[] multipliers => new StatMultiplier[]
    {
        new StatMultiplier(Stat.Attack,-0.25f),
        new StatMultiplier(Stat.MagicAttack,-0.25f),
    };

    public override string DisplayName => "Dazzled";
    public override void OnAdd(MonsterGirl girl)
    {
        monster = girl;
        TurnCounter = 0;
    }

    public override IEnumerator PerTurn(BattleManager bm)
    {
        TurnCounter++;

        if(TurnCounter%4 == 0)
        {
            action = StatusAction.Skip;

            bm.SetDialogue(monster.Monster.MonsterName + " was completely dazzled!");

            yield return new WaitForSeconds(1);
        }
        else
        {
            action = StatusAction.None;
        }

        yield break;
    }

    //TODO: Reduce attacking stats by 25%
}
