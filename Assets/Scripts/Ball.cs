using UnityEngine;


public class Ball : MonoBehaviour
{
    private Rigidbody2D rb;
    public GameObject ballPrefab;
    private Collider2D[] colliderss;
    private ContactFilter2D filter;

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

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (rb.OverlapPoint(mousePos))
        {
            Debug.Log("mouse is touching ball");
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            CheckOverlap();
        }
    }

    void CheckOverlap() { 
        int Collide = rb.Overlap(filter, colliderss);

        if (Collide > 0)
        {
            Debug.Log("overlap");
            Debug.Log($"found {Collide} overlapping colliders:");
                for (int i = 0; i < Collide; i++) 
            {
                //ballPrefab.GetComponent<SpriteRenderer>().color = new Color(Random.value, Random.value, Random.value);
                Debug.Log($"-{colliderss}[i].name");
            }
        }
    }
}
