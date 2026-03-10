using UnityEngine;

public class SceneTransferManager : MonoBehaviour
{
    public static SceneTransferManager Instance;

    public string librarySpawn;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}