using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    // Singleton instance to allow easy access from other scripts
    public static UIManager Instance;

    [Header("UI References")]
    public GameObject bookUIPanel;      // The main container
        // public TextMeshProUGUI titleText;   // TMP for the book's title
    public TextMeshProUGUI contentText; // TMP for the book's story

    private void Awake()
    {
        // Simple Singleton pattern
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // This method fills the UI with data from our ScriptableObject
    public void OpenBook(BookData data)
    {
        bookUIPanel.SetActive(true);
        contentText.text = data.pageContent;

        // Pause the game time so players can read in peace
        Time.timeScale = 0f;
    }

    // Resumes the game and hides the UI
    public void CloseBook()
    {
        bookUIPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}