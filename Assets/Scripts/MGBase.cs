using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Monster Girl", menuName = "Monsters/New Monster Girl")]
public class MGBase : ScriptableObject
{
    public string MonsterName;

    public int BaseHealth;

    public int BaseEnergy;

    public int Attack;

    public int Defense;

    public int MagicAttack;

    public int MagicDefense;

    public int Speed;

    public int Precision;

    public MonsterType Type;

    public List<CombatMove> Moveset;

    public MonsterModel Model;
}

public enum Stat
{
    BaseHealth,
    BaseEnergy,
    Attack,
    Defense,
    MagicAttack,
    MagicDefense,
    Speed,
    Precision
}
