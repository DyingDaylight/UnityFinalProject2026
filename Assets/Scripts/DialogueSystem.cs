using UnityEngine;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    public static DialogueSystem Instance { get; private set; }
    
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    
    private bool isDialogueActive;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    
    public void StartDialogue(string text)
    {
        dialoguePanel.SetActive(true);
        dialogueText.text = text;

        isDialogueActive = true;
    }

    public void EndDialogue()
    {
        if (!isDialogueActive)
            return;
        
        dialoguePanel.SetActive(false);
        isDialogueActive = false;
    }

    public bool IsDialogueActive()
    {
        return isDialogueActive;
    }
}
