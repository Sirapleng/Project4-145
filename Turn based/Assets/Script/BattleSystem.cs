using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { START , PLAYERTURN , ENEMYTURN , WON , LOST}
public enum ActionType { ATTACK, CRITICAL, PARRY }

public class BattleSystem : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    public ActionType playerAction;
    public ActionType enemyAction;

    Unit playerUnit;
    Unit enemyUnit;

    public TMP_Text dialogueText;

    public BattleHUB playerHUD;
    public BattleHUB enemyHUD;

    public BattleState state;

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
        playerUnit = playerGO.GetComponent<Unit>();

        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
        enemyUnit = enemyGO.GetComponent<Unit>();

        dialogueText.text = "A wild " + enemyUnit.unitName + " approaches...";

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        yield return new WaitForSeconds(2);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    IEnumerator PlayerAttack()
    {
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);

        enemyHUD.SetHP(enemyUnit.currentHP);
        dialogueText.text = "The attack is successful!";

        yield return new WaitForSeconds(2f);

        if (isDead)
        {
            state = BattleState.WON;
            EndBattle();
        }

        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator EnemyTurn()
    {
        dialogueText.text = enemyUnit.unitName + " attacks!";
        yield return new WaitForSeconds(1.5f);

        bool isDead = playerUnit.TakeDamage(enemyUnit.damage);

        playerHUD.SetHP(playerUnit.currentHP);
        yield return new WaitForSeconds(1.5f);

        if(isDead)
        {
            state = BattleState.LOST;
            EndBattle();
        }

        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    void EndBattle()
    {
        if (state == BattleState.WON)
        {
            dialogueText.text = "You won the battle!";
        }

        else if (state == BattleState.LOST)
        {
            dialogueText.text = "You were defeated.";
        }


    }
    void PlayerTurn()
    {
        dialogueText.text = "Choose an action : ";
    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerAttack());
    }

    public void OnCriticalButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        playerAction = ActionType.CRITICAL;
        StartCoroutine(ExecuteTurn());
    }

    public void OnParryButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        playerAction = ActionType.PARRY;
        StartCoroutine(ExecuteTurn());
    }

    IEnumerator ExecuteTurn()
    {
        enemyAction = (ActionType)Random.Range(0, 3);
        dialogueText.text = "Enemy chose " + enemyAction.ToString();

        yield return new WaitForSeconds(1f);

        bool playerWinsRound = false;
        bool enemyWinsRound = false;

        if (playerAction == ActionType.ATTACK && enemyAction == ActionType.PARRY)
        {
            dialogueText.text = "Enemy parried your attack!";
            enemyWinsRound = true;
        }

        else if (playerAction == ActionType.CRITICAL && enemyAction == ActionType.ATTACK)
        {
            enemyUnit.TakeDamage(2);
            dialogueText.text = "Landed a critical hit!";
            enemyHUD.SetHP(enemyUnit.currentHP);
            playerWinsRound = true;
        }

        else if (playerAction == ActionType.PARRY && enemyAction == ActionType.CRITICAL)
        {
            dialogueText.text = "Parried the enemy's critical attack!";
            playerWinsRound = true;
        }

        else if (enemyAction == ActionType.ATTACK && playerAction == ActionType.PARRY)
        {
            dialogueText.text = "Parried the enemy's attack!";
            playerWinsRound = true;
        }

        else if (enemyAction == ActionType.CRITICAL && playerAction == ActionType.ATTACK)
        {
            playerUnit.TakeDamage(2);
            dialogueText.text = "Enemy landed a critical hit!";
            playerHUD.SetHP(playerUnit.currentHP);
            enemyWinsRound = true;
        }

        else if (enemyAction == ActionType.PARRY && playerAction == ActionType.CRITICAL)
        {
            dialogueText.text = "Enemy parried your critical attack!";
            enemyWinsRound = true;
        }

        else
        {
            dialogueText.text = "It's a draw!";
        }

        yield return new WaitForSeconds(1.5f);

        if (playerWinsRound && playerAction == ActionType.PARRY)
        {
            if (playerUnit.currentHP < playerUnit.maxHP)
            {
                playerUnit.currentHP = Mathf.Min(playerUnit.currentHP + 1, playerUnit.maxHP);
                playerHUD.SetHP(playerUnit.currentHP);
                dialogueText.text += " +1 HP!";
            }

            else
            {
                dialogueText.text += " HP Full!";
            }
        }

        else if (enemyWinsRound && enemyAction == ActionType.PARRY)
        {
            if (enemyUnit.currentHP < enemyUnit.maxHP)
            {
                enemyUnit.currentHP = Mathf.Min(enemyUnit.currentHP + 1, enemyUnit.maxHP);
                enemyHUD.SetHP(enemyUnit.currentHP);
                dialogueText.text += " Enemy +1 HP!";
            }

            else
            {
                dialogueText.text += " Enemy HP Full!";
            }
        }

        yield return new WaitForSeconds(1.5f);

        if (playerUnit.currentHP <= 0)
        {
            state = BattleState.LOST;
            EndBattle();
        }

        else if (enemyUnit.currentHP <= 0)
        {
            state = BattleState.WON;
            EndBattle();
        }

        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }
}