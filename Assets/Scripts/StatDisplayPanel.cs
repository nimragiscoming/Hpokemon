using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatDisplayPanel : MonoBehaviour
{
    public MonsterGirl Girl;

    public TMP_Text HealthText;

    public TMP_Text EnergyText;


    public void UpdateText()
    {
        HealthText.text = Girl.Health + "/" + Girl.BaseHealth;

        EnergyText.text = Girl.Energy + "/" + Girl.BaseEnergy;
    }
}
