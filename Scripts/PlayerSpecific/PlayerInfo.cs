/*
    This took way too long to implement
*/

using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    // Singleton instance
    public static PlayerInfo Instance { get; private set; }

    // Currently selected character
    public CharacterName SelectedCharacter { get; private set; }

    // Awake method to implement the singleton pattern
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist between scenes
        }
        else
        {
            Destroy(gameObject); // Ensure only one instance
        }

        LoadPlayerData(); // Load player data on startup
    }

    // Function to set selected character and save it to PlayerPrefs
    public void SetSelectedCharacter(CharacterName character)
    {
        SelectedCharacter = character;
        PlayerPrefs.SetString("SelectedCharacter", character.ToString()); // Save character selection
        PlayerPrefs.Save();
    }

    // Function to load player data
    void LoadPlayerData()
    {
        if (PlayerPrefs.HasKey("SelectedCharacter"))
        {
            string characterName = PlayerPrefs.GetString("SelectedCharacter");
            if (System.Enum.TryParse(characterName, out CharacterName loadedCharacter))
            {
                SelectedCharacter = loadedCharacter;
            }
            else
            {
                SelectedCharacter = CharacterName.Ajax; // Default character
            }
        }
        else
        {
            SelectedCharacter = CharacterName.Ajax; // Default character
            PlayerPrefs.SetString("SelectedCharacter", SelectedCharacter.ToString());
            PlayerPrefs.Save();
        }
    }

    //Function to get the character name as a string
    public string GetCharacterName()
    {
        return SelectedCharacter.ToString();
    }
}
