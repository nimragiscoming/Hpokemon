using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGirl : MonoBehaviour
{
    public MGBase Monster;

    public int BaseHealth => Monster.BaseHealth;

    public int Health;

    public int BaseEnergy => Monster.BaseEnergy;

    public int Energy;

    public void ResetStats()
    {
        Health = BaseHealth;

        Energy = BaseEnergy;
    }

}
