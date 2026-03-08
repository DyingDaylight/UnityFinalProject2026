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
        if (UIManager.Instance != null)
        {
            // Pass 'data' AND 'this.gameObject' to the manager
            UIManager.Instance.OpenBook(data, this.gameObject);
        }
    }
}