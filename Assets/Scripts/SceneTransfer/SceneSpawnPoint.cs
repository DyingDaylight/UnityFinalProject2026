using UnityEngine;
using System.Collections; // Required for Coroutines

public class SceneSpawnPoint : MonoBehaviour
{
    public string spawnPointName; 

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        yield return null; 

        if (SceneTransferManager.Instance != null && SceneTransferManager.Instance.librarySpawn == spawnPointName)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                player.transform.position = transform.position;
            }
        }
    }
}