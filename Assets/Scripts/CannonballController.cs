using System.Collections;
using UnityEngine;

public class CannonballController : MonoBehaviour
{
    public int score = 0;
    
  
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 3);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Target"))
        {
            ScoreboardController.Instance.Score += 1;
        }
        Destroy(gameObject);
    }
    void Update() 
    {
        /*
        
        GameObject ship;
       
        //Rigidbody2D targetZone = ship.GetComponent<Rigidbody2D>();

        if (Input.GetMouseButtonDown(0))
        {
            GameObject cannon = Instantiate(gameObject, transform.position, Quaternion.identity);
            Rigidbody2D ball2D = cannon.GetComponent<Rigidbody2D>();

            ball2D.AddForce(Random.insideUnitCircle.normalized, ForceMode2D.Impulse);
           
        }

        if (Input.GetMouseButtonDown(1))
        {
            Instantiate(gameObject);
        }


        cannonHit();
    */
        }


    private void cannonHit()
    {
        /*
         * if(ball2D hits another ball2D)
         * {
         * destroy(gameObject);
         * }
         * 
         * 
         * if(ball2D hits TargetZone)
         * {
         * score++;
         * }
         * */
    }
}
