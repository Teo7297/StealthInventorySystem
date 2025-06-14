using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Class responsible for player inventory management logic, this has been separated from the UI and
/// exposes some utility methods.
/// </summary>

public class Inventory : MonoBehaviour
{
    private static readonly int INVENTORY_SIZE = 25;

    private List<Pickup.ItemData> items;

    private PlayerInput playerInput;

    private void Awake()
    {
        items = new(INVENTORY_SIZE);
        for (int i = 0; i < INVENTORY_SIZE; i++)
        {
            items.Add(new Pickup.ItemData { Count = 0, Name = "", Description = "", Effect = null, Icon = null });
        }

        playerInput = GetComponent<PlayerInput>();

        if (playerInput == null)
            Debug.LogError("[Inventory] Could not initialize PlayerInteraction required components!");

    }

    /// <summary>
    /// Try to add an item to the inventory.
    /// </summary>
    /// <param name="itemData">Item to add</param>
    /// <returns>Success state of the operation</returns>
    public bool TryAddItem(Pickup.ItemData itemData)
    {
        int index = items.FindIndex((item) => { return item.Name.Equals(itemData.Name); });
        bool found = index > -1;

        if (found)
        {
            // Add a stack count if present
            var item = items[index];
            item.Count++;
            items[index] = item;
        }
        else
        {
            // Add a new item
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Count > 0)
                    continue;

                items[i] = itemData;
                break;
            }
        }

        return true;
    }

    /// <summary>
    /// Get the list of items in the inventory.
    /// </summary>
    /// <returns>List of Pickup.ItemData</returns>
    public List<Pickup.ItemData> GetItems()
    {
        return items;
    }

    /// <summary>
    /// Consume the item in the selected cell.
    /// </summary>
    /// <param name="cellID"></param>
    public void UseItem(int cellID)
    {
        var item = items[cellID];

        item.Effect(gameObject);
        item.Count--;

        if (item.Count > 0)
            items[cellID] = item;
        else
            items[cellID] = new();
    }

    /// <summary>
    /// Swap places between the data present in two cell slots.
    /// </summary>
    /// <param name="cellID1"></param>
    /// <param name="cellID2"></param>
    public void MoveItem(int cellID1, int cellID2)
    {
        Debug.Log($"Moved item from cell {cellID1} to {cellID2}");

        // Swap elements
        (items[cellID2], items[cellID1]) = (items[cellID1], items[cellID2]);
    }

}