
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D feetColl;
    private Collider2D bodyColl;

    Animator animator;
    [SerializeField] LayerMask jumpToGround;
    private Vector3 velocity;
    public float maxSpeed = 5.00f;
    public float accTime = 0.95f;
    public float decTime = 0.85f;
    //public FacingDirection currentFacingDirection;
    public float ApexHeight= 3.5f;
    public float ApexTime = 0.5f;
    public float terminalSpeed = 5f;
    //if (fall acceleration > value) {cap vertical component of velocity to not exceed value}
    public float coyoteTime;
    //if (isGrounded == false) {can still jump if within coyoteTime since became ungrounded}


    public float gravity;
    public float jumpVel;

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
    }

    // Update is called once per frame
    void Update()
    {
        // The input from the player needs to be determined and
        // then passed in the to the MovementUpdate which should
        // manage the actual movement of the character.
        Vector2 playerInput = new()
        {
            x = Input.GetAxisRaw("Horizontal"),
            y = Input.GetButtonDown("Jump") ? 1 : 0
        };
        MovementUpdate(playerInput);
    }

    private void MovementUpdate(Vector2 playerInput)
    {
        JumpInput(playerInput);
        transform.position += velocity * Time.deltaTime;

        float accelerationRate = maxSpeed / accTime;
        float decelerationRate = maxSpeed / decTime;
        
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            animator.SetBool("IsWalking", true);
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
        if (jumpVel > terminalSpeed)
        {
            jumpVel -= terminalSpeed;
        }

    //if (isGrounded == false) {can still jump if within coyoteTime since became ungrounded}
    //if(IsGrounded == false)
        {
            //coyoteTime = 0.5f * time.deltaTIme;
            //if(input -= coyoteTime)
            //{
            //JumpInput(playerInput)
            //}
        }

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
