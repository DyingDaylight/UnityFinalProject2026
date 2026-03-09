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
}
