using UnityEngine;

public class NPCController : MonoBehaviour
{
    private enum NPCState
    {
        Patrolling,
        WaitingForPlayer,
        GivingQuest
    }
    
    [Header("Patrol")]
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float arriveDistance = 0.05f;

    [Header("Day / Night")]
    [SerializeField] private float daySpeed = 1.5f;
    [SerializeField] private float nightSpeed = 3f;
    [SerializeField] private Color dayTint = Color.white;
    [SerializeField] private Color nightTint = Color.gray;
    
    [Header("Player Detection")]
    [SerializeField] private Transform player;
    [SerializeField] private float stopDistance = 1.2f;
    
    [Header("Quest")]
    [SerializeField] private QuestData questData;
    
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Transform target;

    private NPCState state = NPCState.Patrolling;
    
    private Vector2 lastMoveDirection = Vector2.down;
    private string currentAnimation;
    private Vector3 originalScale;
    private float moveSpeed;
    
    private DayTime currentTimeOfDay;
    
    private bool playerInRange;
    
    private void Start()
    {
        target = pointA;
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
        UpdateVisual(false);
        originalScale = transform.localScale;
        
        currentTimeOfDay = TimeManager.Instance.CurrentTimeOfDay;
        ApplyTimeOfDaySettings();
    }
    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
    
    private void Update()
    {
        CheckTimeOfDay();
        UpdateState();
        
        if (state == NPCState.WaitingForPlayer && playerInRange)
        {
            if (Input.GetButtonDown("Interact"))
            {
                GiveQuest();
            }
        }

        switch (state)
        {
            case NPCState.Patrolling:
                Patrol();
                break;

            case NPCState.WaitingForPlayer:
                UpdateVisual(false);
                break;
        }
    }
    
    private void UpdateState()
    {
        if (player == null)
            return;

        float distance = Vector2.Distance(transform.position, player.position);

        playerInRange = distance <= stopDistance;

        if (playerInRange)
        {
            ChangeState(NPCState.WaitingForPlayer);
        }
        else if (!playerInRange)
        {
            ChangeState(NPCState.Patrolling);
        }
    }
    
    private void ChangeState(NPCState newState)
    {
        if (state == newState)
            return;

        if (state == NPCState.WaitingForPlayer)
            HintManager.Instance.HideInteraction();

        state = newState;

        if (state == NPCState.WaitingForPlayer)
        {
            if (CanInteract())
                HintManager.Instance.ShowInteraction(transform, "[E]");
            else
                HintManager.Instance.HideInteraction();
        }

        if (state == NPCState.Patrolling)
            DialogueSystem.Instance.EndDialogue();
    }
    
    private void Patrol()
    {
        if (target == null)
            return;

        Vector2 position = transform.position;
        Vector2 targetPos = target.position;

        Vector2 direction = (targetPos - position).normalized;

        if (direction.sqrMagnitude > 0.001f)
        {
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                lastMoveDirection = new Vector2(Mathf.Sign(direction.x), 0);
            else
                lastMoveDirection = new Vector2(0, Mathf.Sign(direction.y));
        }

        Vector2 newPos = Vector2.MoveTowards(position, targetPos, moveSpeed * Time.deltaTime);
        transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);

        UpdateVisual(true);

        float distance = Vector2.Distance(transform.position, target.position);

        if (distance <= arriveDistance)
            target = target == pointA ? pointB : pointA;
    }

    
    private void UpdateVisual(bool isMoving)
    {
        string nextAnimation;

        if (!isMoving)
        {
            nextAnimation = "Idle";
        }
        else if (Mathf.Abs(lastMoveDirection.x) > 0)
        {
            transform.localScale = new Vector3(
                lastMoveDirection.x < 0 ? originalScale.x : originalScale.x * -1,
                originalScale.y,
                originalScale.x);
            nextAnimation = "WalkSide";
        }
        else if (lastMoveDirection.y > 0)
        {
            nextAnimation = "WalkUp";
        }
        else
        {
            nextAnimation = "WalkDown";
        }

        if (currentAnimation == nextAnimation)
            return;

        animator.Play(nextAnimation);
        currentAnimation = nextAnimation;
    }
    
    private void GiveQuest()
    {
        if (!DialogueSystem.Instance.IsDialogueActive())
        {
            HandleQuestInteraction();
        }
        else
        {
            DialogueSystem.Instance.EndDialogue();
        }
    }
    
    private void HandleQuestInteraction()
    {
        if (questData == null)
            return;

        QuestInstance quest = QuestManager.Instance.GetQuest(questData);

        switch (quest.State)
        {
            case QuestState.NotStarted:
                if (!questData.IsAvailableNow())
                {
                    DialogueSystem.Instance.StartDialogue(questData.unavailableDialogue);
                    break;
                }
                
                DialogueSystem.Instance.StartDialogue(questData.startDialogue);
                quest.StartQuest();
                break;

            case QuestState.InProgress:
                DialogueSystem.Instance.StartDialogue(questData.inProgressDialogue);
                break;

            case QuestState.ReadyToComplete:
                DialogueSystem.Instance.StartDialogue(questData.readyDialogue);
                quest.CompleteQuest();
                break;

            case QuestState.Completed:
                DialogueSystem.Instance.StartDialogue(questData.completedDialogue);
                break;
        }
    }
    
    private void CheckTimeOfDay()
    {
        if (TimeManager.Instance == null)
            return;

        DayTime newTimeOfDay = TimeManager.Instance.CurrentTimeOfDay;

        if (newTimeOfDay == currentTimeOfDay)
            return;

        currentTimeOfDay = newTimeOfDay;
        ApplyTimeOfDaySettings();
    }
    
    private void ApplyTimeOfDaySettings()
    {
        if (spriteRenderer != null)
        {
            if (currentTimeOfDay == DayTime.Day)
                spriteRenderer.color = dayTint;
            else
                spriteRenderer.color = nightTint;
        }

        if (currentTimeOfDay == DayTime.Day)
            moveSpeed = daySpeed;
        else
            moveSpeed = nightSpeed;
    }
    
    private bool CanInteract()
    {
        if (questData == null)
            return true;

        QuestInstance quest = QuestManager.Instance.GetQuest(questData);

        switch (quest.State)
        {
            case QuestState.NotStarted:
                return questData.IsAvailableNow();

            case QuestState.InProgress:
            case QuestState.ReadyToComplete:
                return true;

            case QuestState.Completed:
                return false;

            default:
                return false;
        }
    }
}
