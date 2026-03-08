using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 3f;
    
    private Rigidbody2D rb;
    private Animator animator;
    
    private Vector2 input;
    private Vector2 movement;
    private Vector2 lastMoveDirection = Vector2.down;
    
    private Vector3 originalScale;
    
    private string currentAnimation;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        originalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        input.x = Input.GetAxisRaw(("Horizontal"));
        input.y = Input.GetAxisRaw(("Vertical"));
        
        movement = input.normalized; // do not move faster by diagonal
        
        bool isMoving = input.sqrMagnitude > 0.01f;
        
        
        if (isMoving)
        {
            if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
                lastMoveDirection = new Vector2(Mathf.Sign(input.x), 0f);
            else
                lastMoveDirection = new Vector2(0f, Mathf.Sign(input.y));
        }
        
        animator.SetBool("IsMoving", isMoving);
        animator.SetFloat("LastMoveX", lastMoveDirection.x);
        animator.SetFloat("LastMoveY", lastMoveDirection.y);
        
        UpdateDirection(isMoving);
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = movement * moveSpeed;
    }

    private void UpdateDirection(bool isMoving)
    {
        string nextAnimation;

        if (lastMoveDirection.x > 0f)
        {
            transform.localScale = new Vector3(
                originalScale.x * -1,
                originalScale.y,
                originalScale.x);
            
            nextAnimation = isMoving ? "WalkSide" : "IdleSide";
        }
        else if (lastMoveDirection.x < 0f)
        {
            transform.localScale = new Vector3(
                originalScale.x,
                originalScale.y,
                originalScale.x);

            nextAnimation = isMoving ? "WalkSide" : "IdleSide";
        }
        else if (lastMoveDirection.y > 0f)
        {
            nextAnimation = isMoving ? "WalkUp" : "IdleUp";
        }
        else
        {
            nextAnimation = isMoving ? "WalkDown" : "IdleDown";
        }

        if (currentAnimation == nextAnimation)
            return;

        animator.Play(nextAnimation);
        currentAnimation = nextAnimation;
    }
}
