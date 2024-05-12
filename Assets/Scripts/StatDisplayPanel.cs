using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class StatDisplayPanel : MonoBehaviour
{
    public MonsterGirl Girl;

    public TMP_Text HealthText;

    public TMP_Text EnergyText;

    public TMP_Text BaseStatText;


    public void UpdateText()
    {
        HealthText.text = Girl.Health + "/" + Girl.BaseHealth;

        EnergyText.text = Girl.Energy + "/" + Girl.BaseEnergy;

        StringBuilder sb = new StringBuilder();

        sb.AppendLine("Atk: "+Girl.Monster.Attack);
        sb.AppendLine("Dfs: " + Girl.Monster.Defense);
        sb.AppendLine("mAtk: " + Girl.Monster.MagicAttack);
        sb.AppendLine("mDfs: " + Girl.Monster.MagicDefense);
        sb.AppendLine("Spd: " + Girl.Monster.Speed);
        sb.AppendLine("Psn: " + Girl.Monster.Precision);
        sb.AppendLine();
        sb.AppendLine("Type: " + Girl.Monster.Type.ToString());

        BaseStatText.text = sb.ToString();
    }
}
