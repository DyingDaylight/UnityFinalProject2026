using UnityEngine;

public class NPCPatrol : MonoBehaviour
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
    [SerializeField] private float speed = 1.5f;
    [SerializeField] private float arriveDistance = 0.05f;

    [Header("Player Detection")]
    [SerializeField] private Transform player;
    [SerializeField] private float stopDistance = 1.2f;
    
    [Header("Quest")]
    [SerializeField] private QuestData questData;
    
    private Animator animator;
    private Transform target;

    private NPCState state = NPCState.Patrolling;
    
    private Vector2 lastMoveDirection = Vector2.down;
    private string currentAnimation;
    private Vector3 originalScale;
    
    private bool playerInRange;
    
    private void Start()
    {
        target = pointA;
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
        UpdateVisual(false);
        originalScale = transform.localScale;
    }
    
    private void Awake()
    { 
        animator = GetComponent<Animator>();
    }
    
    private void Update()
    {
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
            HintManager.Instance.ShowInteraction(transform, "[E]");
        
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

        Vector2 newPos = Vector2.MoveTowards(position, targetPos, speed * Time.deltaTime);
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
}
