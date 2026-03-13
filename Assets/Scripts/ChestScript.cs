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
        Debug.Log("Chest");
        if (!isOpened)
        {
            OpenChest();
            Debug.Log("Chest opened");
        }
    }
    public void ShowHintManually()
    {
        if (!isOpened && HintManager.Instance != null)
        {
            HintManager.Instance.ShowInteraction(transform, "Open Chest (Left click)");
        }
    }

    public void HideHintManually()
    {
        if (HintManager.Instance != null)
        {
            HintManager.Instance.HideInteraction();
        }
    }
    private void OnMouseEnter()
    {
        if (!isOpened && HintManager.Instance != null)
        {
            HintManager.Instance.ShowInteraction(transform, "Open Chest (Left click)");
        }
    }

    private void OnMouseExit()
    {
        if (HintManager.Instance != null)
        {
            HintManager.Instance.HideInteraction();
        }
    }
    private void OpenChest()
    {
        if (HintManager.Instance != null) HintManager.Instance.HideInteraction();
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