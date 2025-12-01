
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    private Collider2D feetColl;
    private Collider2D bodyColl;

    Animator animator;
    [SerializeField] LayerMask jumpToGround;
    private Vector3 velocity;
    public float maxSpeed = 2.00f;
    public float accTime = 0.095f;
    public float decTime = 0.085f;
    private float acceleration;
    private float deceleration;
    //public FacingDirection currentFacingDirection;
    public float ApexHeight = 3.5f;
    public float ApexTime = 0.5f;
    public float terminalSpeed = 5f;
    
    public float coyoteTime = 0.4f;
    public float coyoteCount = 0f;

    public float gravity;
    public float jumpVel;
    public Vector2 playerInput;
    public bool jumpPressed = false;

    //dashing
    private bool canDash = true;
    private bool isDash;
    private float dashPower = 20f;
    private float dashTime = 0.2f;
    private float dashCooldown = 1f;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private TrailRenderer tr;


    
    public enum CharacterState
    {
        Idle, Walking, Jumping, Falling, Dead
    }

    public CharacterState state = CharacterState.Idle;    
    public enum FacingDirection
    {
        left, right
    }

    // Start is called before the first frame update
    void Start()
    {
        gravity = -2 * ApexHeight / (ApexTime*ApexTime);
        jumpVel = 2 * ApexHeight * ApexTime;

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        rb.gravityScale = 0;

        float acceleration = maxSpeed / accTime;
        float deceleration = maxSpeed / decTime;
    }

    void Update()
    {
        if (isDash)
        {
            return;
        }
        // The input from the player needs to be determined and
        // then passed in the to the MovementUpdate which should
        // manage the actual movement of the character.

        Vector2 playerInput = new()
        {
            x = Input.GetAxisRaw("Horizontal")
            y = Input.GetButtonDown("Jump") ? 1 : 0
        };
        
        if (playerInput.y == 1) jumpPressed = true;
        
        //MovementUpdate(playerInput);
        movement(playerInput);

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    private void FixedUpdate()
    {
        //MovementUpdate();
        //movement(playerInput);
        if (isDash)
        {
            return;
        }
    }

    private void MovementUpdate(Vector2 playerInput)
    {
        WalkInput(playerInput);
        JumpInput(playerInput);

        rb.linearVelocity = velocity;
       
        //<summary>
        //Modifies velocity.x based on playerInput.x.
        //    </summary>
    }

    private void WalkInput(Vector2 playerInput) 
    {
        if (playerInput.x != 0)
        {
            if (Mathf.Sign(playerInput.x) != Mathf.Sign(velocity.x)) velocity.x *= -1;
            velocity.x += playerInput.x * acceleration * Time.fixedDeltaTime;
            velocity.x = Mathf.Clamp(velocity.x, -maxSpeed, maxSpeed);
        }
        else if (Mathf.Abs(velocity.x) > 0.005f)
        {
            velocity.x += Mathf.Sign(velocity.x) * deceleration * Time.fixedDeltaTime;
        }
        else
        {
            velocity.x = 0;
        }
        }

    private void movement(Vector2 playerInput) 
    {
        JumpInput(playerInput);
        transform.position += velocity * Time.deltaTime;

        float accelerationRate = maxSpeed / accTime;
        float decelerationRate = maxSpeed / decTime;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            //animator.SetBool("IsWalking", true);
            playerInput += Vector2.left;
            
            //currentFacingDirection = FacingDirection.left;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            animator.SetBool("IsWalking", true);
            playerInput += Vector2.right;
            //currentFacingDirection = FacingDirection.right;
        }

        // if maxSpeed is met, it stays at maxSpeed
        if (playerInput.magnitude > 0)
        {
            velocity += (Vector3)playerInput.normalized * accelerationRate * Time.deltaTime;

            if (velocity.magnitude > maxSpeed)
            {
                velocity = velocity.normalized * maxSpeed;
            }

        }
        else
        {
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
    private void JumpInput(Vector2 playerInput)
    {
        coyoteCount = 0f;
        if(IsGrounded() && playerInput.y ==1)
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
            coyoteCount = coyoteTime;
        }
        else
        {
            coyoteCount -= Time.deltaTime;
            //coyoteCount = coyoteTime;
        }

    }

  public IEnumerator Dash()
    {
        canDash = false;
        isDash = true;
        float originGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.linearVelocity = new Vector2(transform.localScale.x * dashPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashTime);
        tr.emitting = false;
        rb.gravityScale = originGravity;
        isDash = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;

    }

    public bool IsWalking()
    {
        
        return false;
    }
public bool IsGrounded()
    {
        Vector3 origin = transform.position + Vector3.down * 0.55f;
        return Physics2D.OverlapBox(origin, new Vector2(1f, 0.2f), 0, jumpToGround);
        //return Physics2D.BoxCast(feetColl.bounds.center, feetColl.bounds.size, 0f, Vector2.down, 0.1f, jumpToGround);
        //return false;
    }
    
    
    public FacingDirection GetFacingDirection()
    {
        return FacingDirection.left;
    }

    
}
