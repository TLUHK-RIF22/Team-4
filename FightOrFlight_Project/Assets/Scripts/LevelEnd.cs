using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Include this namespace to work with scenes
using TMPro;

public class LevelEnd : MonoBehaviour
{   
    private Text ScoreText;
    
    void Start()
    {
        ScoreText = GameObject.Find("StarText").GetComponent<Text>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider is tagged as "Player"
        if (other.gameObject.tag == "Player") 
        {
            EndLevel();
        }
    }

    public void EndLevel()
    {
        // Load the menu scene
        Time.timeScale = 0f;
        UIHandler.instance.ShowLevelDialog("Tase läbitud!", ScoreText.text);
    }

    public void EndFinalLevel()
    {
        // Load the menu scene
        Time.timeScale = 0f;
        GameObject levelUICanvas = GameObject.Find("LevelUICanvas");
        GameObject endCreditsImage = levelUICanvas.transform.Find("EndCreditsImage").gameObject;
        TextMeshProUGUI returnToMenuText = endCreditsImage.transform.Find("ReturnToMenuText").GetComponent<TextMeshProUGUI>();
        endCreditsImage.SetActive(true);
        endCreditsImage.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        returnToMenuText.gameObject.SetActive(true);
        returnToMenuText.color = new Color(1, 1, 1, 0);
        StartCoroutine(FadeInEndScreen(endCreditsImage, 4f, returnToMenuText, 1f));
    }

    IEnumerator FadeInEndScreen(GameObject obj, float duration, TextMeshProUGUI textObj, float textDuration)
    {
        float counter = 0;
        while (counter < duration)
        {
            counter += Time.unscaledDeltaTime;
            float alpha = counter / duration;
            obj.GetComponent<Image>().color = new Color(1, 1, 1, alpha);
            yield return null;
        }
        obj.GetComponent<Image>().color = new Color(1, 1, 1, 1);

        counter = 0;
        while (counter < textDuration)
        {
            counter += Time.unscaledDeltaTime;
            float alpha = counter / textDuration;
            textObj.color = new Color(1, 1, 1, alpha);
            yield return null;
        }
        textObj.color = new Color(1, 1, 1, 1);

        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                UIHandler.instance.GetComponent<StarsHandler>().StarsAchieved(0);
                UIHandler.instance.BackToMain(0);
            }
            yield return null;
        }

    }
}
