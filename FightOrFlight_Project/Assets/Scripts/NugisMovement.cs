using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private bool isStunned = false;
    private bool isClimbing = false;
    private float startingHeight;
    private enum Direction {Left, Right};
    private enum ClimbDirection {Up, Down};
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float climbSpeed = 7f;
    [SerializeField] private float climbHeight = 7f;
    [SerializeField] private float stunDuration = 2.0f;
    [SerializeField] private Direction startingDirection = Direction.Right;
    private ClimbDirection climbDirection = ClimbDirection.Up;
    // Start is called before the first frame update
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        startingHeight = transform.position.y;
        SetDirection(startingDirection);
    }

    // Update is called once per frame
    void Update()
    {
        if (isStunned) // Check if the enemy is stunned
        return; // Skip the normal movement logic if stunned
        if (startingDirection == Direction.Right)
        {   
            if (!isClimbing)
            {
                this.transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
            } 
            else
            {
                Climb();
            }
        } 
        else if (startingDirection == Direction.Left)
        {
            if (!isClimbing)
            {
                this.transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
            }
            else
            {
                Climb();
            }
        }
    }

    public void Stun()
    {
        if (!isStunned)
        {
            isStunned = true;
            StartCoroutine(RecoverFromStun());
        }
    }

    private IEnumerator RecoverFromStun()
    {
        yield return new WaitForSeconds(stunDuration); // Wait for the duration of the stun

        isStunned = false; // Reset the stun state
        // Optionally, re-enable the Collider2D
        // GetComponent<Collider2D>().enabled = true;
        // TODO: Remove the visual indicator/effect for the stun
    }


    private void Climb()
    {
        if (climbDirection == ClimbDirection.Up)
        {
            this.transform.Translate(Vector3.up * climbSpeed * Time.deltaTime);
            if (transform.position.y > startingHeight + climbHeight)
            {
                climbDirection = ClimbDirection.Down;
            }
        }
        else
        {
            this.transform.Translate(Vector3.down * climbSpeed * Time.deltaTime);
            if (transform.position.y < startingHeight)
            {
                isClimbing = false;
                SwitchDirection();
                climbDirection = ClimbDirection.Up;
            }
        }
    }

    private void SwitchDirection()
    {
        if (startingDirection == Direction.Right)
        {
            startingDirection = Direction.Left;
        }
        else
        {
            startingDirection = Direction.Right;
        }
    }
    private void SetDirection(Direction direction)
{
    if (direction == Direction.Right)
    {
        animator.SetBool("isMovingRight", true);
        // Flip sprite if needed: spriteRenderer.flipX = false;
    }
    else
    {
        animator.SetBool("isMovingRight", false);
        // Flip sprite if needed: spriteRenderer.flipX = true;
    }
}


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Tree")
        {
            isClimbing = true;
        }
    }
}
