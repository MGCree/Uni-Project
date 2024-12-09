using Unity.Services.Friends;
using Unity.Services.Friends.Notifications;
using UnityEngine;
using TMPro;

public class InvitationPanel : MonoBehaviour
{
    private PartyManager partyManager; // Reference to the PartyManager
    public GameObject InvitationalPanel;

    public TextMeshProUGUI DisplayName;

    private string Username;
    private string LobbyCode;

    void Start()
    {
        // Find or assign the PartyManager instance
        partyManager = FindObjectOfType<PartyManager>();

        FriendsService.Instance.MessageReceived += OnInviteReceived;
    }

    // Handler for the invite message
    private void OnInviteReceived(IMessageReceivedEvent messageEvent)
    {
        // Attempt to deserialize the message into an InviteMessage object
        var inviteMessage = messageEvent.GetAs<InviteMessage>();

        if (inviteMessage != null && !string.IsNullOrEmpty(inviteMessage.LobbyCode))
        {
            Debug.Log("Invite message received with lobby code: " + inviteMessage.LobbyCode);

            Username = inviteMessage.DisplayName;
            LobbyCode = inviteMessage.LobbyCode;

            InvitationalPanel.SetActive(true);
            DisplayName.text = Username;
        }
        else
        {
            Debug.LogError("Failed to parse invite message or no lobby code found.");
        }
    }

    public async void Accept()
    {
        await partyManager.AcceptInviteAsync(LobbyCode);
        InvitationalPanel.SetActive(false);
    }

    public void Reject()
    {
        partyManager.RejectInvite();
        InvitationalPanel.SetActive(false);
    }

    // Cleanup: Unsubscribe from the event when the panel is destroyed
    private void OnDestroy()
    {
        if (FriendsService.Instance != null)
        {
            FriendsService.Instance.MessageReceived -= OnInviteReceived;
        }
    }
}