using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3f;
    
    [Header("Audio Settings")]
    [SerializeField] private AudioClip[] stepSounds; 
    [SerializeField] private float stepVolume = 0.5f;
    
    // Components
    private Rigidbody2D rb;
    private Animator animator;
    private AudioSource audioSource;
    
    private Vector2 input;
    private Vector2 movement;
    private Vector3 originalScale;
    private string currentAnimation;
    private Vector2 lastMoveDirection = Vector2.down;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
    }

    void Start()
    {
        originalScale = transform.localScale;
    }
    
    void Update()
    {
        input.x = Input.GetAxisRaw(("Horizontal"));
        input.y = Input.GetAxisRaw(("Vertical"));
        
        // Do not move faster if going diagonally
        movement = input.normalized;
        
        // Consider the player moving if input vector length is above a small threshold (avoid noise)
        bool isMoving = input.sqrMagnitude > 0.01f;
        
        if (isMoving)
        {
            if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
                lastMoveDirection = new Vector2(Mathf.Sign(input.x), 0f); // moving horizontally
            else
                lastMoveDirection = new Vector2(0f, Mathf.Sign(input.y)); // moving vertically
        }
        
        animator.SetBool("IsMoving", isMoving);
        animator.SetFloat("LastMoveX", lastMoveDirection.x);
        animator.SetFloat("LastMoveY", lastMoveDirection.y);
        
        // update sprite to look in movement direction
        UpdateDirection(isMoving);
    }

    // Fixed update to count physics and collisions correctly
    private void FixedUpdate()
    {
        rb.linearVelocity = movement * moveSpeed;
    }

    // Called from animation events
    public void PlayFootstep()
    {
        if (stepSounds == null || stepSounds.Length == 0) return;
        
        int index = UnityEngine.Random.Range(0, stepSounds.Length);
        audioSource.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(stepSounds[index], stepVolume);
    }
    
    // Animations are updated in the code to avoid complicated uncomprehensible transitions
    // in the Animator graphical editor 
    private void UpdateDirection(bool isMoving)
    {
        string nextAnimation;

        if (lastMoveDirection.x > 0f)
        {
            // Look right
            transform.localScale = new Vector3(
                originalScale.x * -1,
                originalScale.y,
                originalScale.x);
            
            nextAnimation = isMoving ? "WalkSide" : "IdleSide";
        }
        else if (lastMoveDirection.x < 0f)
        {
            // look left
            transform.localScale = new Vector3(
                originalScale.x,
                originalScale.y,
                originalScale.x);

            nextAnimation = isMoving ? "WalkSide" : "IdleSide";
        }
        else if (lastMoveDirection.y > 0f)
        {
            // look up
            nextAnimation = isMoving ? "WalkUp" : "IdleUp";
        }
        else
        {
            // look down
            nextAnimation = isMoving ? "WalkDown" : "IdleDown";
        }

        if (currentAnimation == nextAnimation)
            return;

        animator.Play(nextAnimation);
        currentAnimation = nextAnimation;
    }
}
