using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIHandler : MonoBehaviour
{
    public GameObject LevelDialog;
    public Text LevelStatus;
    public Text ScoreText;
    CoinManager cm;
    MenuManager mm;

    public static UIHandler instance;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void ShowLevelDialog(string status, string score)
    {
        GetComponent<StarsHandler>().StarsAchieved(0);
        LevelDialog.SetActive(true);
        LevelStatus.text = status;
        ScoreText.text = score;
    }
    public static class MenuState
{
    public static bool ShowLevelSelect = false;
}

    public void BackToMain()            
    {
        MenuState.ShowLevelSelect = true; // Set the state before loading
        SceneManager.LoadScene("MenuScene");
    }

    public void ReplayLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevel(int starsAcquired)
{
    FindObjectOfType<LevelCompleteScript>().OnLevelComplete(starsAcquired);
    Debug.Log("Finished level: " + LevelSelectionMenuManager.levelNum.ToString());
    Debug.Log("Current level: " + LevelSelectionMenuManager.currLevel.ToString());

    // Increment the level number
    LevelSelectionMenuManager.levelNum++;

    // Load the next level if it exists
    int nextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;
    if (nextLevelIndex < SceneManager.sceneCountInBuildSettings)
    {
        SceneManager.LoadScene("Level" + LevelSelectionMenuManager.levelNum.ToString());
        Debug.Log("Loading next level: Level" + LevelSelectionMenuManager.levelNum.ToString());
    }
    else
    {
        Debug.LogWarning("No more levels available.");
    }
}

}
