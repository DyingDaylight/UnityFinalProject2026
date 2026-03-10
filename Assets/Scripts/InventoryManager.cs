using System.Collections.Generic;
using System.Linq;
using Quest;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    
    public static InventoryManager Instance;

    Dictionary<ItemCategory, Dictionary<string, int>> items = new Dictionary<ItemCategory, Dictionary<string, int>>();
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }
    
    public void AddItem(ItemCategory category, string itemId, int amount = 1)
    {
        if (!items.ContainsKey(category))
            items[category] = new Dictionary<string, int>();

        var categoryItems = items[category];

        if (!categoryItems.ContainsKey(itemId))
            categoryItems[itemId] = 0;

        categoryItems[itemId] += amount;

        Debug.Log($"Added {itemId} ({category}) x{amount}");
    }

    public bool HasItem(ItemCategory category, string itemId, int amount = 1)
    {
        if (!items.ContainsKey(category))
            return false;

        var categoryItems = items[category];

        if (!categoryItems.ContainsKey(itemId))
            return false;

        return categoryItems[itemId] >= amount;
    }
    
    public bool HasAnyItem(ItemCategory category)
    {
        if (!items.ContainsKey(category))
            return false;

        var categoryItems = items[category];

        return categoryItems.Values.Sum() > 0;
    }

    public void RemoveItem(ItemCategory category, string itemId, int amount = 1)
    {
        if (!items.ContainsKey(category))
            return;

        var categoryItems = items[category];

        if (!categoryItems.ContainsKey(itemId))
            return;
        
        categoryItems[itemId] -= amount;
        if (categoryItems[itemId] == 0)
            categoryItems.Remove(itemId);
    }
}
