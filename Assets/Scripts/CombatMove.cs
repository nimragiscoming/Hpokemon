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
