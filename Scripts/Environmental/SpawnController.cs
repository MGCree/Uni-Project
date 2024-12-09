using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnController : MonoBehaviour
{
    [Header("Spawning Setup")]
    public GameObject playerPrefab; // Prefab for the player
    public GameObject botPrefab; // Prefab for the bots
    public Transform[] spawnPoints; // Array of spawner positions

    private GameObject playerInstance; // To keep track of the player
    private List<GameObject> botInstances = new List<GameObject>(); // To track the bots

    [Header("Spawn Timings")]
    public float initialDelay = 60f; // Time before the next wave of bots
    public int botsPerWave = 1; // Number of bots to spawn at each spawner after a wave

    void Start()
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points assigned!");
            return;
        }

        StartCoroutine(InitializeSpawning());
    }

    IEnumerator InitializeSpawning()
    {
        // Spawn the player at a random spawn point
        SpawnPlayer();

        // Spawn one bot at each of the remaining spawn points
        SpawnInitialBots();

        yield return new WaitForSeconds(initialDelay);

        StartCoroutine(SpawnBotWaves());
    }

    void SpawnPlayer()
    {
        int playerSpawnIndex = Random.Range(0, spawnPoints.Length);
        playerInstance = Instantiate(playerPrefab, spawnPoints[playerSpawnIndex].position, Quaternion.identity);
        Debug.Log("Player spawned at: " + spawnPoints[playerSpawnIndex].position);
    }

    void SpawnInitialBots()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            // Skip the spawn point where the player was spawned
            if (playerInstance != null && spawnPoints[i].position == playerInstance.transform.position)
                continue;

            GameObject bot = Instantiate(botPrefab, spawnPoints[i].position, Quaternion.identity);
            botInstances.Add(bot);
            Debug.Log("Bot spawned at: " + spawnPoints[i].position);
        }
    }

    IEnumerator SpawnBotWaves()
    {
        while (true)
        {
            // Spawn bots at all spawn points
            foreach (Transform spawnPoint in spawnPoints)
            {
                GameObject bot = Instantiate(botPrefab, spawnPoint.position, Quaternion.identity);
                botInstances.Add(bot);
                Debug.Log("New bot spawned at: " + spawnPoint.position);
            }

            yield return new WaitForSeconds(initialDelay); // Wait for the next wave
        }
    }

    void Update()
    {
        CheckForGameEnd();
    }

    void CheckForGameEnd()
    {
        // Check if the player is dead
        if (playerInstance == null)
        {
            Debug.Log("Player is dead! Returning to Main Menu...");
            ReturnToMainMenu();
        }

        // Check if all bots are dead
        botInstances.RemoveAll(bot => bot == null); // Remove destroyed bots from the list
        if (botInstances.Count == 0)
        {
            Debug.Log("All bots are dead! Returning to Main Menu...");
            ReturnToMainMenu();
        }
    }

    void ReturnToMainMenu()
    {
        // Load the main menu scene (This will be improved at a later date)
        SceneManager.LoadScene("MainMenu");
    }
}
