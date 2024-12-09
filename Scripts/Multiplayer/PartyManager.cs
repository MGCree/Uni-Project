using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Authentication;
using Unity.Services.Friends;
using UnityEngine;
using System.Threading.Tasks;

public class PartyManager : MonoBehaviour
{
    private Lobby currentLobby;

    private Lobby3DVisualizer Lobby3DVisualizer;

    void Start()
    {
        Lobby3DVisualizer = FindObjectOfType<Lobby3DVisualizer>();
    }

    private async void StartLobbyPolling()
    {
        while (currentLobby != null)
        {
            // Fetch the latest lobby state
            currentLobby = await LobbyService.Instance.GetLobbyAsync(currentLobby.Id);

            // Update the 3D lobby display
            Lobby3DVisualizer.UpdateLobbyDisplay(currentLobby);

            // Wait before polling again (e.g., every 5 seconds)
            await Task.Delay(5000);
        }
    }


    // Create a private (invite-only) lobby (party)
    public async Task CreateLobbyAsync(string lobbyName, int maxPlayers)
    {
        try
        {
            // Get the current player's ID
            var currentPlayer = new Unity.Services.Lobbies.Models.Player(AuthenticationService.Instance.PlayerId);

            // Set the lobby options (private, current player is the first player)
            var createOptions = new CreateLobbyOptions
            {
                IsPrivate = true, // Make the lobby private (invite-only)
                Player = currentPlayer,
            };

            // Create the lobby
            currentLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, createOptions);
            Debug.Log($"Lobby created: {currentLobby.Name}, ID: {currentLobby.Id}");
            StartLobbyPolling();
        }
        catch (LobbyServiceException e)
        {
            Debug.LogError($"Failed to create lobby: {e.Message}");
        }
    }

    // Send an invite to a friend 
    public async Task SendInviteAsync(string displayName, string friendId)
    {
        // Check if the lobby exists
        if (currentLobby == null)
        {
            Debug.Log("No lobby found. Creating a new lobby...");

            // Create a new lobby with default settings
            string defaultLobbyName = "DefaultLobby_" + AuthenticationService.Instance.PlayerId; // Unique name for the lobby
            int defaultMaxPlayers = 3; 

            await CreateLobbyAsync(defaultLobbyName, defaultMaxPlayers);

            // Check if the lobby creation succeeded
            if (currentLobby == null)
            {
                Debug.LogError("Failed to create a lobby.");
                return;
            }
        }

        // Proceed to send the invite
        try
        {
            // Create an InviteMessage object containing the lobby code
            var inviteMessage = new InviteMessage
            {
                DisplayName = displayName,
                LobbyCode = currentLobby.LobbyCode
            };

            // Send the invite message to the friend (this sends the lobby code as a message)
            await FriendsService.Instance.MessageAsync(friendId, inviteMessage);
            Debug.Log($"Invite sent to {friendId}");
        }
        catch (LobbyServiceException e)
        {
            Debug.LogError($"Failed to send invite: {e.Message}");
        }
    }

    // Accept an invite (join a lobby by its code)
    public async Task AcceptInviteAsync(string Code)
    {
        try
        {
            // Join the lobby using the extracted lobby code
            currentLobby = await LobbyService.Instance.JoinLobbyByCodeAsync(Code);
            Debug.Log($"Joined the lobby: {currentLobby.Name}, ID: {currentLobby.Id}");
            StartLobbyPolling();
        }
        catch (LobbyServiceException e)
        {
            Debug.LogError($"Failed to join lobby: {e.Message}");
        }
    }

    public void RejectInvite()
    {
        Debug.Log("Invite rejected.");
    }

    // Leave the current party (lobby)
    public async Task LeavePartyAsync()
    {
        if (currentLobby == null)
        {
            Debug.LogError("No lobby to leave.");
            return;
        }

        try
        {
            // Remove the player from the lobby
            await LobbyService.Instance.RemovePlayerAsync(currentLobby.Id, AuthenticationService.Instance.PlayerId);
            currentLobby = null;
            Debug.Log("Left the lobby.");
        }
        catch (LobbyServiceException e)
        {
            Debug.LogError($"Failed to leave lobby: {e.Message}");
        }
    }

    // This function can be called when the host wants to remove a player from the lobby (as the party leader)
    public async Task RemovePlayerFromPartyAsync(string playerId)
    {
        if (currentLobby == null)
        {
            Debug.LogError("No lobby to remove player from.");
            return;
        }

        try
        {
            // Remove the player by ID
            await LobbyService.Instance.RemovePlayerAsync(currentLobby.Id, playerId);
            Debug.Log($"Player {playerId} removed from the lobby.");
        }
        catch (LobbyServiceException e)
        {
            Debug.LogError($"Failed to remove player from lobby: {e.Message}");
        }
    }

    // Transfer the host role to another player (only the current host can do this)
    public async Task<bool> TransferHostAsync(string newHostPlayerId)
    {
        if (currentLobby == null)
        {
            Debug.LogError("No lobby available to transfer host.");
            return false;
        }

        if (currentLobby.HostId != AuthenticationService.Instance.PlayerId)
        {
            Debug.LogError("Only the current host can transfer the host role.");
            return false;
        }

        try
        {
            // Set the new host
            var updateOptions = new UpdateLobbyOptions
            {
                HostId = newHostPlayerId
            };
            currentLobby = await LobbyService.Instance.UpdateLobbyAsync(currentLobby.Id, updateOptions);
            Debug.Log($"Host role transferred to {newHostPlayerId}");
            return true;
        }
        catch (LobbyServiceException e)
        {
            Debug.LogError($"Failed to transfer host: {e.Message}");
            return false;
        }
    }

    // Helper function to extract the lobby code from the invite message
    private string ExtractLobbyCodeFromMessage(string message)
    {
        // Assuming the lobby code is included in the message in the format:
        // Not in use for now
        string keyword = "Use this lobby code: ";
        int startIndex = message.IndexOf(keyword);
        if (startIndex >= 0)
        {
            return message.Substring(startIndex + keyword.Length);
        }
        return null;
    }
}

[System.Serializable]
public class InviteMessage
{
    public string DisplayName;
    public string LobbyCode; // Store the lobby code
}
