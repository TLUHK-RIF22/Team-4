using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Include this namespace to work with scenes

public class LevelEnd : MonoBehaviour
{   
    private Text ScoreText;
    
    void Start()
    {
        ScoreText = GameObject.Find("StarText").GetComponent<Text>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider is tagged as "Player"
        if (other.gameObject.tag == "Player") 
        {
            EndLevel();
        }
    }

    public void EndLevel()
    {
        // Load the menu scene
        Time.timeScale = 0f;
        UIHandler.instance.ShowLevelDialog("Tase läbitud!", ScoreText.text);
    }
}
