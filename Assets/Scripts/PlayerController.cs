
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public PlayerStats MoveStats;
    Animator animator;

    private Vector2 velocity;
    private Vector2 playerInput;

    public float maxSpeed = 2.00f;
    public float accTime = 0.05f;
    public float decTime = 0.05f;
    //    private float acceleration;
    //    private float deceleration;
    //    //public FacingDirection currentFacingDirection;
    public float ApexHeight = 3.5f;
    public float ApexTime = 0.5f;
    public float terminalSpeed = 5f;

    public float coyoteTime = 0.4f;
    public float coyoteCount = 0f;

    public float gravity = 0f;
    public float jumpVel;
    public bool jumpPressed = false;

    //dashing
    private bool canDash = true;
    private bool isDash;
    private float dashPower = 20f;
    private float dashTime = 0.2f;
    private float dashCooldown = 1f;

    [SerializeField] LayerMask jumpToGround;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private TrailRenderer tr;
    [SerializeField] private CapsuleCollider2D cc;



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
        //cc = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
        rb.gravityScale = 0;


    }

    void Update()
    {
        //        if (isDash)
        //        {
        //            return;
        //        }
        //        // The input from the player needs to be determined and
        //        // then passed in the to the MovementUpdate which should
        //        // manage the actual movement of the character.

        playerInput = new()
        {
            x = Input.GetAxisRaw("Horizontal"),
            y = Input.GetButtonDown("Jump") ? 1 : 0
        };


        if (playerInput.y == 1) jumpPressed = true;

        //        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        //        {
        //            StartCoroutine(Dash());
        //        
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
        //if (playerInput != Vector2.zero)
        //{
        //    Vector2 targetVel = Vector2.zero;
        //    if (Dash)
        //    {
        //        targetVel = new Vector2(playerInput.x, 0f) * MoveStats.dashSpeed;
        //    }
        //    else
        //    {
        //        targetVel = new Vector2(playerInput.x, 0f) * MoveStats.walkSpeed;
        //    }

        //    velocity = Vector2.Lerp(velocity, targetVel, acceleration * Time.fixedDeltaTime);
        //    rb.linearVelocity = new Vector2(velocity.x, rb.linearVelocity.y);
        //}
        //else if (playerInput == Vector2.zero)
        //{
        //    velocity = Vector.Lerp(velocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
        //    rb.linearVelocity = new Vector2(velocity.x, rb.linearVelocity.y);
        //}



        float acceleration = maxSpeed / accTime;
        float deceleration = maxSpeed / decTime;

        Vector2 playerInput = Vector2.zero;
        if (playerInput.x < 0)
        {
            animator.SetBool("IsWalking", true);
            if (Mathf.Sign(playerInput.x) != Mathf.Sign(velocity.x))
                velocity.x *= -1;

            velocity.x += playerInput.x * acceleration * Time.fixedDeltaTime;
            velocity.x = Mathf.Clamp(velocity.x, -maxSpeed, maxSpeed);
        }
        else if (Mathf.Abs(velocity.x) > 0.005f)
        {
            animator.SetBool("IsWalking", false);
            velocity.x += Mathf.Sign(velocity.x) * deceleration * Time.fixedDeltaTime;
        }
        else
        {
            velocity.x = 0;

        }
        //transform.position += velocity * Time.deltaTime;
    }

    private void JumpInput()
    {
        //coyoteCount = 0f;
        if (IsGrounded() && playerInput.y == 1)
        {
            velocity.y = jumpVel;
        }
        else if (!IsGrounded())
        {
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

    //  public IEnumerator Dash()
    //    {
    //        canDash = false;
    //        isDash = true;
    //        float originGravity = rb.gravityScale;
    //        rb.gravityScale = 0f;
    //        rb.linearVelocity = new Vector2(transform.localScale.x * dashPower, 0f);
    //        tr.emitting = true;
    //        yield return new WaitForSeconds(dashTime);
    //        tr.emitting = false;
    //        rb.gravityScale = originGravity;
    //        isDash = false;
    //        yield return new WaitForSeconds(dashCooldown);
    //        canDash = true;

    //    }




    public bool IsWalking()
    {
        if (playerInput.x == 0)
        {
            animator.SetBool("IsWalking", true);
        }
        return false;
    }
    public bool IsGrounded()
    {
        Vector3 origin = transform.position + Vector3.down * 0.55f;
        return Physics2D.OverlapBox(origin, new Vector2(1f, 0.2f), 0, jumpToGround);
        
        //if(origin.x == null)
        {
          //  IsGrounded();
        } 
        //return false;
    }


    public FacingDirection GetFacingDirection()
    {
        if (playerInput.x < 0)
        {
            return FacingDirection.left;
        }

        if (playerInput.x > 0)
        {
            return FacingDirection.right;
        }
        return FacingDirection.right;
    }


}
