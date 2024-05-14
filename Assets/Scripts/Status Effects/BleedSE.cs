using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BleedSE : StatusEffect
{

    MonsterGirl monster;

    public override StatMultiplier[] multipliers => new StatMultiplier[]
    {
        new StatMultiplier(Stat.Defense,-0.25f),
        new StatMultiplier(Stat.MagicDefense,-0.25f),
    };

    public override string DisplayName => "Bleed";

    public override void OnAdd(MonsterGirl girl)
    {
        monster= girl;
    }

    public override IEnumerator PerTurn(BattleManager bm)
    {
        int dmg = monster.BaseHealth / 16;
        CombatHelper.DoDamage(monster, dmg);

        bm.UpdateStats();

        bm.SetDialogue(monster.Monster.MonsterName + " took " + dmg + " bleed damage!");

        yield return new WaitForSeconds(1);

    }
}
