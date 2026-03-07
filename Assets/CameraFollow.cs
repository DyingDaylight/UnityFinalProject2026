using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothTime = 0.25f;

    private Vector3 velocity = Vector3.zero;
    
    private void LateUpdate()
    {
        if (target == null)
            return;
        
        Vector2 pos2d = Vector2.Lerp(transform.position, target.transform.position, smoothTime * Time.deltaTime);
        transform.position = new Vector3(pos2d.x, pos2d.y, transform.position.z);
        
        Vector3 targetPosition = new Vector3(
            target.position.x,
            target.position.y,
            transform.position.z);
        /*
        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref velocity,
            smoothTime);*/
    }
}
