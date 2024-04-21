using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KassikakkAI : MonoBehaviour
{
    [SerializeField, Range(3, 50)] private int ellipseSegments = 30;
    [SerializeField, Range(0.1f, 30f)] private float xAttackRange = 1.5f;
    [SerializeField, Range(0.1f, 30f)] private float yAttackRange = 1.5f;
    [SerializeField, Range(0.1f, 30f)] private float attackSpeed = 10f;
    [SerializeField, Range(0f, 30f)] private float attackCooldown = 1f;
    [SerializeField] private bool attackWhenBelowPlayer = false;
    private enum State { Idle, Attacking }
    private State state = State.Idle;
    private Vector3[] ellipsePoints;
    private Vector3 startPoint;
    private float attackCooldownTimer;
    private int currentEllipsePoint;
    private SpriteRenderer spriteRenderer;
    private Transform player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        startPoint = transform.position;
        attackCooldownTimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (attackCooldownTimer > 0)
        {
            attackCooldownTimer -= Time.deltaTime;
        }
        switch (state)
        {
            case State.Idle:
                if (attackCooldownTimer <= 0)
                {
                    WaitForAttack();
                }
                break;
            case State.Attacking:
                Attack();
                break;
        }
    }

    private void WaitForAttack()
    {
        if (attackWhenBelowPlayer)
        {
            if (player.position.x < transform.position.x + xAttackRange && player.position.x > transform.position.x - xAttackRange && player.position.y < transform.position.y + yAttackRange && player.position.y > transform.position.y - yAttackRange)
            {
                if (player.position.x < transform.position.x && player.position.y < transform.position.y)
                {
                    CalculateEllipse(false, true);
                }
                else if (player.position.x > transform.position.x && player.position.y < transform.position.y)
                {
                    CalculateEllipse(true, true);
                }
                else if (player.position.x < transform.position.x)
                {
                    CalculateEllipse(false, false);
                }
                else
                {
                    CalculateEllipse(true, false);
                }
                currentEllipsePoint = 0;
                state = State.Attacking;
            }
        }
        else
        {
            if (player.position.x < transform.position.x + xAttackRange && player.position.x > transform.position.x - xAttackRange && player.position.y < transform.position.y && player.position.y > transform.position.y - yAttackRange)
            {
                if (player.position.x < transform.position.x)
                {
                    CalculateEllipse(false, true);
                }
                else
                {
                    CalculateEllipse(true, true);
                }
                currentEllipsePoint = 0;
                state = State.Attacking;
            }
        }
    }

    private void Attack()
    {
        transform.position = Vector3.MoveTowards(transform.position, ellipsePoints[currentEllipsePoint], attackSpeed * Time.deltaTime);
        FlipSprite(transform.position.x > ellipsePoints[currentEllipsePoint].x);
        if (transform.position == ellipsePoints[currentEllipsePoint])
        {
            if (currentEllipsePoint == ellipsePoints.Length - 1)
            {
                attackCooldownTimer = attackCooldown;
                state = State.Idle;
                transform.position = startPoint;
            }
            else
            {
                currentEllipsePoint++;
            }
        }
    }

    private void CalculateEllipse(bool mirroredX = false, bool mirroredY = false)
    {
        ellipsePoints = new Vector3[ellipseSegments];
        float xScale = Mathf.Abs(transform.position.x - player.position.x);
        float yScale = Mathf.Abs(transform.position.y - player.position.y);

        if (mirroredX)
        {
            xScale = -xScale;
        }

        if (mirroredY)
        {
            yScale = -yScale;
        }

        for (int i = 0; i < ellipseSegments; i++)
        {
            float angle = i * 2 * Mathf.PI / ellipseSegments;
            float x = Mathf.Cos(angle) * xScale;
            float y = Mathf.Sin(angle) * yScale;
            ellipsePoints[i] = new Vector3(x + player.position.x, y + transform.position.y, 0);
        }
    }

    private void FlipSprite(bool flip)
    {
        spriteRenderer.flipX = flip;
    }

    /* private void OnDrawGizmos()
    {
        if (ellipsePoints == null || ellipsePoints.Length == 0)
        {
            return;
        }

        for (int i = 0; i < ellipsePoints.Length - 1; i++)
        {
            Gizmos.DrawLine(ellipsePoints[i], ellipsePoints[i + 1]);
        }
        Gizmos.DrawLine(ellipsePoints[ellipsePoints.Length - 1], ellipsePoints[0]);
    } */

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (attackWhenBelowPlayer)
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(xAttackRange * 2, yAttackRange * 2, 0));
        }
        else
        {
            Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y - yAttackRange / 2, 0), new Vector3(xAttackRange * 2, yAttackRange, 0));
        }
    }
}
