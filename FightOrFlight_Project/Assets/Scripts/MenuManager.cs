using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject LevelSelectMenu;
    public GameObject helpMenu;
    public GameObject SettingsMenu;

    UIHandler UIHandler;
    void Start()
    {
    ShowMainMenu();
    
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
        SettingsMenu.SetActive(false);
    }

    public void ShowLevelSelectMenu()
    {
        MainMenu.SetActive(false);
        LevelSelectMenu.SetActive(true);
        helpMenu.SetActive(false);
        SettingsMenu.SetActive(false);
    }

    public void ShowHelpMenu()
    {
        SceneManager.LoadScene("TutorialScene"); 
    }
     public void ShowSettingsMenu()
    {
        MainMenu.SetActive(false);
        LevelSelectMenu.SetActive(false);
        helpMenu.SetActive(false);
        SettingsMenu.SetActive(true); // Only the settings menu is visible
    }
    public void StartGame()
    {
        LevelSelectionMenuManager.levelNum = 1; // Reset the level number
        LevelSelectionMenuManager.currLevel = 1; // Reset the current level
        SceneManager.LoadScene("Level" + 1.ToString());
        Debug.Log("MM Loading levelNum: " + LevelSelectionMenuManager.levelNum);
        Debug.Log("MM CurrLelvel: " + LevelSelectionMenuManager.currLevel);
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

    public void UnlockAllLevels()
    {
        PlayerPrefs.SetInt("UnlockedLevels", 10);
        PlayerPrefs.Save();
    }
}

