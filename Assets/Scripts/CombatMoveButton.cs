using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CombatMoveButton : MonoBehaviour
{

    public BattleManager BattleManager;

    public CombatMove Move;

    public TMP_Text NameText;
    public TMP_Text PowerText;
    public TMP_Text TypeText;

    private void Start()
    {
        NameText.text = Move.MoveName;
        PowerText.text = Move.Power.ToString() + "DP";
        TypeText.text = Move.Type.ToString();
    }

    public void OnPressed()
    {
        //i have to do this due to unity stupidity, because im immediately destroying this object afterwards so it just stalls the coroutine
        BattleManager.StartCoroutine(BattleManager.DoPlayerTurn(Move));
    }
}
