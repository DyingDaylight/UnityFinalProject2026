using UnityEngine;

public class Chest : MonoBehaviour
{
    [Header("Settings")]
    // Drag one of your 5 ScriptableObject files here in the Inspector
    public BookData bookInside;
    
    // Drag your DroppedBook_Prefab here
    public GameObject bookPrefab;
    
    private bool isOpened = false;

    private void OnMouseDown()
    {
        if (!isOpened)
        {
            OpenChest();
        }
    }

    private void OpenChest()
    {
        isOpened = true;
        
        // Spawn the book slightly above the chest
        Vector3 spawnPosition = transform.position + new Vector3(0, 0.5f, 0);
        GameObject newBook = Instantiate(bookPrefab, spawnPosition, Quaternion.identity);
        
        // Pass the data from this chest to the spawned book
        newBook.GetComponent<DroppedBook>().Setup(bookInside);
        
        // ADDED: Physical jump effect
        Rigidbody2D rb = newBook.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Create a random force: up and slightly to the left or right
            float randomX = Random.Range(-2f, 2f); 
            Vector2 jumpForce = new Vector2(randomX, 5f); 
            
            // Apply the force as an instant impulse
            rb.AddForce(jumpForce, ForceMode2D.Impulse);
        }
        
        Debug.Log("Chest opened! Found: " + bookInside.bookTitle);
    }
}