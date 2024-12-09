using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    [Header("Weapon Settings")]
    public Weapon[] weapons;  // Array to hold the player's weapons
    public int currentWeaponIndex = 0; // Index of the currently equipped weapon

    [Header("Character Settings")]
    public bool canCarryThreeWeapons = false; // Special character setting

    private void Start()
    {
        // Equip the first weapon at the start of the game
        EquipWeapon(0);
    }

    private void Update()
    {
        // Switch weapon based on input (1, 2, or 3)
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EquipWeapon(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            EquipWeapon(1);
        }
        if (canCarryThreeWeapons && Input.GetKeyDown(KeyCode.Alpha3))
        {
            EquipWeapon(2);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReloadCurrentWeapon();
        }

        // Scroll to switch weapons
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f)
        {
            SwitchWeapon(1); // Scroll up (next weapon)
        }
        if (scroll < 0f)
        {
            SwitchWeapon(-1); // Scroll down (previous weapon)
        }
    }

    // Equip the selected weapon by index
    void EquipWeapon(int index)
    {
        if (index >= 0 && index < weapons.Length)
        {
            currentWeaponIndex = index;
            Debug.Log($"Equipped: {weapons[currentWeaponIndex].weaponName}");
        }
    }

    // Switch weapon by the given direction
    void SwitchWeapon(int direction)
    {
        int newIndex = (currentWeaponIndex + direction) % weapons.Length;
        if (newIndex < 0) newIndex = weapons.Length - 1;
        EquipWeapon(newIndex);
    }

    // Get the current equipped weapon
    public Weapon GetCurrentWeapon()
    {
        return weapons[currentWeaponIndex];
    }

    // Optionally, trigger reload of the current weapon
    public void ReloadCurrentWeapon()
    {
        Weapon currentWeapon = GetCurrentWeapon();
        if (currentWeapon is Pistol pistol)
        {
            pistol.ReloadPistol();
        }
        else
        {
            // Reload other types of weapons if necessary
            currentWeapon.Reload();
        }
    }
}
