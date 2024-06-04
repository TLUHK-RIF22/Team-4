using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetsameesMeleeTrigger : MonoBehaviour
{
    [HideInInspector ]public bool playerInTrigger = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInTrigger = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInTrigger = false;
        }
    }
}
