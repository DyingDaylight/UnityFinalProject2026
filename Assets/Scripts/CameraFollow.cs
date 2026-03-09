using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothTime = 0.25f;
    [SerializeField] private BoxCollider2D cameraBounds;
    
    private Camera mainCamera;
    
    private Vector3 velocity = Vector3.zero;
    
    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
    }
    
    private void LateUpdate()
    {
        if (target == null || cameraBounds == null)
            return;
        
        Vector3 targetPosition = new Vector3(
            target.position.x,
            target.position.y,
            transform.position.z);
        
        Vector2 pos2d = Vector2.Lerp(transform.position, targetPosition, smoothTime * Time.deltaTime);
        
        Bounds bounds = cameraBounds.bounds;
        
        float halfHeight = mainCamera.orthographicSize;
        float halfWidth = halfHeight * mainCamera.aspect;
        
        float clampedX = Mathf.Clamp(
            pos2d.x,
            bounds.min.x + halfWidth,
            bounds.max.x - halfWidth);

        float clampedY = Mathf.Clamp(
            pos2d.y,
            bounds.min.y + halfHeight,
            bounds.max.y - halfHeight);
        
        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
        
        /*
        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref velocity,
            smoothTime);*/

    }
}
