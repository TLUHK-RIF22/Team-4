using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompleteScript : MonoBehaviour
{
    public void OnLevelComplete(int starsAquired) 
    {
        // Update unlocked levels if necessary
        if (LevelSelectionMenuManager.currLevel == LevelSelectionMenuManager.UnlockedLevels)
        {
            LevelSelectionMenuManager.UnlockedLevels++;
            PlayerPrefs.SetInt("UnlockedLevels", LevelSelectionMenuManager.UnlockedLevels);
        }

        // Save stars acquired for the current level if it's higher than the previous value
        int previousStars = PlayerPrefs.GetInt("stars" + LevelSelectionMenuManager.currLevel.ToString(), 0);
        if (starsAquired > previousStars)
        {
            PlayerPrefs.SetInt("stars" + LevelSelectionMenuManager.currLevel.ToString(), starsAquired);
        }

        // Save player data immediately
        PlayerPrefs.Save();

        // Load the menu scene after a short delay (adjust as needed)
        Invoke("LoadMenuScene", 0.5f);
    }

    private void LoadMenuScene()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
