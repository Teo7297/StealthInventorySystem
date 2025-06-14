using UnityEngine;

class AutoDamagePickup : Pickup
{
    [SerializeField]
    private Sprite icon;

    [SerializeField]
    private float damage = 200f;

    protected void Awake()
    {
        itemData = new ItemData
        {
            Name = "AutoDamage",
            Count = 1,
            Description = "Use to take some free damage",
            Icon = icon,
            Effect = (caller) =>
            {
                caller.GetComponent<Health>().TakeDamage(damage);
            }
        };
    }
}