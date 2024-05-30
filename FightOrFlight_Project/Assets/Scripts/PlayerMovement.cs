using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public PlayerData referencePlayerData;
    private PlayerData playerData;
    private Rigidbody2D rb;
    private CapsuleCollider2D coll;
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    public float dirY { get; private set; } = 0f;
    public float dirX { get; private set; } = 0f;
    public enum MovementState { Grounded, Jumping, Falling, Climbing, Gliding };
    public MovementState movementState { get; private set; } = MovementState.Grounded;
    private bool touchingTree = false;
    private bool isFacingRight;
    private bool isJumpCut = false;
    private bool isMuddy = false;
    private float lastPressedJumpTime = 0;
    private float lastOnGroundTime = 0;
    private float lastClimbingTime = 0;
    private float lastFallWhenClimbingTime = 0;
    private float lastStartGlideTime = 0;
    private Coroutine mudColorCoroutine;

    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private bool climbOnLevelStart = true;
    [SerializeField, Range(0.01f, 0.3f)] private float walkingAnimationSpeedMultiplier = .15f;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<CapsuleCollider2D>();
        playerData = Instantiate(referencePlayerData);
        anim = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        SetGravity(playerData.gravityScale);
        isFacingRight = true;

        if (climbOnLevelStart)
        {
            movementState = MovementState.Climbing;
        }
    }

    private void Update()
    {
        #region Timers
        lastOnGroundTime -= Time.deltaTime;
        lastPressedJumpTime -= Time.deltaTime;
        lastClimbingTime -= Time.deltaTime;
        lastFallWhenClimbingTime -= Time.deltaTime;
        lastStartGlideTime -= Time.deltaTime;
        #endregion

        #region Input Checks
        dirX = Input.GetAxisRaw("Horizontal");
        dirY = Input.GetAxisRaw("Climb");

        if (Input.GetButtonDown("Jump"))
        {
            OnJumpInputDown();
        }
        if (Input.GetButtonUp("Jump"))
        {
            OnJumpInputUp();
        }
        #endregion

        #region Movement direction Check
        if (dirX != 0)
        {
            CheckDirectionToTurn();
        }
        #endregion
    }

    private void FixedUpdate()
    {
        switch (movementState)
        {
            case MovementState.Grounded:
                GroundMovement();
                break;
            case MovementState.Jumping:
                JumpMovement();
                break;
            case MovementState.Falling:
                FallMovement();
                break;
            case MovementState.Climbing:
                ClimbMovement();
                break;
            case MovementState.Gliding:
                GlideMovement();
                break;
        }

        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        ChangeAnimationState(movementState);
        anim.SetFloat("climbSpeed", 1);

        if (movementState == MovementState.Grounded)
        {
            anim.SetFloat("walkSpeed", Mathf.Abs(rb.velocity.x) * walkingAnimationSpeedMultiplier);
            if (rb.velocity.x < 0.01f && rb.velocity.x > -0.01f)
            {
                anim.SetInteger("state", 5);
            }
        }

        if (movementState == MovementState.Climbing)
        {
            if (rb.velocity.y == 0 && rb.velocity.x == 0)
            {
                anim.SetFloat("climbSpeed", 0);
            }
        }
    }

    private void GroundMovement()
    {
        Run(1);

        if (IsGrounded())
        {
            lastOnGroundTime = playerData.coyoteTime;
        }
        else
        {
            lastOnGroundTime = playerData.coyoteTime;
            movementState = MovementState.Falling;
        }

        if (CanJump() && lastPressedJumpTime > 0)
        {
            movementState = MovementState.Jumping;
            Jump();
        }
        if (touchingTree && dirY != 0)
        {
            SetGravity(0);
            movementState = MovementState.Climbing;
        }
    }

    private void JumpMovement()
    {
        MoveInAir(1);
        if (isJumpCut)
        {
            SetGravity(playerData.gravityScale * playerData.jumpCutGravityMult);
        }

        if (rb.velocity.y < 0)
        {
            movementState = MovementState.Falling;
        }
        if (touchingTree && dirY != 0)
        {
            isJumpCut = false;
            SetGravity(0);
            movementState = MovementState.Climbing;
        }
        if(IsGrounded())
        {
            isJumpCut = false;
            movementState = MovementState.Grounded;
        }
    }

    private void FallMovement()
    {
        MoveInAir(1);

        if (CanJump() && lastPressedJumpTime > 0)
        {
            movementState = MovementState.Jumping;
            Jump();
        }
        if (isJumpCut)
        {
            SetGravity(playerData.gravityScale * playerData.jumpCutGravityMult);
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -playerData.maxFastFallSpeed));
        }
        if (IsGrounded())
        {
            isJumpCut = false;
            SetGravity(playerData.gravityScale);
            movementState = MovementState.Grounded;
        }
        if (touchingTree && (dirY != 0 || lastFallWhenClimbingTime > 0))
        {
            isJumpCut = false;
            SetGravity(0);
            movementState = MovementState.Climbing;
        }
        if (lastClimbingTime > 0 && lastPressedJumpTime > 0)
        {
            SetGravity(playerData.glideGravityScale);
            movementState = MovementState.Gliding;
        }
    }

    private void ClimbMovement()
    {
        lastClimbingTime = playerData.glideCoyoteTime;

        rb.velocity = new Vector2(dirX * playerData.horizontalClimbSpeed, dirY * playerData.verticalClimbSpeed);

        if (!touchingTree)
        {
        lastFallWhenClimbingTime = playerData.climbAfterFallTime;
        SetGravity(playerData.gravityScale);
        movementState = MovementState.Falling;
        }
        if (lastPressedJumpTime > 0)
        {
        lastStartGlideTime = playerData.noClimbingAfterGlideTime;
        SetGravity(playerData.glideGravityScale);
        rb.velocity = new Vector2(rb.velocity.x, 0);
        movementState = MovementState.Gliding;
        }
    }

    private void GlideMovement()
    {
        Glide(1);

        if (IsGrounded() || isMuddy)
        {
            SetGravity(playerData.gravityScale);
            movementState = MovementState.Grounded;
        }
        if (touchingTree && dirY != 0 && lastStartGlideTime < 0)
        {
            SetGravity(0);
            movementState = MovementState.Climbing;
        }
    }

    private void Run(float lerpAmount)
    {
        float targetSpeed = dirX * playerData.runMaxSpeed;
        targetSpeed = Mathf.Lerp(rb.velocity.x, targetSpeed, lerpAmount);

        float accelerationRate;
        accelerationRate = (Mathf.Abs(targetSpeed) > 0.01f) ? playerData.runAccelAmount : playerData.runDeccelAmount;

        if (playerData.doConserveMomentum && Mathf.Abs(rb.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(rb.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f)
        {
            accelerationRate = 0;
        }

        float speedDiff = targetSpeed - rb.velocity.x;
        float force = speedDiff * accelerationRate;
        rb.AddForce(force * Vector2.right, ForceMode2D.Force);
    }

    private void MoveInAir(float lerpAmount)
    {
        float targetSpeed = dirX * playerData.runMaxSpeed;
        targetSpeed = Mathf.Lerp(rb.velocity.x, targetSpeed, lerpAmount);

        float accelerationRate;
        accelerationRate = (Mathf.Abs(targetSpeed) > 0.01f) ? playerData.runAccelAmount * playerData.accelInAir : playerData.runDeccelAmount * playerData.deccelInAir;

        if (Mathf.Abs(rb.velocity.y) < playerData.jumpHangTimeThreshold)
        {
            accelerationRate *= playerData.jumpHangAccelerationMult;
            targetSpeed *= playerData.jumpHangMaxSpeedMult;
        }

        float speedDiff = targetSpeed - rb.velocity.x;
        float force = speedDiff * accelerationRate;
        rb.AddForce(force * Vector2.right, ForceMode2D.Force);
    }

    private void Glide(float lerpAmount)
    {
        float targetSpeed = dirX * playerData.glideMaxSpeed;
        targetSpeed = Mathf.Lerp(rb.velocity.x, targetSpeed, lerpAmount);

        float accelerationRate;
        accelerationRate = (Mathf.Abs(targetSpeed) > 0.01f) ? playerData.glideAccelAmount : playerData.glideDeccelAmount;

        float speedDiff = targetSpeed - rb.velocity.x;
        float force = speedDiff * accelerationRate;
        rb.AddForce(force * Vector2.right, ForceMode2D.Force);
    }

    private void Jump()
    {
        lastPressedJumpTime = 0;

        float force = playerData.jumpForce;
        if (rb.velocity.y < 0)
        {
            force -= rb.velocity.y;
        }
        rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
    }

    private void OnJumpInputDown()
    {
        lastPressedJumpTime = playerData.jumpInputBufferTime;
    }

    private void OnJumpInputUp()
    {
        if (CanJumpCut())
        {
            isJumpCut = true;
        }
    }

    private bool CanJump()
    {
        return (lastOnGroundTime > 0);
    }

    private bool CanJumpCut()
    {
        return (movementState == MovementState.Jumping && rb.velocity.y > 0);
    }

    private void CheckDirectionToTurn()
    {
        if (dirX > 0 != isFacingRight)
        {
            Turn();
        }
    }

    private void Turn()
    {
        Vector3 scale = transform.localScale; 
            scale.x *= -1;
            transform.localScale = scale;

            isFacingRight = !isFacingRight;
    }

    private bool IsGrounded()
    {
        Vector3 boxCastSize = new Vector3(coll.bounds.size.x - 0.1f, coll.bounds.size.y, coll.bounds.size.z);
        RaycastHit2D hit = Physics2D.BoxCast(coll.bounds.center, boxCastSize, 0f, Vector2.down, .1f, jumpableGround);
        return hit.collider != null;
    }

    private void SetGravity(float gravity)
    {
        rb.gravityScale = gravity;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Tree")
        {
            touchingTree = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Mud")
        {
            EnableMudEffect();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Tree")
        {
            touchingTree = false;
        }
        if (other.gameObject.tag == "Mud")
        {
            DisableMudEffect();
        }
    }

    private void ChangeAnimationState(MovementState newState)
    {
        anim.SetInteger("state", (int)newState);
    }

    public IEnumerator AddSpeedBoost(float speed, float duration)
    {
        playerData.runMaxSpeed += speed;
        playerData.verticalClimbSpeed += speed * 0.5f;
        playerData.horizontalClimbSpeed += speed * 0.5f;
        playerData.glideMaxSpeed += speed;

        yield return new WaitForSeconds(duration);

        playerData.runMaxSpeed -= speed;
        playerData.verticalClimbSpeed -= speed * 0.5f;
        playerData.horizontalClimbSpeed -= speed * 0.5f;
        playerData.glideMaxSpeed -= speed;
    }

    private void EnableMudEffect()
    {
        rb.drag = playerData.mudDrag;
        mudColorCoroutine = StartCoroutine(MudEffectColor());
    }

    private void DisableMudEffect()
    {
        rb.drag = 0;
        StopCoroutine(mudColorCoroutine);
        isMuddy = false;
        spriteRenderer.color = Color.white;
    }

    private void LateUpdate()
    {
        if (isMuddy)
        {
            spriteRenderer.color = Color.Lerp(spriteRenderer.color, new Color(0.37f, 0.23f, 0.05f, 1), .5f);
        }
    }

    private IEnumerator MudEffectColor()
    {
        float duration = 1f;
        float t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            spriteRenderer.color = Color.Lerp(Color.white, new Color(0.37f, 0.23f, 0.05f, 1), t / duration);
            yield return null;
        }
        isMuddy = true;
    }

}
