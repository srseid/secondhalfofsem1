using UnityEngine;


public class Ball : MonoBehaviour
{
    private Rigidbody2D rb;
    

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();


        if (rb != null)
        {
            Debug.Log("oop");
        }


    }

    void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
            Debug.Log("interpolate!");
        }
    }
}
