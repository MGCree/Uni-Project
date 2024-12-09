using UnityEngine;
using Unity.Services.Lobbies.Models;
using System.Collections.Generic;

public class Lobby3DVisualizer : MonoBehaviour
{
    public Transform centerPosition;  // Reference to center position in the scene
    public Transform leftPosition;    // Reference to left position
    public Transform rightPosition;   // Reference to right position

    public GameObject characterPrefab; // The player character prefab (Temporary)
    private Dictionary<string, GameObject> playerInstances = new Dictionary<string, GameObject>();

    private Lobby currentLobby;

    public void UpdateLobbyDisplay(Lobby lobby)
    {
        currentLobby = lobby;

        // Clear previous instances
        foreach (var instance in playerInstances.Values)
        {
            DestroyImmediate(instance);
        }
        playerInstances.Clear();

        // Instantiate characters based on their order in the lobby
        for (int i = 0; i < currentLobby.Players.Count; i++)
        {
            var player = currentLobby.Players[i];
            Transform spawnPoint = GetPlayerPosition(i);
            SpawnCharacter(player, spawnPoint);
        }
    }

    private Transform GetPlayerPosition(int playerIndex)
    {
        if (playerIndex == 0) return centerPosition; // Lobby owner in the center
        else if (playerIndex == 1) return leftPosition;
        else return rightPosition;
    }

    private void SpawnCharacter(Player player, Transform spawnPoint)
    {
        // Instantiate the character and place it at the correct position
        GameObject characterInstance = Instantiate(characterPrefab, spawnPoint.position, spawnPoint.rotation);
        playerInstances.Add(player.Id, characterInstance);
    }
}
