using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using System.Collections.Generic;
using System.Threading.Tasks;

public class CloudSaveManager : MonoBehaviour
{
    public static CloudSaveManager Instance;

    [Header("Cloud Save Data")]
    public string username = "Player";
    public int coins = 0;
    public int playerLevel = 1;
    public int playerXP = 0;
    public int battlesWon = 0;

    public bool IsReady { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private async void Start()
    {
        await InitializeServices();
        await LoadPlayerData();

        IsReady = true;

        Debug.Log("Cloud Save listo");
    }

    private async Task InitializeServices()
    {
        if (UnityServices.State == ServicesInitializationState.Uninitialized)
        {
            await UnityServices.InitializeAsync();
        }

        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        Debug.Log("Player ID: " + AuthenticationService.Instance.PlayerId);
    }

    public async Task SavePlayerData()
    {
        var data = new Dictionary<string, object>
        {
            { "username", username },
            { "coins", coins },
            { "player_level", playerLevel },
            { "player_xp", playerXP },
            { "battles_won", battlesWon }
        };

        await CloudSaveService.Instance.Data.Player.SaveAsync(data);

        Debug.Log("Datos guardados en Cloud Save");
    }

    public async Task LoadPlayerData()
    {
        var keys = new HashSet<string>
        {
            "username",
            "coins",
            "player_level",
            "player_xp",
            "battles_won"
        };

        var loadedData = await CloudSaveService.Instance.Data.Player.LoadAsync(keys);

        if (loadedData.TryGetValue("username", out var usernameData))
            username = usernameData.Value.GetAs<string>();

        if (loadedData.TryGetValue("coins", out var coinsData))
            coins = coinsData.Value.GetAs<int>();

        if (loadedData.TryGetValue("player_level", out var levelData))
            playerLevel = levelData.Value.GetAs<int>();

        if (loadedData.TryGetValue("player_xp", out var xpData))
            playerXP = xpData.Value.GetAs<int>();

        if (loadedData.TryGetValue("battles_won", out var battlesData))
            battlesWon = battlesData.Value.GetAs<int>();

        Debug.Log("Datos cargados desde Cloud Save:");
        Debug.Log("Username: " + username);
        Debug.Log("Coins: " + coins);
        Debug.Log("Level: " + playerLevel);
        Debug.Log("XP: " + playerXP);
        Debug.Log("Battles Won: " + battlesWon);
    }

    public async void SetUsername(string newUsername)
    {
        if (string.IsNullOrWhiteSpace(newUsername))
            return;

        username = newUsername.Trim();
        await SavePlayerData();
    }

    public async void AddCoins(int amount)
    {
        coins += amount;
        await SavePlayerData();
    }

    public async void AddXP(int amount)
    {
        playerXP += amount;

        while (playerXP >= 100)
        {
            playerXP -= 100;
            playerLevel++;
            Debug.Log("Subió de nivel. Nivel actual: " + playerLevel);
        }

        await SavePlayerData();
    }

    public async void AddBattleWon()
    {
        battlesWon++;
        await SavePlayerData();
    }
}