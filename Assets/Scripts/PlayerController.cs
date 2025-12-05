
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    Animator animator;

    private Vector3 velocity;
    private Vector2 playerInput;

    public float maxSpeed = 2.00f;
    public float accTime = 0.05f;
    public float decTime = 0.05f;
    
    public float ApexHeight = 3.5f;
    public float ApexTime = 0.5f;
    public float terminalSpeed = 5f;

    public float coyoteTime = 0.4f;
    public float coyoteCount = 0f;

    public float gravity = 0f;
    public float horizontal;
    public float jumpVel;
    public bool jumpPressed = false;
    private bool isFacingRight = true;

    //dashing
    private bool canDash = true;
    private bool isDash = false;
    private float dashSpeed = 10f;
    private float dashTime = 0.2f;
    private float dashCooldown = 1f;

    private bool isWallJumping;
    private float wallJumpDirection;
    private float wallJumpTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    private Vector2 wallJumpingPower = new Vector2(8f, 16f);
    public SpriteRenderer bodyRenderer;

    [SerializeField] LayerMask jumpToGround;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private TrailRenderer tr;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Transform wallCheck;




    public enum CharacterState
    {
        Idle, Walking, Jumping, Falling, Dead
    }

    public CharacterState state = CharacterState.Idle;

    public enum FacingDirection
    {
        left, right
    }


    void Start()
    {
        gravity = -2 * ApexHeight / (ApexTime * ApexTime);
        jumpVel = 2 * ApexHeight / ApexTime;

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        rb.gravityScale = 0;
        

    }

    void Update()
    {
        WallJump();
        Flip();

        if (!isWallJumping)
        {
            Flip(); 
        }

        horizontal = Input.GetAxisRaw("Horizontal");

        playerInput = new()
        {
            x = horizontal,
            y = Input.GetButtonDown("Jump") ? 1 : 0
        };


        if (playerInput.y == 1) jumpPressed = true;


        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());

        }
        if (isDash)
        {
            return;
        }

    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDash = true;

        float originGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        velocity = new Vector2(transform.localScale.x * dashSpeed, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashTime);


        tr.emitting = false;
        rb.gravityScale = originGravity;
        isDash = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;

    }


    private void FixedUpdate()
    {
        MovementUpdate();
    }

    private void MovementUpdate()
    {
        WalkInput();
        JumpInput();

        print(velocity);
        rb.linearVelocity = velocity;

        //<summary>
        //Modifies velocity.x based on playerInput.x.
        //    </summary>
    }

    private void WalkInput()
    {
        transform.position += velocity * Time.deltaTime;
        float accelerationRate = maxSpeed / accTime;
        float decelerationRate = maxSpeed / decTime;
        // if maxSpeed is met, it stays at maxSpeed
        if (playerInput.magnitude > 0)
        {
            animator.SetBool("IsWalking", true);
            velocity += (Vector3)playerInput.normalized * accelerationRate * Time.deltaTime;

            if (velocity.magnitude > maxSpeed)
            {
                velocity = velocity.normalized * maxSpeed;
            }
        }
        else
        {
            animator.SetBool("IsWalking", false);
            Vector3 changeInVelocity = velocity.normalized * decelerationRate * Time.deltaTime;
            if (changeInVelocity.magnitude > velocity.magnitude)
            {
                velocity = Vector3.zero;
            }
            else
            {
                velocity -= changeInVelocity;
            }
        }
        transform.position += velocity * Time.deltaTime;
    }

    private void JumpInput()
    {
        //coyoteCount = 0f;
        if (IsGrounded() && playerInput.y == 1)
        {
            animator.SetBool("IsJumping", true);
            velocity.y = jumpVel;
        }
        else if (!IsGrounded())
        {
            animator.SetBool("IsJumping", false);
            velocity.y += gravity * Time.deltaTime;
        }
        else
        {
            velocity.y = 0;
        }

        //if (fall acceleration > value) {cap vertical component of velocity to not exceed value}
        if (rb.linearVelocity.y > terminalSpeed)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, terminalSpeed);
        }
        /*
    if (isGrounded == false) {can still jump if within coyoteTime since became ungrounded}
    if(IsGrounded == false)
        {
            coyoteTime = 0.5f * time.deltaTIme;
            if(input -= coyoteTime)
            {
            JumpInput(playerInput)
            }
        }
        */
        if (IsGrounded())
        {
            //coyoteCount -= Time.deltaTime;
            //coyoteCount = coyoteTime;
        }
        else
        {
            //coyoteCount -= Time.deltaTime;
            //coyoteCount = coyoteTime;
        }

    }

    private void Flip() 
    {
        if(isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }

        //if (playerInput != 0f)
        {
            //bodyRenderer.flipX = true;
        }

       // if (playerInput != 0f)
        {
            //bodyRenderer.flipX = false;
        }
    }
    
    
    public bool isWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void WallJump()
    {
       wallJumpingCounter -= Time.deltaTime;

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rb.linearVelocity = new Vector2(wallJumpDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if(transform.localScale.x != wallJumpDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale; 
            }
            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
   }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }
    public bool IsJumping()
    {
        return false;
    }


    public bool IsWalking()
    {
        return false;
    }
    public bool IsGrounded()
    {
        Vector3 origin = transform.position + Vector3.down * 0.55f;
        return Physics2D.OverlapBox(origin, new Vector2(1f, 0.2f), 0, jumpToGround);
       
    }


    public FacingDirection GetFacingDirection()
    {
        }
        return FacingDirection.left;
    }


}
