using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class GirlOptionButton : MonoBehaviour
{
    public BattleManager BattleManager;

    public TMP_Text Label;

    public MonsterGirl Girl;
    private void Start()
    {
        Label.text = Girl.Title;
    }

    public void OnPressed()
    {
        //i have to do this due to unity stupidity, because im immediately destroying this object afterwards so it just stalls the coroutine
        BattleManager.StartCoroutine(BattleManager.DoPlayerSwitch(Girl));
    }
}
