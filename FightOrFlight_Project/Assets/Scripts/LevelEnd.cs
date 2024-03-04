using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Include this namespace to work with scenes

public class LevelEnd : MonoBehaviour
{
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
        SceneManager.LoadScene("MenuScene"); // Make sure "MenuScene" matches the exact name of your menu scene
    }
}
