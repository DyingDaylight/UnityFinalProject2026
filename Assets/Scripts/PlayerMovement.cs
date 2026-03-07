using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 3f;
    
    private Rigidbody2D rb;
    private Vector2 input;
    private Vector2 movement;
    
    private Vector3 originalScale;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        input.x = Input.GetAxisRaw(("Horizontal"));
        input.y = Input.GetAxisRaw(("Vertical"));
        
        movement = input.normalized; // do not move faster by diagonal
        
        if (input.x != 0 || input.y != 0)
        {
            /*float x = horizontalValue < 0 ? originalScale.x * -1 : originalScale.x;
            transform.localScale = new Vector3(
                x,
                originalScale.y,
                originalScale.x);*/
            
            //Vector3 deltaX = Vector3.right * horizontalValue * moveSpeed * Time.deltaTime;
            //Vector3 deltaY = Vector3.up * verticalValue * moveSpeed * Time.deltaTime;
            //Vector3 newPosition = transform.position + deltaX + deltaY;
            
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = movement * moveSpeed;
    }
}
