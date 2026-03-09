using UnityEngine;

[CreateAssetMenu(menuName = "Quests/Quest Data")]
public class QuestData : ScriptableObject
{
    public string questId;

    [Header("Dialogue")]
    [TextArea] public string startDialogue;
    [TextArea] public string inProgressDialogue;
    [TextArea] public string readyDialogue;
    [TextArea] public string completedDialogue;

    [Header("Objective")]
    public string objectiveDescription;
    
    [Header("Availability")]
    public QuestAvailability availability = QuestAvailability.Always;
    [TextArea] public string unavailableDialogue;
    
    public bool IsAvailableNow()
    {
        if (TimeManager.Instance == null)
            return true;

        return IsAvailableAt(TimeManager.Instance.CurrentTimeOfDay);
    }
    
    public bool IsAvailableAt(DayTime timeOfDay)
    {
        switch (availability)
        {
            case QuestAvailability.Always:
                return true;

            case QuestAvailability.DayOnly:
                return timeOfDay == DayTime.Day;

            case QuestAvailability.NightOnly:
                return timeOfDay == DayTime.Night;

            default:
                return true;
        }
    }
}
