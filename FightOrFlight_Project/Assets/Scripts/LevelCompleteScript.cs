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
            Debug.Log("LCS if1 ");   
            LevelSelectionMenuManager.UnlockedLevels++;
            
            PlayerPrefs.SetInt("UnlockedLevels", LevelSelectionMenuManager.UnlockedLevels);
        }
        if(starsAquired > PlayerPrefs.GetInt("stars" + LevelSelectionMenuManager.currLevel.ToString(), 0))
            PlayerPrefs.SetInt("stars" + LevelSelectionMenuManager.currLevel.ToString(), starsAquired);
            Debug.Log("LCS if2 ");
        Debug.Log("LCS Current levelBe: " + LevelSelectionMenuManager.currLevel.ToString());    
        LevelSelectionMenuManager.currLevel++;    
        Debug.Log("LCS Current levelAf: " + LevelSelectionMenuManager.currLevel.ToString());
        Debug.Log("LCS Unlocked level: " + LevelSelectionMenuManager.UnlockedLevels.ToString());   
        SceneManager.LoadScene("MenuScene");
    }
    // Start is called before the first frame update
    void Start()
    {
        /*  PlayerPrefs.DeleteAll();  */ 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
