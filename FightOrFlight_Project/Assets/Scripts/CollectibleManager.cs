using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public HealthManager healthManager;
    [SerializeField] private float leafSpeedBoost = 9f;
    [SerializeField] private float leafSpeedBoostDuration = 5f;
    public void CollectLeaf()
    {
        // Increase player's movement speed

        StartCoroutine(playerMovement.AddSpeedBoost(leafSpeedBoost, leafSpeedBoostDuration));
    }

    public void CollectAcorn()
    {
        healthManager.GainHearts();
    }
}
