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
        GetComponent<StarsHandler>().StarsAchieved();
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

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
