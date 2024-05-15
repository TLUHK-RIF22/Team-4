using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KanakullAI : MonoBehaviour
{
    private Transform player;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private float patrolMinX;
    private float patrolMaxX;
    private float patrolMinY;
    private float patrolMaxY;
    private float returnX;
    private float distanceFromPlayer;
    private bool hasStoppedTracking;
    private Vector2 lockOnTarget;
    private float attackCooldown;
    private State state;
    private Direction direction;
    [SerializeField] private float patrolSpeed;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float attackCooldownLength;
    [SerializeField] private Direction startingDirection;
    [SerializeField] private float patrolAreaWidth;
    [SerializeField] private float patrolAreaHeight;
    [SerializeField] private float attackRange;
    [SerializeField] private float stopTrackingDistance;
    [SerializeField] private float flyUpAngle;
    [SerializeField] private float legAnimationDistance;
    private enum State{ Patrolling, Attacking, Returning };
    private enum Direction { Left, Right };

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        patrolMaxX = transform.position.x + patrolAreaWidth / 2 - attackRange / 2;
        patrolMinX = transform.position.x - patrolAreaWidth / 2 + attackRange / 2;
        patrolMinY = transform.position.y - patrolAreaHeight;
        patrolMaxY = transform.position.y;
        state = State.Patrolling;
        direction = startingDirection;
        attackCooldown = attackCooldownLength;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per physics update
    void FixedUpdate()
    {
        distanceFromPlayer = GetDistanceFromPlayer();

        if (attackCooldown > 0)
        {
            attackCooldown -= Time.deltaTime;
        }

        switch (state)
        {
            case State.Patrolling:
                Patrol();
                break;
            case State.Attacking:
                Attack();
                break;
            case State.Returning:
                Return();
                break;
        }

        FlipSprite();
        RotateBasedOnState();
        ManageLegsAnimation();
    }

    private void Patrol()
    {
        if (direction == Direction.Right)
        {
            if (transform.position.x < patrolMaxX)
            {
                transform.Translate(Vector3.right * patrolSpeed * Time.deltaTime, Space.World);
            }
            else
            {
                direction = Direction.Left;
            }
        }
        else
        {
            if (transform.position.x > patrolMinX)
            {
                transform.Translate(Vector3.left * patrolSpeed * Time.deltaTime, Space.World);
            }
            else
            {
                direction = Direction.Right;
            }
        }

        if (CheckIfPlayerIsInAttackRange() && CheckIfPlayerIsInFront() && attackCooldown <= 0)
        {
            hasStoppedTracking = false;
            state = State.Attacking;
        }
    }

    private void Attack()
    {
        if (!hasStoppedTracking && CheckIfPlayerIsInFront())
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.position.x, player.position.y, transform.position.z), attackSpeed * Time.deltaTime);
            lockOnTarget = player.position;

            if (distanceFromPlayer < stopTrackingDistance)
            {
                hasStoppedTracking = true;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(lockOnTarget.x, lockOnTarget.y, transform.position.z), attackSpeed * Time.deltaTime);

            if (transform.position.y - lockOnTarget.y < 0.01f && transform.position.y - lockOnTarget.y > -0.01f)
            {
                returnX = CalculateReturnX();
                state = State.Returning;
            }
        }
    }

    private void Return()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(returnX, patrolMaxY, transform.position.z), attackSpeed * Time.deltaTime);   

        if (transform.position.y == patrolMaxY)
        {
            attackCooldown = attackCooldownLength;
            state = State.Patrolling;
        }
    }

    private void FlipSprite()
    {
        if (direction == Direction.Right)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }
    }

    private bool CheckIfPlayerIsInAttackRange()
    {
        return (player.position.x > transform.position.x - attackRange / 2 && player.position.x < transform.position.x + attackRange / 2 && player.position.y > patrolMinY && player.position.y < patrolMaxY);
    }

    private bool CheckIfPlayerIsInFront()
    {
        return (direction == Direction.Right && player.position.x > transform.position.x || direction == Direction.Left && player.position.x < transform.position.x);
    }

    private float CalculateReturnX()
    {
        if (direction == Direction.Right)
        {
            float xDistance = Mathf.Tan(flyUpAngle * Mathf.Deg2Rad) * (patrolMaxY - transform.position.y);
            return transform.position.x + xDistance;
        }
        else
        {
            float xDistance = Mathf.Tan(flyUpAngle * Mathf.Deg2Rad) * (patrolMaxY - transform.position.y);
            return transform.position.x - xDistance;
        }
    }

    private void RotateBasedOnState()
    {
        if (state == State.Attacking)
        {
            if (direction == Direction.Right)
            {
                transform.eulerAngles = new Vector3(0, 0, Mathf.LerpAngle(transform.eulerAngles.z, -flyUpAngle, 0.05f));
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, Mathf.LerpAngle(transform.eulerAngles.z, flyUpAngle, 0.05f));
            }
        }
        if (state == State.Returning)
        {
            if (direction == Direction.Right)
            {
                transform.eulerAngles = new Vector3(0, 0, Mathf.LerpAngle(transform.eulerAngles.z, flyUpAngle, 0.05f));
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, Mathf.LerpAngle(transform.eulerAngles.z, -flyUpAngle, 0.05f));
            }
        }
        else if (state == State.Patrolling)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }

    private float GetDistanceFromPlayer()
    {
        return Vector2.Distance(transform.position, player.position);
    }

    private void ManageLegsAnimation()
    {
        if (distanceFromPlayer < legAnimationDistance)
        {
            animator.SetBool("legsOut", true);
        }
        else
        {
            animator.SetBool("legsOut", false);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y - patrolAreaHeight / 2, transform.position.z), new Vector3(patrolAreaWidth, patrolAreaHeight, 1));
      
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y - patrolAreaHeight / 2, transform.position.z), new Vector3(attackRange, patrolAreaHeight, 1));
    }

    void OnValidate()
    {
        attackRange = Mathf.Clamp(attackRange, 0, patrolAreaWidth);
        stopTrackingDistance = Mathf.Clamp(stopTrackingDistance, 0, patrolAreaHeight);
        flyUpAngle = Mathf.Clamp(flyUpAngle, 0, 50);
    }
}
