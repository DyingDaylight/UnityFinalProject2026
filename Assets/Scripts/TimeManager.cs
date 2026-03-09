using System;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;
    
    public DayTime CurrentTimeOfDay { get; private set; }
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
    }
    
    // TODO: temp method
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            ToggleDayTime();
        }
    }
    
    public void ToggleDayTime() 
    {
        CurrentTimeOfDay = CurrentTimeOfDay == DayTime.Day
            ? DayTime.Night
            : DayTime.Day;

        Debug.Log("Time of day changed to: " + CurrentTimeOfDay);
    }
}
