using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonSE : StatusEffect
{
    MonsterGirl monster;

    int n = 0;

    public override string DisplayName => "Poison";

    public override void OnAdd(MonsterGirl girl)
    {
        monster = girl;
    }

    public override IEnumerator PerTurn(BattleManager bm)
    {
        int dmg = 1 + ((n* monster.BaseHealth) /16);
        CombatHelper.DoDamage(monster, dmg);
        n++;

        bm.UpdateStats();

        bm.SetDialogue(monster.Monster.MonsterName + " took " + dmg + " poison damage!");

        yield return new WaitForSeconds(1);
    }
}
