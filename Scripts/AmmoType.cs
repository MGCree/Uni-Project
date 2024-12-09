using UnityEngine;

public enum AmmoType
{
    LightAmmo,
    HeavyAmmo,
    SniperAmmo,
}

public static class AmmoTypeExtensions
{
    public static string GetFriendlyName(this AmmoType ammo)
    {
        switch (ammo)
        {
            case AmmoType.LightAmmo: return "Pistol Ammo";
            case AmmoType.HeavyAmmo: return "Rifle Ammo";
            case AmmoType.SniperAmmo: return "Sniper Rounds";
            default: return "Unknown Ammo";
        }
    }
}
