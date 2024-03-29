using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public void CollectLeaf()
    {
        // Increase player's movement speed

        StartCoroutine(playerMovement.AddSpeedBoost(10f, 5f));
    }
}
