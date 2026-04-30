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

    private void Awake()
    {
        Instance = this;

        if (battleCanvas != null)
            battleCanvas.SetActive(false);
    }

    public void StartBattle(BattleTrigger trigger)
    {
        if (trigger == null) return;

        currentTrigger = trigger;
        enemy = trigger.Enemy;

        if (enemy == null)
        {
            Debug.LogError("BattleTrigger no tiene Enemy asignado.");
            return;
        }

        enemyReward = enemy.GetComponent<BattleReward>();

        GameManager.Instance.SetState(GameState.Battle);

        if (joystick != null)
            joystick.ResetJoystick();

        if (joystickGO != null)
            joystickGO.SetActive(false);

        if (battleCanvas != null)
            battleCanvas.SetActive(true);

        enemy.gameObject.SetActive(true);
        enemy.ResetHP();

        if (hud != null)
            hud.Init(enemy);

        currentTurn = TurnState.PlayerTurn;

        Debug.Log("Battle started contra: " + enemy.name);
    }

    public void Fight()
    {
        if (currentTurn != TurnState.PlayerTurn) return;

        StartCoroutine(PlayerAttackRoutine());
    }

    private IEnumerator PlayerAttackRoutine()
    {
        currentTurn = TurnState.Busy;

        Debug.Log("Player attacks");

        int playerDamage = Random.Range(10, 45);

        enemy.TakeDamage(playerDamage);

        if (hud != null)
            hud.UpdateEnemyHP(enemy);

        if (CameraShake.Instance != null)
            CameraShake.Instance.Shake();

        yield return new WaitForSeconds(1f);

        if (enemy.IsDead())
        {
            Debug.Log("Enemy died");

            GiveBattleRewards();

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

            if (enemy != null)
                enemy.PlayAttack();

            yield return new WaitForSeconds(0.4f);

            int enemyDamage = GetEnemyDamageFromRemoteConfig();

            PlayerStats.Instance.TakeDamage(enemyDamage);

            if (hud != null)
                hud.UpdatePlayerHP();

            if (CameraShake.Instance != null)
                CameraShake.Instance.Shake();
        }
        else
        {
            Debug.Log("Enemy heals");

            int healAmount = 10;

            enemy.Heal(healAmount);

            if (hud != null)
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

    private int GetEnemyDamageFromRemoteConfig()
    {
        int minDamage = 10;
        int maxDamage = 35;
        bool enableCrit = false;

        if (RemoteConfigManager.Instance != null)
        {
            minDamage = RemoteConfigManager.Instance.enemyDamageMin;
            maxDamage = RemoteConfigManager.Instance.enemyDamageMax;
            enableCrit = RemoteConfigManager.Instance.enableCrit;
        }

        int damage = Random.Range(minDamage, maxDamage + 1);

        if (enableCrit)
        {
            float critChance = 0.25f;

            if (Random.value <= critChance)
            {
                damage *= 2;
                Debug.Log("CRITICO enemigo. Daño final: " + damage);
            }
        }

        Debug.Log("Daño enemigo: " + damage);
        return damage;
    }

    private void GiveBattleRewards()
    {
        if (enemyReward == null)
        {
            Debug.LogError("EnemyReward es NULL en " + enemy.name);
            return;
        }

        int baseReward = enemyReward.GetReward();
        int rewardMultiplier = 1;
        int xpReward = 25;

        if (RemoteConfigManager.Instance != null)
        {
            rewardMultiplier = RemoteConfigManager.Instance.rewardMultiplier;
            xpReward = RemoteConfigManager.Instance.xpPerEnemy;
        }

        int finalReward = baseReward * rewardMultiplier;

        Debug.Log("Recompensa base: " + baseReward);
        Debug.Log("Multiplicador recompensa: " + rewardMultiplier);
        Debug.Log("Recompensa final: " + finalReward);
        Debug.Log("XP ganada: " + xpReward);

        if (CurrencyManager.Instance != null)
            CurrencyManager.Instance.AddCoins(finalReward);

        if (CloudSaveManager.Instance != null)
        {
            CloudSaveManager.Instance.AddCoins(finalReward);
            CloudSaveManager.Instance.AddXP(xpReward);
            CloudSaveManager.Instance.AddBattleWon();
        }
        else
        {
            Debug.LogWarning("CloudSaveManager no encontrado. No se guardaron datos en la nube.");
        }
    }

    public void Run()
    {
        if (currentTurn != TurnState.PlayerTurn) return;

        EndBattle();
    }

    private void EndBattle()
    {
        if (battleCanvas != null)
            battleCanvas.SetActive(false);

        if (joystickGO != null)
            joystickGO.SetActive(true);

        GameManager.Instance.SetState(GameState.Exploration);

        enemy = null;
        enemyReward = null;
        currentTrigger = null;
    }
}