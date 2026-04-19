using UnityEngine;
using TMPro;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;

    [SerializeField] private int currentCoins = 0;
    [SerializeField] private TMP_Text coinsText;

    public int CurrentCoins => currentCoins;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateCoinsUI();
    }

    public void AddCoins(int amount)
    {
        currentCoins += amount;
        UpdateCoinsUI();
    }

    public bool SpendCoins(int amount)
    {
        if (currentCoins < amount)
            return false;

        currentCoins -= amount;
        UpdateCoinsUI();
        return true;
    }

    public void ResetCoins()
    {
        currentCoins = 0;
        UpdateCoinsUI();
    }

    public void UpdateCoinsUI()
    {
        if (coinsText != null)
            coinsText.text = currentCoins.ToString();
    }
}