using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Combat Move", menuName = "Monsters/New Combat Move")]
public class CombatMove : ScriptableObject
{
    public string MoveName;

    public int Power;

    public int Cost;

    public bool IsMagic = false;

    public MonsterType Type;
}
