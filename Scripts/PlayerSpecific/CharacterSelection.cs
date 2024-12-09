using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    // Function to set the selected character based on button input
    public void SetSelectedCharacter(string characterName)
    {
        // Parse the button name to enum and set it in PlayerInfo
        if (System.Enum.TryParse(characterName, out CharacterName selectedCharacter))
        {
            PlayerInfo.Instance.SetSelectedCharacter(selectedCharacter);
            Debug.Log("Selected character: " + selectedCharacter);
        }
        else
        {
            Debug.LogError("Invalid character name: " + characterName);
        }
    }
}
