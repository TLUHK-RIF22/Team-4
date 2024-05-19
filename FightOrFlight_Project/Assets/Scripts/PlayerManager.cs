using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{

    public CoinManager cm;
    [SerializeField] private HealthManager healthManager;
    [SerializeField] private CollectibleManager collectibleManager;
    [SerializeField] private AudioSource audioSource; 
    [SerializeField] private AudioClip stunSound; 
    [SerializeField] private int yDamageThreshold = -10;

    void FixedUpdate()
    {
        if (transform.position.y < yDamageThreshold)
        {
            healthManager.LoseHearts(10);
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "coin")
        {
            Destroy(other.gameObject);
            cm.coinCount++;
        }
        if (other.gameObject.tag == "Leaf")
        {
            Destroy(other.gameObject);
            collectibleManager.CollectLeaf();
        }
        if (other.gameObject.tag == "Acorn")
        {
            Destroy(other.gameObject);
            collectibleManager.CollectAcorn();
        }
        
        if (other.gameObject.tag == "Kanakull")
        {
            healthManager.LoseHearts(1);
        }
        if (other.gameObject.tag == "Kassikakk")
        {
            healthManager.LoseHearts(1);
        }
        if (other.gameObject.CompareTag("Nugis"))
        {
            HandleEnemyCollision(other.transform);
        }
    }

    private void HandleEnemyCollision(Transform enemy)
    {
        // Simple check: if player's bottom is above enemy's top, it's a "jump on top"
        if (transform.position.y > enemy.position.y + (enemy.GetComponent<Collider2D>().bounds.size.y / 2))
        {
            // Logic to stun the enemy
             enemy.GetComponent<NugisAI>().Stun();
             if (audioSource != null && stunSound != null)
            {
                audioSource.PlayOneShot(stunSound);
            }
        }
        else
        {
            // Player loses a heart
            healthManager.LoseHearts();
        }
    }
}
