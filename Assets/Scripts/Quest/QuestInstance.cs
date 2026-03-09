public class QuestInstance
{
    public QuestData Data { get; private set; }
    public QuestState State { get; private set; }

    public QuestInstance(QuestData data)
    {
        Data = data;
        State = QuestState.NotStarted;
    }

    public void StartQuest()
    {
        State = QuestState.InProgress;
    }

    public void MarkReadyToComplete()
    {
        State = QuestState.ReadyToComplete;
    }

    public void CompleteQuest()
    {
        State = QuestState.Completed;
    }
}
