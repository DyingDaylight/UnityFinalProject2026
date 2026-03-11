using UnityEngine;

public class Chest : MonoBehaviour
{
    [Header("Settings")]
    public BookData bookInside;
    public GameObject bookPrefab;

    [Header("Audio")]
    [SerializeField] private AudioClip openSound; 
    [SerializeField] private float volume = 0.7f;

    private AudioSource audioSource;
    private bool isOpened = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

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

        if (openSound != null)
        {
            audioSource.PlayOneShot(openSound, volume);
        }
        
        Vector3 spawnPosition = transform.position + new Vector3(0, 0.5f, 0);
        GameObject newBook = Instantiate(bookPrefab, spawnPosition, Quaternion.identity);
        
        newBook.GetComponent<DroppedBook>().Setup(bookInside);
        
        Rigidbody2D rb = newBook.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            float randomX = Random.Range(-2f, 2f); 
            Vector2 jumpForce = new Vector2(randomX, 1.5f); 
            
            rb.AddForce(jumpForce, ForceMode2D.Impulse);
        }
        
    }
}