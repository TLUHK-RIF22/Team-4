using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectionMenuManager : MonoBehaviour
{   
    public LevelObject[] levelObjects;
    public Sprite goldenStarSprite;
    public static int currLevel;
    public static int levelNum; // This should be reset after loading a level
    public static int UnlockedLevels;

    public void OnClickLevel(int levelIndex)
    {
        levelNum = levelIndex; // Update levelNum based on the selected level
        currLevel = levelIndex; // Update currLevel (zero-based index)
        SceneManager.LoadScene("Level" + levelNum.ToString());
        Debug.Log("Loading curr: " + currLevel);
        Debug.Log("Loading level: " + levelNum);
    }

    public void OnClickBack()
    {
        SceneManager.LoadScene("MenuScene");
    }  

    void Start()
    {
        UnlockedLevels = PlayerPrefs.GetInt("UnlockedLevels", 0);
        for (int i = 0; i < levelObjects.Length; i++)
        {
            if (i <= UnlockedLevels)
            {
                levelObjects[i].levelButton.interactable = true;
                int stars = PlayerPrefs.GetInt("stars" + i.ToString(), 0);
                for (int j = 0; j < stars; j++)
                {
                    levelObjects[i].stars[j].sprite = goldenStarSprite;
                }
            }
        }
    }
}
