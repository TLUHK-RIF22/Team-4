using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompleteScript : MonoBehaviour
{
    public void OnLevelComplete(int starsAquired) 
    {
        if(LevelSelectionMenuManagar.currLevel == LevelSelectionMenuManagar.UnlockedLevels)
        {
            LevelSelectionMenuManagar.UnlockedLevels++;
            PlayerPrefs.SetInt("UnlockedLevels", LevelSelectionMenuManagar.UnlockedLevels);
        }
        if(starsAquired > PlayerPrefs.GetInt("stars" + LevelSelectionMenuManagar.currLevel.ToString(), 0))
            PlayerPrefs.SetInt("stars" + LevelSelectionMenuManagar.currLevel.ToString(), starsAquired);
        SceneManager.LoadScene("MenuScene");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
