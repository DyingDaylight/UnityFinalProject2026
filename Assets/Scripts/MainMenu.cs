using UnityEngine;
using UnityEngine.SceneManagement;

public partial class MainMenu : MonoBehaviour
{
    [Header("Scene Settings")]
    [SerializeField] private string firstSceneName = "HubScene";
    [SerializeField] private string startingSpawnPoint = "StartGamePoint";

    public void PlayGame()
    {
        if (SceneTransferManager.Instance != null)
        {
            SceneTransferManager.Instance.librarySpawn = startingSpawnPoint;
        }

        SceneManager.LoadScene(firstSceneName);
    }

    public void ExitGame()
    {
        Debug.Log("Game Exited");
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}