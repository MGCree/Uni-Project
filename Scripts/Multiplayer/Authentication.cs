/*
    This script mostly uses functions from the unity docs to call their api
*/

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Core;
using UnityEngine;
using Unity.Services.Authentication.PlayerAccounts;
using Unity.Services.Authentication;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class Authentication : MonoBehaviour
{

    async void Awake()
    {
        var options = new InitializationOptions();
        options.SetProfile("default_profile");

        await UnityServices.InitializeAsync();
        await SignInCachedUserAsync();
    }

    public async void StartSignInAsync()
    {
        if (PlayerAccountService.Instance.IsSignedIn)
        {
            SignInWithUnity();
            SceneManager.LoadScene("MainMenu");
            return;
        }

        try
        {
            await PlayerAccountService.Instance.StartSignInAsync();
        }
        catch (RequestFailedException ex)
        {
            Debug.LogException(ex);
            SetException(ex);
        }
    }

    public void SignOut()
    {
        AuthenticationService.Instance.SignOut();

        
        PlayerAccountService.Instance.SignOut();
    }

    public void OpenAccountPortal()
    {
        Application.OpenURL(PlayerAccountService.Instance.AccountPortalUrl);
    }

    async void SignInWithUnity()
    {
        try
        {
            await AuthenticationService.Instance.SignInWithUnityAsync(PlayerAccountService.Instance.AccessToken);
            SceneManager.LoadScene("MainMenu");
        }
        catch (RequestFailedException ex)
        {
            Debug.LogException(ex);
            SetException(ex);
        }
    }

    async Task SignInCachedUserAsync()
    {
        if (AuthenticationService.Instance.SessionTokenExists)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            SignInWithUnity();
        }
    }

    void SetException(Exception ex)
    {
        Debug.LogError(ex);
    }

}
