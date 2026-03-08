using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI References")]
    public GameObject bookUIPanel;
    public TextMeshProUGUI contentText;

    // We store a reference to the book object currently being read
    private GameObject currentBookObject;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Updated OpenBook to accept the GameObject as well
    public void OpenBook(BookData data, GameObject bookObj)
    {
        currentBookObject = bookObj; // Remember which book is on the floor
        bookUIPanel.SetActive(true);
        contentText.text = data.pageContent;
        Time.timeScale = 0f;
    }

    public void CloseBook()
    {
        bookUIPanel.SetActive(false);
        Time.timeScale = 1f;
        currentBookObject = null;
    }

    // New method for the "Take" button
    public void TakeBook()
    {
        if (currentBookObject != null)
        {
            // Destroy the book from the world
            Destroy(currentBookObject);
        }
        
        // Close the UI and resume time
        CloseBook();
        
        Debug.Log("Book taken and removed from the map");
    }
}