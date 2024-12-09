/*
    This is a general script that will be on all player objects
*/

using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("Shield Settings")]
    public float maxShield = 50f;
    private float currentShield;

    private PlayerUI playerUI; // Reference to the Player UI script

    void Start()
    {
        currentHealth = maxHealth;
        currentShield = maxShield;

        // Find the PlayerUI in the scene
        playerUI = FindObjectOfType<PlayerUI>();

        // Initialize UI values
        if (playerUI != null)
        {
            playerUI.UpdateHealth(currentHealth, maxHealth);
            playerUI.UpdateShield(currentShield, maxShield);
        }
    }

    public void TakeDamage(float amount)
    {
        if (currentShield > 0)
        {
            float damageToShield = Mathf.Min(amount, currentShield);
            currentShield -= damageToShield;
            amount -= damageToShield;

            // Update shield UI
            playerUI?.UpdateShield(currentShield, maxShield);
        }

        if (amount > 0)  // Apply remaining damage to health
        {
            currentHealth -= amount;
            // Update health UI
            playerUI?.UpdateHealth(currentHealth, maxHealth);
        }

        if (currentHealth <= 0f)
        {
            Die();
        }

        Debug.Log($"Health: {currentHealth}, Shield: {currentShield}");
    }

    void Die()
    {
        Debug.Log($"{gameObject.name} died.");
        Destroy(gameObject);
    }
}
