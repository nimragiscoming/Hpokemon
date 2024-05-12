using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MonsterTypes
{

    //let the record show that is terrible, and im only writing it like this cause its a game jam
    public static int GetTypeBonus(MonsterType T1, MonsterType T2)
    {
        if(T1 == MonsterType.Slime)
        {
            if(T2 == MonsterType.Enchanted || T2 == MonsterType.Space)
            {
                return -1;
            }

            if (T2 == MonsterType.Beast || T2 == MonsterType.Occult)
            {
                return 1;
            }

            return 0;
        }

        if (T1 == MonsterType.Beast)
        {
            if (T2 == MonsterType.Slime || T2 == MonsterType.Occult)
            {
                return -1;
            }

            if (T2 == MonsterType.Space || T2 == MonsterType.Enchanted)
            {
                return 1;
            }

            return 0;
        }

        if (T1 == MonsterType.Occult)
        {
            if (T2 == MonsterType.Slime || T2 == MonsterType.Space)
            {
                return -1;
            }

            if (T2 == MonsterType.Beast || T2 == MonsterType.Enchanted)
            {
                return 1;
            }

            return 0;
        }

        if (T1 == MonsterType.Space)
        {
            if (T2 == MonsterType.Beast || T2 == MonsterType.Enchanted)
            {
                return -1;
            }

            if (T2 == MonsterType.Slime || T2 == MonsterType.Occult)
            {
                return 1;
            }

            return 0;
        }

        if (T1 == MonsterType.Enchanted)
        {
            if (T2 == MonsterType.Occult || T2 == MonsterType.Beast)
            {
                return -1;
            }

            if (T2 == MonsterType.Slime|| T2 == MonsterType.Space)
            {
                return 1;
            }

            return 0;
        }

        return 0;
    }
}

public enum MonsterType
{
    Slime,
    Beast,
    Occult,
    Space,
    Enchanted
}
