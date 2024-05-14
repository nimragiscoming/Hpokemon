using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MonsterGirl
{
    public MGBase Monster;

    public string Title = "Unnamed Monster";

    public int BaseHealth => Monster.BaseHealth;

    private int health;

    public int Health
    {
        get { return health;}

        set { health = Math.Min(value, Monster.BaseHealth); }
    }

    public int BaseEnergy => Monster.BaseEnergy;

    private int energy;

    public int Energy
    {
        get { return energy; }

        set { energy = Math.Min(value, Monster.BaseEnergy); }
    }


    public int AtkStage;

    public int DfsStage;

    public int MAtkStage;

    public int MDfsStage;

    public int SpdStage;

    public MonsterModel Model;

    public List<StatusEffect> StatusEffects = new List<StatusEffect>();

    public void ResetStats()
    {
        Health = BaseHealth;

        Energy = BaseEnergy;

        AtkStage = 0;
        DfsStage = 0;
        MAtkStage = 0;
        MDfsStage = 0;
        SpdStage = 0;

        StatusEffects.Clear();
    }

}
