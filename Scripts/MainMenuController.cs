using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services.Authentication.PlayerAccounts;
using Unity.Services.Authentication;

public class MainMenuController : MonoBehaviour
{
   
    public void PlayGame()
    {
       
        SceneManager.LoadScene("DevScene"); 
    }

    public void SignOut()
    {
        PlayerAccountService.Instance.SignOut();
        AuthenticationService.Instance.SignOut();


        

        SceneManager.LoadScene(0);
    }

    public void PlayBotBattle()
    {
        SceneManager.LoadScene("Boros");
    }

    public void ExitGame()
    {
        
        Debug.Log("Exit button clicked.");
        Application.Quit();

        
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
