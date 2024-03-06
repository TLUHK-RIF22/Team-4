using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject levelSelectMenu;
    public GameObject helpMenu;

    void Start()
    {
        ShowMainMenu(); // Show main menu by default
    }

    public void ShowMainMenu()
    {
        MainMenu.SetActive(true);
        levelSelectMenu.SetActive(false);
        helpMenu.SetActive(false);
    }

    public void ShowLevelSelectMenu()
    {
        MainMenu.SetActive(false);
        levelSelectMenu.SetActive(true);
        helpMenu.SetActive(false);
    }

    public void ShowHelpMenu()
    {
        MainMenu.SetActive(false);
        levelSelectMenu.SetActive(false);
        helpMenu.SetActive(true);
    }
    public void StartGame()
    {
        
        SceneManager.LoadScene("GameScene");
    }
    // Add this if you have a back button in your LevelSelect and Help Menus
    public void GoBack()
    {
        ShowMainMenu();
    }

        public void QuitGame()
    {
        // Quit the application
        Application.Quit();

        // If running in the Unity editor, log quitting for clarity
        #if UNITY_EDITOR
        Debug.Log("Game is exiting...");
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}

