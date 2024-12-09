using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMode
{
    Trios,
    Duos,
    FiringRange
}

public static class SelectedGamemode
{

    public static string GetGamemode(this GameMode gamemode)
    {
        switch (gamemode)
        {
            case GameMode.Trios: return "Trios";
            case GameMode.Duos: return "Duos";
            case GameMode.FiringRange: return "Firing Range";
            default: return "Unknown Gamemode";
        }
    }

}
