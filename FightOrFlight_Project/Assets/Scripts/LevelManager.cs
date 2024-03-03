using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject[] levels;
    // Start is called before the first frame update
    void Start()
    {
        levels[LevelSelectionMenuManagar.currLevel].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
