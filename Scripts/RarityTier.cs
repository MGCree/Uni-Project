using UnityEngine;

public enum RarityTier
{
    Common,
    Uncommon,
    Rare,
    Legendary,
    Mythic
}

// The below functions are for the future and are not used right now as the prefabs are not ready yet

public static class RarityTierExtensions
{
    public static Color GetColor(this RarityTier rarity)
    {
        switch (rarity)
        {
            case RarityTier.Common: return Color.white;
            case RarityTier.Uncommon: return Color.green;
            case RarityTier.Rare: return Color.blue;
            case RarityTier.Legendary: return Color.yellow;
            case RarityTier.Mythic: return Color.magenta;
            default: return Color.gray;
        }
    }

    public static float GetWeight(this RarityTier rarity)
    {
        switch (rarity)
        {
            case RarityTier.Common: return 50f;
            case RarityTier.Uncommon: return 30f;
            case RarityTier.Rare: return 15f;
            case RarityTier.Legendary: return 4f;
            case RarityTier.Mythic: return 1f;
            default: return 1f;
        }
    }
}
