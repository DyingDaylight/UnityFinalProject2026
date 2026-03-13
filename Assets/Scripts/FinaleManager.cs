using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinaleManager : MonoBehaviour
{
    public static FinaleManager Instance { get; private set; }

    [SerializeField] private QuestData[] requiredQuests;
    [SerializeField] private string creditsSceneName = "CreditsScene";
    [SerializeField] private float finaleDelay = 2f;

    private bool finaleTriggered;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void CheckFinale()
    {
        if (finaleTriggered || QuestManager.Instance == null)
            return;

        foreach (QuestData questData in requiredQuests)
        {
            QuestInstance quest = QuestManager.Instance.GetQuest(questData);

            if (quest == null || quest.State != QuestState.Completed)
                return;
        }

        finaleTriggered = true;
        Debug.Log("All quests completed!");
        StartCoroutine(LoadCreditsAfterDelay());
    }
    
    private IEnumerator LoadCreditsAfterDelay()
    {
        yield return new WaitForSeconds(finaleDelay);
        SceneManager.LoadScene(creditsSceneName);
    }
}
