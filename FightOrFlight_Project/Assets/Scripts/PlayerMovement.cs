using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private float dirY = 0f;
    private float dirX = 0f;
    private bool touchingTree = false;
    private bool isClimbing = false;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;
    [SerializeField] private float climbSpeed = 7f;
    [SerializeField] private LayerMask jumpableGround;

    public CoinManager cm;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {   

        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);


        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }


        dirY = Input.GetAxisRaw("Climb");
        if (touchingTree && dirY != 0)
        {
            isClimbing = true;
        }
        if (isClimbing)
        {
            rb.velocity = new Vector2(rb.velocity.x, dirY * climbSpeed);
            rb.gravityScale = 0;
            if (Input.GetButtonDown("Jump"))
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                isClimbing = false;
            }
        }
        else
        {
            rb.gravityScale = 1;
        }

    }

    private bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
        return hit.collider != null;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Tree")
        {
            touchingTree = true;
        }

        if (other.gameObject.tag == "coin")
        {
            Destroy(other.gameObject);
            cm.coinCount++;
        }
  
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Tree")
        {
            touchingTree = false;
            isClimbing = false;
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
            // Logic to reset the player or the level
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    }

