using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEndTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TutorialLevelScript tutorialLevelScript = GameObject.Find("GameManager").GetComponent<TutorialLevelScript>();
            tutorialLevelScript.passedEndTrigger = true;
            tutorialLevelScript.EndTutorial();
        }
    }
}
