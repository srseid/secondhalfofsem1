using UnityEngine;

public class CannonballController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 3);
    }

    private void cannonSpawn()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(gameObject);
        }
    }
}
