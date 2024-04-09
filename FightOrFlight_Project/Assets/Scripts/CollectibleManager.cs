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
    
    [SerializeField] private AudioSource audioSource; // Reference to the AudioSource
    [SerializeField] private AudioClip collectLeafSound; // Sound for collecting a leaf
    [SerializeField] private AudioClip collectAcornSound; // Sound for collecting an acorn    
    public void CollectLeaf()
    {
        // Increase player's movement speed
        if (!speedBoostActive) {
            StartCoroutine(playerMovement.AddSpeedBoost(leafSpeedBoost, leafSpeedBoostDuration));
            speedBoostActive = true;
            StartCoroutine(SpeedBoostTimer(leafSpeedBoostDuration));
            PlaySound(collectLeafSound);
        }
    }

    public void CollectAcorn()
    {
        healthManager.GainHearts();
        PlaySound(collectAcornSound);
    }

    private IEnumerator SpeedBoostTimer(float duration)
    {
        yield return new WaitForSeconds(duration);
        speedBoostActive = false;
    }
        private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null) {
            audioSource.PlayOneShot(clip);
        }
    }
}
