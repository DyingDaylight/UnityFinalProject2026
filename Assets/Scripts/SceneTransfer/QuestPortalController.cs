using System;
using UnityEngine;

public class QuestPortalController : MonoBehaviour
{
    [SerializeField] private QuestData quest;
    [SerializeField] private Collider2D portalCollider;

    private void Awake()
    {
        portalCollider =  GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (QuestManager.Instance == null || quest == null)
            return;

        QuestInstance q = QuestManager.Instance.GetQuest(quest);

        bool shouldBeAvailable = q.State == QuestState.InProgress;

        if (portalCollider != null)
            portalCollider.enabled = shouldBeAvailable;
    }
}
