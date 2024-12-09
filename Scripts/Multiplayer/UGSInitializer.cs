using Unity.Services.Core;
using Unity.Services.Authentication;
using UnityEngine;
using Unity.Services.Friends;

public class UGSInitializer : MonoBehaviour
{
    async void Start()
    {
        await UnityServices.InitializeAsync();
        await FriendsService.Instance.InitializeAsync();
    }
}
