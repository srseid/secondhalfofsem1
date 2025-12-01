using UnityEngine;

public class ParallaxLayerController : MonoBehaviour
{
    [SerializeField] private Camera viewCamera;
    [SerializeField] private float cameraDeltaScalar = 1f;

    private Vector3 cameraStartPos;
    private Vector3 layerStartPos;
    void Start()
    {
        cameraStartPos = viewCamera.transform.position;
        layerStartPos = transform.position;
    }

    private void LateUpdate()
    {
        Vector3 cameraDelta = viewCamera.transform.position - cameraStartPos;

        float layerDeltaX = cameraDelta.x * cameraDeltaScalar;
        float layerDeltay = cameraDelta.y * cameraDeltaScalar;

        Vector3 newLayerPos = layerStartPos + new Vector3(layerDeltaX, layerDeltay);
        transform.position = Vector3.Lerp(transform.position, newLayerPos, cameraDeltaScalar);
    }

}
