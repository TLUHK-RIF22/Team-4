using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarsHandler : MonoBehaviour
{
    public GameObject[] stars;
    private int coinsCount;
    
    void Start()
    {
        coinsCount = GameObject.FindGameObjectsWithTag("coin").Length;
    }

    public void starsAcheived() 
    {
        int coinsLeft = GameObject.FindGameObjectsWithTag("coin").Length;
        int coinsCollected = coinsCount - coinsLeft;

        float percentage = float.Parse( coinsCollected.ToString() ) / float.Parse( coinsCount.ToString() ) * 100f;

        if(percentage <= 40)
        {
            stars[0].SetActive(true);
        }
        else if(percentage >= 40 && percentage < 70)
        {
            stars[0].SetActive(true);
            stars[1].SetActive(true);
        }
        else if(percentage >= 70)
        {
            stars[0].SetActive(true);
            stars[1].SetActive(true);
            stars[2].SetActive(true);
        }
    }
}
