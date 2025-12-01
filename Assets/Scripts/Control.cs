using UnityEngine;

public class Control : MonoBehaviour
{
    
    private Vector3 velocity;
    public float maxSpeed = 2.00f;
    public float accTime = 0.095f;
    public float decTime = 0.085f;
    private float acceleration;
    private float deceleration;
    //public FacingDirection currentFacingDirection;
    //public float ApexHeight = 3.5f;
    //public float ApexTime = 0.5f;
    //public float terminalSpeed = 5f;

    //public float coyoteTime = 0.4f;
    //public float coyoteCount = 0f;

    
    //dashing
    
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private TrailRenderer tr;


    // Start is called before the first frame update
    void Start()
    {
       
        rb = GetComponent<Rigidbody2D>();
        
       
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 playerInput = new()
        {
            x = Input.GetAxisRaw("Horizontal"),
            y = Input.GetButtonDown("Jump") ? 1 : 0
        };

        //if (playerInput.y == 1) jumpPressed = true;

        movement(playerInput);
    }
    private void movement(Vector2 playerInput)
    {

        float acceleration = maxSpeed / accTime;
        float deceleration = maxSpeed / decTime;
        //JumpInput(playerInput);
        transform.position += velocity * Time.deltaTime;

        
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            //animator.SetBool("IsWalking", true);
            playerInput += Vector2.left;

            //currentFacingDirection = FacingDirection.left;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            
            playerInput += Vector2.right;
            //currentFacingDirection = FacingDirection.right;
        }

        // if maxSpeed is met, it stays at maxSpeed
        if (playerInput.magnitude > 0)
        {
            velocity += (Vector3)playerInput.normalized * acceleration * Time.deltaTime;

            if (velocity.magnitude > maxSpeed)
            {
                velocity = velocity.normalized * maxSpeed;
            }

        }
        else
        {
            Vector3 changeInVelocity = velocity.normalized * deceleration * Time.deltaTime;
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
}
