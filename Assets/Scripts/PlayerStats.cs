using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Range(1f, 100f)] public float walkSpeed = 10.5f;
    [Range(0.25f, 50f)] public float acceleration = 2f;
    [Range(0.25f, 50f)] public float deceleration = 20f;

    [Range(1f, 100f)] public float dashSpeed = 20f;

    public LayerMask jumpToGround;
    public float GroundDetection = 0.02f;
   


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
