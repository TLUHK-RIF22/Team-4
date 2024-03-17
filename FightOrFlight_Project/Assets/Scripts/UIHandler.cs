using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    public GameObject LevelDialog;
    public Text LevelStatus;
    public Text ScoreText;
    CoinManager cm;

    public static UIHandler instance;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void ShowLevelDialog(string status, string score)
    {
        LevelDialog.SetActive(true);
        LevelStatus.text = status;
        ScoreText.text = score;
    }

}
