using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class CoinManager : MonoBehaviour
{
    public int coinCount;
    private Text coinText;
    // Start is called before the first frame update
    void Start()
    {
        coinText = GameObject.Find("StarText").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        coinText.text = coinCount.ToString();
    }
}
