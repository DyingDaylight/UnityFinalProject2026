using System;
using UnityEngine;
using UnityEngine.UI;

public class WorldTintController : MonoBehaviour
{
    
    [SerializeField] private Color dayColor = new Color(1f, 1f, 1f, 0f);
    [SerializeField] private Color nightColor = new Color(0.069f, 0.069f, 0.3f, 0.8f);

    private Image worldTintOverlay;
    private DayTime currentTimeOfDay;

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
        if (currentTimeOfDay == DayTime.Day)
            worldTintOverlay.color = dayColor;
        else
            worldTintOverlay.color = nightColor;
    }
}
