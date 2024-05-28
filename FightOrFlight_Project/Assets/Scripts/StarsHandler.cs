using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarsHandler : MonoBehaviour
{
    public GameObject[] stars;
    private int coinsCount;
    private int starsAquired;
    private bool starsAwarded;
    private GameObject levelUICanvas;

    UIHandler UI;

    void Start()
    {   
        // Find StarsHolder and store all stars in an array
        levelUICanvas = GameObject.Find("LevelUICanvas");
        GameObject levelDialog = levelUICanvas.transform.Find("LevelDialog").gameObject;
        GameObject starsHolder = levelDialog.transform.Find("StarsHolder").gameObject;
        foreach (Transform star in starsHolder.transform)
        {
            stars[star.GetSiblingIndex()] = star.gameObject;
        }


        // Initialize coinsCount once at the start of the level
        coinsCount = GameObject.FindGameObjectsWithTag("coin").Length;
        starsAwarded = false;

        // Retrieve previously saved starsAquired if available
        if (PlayerPrefs.HasKey("stars" + LevelSelectionMenuManager.currLevel.ToString()))
        {
            starsAquired = PlayerPrefs.GetInt("stars" + LevelSelectionMenuManager.currLevel.ToString());
        }
        else
        {
            starsAquired = 0; // Initialize to 0 if no saved value exists
        }
    }

    public void StarsAchieved(int newStarsAquired) 
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
            newStarsAquired = 1;
        }
        else if (percentage >= 40 && percentage < 70)
        {
            stars[0].SetActive(true);
            stars[1].SetActive(true);
            newStarsAquired = 2;
        }
        else
        {
            stars[0].SetActive(true);
            stars[1].SetActive(true);
            stars[2].SetActive(true);
            newStarsAquired = 3;
        }

        // Only update starsAquired if the newly acquired stars are greater than the previously saved value
        if (newStarsAquired > starsAquired)
        {
            starsAquired = newStarsAquired;
            PlayerPrefs.SetInt("stars" + (LevelSelectionMenuManager.currLevel - 1).ToString(), starsAquired);
            PlayerPrefs.Save(); // Save PlayerPrefs data
        } 

        starsAwarded = true; // Set to true to prevent re-awarding
        Debug.Log("newStarsAquired: " + newStarsAquired);
        Debug.Log("starsAquired: " + starsAquired);
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
