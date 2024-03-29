using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{

    public CoinManager cm;
    [SerializeField] private HeartCounter heartCounter;
    [SerializeField] private CollectibleManager collectibleManager;


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
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Nugis"))
        {
            HandleEnemyCollision(collision.transform);
        }
    }

    private void HandleEnemyCollision(Transform enemy)
    {
        // Simple check: if player's bottom is above enemy's top, it's a "jump on top"
        if (transform.position.y > enemy.position.y + (enemy.GetComponent<Collider2D>().bounds.size.y / 2))
        {
            // Logic to stun the enemy
             enemy.GetComponent<NewBehaviourScript>().Stun();
        }
        else
        {
            // Player loses a heart
            heartCounter.LoseHeart();
        }
    }
}
