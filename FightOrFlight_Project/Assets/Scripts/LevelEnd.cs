using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Include this namespace to work with scenes

public class LevelEnd : MonoBehaviour
{   
    public Text ScoreText;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider is tagged as "Player"
        if (other.gameObject.tag == "Player") 
        {
            EndLevel();
        }
    }

    void EndLevel()
    {
        // Load the menu scene
        UIHandler.instance.ShowLevelDialog("Level Complete", ScoreText.text);
    }
}
