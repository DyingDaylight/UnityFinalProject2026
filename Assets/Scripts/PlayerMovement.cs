using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 1f;
    
    private Vector3 originalScale;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalValue = Input.GetAxisRaw(("Horizontal"));
        float verticalValue = Input.GetAxisRaw(("Vertical"));
        
        if (horizontalValue != 0 || verticalValue != 0)
        {
            float x = horizontalValue < 0 ? originalScale.x * -1 : originalScale.x;
            transform.localScale = new Vector3(
                x,
                originalScale.y,
                originalScale.x);
            
            Vector3 deltaX = Vector3.right * horizontalValue * moveSpeed * Time.deltaTime;
            Vector3 deltaY = Vector3.up * verticalValue * moveSpeed * Time.deltaTime;
            Vector3 newPosition = transform.position + deltaX + deltaY;
            transform.position = newPosition;
        }
                
    }
}
