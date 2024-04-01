using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject LevelSelectMenu;
    public GameObject helpMenu;

    UIHandler UIHandler;
    void Start()
    {
    if (UIHandler.MenuState.ShowLevelSelect)
    {
        ShowLevelSelectMenu();
        UIHandler.MenuState.ShowLevelSelect = false; // Reset the state
    }
}

    public void ShowMainMenu()
    {
        MainMenu.SetActive(true);
        LevelSelectMenu.SetActive(false);
        helpMenu.SetActive(false);
    }

    public void ShowLevelSelectMenu()
    {
        MainMenu.SetActive(false);
        LevelSelectMenu.SetActive(true);
        helpMenu.SetActive(false);
    }

    public void ShowHelpMenu()
    {
        MainMenu.SetActive(false);
        LevelSelectMenu.SetActive(false);
        helpMenu.SetActive(true);
    }
    public void StartGame()
    {
        
        SceneManager.LoadScene("Level" + 1.ToString());
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

