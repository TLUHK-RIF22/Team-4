using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIHandler : MonoBehaviour
{
    private GameObject levelUICanvas;
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

    void Start()
    {
        levelUICanvas = GameObject.Find("LevelUICanvas");
        LevelDialog = levelUICanvas.transform.Find("LevelDialog").gameObject;
        LevelStatus = LevelDialog.transform.Find("LevelStatus").GetComponent<Text>();
        ScoreText = LevelDialog.transform.Find("CoinScore").GetComponent<Text>();
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

    public void BackToMain(int starsAcquired)            
    {
        Time.timeScale = 1f;
        FindObjectOfType<LevelCompleteScript>().OnLevelComplete(starsAcquired);
        MenuState.ShowLevelSelect = true; // Set the state before loading
        SceneManager.LoadScene("MenuScene");
    }

    public void ReplayLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevel(int starsAcquired)
    {
        Time.timeScale = 1f;
        FindObjectOfType<LevelCompleteScript>().OnLevelComplete(starsAcquired);
        Debug.Log("UIH Finished level: " + LevelSelectionMenuManager.levelNum.ToString());
        Debug.Log("UIH Current level: " + LevelSelectionMenuManager.currLevel.ToString());
        Debug.Log("UIH Unlocked level: " + LevelSelectionMenuManager.UnlockedLevels.ToString());
        

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
