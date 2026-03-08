using UnityEngine;

public class DroppedBook : MonoBehaviour
{
    private BookData data;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Setup(BookData bookData)
    {
        data = bookData;
        // Applying the specific sprite from your book asset pack
        if (data.coverImage != null)
        {
            spriteRenderer.sprite = data.coverImage;
        }
    }

    private void OnMouseDown()
    {
        // Check if the UI Manager is present and open the book
        if (UIManager.Instance != null)
        {
            UIManager.Instance.OpenBook(data);
            
            // Optional: Destroy the book on the floor after reading
            // Destroy(gameObject);
        }
    }
}