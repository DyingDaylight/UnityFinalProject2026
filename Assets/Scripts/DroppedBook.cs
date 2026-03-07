using UnityEngine;

public class DroppedBook : MonoBehaviour
{
    // The data container for this specific book
    private BookData data;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        // Get the renderer to apply the sprite from data
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Method to initialize the book with specific data
    public void Setup(BookData bookData)
    {
        data = bookData;
        
        // If you have a specific sprite for the world, apply it here
        if (data.coverImage != null)
        {
            spriteRenderer.sprite = data.coverImage;
        }
    }

    private void OnMouseDown()
    {
        // Here we will call the UI Manager to show the book content
        Debug.Log("Reading: " + data.bookTitle);
        
        // For now, let's just destroy the object like we "picked it up"
        // Destroy(gameObject);
    }
}