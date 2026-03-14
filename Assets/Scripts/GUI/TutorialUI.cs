using System;
using System.Collections;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    private const string Key = "TutorialShown";

    [SerializeField] private float duration = 5f;
    
    void Start()
    {
        //PlayerPrefs.DeleteKey("TutorialShown");
        if (PlayerPrefs.GetInt(Key, 0) == 1)
            return;
        
        HintManager.Instance.ShowTutorial("Move with WASD", duration);
        
        PlayerPrefs.SetInt(Key, 1);
        PlayerPrefs.Save();
    }
}
