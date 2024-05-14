using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Combat Move", menuName = "Monsters/New Combat Move")]
public class CombatMove : ScriptableObject
{
    public string MoveName;

    public int Power;

    public int Cost;

    public float Accuracy = 1;

    public string StatusText;

    public MoveType MoveType = MoveType.Physical;

    public MonsterType Type;

    public List<string> Args = new List<string>();
}

public enum MoveType
{
    Physical,
    Magical,
    Status
}

public static class MoveTypes
{
    public static Color GetMoveTypeColour(MoveType type)
    {
        switch (type)
        {
            case MoveType.Physical:
                return Color.white;
            case MoveType.Magical:
                return new Color32(200,200,255,255);
            case MoveType.Status:
                return new Color32(200, 255, 200, 255);

            default:
                return Color.white;
        }
    }
}
