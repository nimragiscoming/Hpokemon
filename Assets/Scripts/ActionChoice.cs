using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionChoice : MonoBehaviour
{
    public BattleManager BM;

    public GameObject CombatMoveButtonGroup;

    public CombatMoveButton CombatMoveButton;

    public GirlOptionButton GirlOptionButton;

    public void Attack()
    {
        CreateCombatChoices();

        CloseMenu();
    }
    public void CreateCombatChoices()
    {
        foreach (CombatMove move in BM.CurPlayerMG.Monster.Moveset)
        {
            if (move.Cost > BM.CurPlayerMG.Energy) { continue; }
            CombatMoveButton Mv = Instantiate(CombatMoveButton, CombatMoveButtonGroup.transform);
            Mv.BattleManager = BM;
            Mv.Move = move;
        }
    }

    public void Switch()
    {
        CreateSwitchChoices();

        CloseMenu();
    }

    public void Rest()
    {
        BM.StartCoroutine(BM.DoPlayerRest());

        CloseMenu();
    }

    public void CreateSwitchChoices()
    {
        foreach (MonsterGirl girl in BM.Player.Girls)
        {
            if (girl == BM.CurPlayerMG) { continue; }
            GirlOptionButton Mv = Instantiate(GirlOptionButton, CombatMoveButtonGroup.transform);
            Mv.BattleManager = BM;
            Mv.Girl = girl;
        }
    }

    public void ClearBox()
    {
        foreach (Transform child in CombatMoveButtonGroup.transform)
        {
            Destroy(child.gameObject);
        }
    }



    void CloseMenu()
    {
        this.gameObject.SetActive(false);
    }
}
