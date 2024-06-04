using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetsameesAI : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float throwRange = 6f;
    [SerializeField] private float axeSpeed = 10f;
    [SerializeField] private float chopRange = 5f;
    [SerializeField] private float treeChoppingCooldown = 10f;
    [SerializeField] private float throwCooldown = 3f;
    [SerializeField] private float meleeCooldown = 1f;
    [SerializeField] private float meleeAttackSpeed = 1f;
    [SerializeField] private float allAttackCooldown = 1f;
    [SerializeField] private float speedRequiredToDamage = 5f;
    [SerializeField] private GameObject axePrefab;
    [SerializeField] private List<GameObject> choppableTrees;
    [SerializeField] private GameObject finalPine;
    private Transform player;
    private PlayerMovement playerMovement;
    private enum State { Chase, ThrowAxe, Chop, Melee }
    private State state = State.Chase;
    private enum Direction { Left, Right }
    private Direction direction = Direction.Left;
    private Animator anim;
    private Collider2D headCollider;
    private MetsameesMeleeTrigger meleeCollider;
    private LevelEnd levelEnd;
    private HealthManager playerHealthManager;
    private MetsameesHealthManager healthManager;
    private bool isAxeThrown = false;
    private float axeXOffset = 1f;
    private float axeYOffset = 1.464f;
    private float axeRotation = 53.34f;
    private float axeRotationSpeed = 500f;
    private float treeChopDistance = 2f;
    private float treeChoppingCooldownTimer = 0f;
    private float throwCooldownTimer = 0f;
    private float meleeCooldownTimer = 0f;
    private float allAttackCooldownTimer = 0f;
    private GameObject axe;
    private Vector3[] axeFlightPath;
    private int currentAxeFlightPathPoint = 0;
    private SpriteRenderer spriteRenderer;
    private GameObject treeToChop;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; 
        playerMovement = player.GetComponent<PlayerMovement>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        headCollider = GetComponent<Collider2D>();
        meleeCollider = GetComponentInChildren<MetsameesMeleeTrigger>();
        throwCooldownTimer = throwCooldown;
        meleeCooldownTimer = meleeCooldown;
        playerHealthManager = GameObject.Find("GameManager").GetComponent<HealthManager>();
        levelEnd = GameObject.Find("GameManager").GetComponent<LevelEnd>();
        healthManager = GetComponent<MetsameesHealthManager>();
        anim.SetFloat("meleeSpeed", meleeAttackSpeed);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Flip();

        if (healthManager.bossDefeated)
        {
            Escape();
            return;
        }

        if (throwCooldownTimer > 0)
        {
            throwCooldownTimer -= Time.deltaTime;
        }
        if (meleeCooldownTimer > 0)
        {
            meleeCooldownTimer -= Time.deltaTime;
        }
        if (treeChoppingCooldownTimer > 0)
        {
            treeChoppingCooldownTimer -= Time.deltaTime;
        }
        if (allAttackCooldownTimer > 0)
        {
            allAttackCooldownTimer -= Time.deltaTime;
        }

        switch (state)
        {
            case State.Chase:
                Chase();
                break;
            case State.ThrowAxe:
                ThrowAxe();
                break;
            case State.Chop:
                Chop();
                break;
            case State.Melee:
                Melee();
                break;
        }
    }

    private void Chase()
    {
        float xDistance = Mathf.Abs(transform.position.x - player.position.x);
        if (transform.position.x > player.position.x)
        {
            direction = Direction.Left;
        }
        else
        {
            direction = Direction.Right;
        }
        if (xDistance < throwRange && throwCooldownTimer <= 0 && allAttackCooldownTimer <= 0)
        {
            SetAnimationState(1);
            state = State.ThrowAxe;
            return;
        }
        if (xDistance < chopRange && playerMovement.movementState == PlayerMovement.MovementState.Climbing && player.position.y > transform.position.y && choppableTrees.Count > 0 && treeChoppingCooldownTimer <= 0 && allAttackCooldownTimer <= 0)
        {
            treeToChop = FindClosestTreeToPlayer();
            if (treeToChop != null && treeToChop != finalPine)
            {
                state = State.Chop;
                return;
            }
        }
        if (meleeCollider.playerInTrigger && transform.position.y > player.position.y && meleeCooldownTimer <= 0 && allAttackCooldownTimer <= 0)
        {
            SetAnimationState(3);
            state = State.Melee;
            return;
        }
        if (xDistance > 0.1)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.position.x, transform.position.y, transform.position.z), speed * Time.deltaTime);    
        }
    }

    private void ThrowAxe()
    {
        if (!isAxeThrown && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !anim.IsInTransition(0) && anim.GetCurrentAnimatorStateInfo(0).IsName("Metsamees_Throw"))
        {
            if (player.position.x < transform.position.x)
            {
                direction = Direction.Left;
                Flip();
            }
            else
            {
                direction = Direction.Right;
                Flip();
            }
            isAxeThrown = true;
            if (direction == Direction.Left) 
            {
                Vector3 axeLaunchPoint = new Vector3(transform.position.x - axeXOffset, transform.position.y + axeYOffset, transform.position.z);
                Quaternion axeLaunchRotation = Quaternion.Euler(0, 0, axeRotation);
                axe = Instantiate(axePrefab, axeLaunchPoint, axeLaunchRotation);
                axe.GetComponent<SpriteRenderer>().flipY = true;
                if (player.position.y < axeLaunchPoint.y)
                {
                    axeFlightPath = CalculateEllipse(30, axe.transform.position, false, true);
                }
                else
                {
                    axeFlightPath = CalculateEllipse(30, axe.transform.position, false, false);
                }
            }
            else
            {
                Vector3 axeLaunchPoint = new Vector3(transform.position.x + axeXOffset, transform.position.y + axeYOffset, transform.position.z);
                Quaternion axeLaunchRotation = Quaternion.Euler(0, 0, -axeRotation);
                axe = Instantiate(axePrefab, axeLaunchPoint, axeLaunchRotation);
                axe.GetComponent<SpriteRenderer>().flipY = true;
                axe.GetComponent<SpriteRenderer>().flipX = true;
                if (player.position.y < axeLaunchPoint.y)
                {
                    axeFlightPath = CalculateEllipse(30, axe.transform.position, true, true);
                }
                else
                {
                    axeFlightPath = CalculateEllipse(30, axe.transform.position, true, false);
                }
            }
        }
        if (isAxeThrown)
        {
            axe.transform.position = Vector3.MoveTowards(axe.transform.position, axeFlightPath[currentAxeFlightPathPoint], axeSpeed * Time.deltaTime);
            if (direction == Direction.Left)
            {
                axe.transform.rotation = Quaternion.Euler(0, 0, axe.transform.rotation.eulerAngles.z + axeRotationSpeed * Time.deltaTime);
            }
            else
            {
                axe.transform.rotation = Quaternion.Euler(0, 0, axe.transform.rotation.eulerAngles.z - axeRotationSpeed * Time.deltaTime);
            }
            if (axe.transform.position == axeFlightPath[currentAxeFlightPathPoint])
            {
                currentAxeFlightPathPoint++;
            }
            RenderAxeFlightPath();
            if (currentAxeFlightPathPoint == axeFlightPath.Length - 1)
            {
                currentAxeFlightPathPoint = 0;
                Destroy(axe);
                isAxeThrown = false;
                throwCooldownTimer = throwCooldown;
                allAttackCooldownTimer = allAttackCooldown;
                SetAnimationState(0);
                state = State.Chase;
            }
        }
    }

    private void Chop()
    {
        if (transform.position.x > treeToChop.transform.position.x)
        {
            direction = Direction.Left;
        }
        else
        {
            direction = Direction.Right;
        }
        if (Mathf.Abs(transform.position.x - treeToChop.transform.position.x) >= treeChopDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(treeToChop.transform.position.x, transform.position.y, transform.position.z), speed * Time.deltaTime);
        }
        if (Mathf.Abs(transform.position.x - treeToChop.transform.position.x) < treeChopDistance)
        {
            SetAnimationState(2);
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !anim.IsInTransition(0) && anim.GetCurrentAnimatorStateInfo(0).IsName("Metsamees_Chop"))
            {
                treeToChop.transform.GetChild(0).gameObject.SetActive(false);
                treeToChop.transform.GetChild(1).gameObject.SetActive(true); // Enable the tree stump
                choppableTrees.Remove(treeToChop);
                treeChoppingCooldownTimer = treeChoppingCooldown;
                allAttackCooldownTimer = allAttackCooldown;
                SetAnimationState(0);
                state = State.Chase;
            }
        }
    }

    private void Melee() {
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !anim.IsInTransition(0) && anim.GetCurrentAnimatorStateInfo(0).IsName("Metsamees_Melee"))
        {
            if (meleeCollider.playerInTrigger)
            {
                playerHealthManager.LoseHearts(1);
            }
            meleeCooldownTimer = meleeCooldown;
            allAttackCooldownTimer = allAttackCooldown;
            SetAnimationState(0);
            state = State.Chase;
        }
    }

    private Vector3[] CalculateEllipse(int ellipseSegments, Vector3 startPostition, bool mirroredX = false, bool mirroredY = false)
    {
        Vector3[] ellipsePoints = new Vector3[ellipseSegments + 1];
        float xScale = Mathf.Abs(startPostition.x - player.position.x);
        float yScale = Mathf.Abs(startPostition.y - player.position.y);

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
            ellipsePoints[i] = new Vector3(x + player.position.x, y + startPostition.y, 0);
        }

        ellipsePoints[ellipseSegments] = startPostition;

        return ellipsePoints;
    }

    private void Escape()
    {
        direction = Direction.Right;
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(50f, transform.position.y, transform.position.z), speed * 1.5f * Time.deltaTime);
        if (transform.position.x > 35f)
        {
            levelEnd.EndLevel();
            Destroy(gameObject);
        }
    }

    private GameObject FindClosestTreeToPlayer()
    {
        if (choppableTrees.Count == 0)
        {
            return null;
        }
        GameObject closestTree = null;
        float closestDistance = Mathf.Infinity;
        foreach (GameObject tree in choppableTrees)
        {
            float distance = Mathf.Abs(tree.transform.position.x - player.position.x);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTree = tree;
            }
        }
        if (Mathf.Abs(finalPine.transform.position.x - player.position.x) < closestDistance)
        {
            closestTree = finalPine;
        }
        return closestTree;
    }

    private void Flip() {
        if (direction == Direction.Left)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            if (player.GetComponent<Rigidbody2D>().velocity.y < -speedRequiredToDamage)
            {
                healthManager.LoseHearts(1);
            }
        }
    }

    private void SetAnimationState(int state)
    {
        anim.SetInteger("state", state);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(throwRange * 2, 10, 1));
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(chopRange * 2, 10, 1));
    }

    private void RenderAxeFlightPath()
    {
        for (int i = 0; i < axeFlightPath.Length - 1; i++)
        {
            Debug.DrawLine(axeFlightPath[i], axeFlightPath[i + 1], Color.red);
        }
    }

}
