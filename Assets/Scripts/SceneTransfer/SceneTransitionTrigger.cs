using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionTrigger : MonoBehaviour
{
    [Header("Destination Settings")]
    [SerializeField] private string targetScene; 
    [SerializeField] private string targetSpawn; 
    
    private float entryDelay = 1.0f; 
    private float timer = 0f;
    
    private void Start()
    {
        timer = entryDelay;
    }

    private void Update()
    {
        if (timer > 0) timer -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;
        
        if (timer > 0) 
        {
            Debug.Log("Teleport on cooldown: " + timer);
            return;
        }

        if (SceneTransferManager.Instance != null)
        {
            SceneTransferManager.Instance.librarySpawn = targetSpawn;
        }

        SceneManager.LoadScene(targetScene);
    }
}