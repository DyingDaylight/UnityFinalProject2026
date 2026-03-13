using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapFader : MonoBehaviour
{
    public Tilemap wallTilemap;
    private int playersInside = 0; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playersInside++;
            UpdateVisibility();

            Chest[] allChests = Object.FindObjectsByType<Chest>(FindObjectsSortMode.None);
            foreach (Chest chest in allChests)
            {
                chest.ShowHintManually();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playersInside--;
            if (playersInside < 0) playersInside = 0; 
            UpdateVisibility();

            Chest[] allChests = Object.FindObjectsByType<Chest>(FindObjectsSortMode.None);
            foreach (Chest chest in allChests)
            {
                chest.HideHintManually();
            }
        }
    }

    private void UpdateVisibility()
    {
        if (playersInside > 0)
        {
            
            wallTilemap.color = new Color(1f, 1f, 1f, 0f);
        }
        else
        {
            
            wallTilemap.color = new Color(1f, 1f, 1f, 1f);
        }
    }
}