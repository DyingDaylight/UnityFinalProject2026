using System;
using UnityEngine;

public class PersistantSystem : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}