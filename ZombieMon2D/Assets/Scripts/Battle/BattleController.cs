using UnityEngine;
using System.Collections;

public class BattleController : MonoBehaviour
{
    public static BattleController Instance;

    [Header("Refs")]
    [SerializeField] private GameObject battleCanvas;
    [SerializeField] private GameObject enemyGO;
    [SerializeField] private Enemy enemy;
    [SerializeField] private BattleReward enemyReward;
    [SerializeField] private BattleHUD hud;
    [SerializeField] private BattleRewardManager battleRewardManager;

    private TurnState currentTurn;

    private void Awake()
    {
        Instance = this;
        battleCanvas.SetActive(false);
        enemyGO.SetActive(false);
    }

    public void StartBattle()
    {
        GameManager.Instance.SetState(GameState.Battle);

        battleCanvas.SetActive(true);
        enemyGO.SetActive(true);

        enemy.ResetHP();
        hud.Init(enemy);

        currentTurn = TurnState.PlayerTurn;

        Debug.Log("Battle started");
    }

    // ===== PLAYER =====

    public void Fight()
    {
        if (currentTurn != TurnState.PlayerTurn) return;

        StartCoroutine(PlayerAttackRoutine());
    }

    private IEnumerator PlayerAttackRoutine()
    {
        currentTurn = TurnState.Busy;

        Debug.Log("Player attacks");
        enemy.TakeDamage(Random.Range(25, 45));
        hud.UpdateEnemyHP(enemy);
        CameraShake.Instance.Shake();

        yield return new WaitForSeconds(1f);

        if (enemy.IsDead())
        {
            Debug.Log("Enemy died");

            if (enemyReward != null)
            {
                Debug.Log("EnemyReward encontrado. Recompensa: " + enemyReward.GetReward());
            }
            else
            {
                Debug.LogError("EnemyReward es NULL");
            }

            if (CurrencyManager.Instance != null)
            {
                Debug.Log("CurrencyManager encontrado");
                CurrencyManager.Instance.AddCoins(enemyReward.GetReward());
            }
            else
            {
                Debug.LogError("CurrencyManager.Instance es NULL");
            }

            EndBattle();
            yield break;
        }
        currentTurn = TurnState.EnemyTurn;
        StartCoroutine(EnemyTurnRoutine());
    }

    // ===== ENEMY =====

    private IEnumerator EnemyTurnRoutine()
    {
        Debug.Log("Enemy turn");

        yield return new WaitForSeconds(1f);

        int action = Random.Range(0, 2);

        if (action == 0)
        {
            Debug.Log("Enemy attacks");
            PlayerStats.Instance.TakeDamage(Random.Range(10, 35));
            hud.UpdatePlayerHP();
            CameraShake.Instance.Shake();
        }
        else
        {
            Debug.Log("Enemy heals");
            enemy.Heal(10);
            hud.UpdateEnemyHP(enemy);
        }

        yield return new WaitForSeconds(1f);

        if (PlayerStats.Instance.IsDead())
        {
            Debug.Log("Player died");
            EndBattle();
            yield break;
        }

        currentTurn = TurnState.PlayerTurn;
    }

    // ===== RUN =====

    public void Run()
    {
        if (currentTurn != TurnState.PlayerTurn) return;

        EndBattle();
    }

    // ===== END =====

    private void EndBattle()
    {
        battleCanvas.SetActive(false);
        enemyGO.SetActive(false);

        GameManager.Instance.SetState(GameState.Exploration);
    }
}