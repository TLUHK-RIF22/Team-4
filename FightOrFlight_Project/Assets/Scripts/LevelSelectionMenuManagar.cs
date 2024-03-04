using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectionMenuManagar : MonoBehaviour
{   
    public LevelObject[] levelObjects;
    public Sprite goldenStarSprite;
    public static int currLevel;
    public static int UnlockedLevels;
    public void OnClickLevel(int levelNum)
    {
        currLevel = levelNum;
        SceneManager.LoadScene("GameScene");
    }
    public void OnClickBack()
    {
        this.gameObject.SetActive(false);
    }  
    // Start is called before the first frame update
    void Start()
    {
        UnlockedLevels = PlayerPrefs.GetInt("UnlockedLevels", 0);
        for(int i = 0; i < levelObjects.Length; i++)
        {
            if(i <= UnlockedLevels)
            {
                levelObjects[i].levelButton.interactable = true;
                int stars = PlayerPrefs.GetInt("stars" + i.ToString(), 0);
                for(int j = 0; j < stars; j++)
                {
                    levelObjects[i].stars[j].sprite = goldenStarSprite;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
