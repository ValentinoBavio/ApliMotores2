using UnityEngine;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private int healPrice = 10;
    [SerializeField] private float healAmount = 25f;

    public void BuyHeal()
    {
        if (PlayerStats.Instance.CurrentHP >= PlayerStats.Instance.GetMaxHP())
            return;

        if (!CurrencyManager.Instance.SpendCoins(healPrice))
            return;

        PlayerStats.Instance.Heal(healAmount);
    }

    public void BuyDamage()
    {
        if (!CurrencyManager.Instance.SpendCoins(10))
            return;

        BattleController.Instance.IncreaseAttackMultiplier(0.25f);
    }

    public void CloseShop()
    {
        ShopManager.Instance.CloseShop();
    }
}
