using UnityEngine;
using UnityEngine.SceneManagement; // Include this namespace to work with scenes

public class LevelEnd : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider is tagged as "Player"
        if (other.CompareTag("Player")) 
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

