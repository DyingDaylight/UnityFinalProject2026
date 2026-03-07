using UnityEngine;

[CreateAssetMenu(fileName = "NewBook", menuName = "Library/Book")]
public class BookData : ScriptableObject
{
    [Header("Book Identity")]
    public string bookTitle;

    [Header("Visuals")]
    // This is for the big UI screen
    public Sprite coverImage;
    // ADD THIS: This is for the small book on the floor
    public Sprite worldIcon; 

    [Header("Content")]
    [TextArea(15, 30)]
    public string pageContent;
}