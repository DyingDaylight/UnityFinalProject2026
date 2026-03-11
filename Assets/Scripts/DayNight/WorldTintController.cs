using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DayNight
{
    public class WorldTintController : MonoBehaviour
    {
    
        [SerializeField] private Color dayColor = new Color(1f, 1f, 1f, 0f);
        [SerializeField] private Color nightColor = new Color(0.069f, 0.069f, 0.3f, 0.8f);
        [SerializeField] private float transitionDuration = 3f;
    
        private Image worldTintOverlay;
        private DayTime currentTimeOfDay;
        private Coroutine transitionRoutine;

        private void Awake()
        {
            worldTintOverlay = GetComponent<Image>();
        }

        private void Start()
        {
            currentTimeOfDay = TimeManager.Instance.CurrentTimeOfDay;
            ApplyTimeOfDay();
        }

        private void Update()
        {
            if (TimeManager.Instance == null)
                return;

            if (currentTimeOfDay == TimeManager.Instance.CurrentTimeOfDay)
                return;

            currentTimeOfDay = TimeManager.Instance.CurrentTimeOfDay;
            ApplyTimeOfDay();
        }

        private void ApplyTimeOfDay()
        {
            Color targetColor = currentTimeOfDay == DayTime.Day
                ? dayColor
                : nightColor;
        
            if (transitionRoutine != null)
                StopCoroutine(transitionRoutine);

            transitionRoutine = StartCoroutine(FadeOverlay(targetColor));
        }
    
        private IEnumerator FadeOverlay(Color targetColor)
        {
            Color startColor = worldTintOverlay.color;
            float time = 0f;

            while (time < transitionDuration)
            {
                time += Time.deltaTime;
                float t = time / transitionDuration;

                worldTintOverlay.color = Color.Lerp(startColor, targetColor, t);

                yield return null;
            }

            worldTintOverlay.color = targetColor;
        }
    }
}
