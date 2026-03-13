using System;
using UnityEngine;

public class QuestPortalController : MonoBehaviour
{
    [SerializeField] private Collider2D portalCollider;

    private void Awake()
    {
        portalCollider =  GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (QuestManager.Instance == null)
            return;

        bool shouldBeAvailable = QuestManager.Instance.HasActiveQuest();

        if (portalCollider != null)
            portalCollider.enabled = shouldBeAvailable;
    }
}
