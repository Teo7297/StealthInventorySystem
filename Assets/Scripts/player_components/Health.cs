using UnityEngine;

/// <summary>
/// Simple health component. Health resets when reaches 0.
/// This has been added to demonstrate simple item effects.
/// </summary>

public class Health : MonoBehaviour
{
    [SerializeField]
    private float maxHealth = 1000f;
    private float currentHealth;

    public float GetMaxHealth() => maxHealth;
    public float GetHealth() => currentHealth;

    public delegate void HealthChangeEvent(float current, float max);
    public event HealthChangeEvent OnHealthChanged;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    /// <summary>
    /// Apply damage to this unit.
    /// </summary>
    /// <param name="damage">Damage amount</param>
    /// <returns>Actually applied damage</returns>
    public float TakeDamage(float damage)
    {
        if (damage >= currentHealth)
        {
            damage = currentHealth;
            currentHealth = maxHealth; // Just reset if dead
        }
        else
        {
            currentHealth -= damage;
        }

        OnHealthChanged(currentHealth, maxHealth);

        return damage;
    }

    /// <summary>
    /// Apply a heal to this unit.
    /// </summary>
    /// <param name="amount">Amount to heal</param>
    /// <returns>Actual heal applied</returns>
    public float Heal(float amount)
    {
        var diff = maxHealth - currentHealth;
        if (diff <= amount)
        {
            amount = diff;
            currentHealth = maxHealth;
        }
        else
        {
            currentHealth += amount;
        }

        OnHealthChanged(currentHealth, maxHealth);

        return amount;
    }


}