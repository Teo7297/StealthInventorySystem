using UnityEngine;

class AutoHealPickup : Pickup
{
    [SerializeField]
    private Sprite icon;

    [SerializeField]
    private float amount = 300f;

    protected void Awake()
    {
        itemData = new ItemData
        {
            Name = "AutoHeal",
            Count = 1,
            Description = "Use to get some free heals",
            Icon = icon,
            Effect = (caller) =>
            {
                caller.GetComponent<Health>().Heal(amount);
            }
        };
    }
}