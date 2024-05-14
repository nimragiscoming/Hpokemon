using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatMoveButton : MonoBehaviour
{

    public BattleManager BattleManager;

    public CombatMove Move;

    public TMP_Text NameText;
    public TMP_Text PowerText;
    public TMP_Text TypeText;

    public Image Background;

    private void Start()
    {
        NameText.text = Move.MoveName;

        if(Move.MoveType == MoveType.Status)
        {
            PowerText.text = Move.StatusText;
        }
        else
        {
            PowerText.text = Move.Power.ToString() + "DP";
        }


        TypeText.text = Move.Type.ToString();
        TypeText.color = MonsterTypes.GetTypeColour(Move.Type);

        Background.color = MoveTypes.GetMoveTypeColour(Move.MoveType);
    }

    public void OnPressed()
    {
        //i have to do this due to unity stupidity, because im immediately destroying this object afterwards so it just stalls the coroutine
        BattleManager.StartCoroutine(BattleManager.DoPlayerAttack(Move));
    }
}
