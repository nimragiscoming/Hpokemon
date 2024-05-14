using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MonsterTypes
{

    //let the record show that is terrible, and im only writing it like this cause its a game jam
    public static float GetTypeEffect(MonsterType attackingType, MonsterType defendingType)
    {
        if (attackingType == defendingType) // types are always neutral against themselves
            return 1.0f;

        float result = 1.0f;

        // check for resistances, if it isn't a resistance, it's a weakness
        switch (attackingType)
        {
            case MonsterType.Slime:
                result = defendingType == MonsterType.Enchanted || defendingType == MonsterType.Space ? 0.5f : 2.0f;
                break;

            case MonsterType.Beast:
                result = defendingType == MonsterType.Slime || defendingType == MonsterType.Occult ? 0.5f : 2.0f;
                break;

            case MonsterType.Occult:
                result = defendingType == MonsterType.Slime || defendingType == MonsterType.Space ? 0.5f : 2.0f; 
                break;

            case MonsterType.Space:
                result = defendingType == MonsterType.Beast || defendingType == MonsterType.Enchanted ? 0.5f : 2.0f; 
                break;

            case MonsterType.Enchanted:
                result = defendingType == MonsterType.Beast || defendingType == MonsterType.Occult ? 0.5f : 2.0f;
                break;
        }

        return result;
    }

    public static Color GetTypeColour(MonsterType Type)
    {
        switch (Type)
        {
            case MonsterType.Slime:
                return new Color32(175,255,191,255);

            case MonsterType.Beast:
                return new Color32(224, 79, 63, 255);

            case MonsterType.Occult:
                return new Color32(33, 33, 33, 255);

            case MonsterType.Space:
                return new Color32(188, 210, 234, 255);

            case MonsterType.Enchanted:
                return new Color32(255, 188, 254, 255);

            default:
                return Color.white;
        }


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
