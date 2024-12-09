using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Services.Friends;
using System.Threading.Tasks;
using Unity.Services.Friends.Exceptions;
using Unity.Services.Friends.Models;
using System.Linq;

public class InviteButton : MonoBehaviour
{
    private GameObject MultiController;
    public TextMeshProUGUI displayName;

    private void Start()
    {
        MultiController = GameObject.FindGameObjectWithTag("MultiplayerController");
    }

    public async void InviteFriend()
    {
        // Get the friend's ID based on the display name from the UI text.
        string ID = GetFriendIdByDisplayName(displayName.text);

        // If a valid ID is returned, send an invite to that friend.
        if (!string.IsNullOrEmpty(ID))
        {
            Debug.Log("Sending invite to: " + ID);
            await MultiController.GetComponent<PartyManager>().SendInviteAsync(displayName.text, ID);
        }
        else
        {
            Debug.LogError("Friend ID not found.");
        }
    }

    public string GetFriendIdByDisplayName(string displayName)
    {
        try
        {
            // Fetch the list of friends.
            var friendsList = FriendsService.Instance.Friends;

            // Get non-blocked friends from the list.
            var nonBlockedFriends = GetNonBlockedMembers(friendsList);

            // Search for the friend with the specified display name.
            var friend = nonBlockedFriends.FirstOrDefault(f => f.Profile.Name == displayName);

            if (friend != null)
            {
                Debug.Log($"Found Friend: {friend.Profile.Name}, User ID: {friend.Id}");
                return friend.Id;  // Return the ID if the friend is found.
            }
            else
            {
                Debug.LogError("Friend not found in non-blocked list.");
                return null;  // Return null if no friend with the display name is found.
            }
        }
        catch (FriendsServiceException e)
        {
            Debug.LogError($"Error retrieving friends: {e.Message}");
            return null;
        }
    }

    private List<Member> GetNonBlockedMembers(IReadOnlyList<Relationship> relationships)
    {
        // Get the list of blocked members.
        var blocks = FriendsService.Instance.Blocks;

        // Filter out blocked members and return a list of non-blocked members.
        return relationships
               .Where(relationship =>
                   !blocks.Any(blockedRelationship => blockedRelationship.Member.Id == relationship.Member.Id))
               .Select(relationship => relationship.Member)
               .ToList();
    }
}
