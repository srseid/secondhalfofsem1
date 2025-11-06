using UnityEngine;
using System.Collections.Generic;


public class BallPitFloor : MonoBehaviour
{
    private Rigidbody2D rb;
    private List<ContactPoint2D> contacts = new List<ContactPoint2D>();

   

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {

        int Contact = rb.GetContacts(contacts);
        if (contacts.Count > 0)
        {
            Debug.Log("Contact");
        }

    }
}
