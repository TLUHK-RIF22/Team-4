using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public HealthManager healthManager;
    [SerializeField] private float leafSpeedBoost = 9f;
    [SerializeField] private float leafSpeedBoostDuration = 5f;
    private bool speedBoostActive = false;
    public void CollectLeaf()
    {
        // Increase player's movement speed
        if (!speedBoostActive) {
            StartCoroutine(playerMovement.AddSpeedBoost(leafSpeedBoost, leafSpeedBoostDuration));
            speedBoostActive = true;
            StartCoroutine(SpeedBoostTimer(leafSpeedBoostDuration));
        }
    }

    public void CollectAcorn()
    {
        healthManager.GainHearts();
    }

    private IEnumerator SpeedBoostTimer(float duration)
    {
        yield return new WaitForSeconds(duration);
        speedBoostActive = false;
    }
}
