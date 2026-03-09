using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class HintView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hintText;
    [SerializeField] private float fadeDuration = 0.15f;

    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Coroutine fadeRoutine;

    public bool IsVisible { get; private set; }
    
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();

        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
        IsVisible = false;
    }

    public void SetText(string text)
    {
        if (hintText != null)
            hintText.text = text;
    }
    
    public void SetScreenPosition(Vector2 screenPosition)
    {
        rectTransform.position = screenPosition;
    }
    
    public void Show()
    {
        if (IsVisible)
            return;
        
        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);
        
        fadeRoutine = StartCoroutine(FadeTo(1f));
        IsVisible = true;
    }

    public void Hide()
    {
        if (!IsVisible)
            return;
        
        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(FadeTo(0f));
        IsVisible = false;
    }

    private IEnumerator FadeTo(float targetAlpha)
    {
        float startAlpha = canvasGroup.alpha;
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float t = time / fadeDuration;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            yield return null;
        }

        canvasGroup.alpha = targetAlpha;

        if (Mathf.Approximately(targetAlpha, 0f))
            canvasGroup.alpha = targetAlpha;

        fadeRoutine = null;
    }
}
