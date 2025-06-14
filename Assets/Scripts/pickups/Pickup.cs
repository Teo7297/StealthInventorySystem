using UnityEngine;

/// <summary>
/// Pickup base class. Offers basic functionality.
/// To create a new item/pickup just extend this class and
/// add custom ItemData
/// </summary>

public class Pickup : MonoBehaviour
{
    public struct ItemData
    {
        public delegate void EffectEvent(GameObject caller);

        public int Count;
        public string Name;
        public string Description;
        public EffectEvent Effect;
        public Sprite Icon;
    }

    protected ItemData itemData;

    public bool TryObtain(ref Inventory playerInventory)
    {
        bool success = playerInventory.TryAddItem(itemData);
        if (success)
            Destroy(gameObject); // Destroy takes place at the end of the frame! We can still finish up eventual work with this object.
        return success;
    }

    public string GetItemName()
    {
        return itemData.Name;
    }
}