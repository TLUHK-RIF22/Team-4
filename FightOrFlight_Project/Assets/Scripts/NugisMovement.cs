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
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (animator == null || spriteRenderer == null)
        {
            Debug.LogError("Required components are missing from the GameObject.");
        }
        startingHeight = transform.position.y;
        //SetDirection(startingDirection);
    }

    // Update is called once per physics update
    void FixedUpdate()
    {
        if (isStunned)
            return;

        // Determine the direction of movement
        bool isMovingRight = startingDirection == Direction.Right;

        // Move the enemy left or right unless it's climbing
        if (!isClimbing)
        {
            Vector3 direction = isMovingRight ? Vector3.right : Vector3.left;
            this.transform.Translate(direction * moveSpeed * Time.deltaTime);
            spriteRenderer.flipX = !isMovingRight; // Flip sprite based on direction
        }
        else
        {
            Climb();
        }

        // animator.SetBool("isMovingRight", isMovingRight); // Pole vaja?
    }

    public void Stun()
    {
        if (!isStunned)
        {
            isStunned = true;
            animator.SetFloat("speed", 0); // Set the speed parameter to 0
            StartCoroutine(RecoverFromStun());
        }
    }

    private IEnumerator RecoverFromStun()
    {
        yield return new WaitForSeconds(stunDuration); // Wait for the duration of the stun

        animator.SetFloat("speed", 1); // Set the speed parameter to 1

        isStunned = false; // Reset the stun state
        // Optionally, re-enable the Collider2D
        // GetComponent<Collider2D>().enabled = true;
        // TODO: Remove the visual indicator/effect for the stun
    }


    private void Climb()
    {
        if (climbDirection == ClimbDirection.Up)
        {
            if (startingDirection == Direction.Right)
            {
                this.transform.eulerAngles = new Vector3(0, 0, 90);
                this.transform.Translate(Vector3.right * climbSpeed * Time.deltaTime);
            }
            else
            {
                this.transform.eulerAngles = new Vector3(0, 0, -90);
                this.transform.Translate(Vector3.left * climbSpeed * Time.deltaTime);
            }
            if (transform.position.y > startingHeight + climbHeight)
            {
                climbDirection = ClimbDirection.Down;
                spriteRenderer.flipX = !spriteRenderer.flipX;
            }
        }
        else
        {
            if (startingDirection == Direction.Right)
            {
                this.transform.Translate(Vector3.left * climbSpeed * Time.deltaTime);
            }
            else
            {
                this.transform.Translate(Vector3.right * climbSpeed * Time.deltaTime);
            }
            if (transform.position.y < startingHeight)
            {
                this.transform.eulerAngles = new Vector3(0, 0, 0);
                transform.position = new Vector3(transform.position.x, startingHeight, transform.position.z);
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

    /* private void SetDirection(Direction direction)
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
    } */


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Tree")
        {
            Vector3 closestPoint = other.ClosestPoint(this.transform.position);
            this.transform.position = new Vector3(closestPoint.x, this.transform.position.y, this.transform.position.z);
            isClimbing = true;
        }
        if (other.gameObject.tag == "Stump")
        {
            SwitchDirection();
        }
    }
}
