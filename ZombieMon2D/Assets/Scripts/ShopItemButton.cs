using UnityEngine;
using TMPro;

public class ShopItemButton : MonoBehaviour
{
    [SerializeField] private ShopItemType itemType;
    [SerializeField] private int price = 10;
    [SerializeField] private TMP_Text priceText;

    private void Start()
    {
        if (priceText != null)
            priceText.text = price.ToString();
    }

    public void BuyItem()
    {
        if (CurrencyManager.Instance.SpendCoins(price))
        {
            InventoryManager.Instance.AddItem(itemType, 1);
            Debug.Log("Compraste: " + itemType);
        }
        else
        {
            Debug.Log("No alcanza el dinero.");
        }
    }
}