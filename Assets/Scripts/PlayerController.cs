
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    //walk
    private Vector3 velocity;
    private Vector2 playerInput;
    public float maxSpeed = 2.00f;
    public float accTime = 0.05f;
    public float decTime = 0.05f;
    
    //jump
    public float ApexHeight = 3.5f;
    public float ApexTime = 0.5f;
    public float terminalSpeed = 5f;

    public float coyoteTime = 0.4f;
    public float coyoteCount = 0f;

    public float gravity = 0f;
    public float horizontal;
    public float jumpVel;
    public bool jumpPressed = true;

    //charge
    private bool canCharge = true;
    private bool isCharging = false;
    private float chargeSpeed = 10f;
    private float chargeTime = 0.2f;
    private float chargeCooldown = 1f;

    //wall jump
    private bool wallTouch;
    private bool wallBounce;
    private float wallBounceDirection;
    private float wallBounceTime = 0.3f;
    private float wallBounceTimer;
    private float wallBounceDuration = 0.3f;
    private Vector2 wallBouncePower = new Vector2(10f, 15f);
    public Vector2 wallCheckSize = new Vector2(0.49f, 0.03f);


    //fall damage
    private float maxFallSpeed = 0f;
    public float minFallDamage = 10f;
    private float fallDistance;
   
    //player attributes
    [SerializeField] LayerMask jumpToGround;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private TrailRenderer tr;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Transform wallCheck;
    Animator animator;
    private bool directionDefaultRight = true;
    public SpriteRenderer bodyRenderer;



    public enum CharacterState
    {
        Idle, Walking, Jumping, Falling, Dead
    }

    public CharacterState state = CharacterState.Idle;

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
        horizontal = Input.GetAxisRaw("Horizontal");

        playerInput = new()
        {
            x = horizontal,
            y = Input.GetButtonDown("Jump") ? 1 : 0
        };


        if (playerInput.y == 1) jumpPressed = true;


        if (Input.GetKeyDown(KeyCode.LeftShift) && canCharge)
        {
            StartCoroutine(Charge());

        }

        if (isCharging)
        {
            return;
        }

        WallBounce();
        Flip();
        if (!wallBounce)
        {
            Flip();
        }
        //print(jumpVel);
        if (rb.velocity.y > 8) //if falling
        {
            animator.SetTrigger("Die");
        }
        print(velocity.y);
        if (velocity.y <= -7)
        {
            animator.SetTrigger("Die");
        }

    }

    private IEnumerator Charge()
    {
        canCharge = false;
        isCharging = true;

        float originGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        velocity = new Vector2(transform.localScale.x * chargeSpeed, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(chargeTime);


        tr.emitting = false;
        rb.gravityScale = originGravity;
        isCharging = false;
        yield return new WaitForSeconds(chargeCooldown);
        canCharge = true;
    }

    private void FixedUpdate()
    {
        MovementUpdate();
    }

    private void MovementUpdate()
    {
        WalkInput();
        JumpInput();

        //print(velocity);
        rb.linearVelocity = velocity;
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
        if (IsGrounded() && jumpPressed)
        {
            animator.SetBool("IsJumping", true);
            velocity.y = jumpVel;
            jumpPressed = false;
        }
        else if (!IsGrounded())
        {
            animator.SetBool("IsJumping", false);
            velocity.y += gravity * Time.deltaTime;
            jumpPressed = false;
        }
        else
        {
            velocity.y = 0;
        }
    }

    private void Flip() 
    {
        if (horizontal < 0f)
        {
            bodyRenderer.flipX = true;
        }

        if (horizontal > 0f)
        {
            bodyRenderer.flipX = false;
        }
    }

    private void WallBounce()
    {
       wallBounceTimer -= Time.deltaTime;
        //print(Time.deltaTime);
        if (Input.GetButtonDown("Jump") && wallBounceTimer > 0f)
        {
            wallBounce = true;
            rb.linearVelocity = new Vector2(wallBounceDirection * wallBouncePower.x, wallBouncePower.y);
            //rb.velocity = new Vector2(horizontal * wallBouncePower.x, wallBouncePower.y);
            wallBounceTimer = 0f;

            wallBounce = false;
        }
       
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
}
