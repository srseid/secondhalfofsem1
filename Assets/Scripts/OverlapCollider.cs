using UnityEngine;

public class OverlapCollider : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D[] colliders;
    private ContactFilter2D filter;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
       
    }

    // Update is called once per frame
    void Update()
    {
        CheckOverlap();
    }

    void CheckOverlap()
    {
        int Collidess = rb.Overlap(filter, colliders);

        if (Collidess > 0)
        {
            Debug.Log($"found {Collidess} overlapping colliders:");
            for (int i = 0; i < Collidess; i++)
            {
                //ballPrefab.GetComponent<SpriteRenderer>().color = new Color(Random.value, Random.value, Random.value);
                Debug.Log($"-{colliders}[i].name");
            }
        }
    }
}
