using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    private Dictionary<ShopItemType, int> items = new Dictionary<ShopItemType, int>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            foreach (ShopItemType item in System.Enum.GetValues(typeof(ShopItemType)))
            {
                items[item] = 0;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddItem(ShopItemType itemType, int amount)
    {
        if (!items.ContainsKey(itemType))
            items[itemType] = 0;

        items[itemType] += amount;
    }

    public int GetItemAmount(ShopItemType itemType)
    {
        if (!items.ContainsKey(itemType))
            return 0;

        return items[itemType];
    }

    public bool UseItem(ShopItemType itemType, int amount = 1)
    {
        if (!items.ContainsKey(itemType))
            return false;

        if (items[itemType] < amount)
            return false;

        items[itemType] -= amount;
        return true;
    }
}