using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CombatMoveButton : MonoBehaviour
{

    public BattleManager BattleManager;

    public CombatMove Move;

    public TMP_Text text;

    private void Start()
    {
        text.text = Move.MoveName;
    }

    public void OnPressed()
    {
        //i have to do this due to unity stupidity, because im immediately destroying this object afterwards so it just stalls the coroutine
        BattleManager.StartCoroutine(BattleManager.DoPlayerTurn(Move));
    }
}
