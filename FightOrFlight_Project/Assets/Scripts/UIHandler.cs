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
        GetComponent<StarsHandler>().starsAcheived();
        LevelDialog.SetActive(true);
        LevelStatus.text = status;
        ScoreText.text = score;
    }

    public void BackTomain()
    {
        SceneManager.LoadScene("MenuScene");
        mm.ShowLevelSelectMenu();
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
