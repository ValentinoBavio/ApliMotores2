using UnityEngine;
using System.Collections;

public class BattleController : MonoBehaviour
{
    public static BattleController Instance;

    [Header("Refs")]
    [SerializeField] private GameObject battleCanvas;
    [SerializeField] private BattleHUD hud;
    [SerializeField] private BattleRewardManager battleRewardManager;

    [Header("Mobile UI")]
    [SerializeField] private GameObject joystickGO;
    [SerializeField] private Joystick_Controller joystick;

    private Enemy enemy;
    private BattleReward enemyReward;
    private BattleTrigger currentTrigger;
    private TurnState currentTurn;


    [SerializeField] private SpriteRenderer enemySprite;
    [SerializeField] private SpriteRenderer playerSprite;

    [SerializeField] private float flashDuration = 0.1f;
    [SerializeField] private Color flashColor = Color.red;

    [Header("End Screens")]
    [SerializeField] private SpriteRenderer defeatSprite;
    [SerializeField] private SpriteRenderer victorySprite;

    private int enemiesKilled = 0;

    private IEnumerator FlashSprite(SpriteRenderer sr)
    {
        if (sr == null) yield break;

        Color original = sr.color;

        sr.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        sr.color = original;
    }

    private void Awake()
    {
        Instance = this;

        if (battleCanvas != null)
            battleCanvas.SetActive(false);

        if (defeatSprite != null)
            defeatSprite.gameObject.SetActive(false);

        if (victorySprite != null)
            victorySprite.gameObject.SetActive(false);
    }

    public void StartBattle(BattleTrigger trigger)
    {
        if (trigger == null) return;

        currentTrigger = trigger;
        enemy = trigger.Enemy;

        if (enemy == null)
        {
            return;
        }

        enemyReward = enemy.GetComponent<BattleReward>();

        GameManager.Instance.SetState(GameState.Battle);

        if (joystick != null)
            joystick.ResetJoystick();

        if (joystickGO != null)
            joystickGO.SetActive(false);

        battleCanvas.SetActive(true);

        enemy.gameObject.SetActive(true);
        enemy.ResetHP();

        hud.Init(enemy);

        currentTurn = TurnState.PlayerTurn;

        Debug.Log("Battle started contra: " + enemy.name);
    }

    public void Fight()
    {
        if (currentTurn != TurnState.PlayerTurn) return;

        StartCoroutine(PlayerAttackRoutine());
    }

    [SerializeField] private float attackMultiplier = 1f;

    public void IncreaseAttackMultiplier(float amount)
    {
        attackMultiplier += amount;
        
        attackMultiplier = Mathf.Min(attackMultiplier, 3f);

        Debug.Log("Attack Multiplier: " + attackMultiplier);
    }

    private IEnumerator PlayerAttackRoutine()
    {
        currentTurn = TurnState.Busy;

        Debug.Log("Player attacks");

        int damage = (int)(Random.Range(10, 45) * attackMultiplier);
        enemy.TakeDamage(damage);

        hud.UpdateEnemyHP(enemy);

        if (CameraShake.Instance != null)
            CameraShake.Instance.Shake();

        enemy.TakeDamage(damage);

        // flash enemigo
        StartCoroutine(FlashSprite(enemySprite));

        yield return new WaitForSeconds(1f);

        if (enemy.IsDead())
        {
            Debug.Log("Enemy died");

            enemiesKilled++;

            if (enemiesKilled >= 2)
            {
                if (victorySprite != null)
                    victorySprite.gameObject.SetActive(true);

                Time.timeScale = 0f;
            }
                        
            if (enemyReward != null)
            {
                Debug.Log("Recompensa: " + enemyReward.GetReward());

                if (CurrencyManager.Instance != null)
                    CurrencyManager.Instance.AddCoins(enemyReward.GetReward());
            }
            else
            {
                Debug.LogError("EnemyReward es NULL en " + enemy.name);
            }

            if (currentTrigger != null)
                currentTrigger.MarkAsDefeated();

            EndBattle();
            yield break;
        }

       

        currentTurn = TurnState.EnemyTurn;
        StartCoroutine(EnemyTurnRoutine());
    }

    private IEnumerator EnemyTurnRoutine()
    {
        Debug.Log("Enemy turn");

        yield return new WaitForSeconds(1f);

        int action = Random.Range(0, 2);

        if (action == 0)
        {
            Debug.Log("Enemy attacks");

            enemy.PlayAttack();

            yield return new WaitForSeconds(0.4f);

            PlayerStats.Instance.TakeDamage(Random.Range(10, 35));
            hud.UpdatePlayerHP();

            if (CameraShake.Instance != null)
                CameraShake.Instance.Shake();
        }
        else
        {
            Debug.Log("Enemy heals");

            enemy.Heal(10);
            hud.UpdateEnemyHP(enemy);
        }

        PlayerStats.Instance.TakeDamage(Random.Range(10, 35));

        // flash player
        StartCoroutine(FlashSprite(playerSprite));

        yield return new WaitForSeconds(1f);

        if (PlayerStats.Instance.IsDead())
        {
            Debug.Log("Player died");

            if (defeatSprite != null)
                defeatSprite.gameObject.SetActive(true);

            Time.timeScale = 0f;

            EndBattle();
            yield break;
        }

        currentTurn = TurnState.PlayerTurn;
    }

    public void Run()
    {
        if (currentTurn != TurnState.PlayerTurn) return;

        EndBattle();
    }

    private void EndBattle()
    {
        battleCanvas.SetActive(false);

        if (joystickGO != null)
            joystickGO.SetActive(true);

        GameManager.Instance.SetState(GameState.Exploration);

        enemy = null;
        enemyReward = null;
        currentTrigger = null;
    }
}