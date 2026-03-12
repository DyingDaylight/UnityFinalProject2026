using UnityEngine;

namespace DayNight
{
    public class TimeManager : MonoBehaviour
    {
        public static TimeManager Instance;

        [Header("Time Settings")] [SerializeField]
        private float dayDuration = 40f;

        [SerializeField] private float nightDuration = 40f;

        private float timer;

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
            timer += Time.deltaTime;

            float duration = CurrentTimeOfDay == DayTime.Day ? dayDuration : nightDuration;

            if (timer >= duration)
            {
                ToggleDayTime();
                timer = 0f;
            }
        
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
        }
    
        public float CurrentPhaseDuration()
        {
            return CurrentTimeOfDay == DayTime.Day ? dayDuration : nightDuration;
        }
    
        public float CurrentPhaseProgress()
        {
            return CurrentPhaseDuration() <= 0f ? 0f : timer / CurrentPhaseDuration();
        }

        public float TimeRemaining()
        {
            return CurrentPhaseDuration() - timer;
        }
    }
}
