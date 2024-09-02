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

    [SerializeField]
    private GameObject lose;

    [SerializeField]
    private GameObject win;

    public BattleState state;

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            audioManager.PlaySFX(audioManager.click);
        }
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
        playerBattleStation.gameObject.SetActive(false);
        enemyBattleStation.gameObject.SetActive(false);

        if (state == BattleState.WON)
        {
            dialogueText.text = "You won the battle!";
            win.SetActive(true);
        }
        else if (state == BattleState.LOST)
        {
            dialogueText.text = "You were defeated.";
            lose.SetActive(true);
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

    playerAction = ActionType.ATTACK;
    audioManager.PlaySFX(audioManager.attack);
    StartCoroutine(ExecuteTurn());
}

    public void OnCriticalButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        playerAction = ActionType.CRITICAL;
        audioManager.PlaySFX(audioManager.critical);
        StartCoroutine(ExecuteTurn());
    }

    public void OnParryButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        playerAction = ActionType.PARRY;
        audioManager.PlaySFX(audioManager.parry);
        StartCoroutine(ExecuteTurn());
    }

    IEnumerator ExecuteTurn()
    {
        audioManager.PlaySFX(audioManager.click);
        enemyAction = (ActionType)Random.Range(0, 3);
        dialogueText.text = "Enemy chose " + enemyAction.ToString();

        yield return new WaitForSeconds(1f);

        bool playerWinsRound = false;
        bool enemyWinsRound = false;

        if (playerAction == ActionType.ATTACK && enemyAction == ActionType.PARRY)
        {
            dialogueText.text = "You broke through the enemy's parry!";
            enemyUnit.TakeDamage(playerUnit.damage);
            enemyHUD.SetHP(enemyUnit.currentHP);
            playerWinsRound = true;
        }

        else if (playerAction == ActionType.PARRY && enemyAction == ActionType.CRITICAL)
        {
            dialogueText.text = "You parried the enemy's critical!";
            playerWinsRound = true;
        }

        else if (playerAction == ActionType.CRITICAL && enemyAction == ActionType.ATTACK)
        {
            dialogueText.text = "You crushed the enemy's attack!";
            enemyUnit.TakeDamage(2);
            enemyHUD.SetHP(enemyUnit.currentHP);
            playerWinsRound = true;
        }

        else if (enemyAction == ActionType.ATTACK && playerAction == ActionType.PARRY)
        {
            dialogueText.text = "Enemy broke through your parry!";
            playerUnit.TakeDamage(enemyUnit.damage);
            playerHUD.SetHP(playerUnit.currentHP);
            enemyWinsRound = true;
        }

        else if (enemyAction == ActionType.PARRY && playerAction == ActionType.CRITICAL)
        {
            dialogueText.text = "Enemy parried your critical!";
            enemyWinsRound = true;
        }

        else if (enemyAction == ActionType.CRITICAL && playerAction == ActionType.ATTACK)
        {
            dialogueText.text = "Enemy crushed your attack!";
            playerUnit.TakeDamage(2);
            playerHUD.SetHP(playerUnit.currentHP);
            enemyWinsRound = true;
        }

        else
        {
            dialogueText.text = "It's a draw!";
        }

        yield return new WaitForSeconds(1.5f);

        if (playerUnit.currentHP <= 0)
        {
            state = BattleState.LOST;
            EndBattle();
            yield break;
        }

        else if (enemyUnit.currentHP <= 0)
        {
            state = BattleState.WON;
            EndBattle();
            yield break;
        }

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

    public void CheckBattleOutcome()
    {
        if (enemyUnit.currentHP <= 0)
        {
            state = BattleState.WON;
            EndBattle();
        }
        else if (playerUnit.currentHP <= 0)
        {
            state = BattleState.LOST;
            EndBattle();
        }
    }
}