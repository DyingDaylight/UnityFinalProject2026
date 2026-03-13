using System.Collections;
using UnityEngine;

public class HintManager : MonoBehaviour
{
    public static HintManager Instance { get; private set; }
    
    [Header("Hint Views")]
    [SerializeField] private HintView interactionHint;
    [SerializeField] private HintView tutorialHint;
    
    [Header("Dependencies")]
    [SerializeField] private DialogueSystem dialogueSystem;
    
    [Header("Interaction Hint")]
    [SerializeField] private Vector3 interactionOffset = new Vector3(0f, 1.2f, 0f);

    private Camera mainCamera;
    private Transform target;
    private Transform interactionTarget;
    private Coroutine tutorialRoutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        
        mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        UpdateInteractionHintPosition();
        UpdateInteractionHintVisibilityByDialogue();
    }
    
    public void ShowInteraction(Transform target, string text = "")
    {
        if (target == null) return;

        interactionTarget = target;

        string finalPoint = string.IsNullOrEmpty(text) || text == "E" ? "[E] Interact" : text;
    
        interactionHint.SetText(finalPoint);

        if (dialogueSystem != null && dialogueSystem.IsDialogueActive())
            return;

        interactionHint.Show();
    }
    
    public void HideInteraction()
    {
        interactionTarget = null;
        interactionHint.Hide();
    }

    public void ShowTutorial(string text, float duration)
    {
        tutorialHint.SetText(text);
        tutorialHint.Show();

        if (tutorialRoutine != null)
            StopCoroutine(tutorialRoutine);

        tutorialRoutine = StartCoroutine(HideTutorialAfterDelay(duration));
    }
    
    public void HideTutorial()
    {
        if (tutorialRoutine != null)
        {
            StopCoroutine(tutorialRoutine);
            tutorialRoutine = null;
        }

        tutorialHint.Hide();
    }
    
    private IEnumerator HideTutorialAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        tutorialHint.Hide();
        tutorialRoutine = null;
    }
    
    private void UpdateInteractionHintPosition()
    {
        if (interactionTarget == null || interactionHint == null || mainCamera == null)
            return;

        Vector3 screenPos = mainCamera.WorldToScreenPoint(interactionTarget.position + interactionOffset);
        interactionHint.SetScreenPosition(screenPos);
    }
    
    private void UpdateInteractionHintVisibilityByDialogue()
    {
        if (interactionHint == null)
            return;

        bool dialogueActive = dialogueSystem != null && dialogueSystem.IsDialogueActive();

        if (dialogueActive && interactionHint.IsVisible)
        {
            interactionHint.Hide();
        }
        else if (!dialogueActive && interactionTarget != null && !interactionHint.IsVisible)
        {
            interactionHint.Show();
        }
    }
}
