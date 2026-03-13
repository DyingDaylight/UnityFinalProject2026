using DayNight;
using Quest;
using UnityEngine;

[CreateAssetMenu(menuName = "Quests/Quest Data")]
public class QuestData : ScriptableObject
{
    public string questId;

    [Header("Prerequisites")]
    public QuestData[] requiredQuests;
    
    [Header("Dialogue")]
    [TextArea] public string startDialogue;
    [TextArea] public string inProgressDialogue;
    [TextArea] public string wrongItemDialogue;
    [TextArea] public string readyDialogue;
    [TextArea] public string completedDialogue;

    [Header("Unavailable Dialogues")]
    [SerializeField][TextArea] private string prerequisitesBlockedDialogue;
    [SerializeField][TextArea] private string activeQuestBlockedDialogue;
    
    [Header("Required Item")]
    public ItemCategory requiredCategory;
    public string requiredItem;
    public int requiredItemCount = 1;
    
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
    
    public string GetPrerequisitesBlockedDialogue()
    {
        return string.IsNullOrWhiteSpace(prerequisitesBlockedDialogue)
            ? "You should help someone else first."
            : prerequisitesBlockedDialogue;
    }

    public string GetActiveQuestBlockedDialogue()
    {
        return string.IsNullOrWhiteSpace(activeQuestBlockedDialogue)
            ? "Finish your current task first."
            : activeQuestBlockedDialogue;
    }
}
