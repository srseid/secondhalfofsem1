using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D feetColl;
    private Collider2D bodyColl;

    Animator animator;
    [SerializeField] LayerMask jumpToGround;
    private Vector3 velocity;
    public float maxSpeed = 1.00f;
    public float accTime = 0.95f;
    public float decTime = 0.85f;
    //public FacingDirection currentFacingDirection;
    

    public enum FacingDirection
    {
        left, right
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // The input from the player needs to be determined and
        // then passed in the to the MovementUpdate which should
        // manage the actual movement of the character.
        Vector2 playerInput = Vector2.zero;
        MovementUpdate(playerInput);

    }

    private void MovementUpdate(Vector2 playerInput)
    {
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

    public bool IsWalking()
    {
        return false;
    }
    public bool _IsGrounded()
    {
       return Physics2D.BoxCast(feetColl.bounds.center, feetColl.bounds.size, 0f, Vector2.down, .1f, jumpToGround);
    }
    
    
    public FacingDirection GetFacingDirection()
    {
        //transform.Rotate(0f, 180f, 0f);
        return FacingDirection.left;
    }

    
}
