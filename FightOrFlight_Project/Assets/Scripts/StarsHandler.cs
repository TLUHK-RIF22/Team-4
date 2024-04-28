using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarsHandler : MonoBehaviour
{
    public GameObject[] stars;
    private int coinsCount;
    private bool starsAwarded;

    UIHandler UI;

    void Start()
    {
        // Initialize coinsCount once at the start of the level
        coinsCount = GameObject.FindGameObjectsWithTag("coin").Length;
        starsAwarded = false;
    }

    public void StarsAchieved(int starsAquired) 
    {
        // Check if stars are already awarded to prevent re-awarding
        if (starsAwarded)
            return;

        int coinsLeft = GameObject.FindGameObjectsWithTag("coin").Length;
        int coinsCollected = coinsCount - coinsLeft;

        float percentage = coinsCount > 0 ? (float)coinsCollected / coinsCount * 100 : 0; // Check if coinsCount is greater than 0 to avoid division by zero

        // Deactivate all stars first
        foreach (var star in stars)
        {
            star.SetActive(false);
        }

        if (percentage <= 40)
        {
            stars[0].SetActive(true);
            starsAquired = 1;
        }
        else if (percentage >= 40 && percentage < 70)
        {
            stars[0].SetActive(true);
            stars[1].SetActive(true);
            starsAquired = 2;
        }
        else
        {
            stars[0].SetActive(true);
            stars[1].SetActive(true);
            stars[2].SetActive(true);
            starsAquired = 3;
        }

        starsAwarded = true; // Set to true to prevent re-awarding
        PlayerPrefs.SetInt("stars" + LevelSelectionMenuManagar.currLevel.ToString(), starsAquired);
        PlayerPrefs.Save(); // Save PlayerPrefs data
    }

    public void ResetStars()
    {
        // Reset starsAwarded and deactivate all stars
        starsAwarded = false;
        foreach (var star in stars)
        {
            star.SetActive(false);
        }
    }
}
