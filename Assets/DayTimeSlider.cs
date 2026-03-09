using System;
using UnityEngine;
using UnityEngine.UI;

public class DayTimeSlider : MonoBehaviour
{

    [SerializeField] private Slider slider;
    [SerializeField] private Image handleImage;
    [SerializeField] private Image fillImage;
    [SerializeField] private Color dayColor = new Color(1f, 0.8f, 0.2f);
    [SerializeField] private Color nightColor = new Color(0.3f, 0.4f, 0.9f);

    [Header("Icons")]
    [SerializeField] private Sprite sunIcon;
    [SerializeField] private Sprite moonIcon;

    private DayTime currentTime;

    private void Start()
    {
        if (TimeManager.Instance == null)
            return;

        currentTime = TimeManager.Instance.CurrentTimeOfDay;
        UpdateIcon();
    }

    private void Update()
    {
        if (TimeManager.Instance == null)
            return;

        float progress = TimeManager.Instance.CurrentPhaseProgress();
        
        DayTime time = TimeManager.Instance.CurrentTimeOfDay;

        slider.value = time == DayTime.Day
            ? progress
            : 1f - progress;
        
        if (time == DayTime.Day)
            fillImage.color = Color.Lerp(dayColor, nightColor, slider.value);
        else
            fillImage.color = Color.Lerp(dayColor, nightColor, slider.value);
        
        if (time != currentTime)
        {
            currentTime = time;
            UpdateIcon();
        }
    }
    
    private void UpdateIcon()
    {
        handleImage.sprite = currentTime == DayTime.Day
            ? sunIcon
            : moonIcon;
    }
}
