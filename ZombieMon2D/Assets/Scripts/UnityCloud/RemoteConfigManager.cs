using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.RemoteConfig;

public class RemoteConfigManager : MonoBehaviour
{
    public static RemoteConfigManager Instance;

    public int enemyDamageMin = 10;
    public int enemyDamageMax = 35;
    public int rewardMultiplier = 1;
    public int xpPerEnemy = 25;
    public bool enableCrit = true;

    public bool IsReady { get; private set; }

    private struct UserAttributes { }
    private struct AppAttributes { }

    private async void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        await UnityServices.InitializeAsync();

        if (!AuthenticationService.Instance.IsSignedIn)
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

        RemoteConfigService.Instance.FetchCompleted += ApplyRemoteConfig;

        RemoteConfigService.Instance.FetchConfigs(
            new UserAttributes(),
            new AppAttributes()
        );
    }

    private void ApplyRemoteConfig(ConfigResponse response)
    {
        enableCrit = RemoteConfigService.Instance.appConfig.GetBool("enable_crit", true);
        enemyDamageMax = RemoteConfigService.Instance.appConfig.GetInt("enemy_damage_max", 35);
        enemyDamageMin = RemoteConfigService.Instance.appConfig.GetInt("enemy_damage_min", 10);
        rewardMultiplier = RemoteConfigService.Instance.appConfig.GetInt("reward_multiplier", 1);
        xpPerEnemy = RemoteConfigService.Instance.appConfig.GetInt("xp_per_enemy", 25);

        IsReady = true;

        Debug.Log("Remote Config cargado:");
        Debug.Log("enable_crit: " + enableCrit);
        Debug.Log("enemy_damage_max: " + enemyDamageMax);
        Debug.Log("enemy_damage_min: " + enemyDamageMin);
        Debug.Log("reward_multiplier: " + rewardMultiplier);
        Debug.Log("xp_per_enemy: " + xpPerEnemy);
    }
}