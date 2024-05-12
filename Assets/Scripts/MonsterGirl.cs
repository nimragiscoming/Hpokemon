using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MonsterGirl
{
    public MGBase Monster;

    public int BaseHealth => Monster.BaseHealth;

    public int Health;

    public int BaseEnergy => Monster.BaseEnergy;

    public int Energy;


    public int AtkStage;

    public int DfsStage;

    public int MAtkStage;

    public int MDfsStage;

    public int SpdStage;

    public void ResetStats()
    {
        Health = BaseHealth;

        Energy = BaseEnergy;

        AtkStage = 0;
        DfsStage = 0;
        MAtkStage = 0;
        MDfsStage = 0;
        SpdStage = 0;
    }

}
