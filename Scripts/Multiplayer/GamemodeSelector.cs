using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamemodeSelector : MonoBehaviour
{
    public GameMode gamemode;

    void Start()
    {
        gamemode = GameMode.Trios;
    }

    public void SetTrios()
    {
        gamemode = GameMode.Trios;
    }

    public void SetDuos()
    {
        gamemode = GameMode.Duos;
    }

    public void SetFiringRange()
    {
        gamemode = GameMode.FiringRange;
    }
}
