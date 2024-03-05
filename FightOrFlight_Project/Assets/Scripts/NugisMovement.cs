using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private bool isClimbing = false;
    private float startingHeight;
    private enum Direction {Left, Right};
    private enum ClimbDirection {Up, Down};
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float climbSpeed = 7f;
    [SerializeField] private float climbHeight = 7f;
    [SerializeField] private Direction startingDirection = Direction.Right;
    private ClimbDirection climbDirection = ClimbDirection.Up;
    // Start is called before the first frame update
    void Start()
    {
        startingHeight = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Tree")
        {
            isClimbing = true;
        }
    }
}
