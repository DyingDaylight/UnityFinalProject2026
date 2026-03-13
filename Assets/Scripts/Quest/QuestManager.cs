using System;
using System.Collections.Generic;
using Quest;
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
    
    public void CheckQuestItem(string itemId)
    {
        foreach (var quest in quests.Values)
        {
            if (quest.State == QuestState.InProgress &&
                quest.Data.requiredItem == itemId)
            {
                quest.MarkReadyToComplete();
            }
        }
    }

    public QuestTurnInResult TryCompleteQuest(QuestData questData)
    {
        QuestInstance quest = GetQuest(questData);
        
        if (quest.State != QuestState.InProgress)
            return QuestTurnInResult.MissingItem;
        
        if (!InventoryManager.Instance.HasAnyItem(questData.requiredCategory))
            return QuestTurnInResult.MissingItem;
        
        if (!InventoryManager.Instance.HasItem(
                questData.requiredCategory,
                questData.requiredItem,
                questData.requiredItemCount))
        {
            return QuestTurnInResult.WrongItem;
        }
        
        InventoryManager.Instance.RemoveItem(
            questData.requiredCategory,
            questData.requiredItem,
            questData.requiredItemCount);

        quest.CompleteQuest();

        return QuestTurnInResult.Completed;
    }
    
    public bool ArePrerequisitesCompleted(QuestData questData)
    {
        if (questData.requiredQuests == null)
            return true;

        foreach (QuestData prereq in questData.requiredQuests)
        {
            QuestInstance q = GetQuest(prereq);

            if (q.State != QuestState.Completed)
                return false;
        }

        return true;
    }
    
    public bool HasActiveQuest()
    {
        foreach (QuestInstance quest in quests.Values)
        {
            if (quest.State == QuestState.InProgress || quest.State == QuestState.ReadyToComplete)
                return true;
        }

        return false;
    }
}
