using System;
using System.Collections.Generic;
using UnityEngine;


public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    private Dictionary<string, QuestInstance> quests = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public QuestInstance GetQuest(QuestData data)
    {
        if (!quests.TryGetValue(data.questId, out var quest))
        {
            quest = new QuestInstance(data);
            quests.Add(data.questId, quest);
        }

        return quest;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            foreach (var quest in quests.Values)
            {
                if (quest.State == QuestState.InProgress)
                {
                    quest.MarkReadyToComplete();
                    Debug.Log($"Quest {quest.Data.questId} is now ReadyToComplete");
                }
            }
        }
    }
}
