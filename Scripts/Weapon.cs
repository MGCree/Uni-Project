// A base class for all weapons

using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Weapon Info")]
    public string weaponName;           // Name of the weapon
    public float damage;                // Base damage of the weapon

    [Header("Ammo Settings")]
    public int maxAmmo;                 // Max ammo the weapon can hold
    public int currentAmmo;             // Current ammo in the weapon
    public AmmoType ammoType;           // Type of ammo used by this weapon

    [Header("Rarity Settings")]
    public RarityTier rarity;           // Rarity of the weapon

    public virtual void UseWeapon()
    {
        if (currentAmmo > 0)
        {
            currentAmmo--;
            Debug.Log($"{weaponName} fired. Ammo left: {currentAmmo}");
        }
        else
        {
            Debug.Log($"{weaponName} is out of ammo! Reload required.");
        }
    }

    public void Reload()
    {
        currentAmmo = maxAmmo;
        Debug.Log($"{weaponName} reloaded with {ammoType.GetFriendlyName()}.");
    }

    void Start()
    {
        Debug.Log($"{weaponName} - Rarity: {rarity} - Uses: {ammoType.GetFriendlyName()}");
    }
}
