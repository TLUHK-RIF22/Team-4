using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompleteScript : MonoBehaviour
{
    public void OnLevelComplete(int starsAquired) 
    {    
        if(LevelSelectionMenuManager.currLevel == LevelSelectionMenuManager.UnlockedLevels)
        {
            LevelSelectionMenuManager.UnlockedLevels++;
            PlayerPrefs.SetInt("UnlockedLevels", LevelSelectionMenuManager.UnlockedLevels);
        }
        if(starsAquired > PlayerPrefs.GetInt("stars" + LevelSelectionMenuManager.currLevel.ToString(), 0))
            PlayerPrefs.SetInt("stars" + LevelSelectionMenuManager.currLevel.ToString(), starsAquired);
        Debug.Log("Current level: " + LevelSelectionMenuManager.currLevel.ToString());
        Debug.Log("Current level: " + LevelSelectionMenuManager.UnlockedLevels.ToString());   
        SceneManager.LoadScene("MenuScene");
    }
    // Start is called before the first frame update
    void Start()
    {
       /*  PlayerPrefs.DeleteAll(); */
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
