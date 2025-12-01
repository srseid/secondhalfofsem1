using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Camera))]
public class followCamera2D : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float speed = 5f;
    [SerializeField] private Tilemap tilemap;

    private Camera followCamera;
    private Vector3 offset;
    private Vector2 viewportHalfSize;

    private float leftCameraBound;
    private float rightCameraBound;
    private float bottomCameraBound;

    void Start()
    {
        followCamera = GetComponent<Camera>();
        offset = transform.position - target.position;

        CalculateCameraBounds();
    }
    private void CalculateCameraBounds()
    { 
        tilemap.CompressBounds();

        float orthoSize = followCamera.orthographicSize; 
        viewportHalfSize = new (orthoSize * followCamera.aspect, orthoSize);

        Vector3Int tilemapMin = tilemap.cellBounds.min;
        Vector3Int tilemapMax = tilemap.cellBounds.max;

        leftCameraBound = tilemapMin.x + viewportHalfSize.x;
        rightCameraBound = tilemapMax.x - viewportHalfSize.x;
        bottomCameraBound = tilemapMin.y + viewportHalfSize.y;
        //set bounds
    }

    private void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 steppedPosition = Vector3.Lerp(transform.position, desiredPosition, speed * Time.deltaTime);

        steppedPosition.x = Mathf.Clamp(steppedPosition.x, leftCameraBound, rightCameraBound);
        steppedPosition.y = Mathf.Clamp(steppedPosition.y, bottomCameraBound, steppedPosition.y);
        //make sure camera cannot see out of bounds
        //top limit is however high the player can go
        transform.position = steppedPosition;
    }

}
