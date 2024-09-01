using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Transform playerBattleState;
    public Transform enemyBattleState;

    Unit playerUnit;
    Unit enemyUnit;

    public TMP_Text dialogueText;

    public BattleHub playerHUB;
    public BattleHub enemyHUB;

    public BattleState state;

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        GameObject playerGO =  Instantiate(playerPrefab, playerBattleState);
        playerUnit = playerGO.GetComponent<Unit>();

        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleState);
        enemyUnit = playerGO.GetComponent<Unit>();

        dialogueText.text = "A wild " + enemyUnit.unitName + " approches...";

        playerHUB.SetHUB(playerUnit);
        enemyHUB.SetHUB(enemyUnit);

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    IEnumerator PlayerAttack()
    {
        yield return new WaitForSeconds(2f);
    }

    void PlayerTurn()
    {
        dialogueText.text = "Choose an action :";
    }

    public void onAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerAttack());
    }
}
