using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public Image healthFill;
    public Image shieldFill;

    // Update the health bar fill amount (called from Player's Health script)
    public void UpdateHealth(float currentHealth, float maxHealth)
    {
        healthFill.fillAmount = currentHealth / maxHealth;
    }

    // Update the shield bar fill amount (called from Player's Shield script)
    public void UpdateShield(float currentShield, float maxShield)
    {
        shieldFill.fillAmount = currentShield / maxShield;
    }
}
