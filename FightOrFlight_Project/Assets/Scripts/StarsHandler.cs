using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarsHandler : MonoBehaviour
{
    public GameObject[] stars;
    private int coinsCount;
    private bool starsAwarded;

    void Start()
    {
        // Initialize coinsCount once at the start of the level
        coinsCount = GameObject.FindGameObjectsWithTag("coin").Length;
        starsAwarded = false;
    }

    public void StarsAchieved() 
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
        }
        else if (percentage >= 40 && percentage < 70)
        {
            stars[0].SetActive(true);
            stars[1].SetActive(true);
        }
        else
        {
            stars[0].SetActive(true);
            stars[1].SetActive(true);
            stars[2].SetActive(true);
        }

        starsAwarded = true; // Set to true to prevent re-awarding
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
