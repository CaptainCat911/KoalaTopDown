using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    public static GameAssets instance;         // �������

    public Transform chatBubblePrefab;

    private void Awake()
    {
        instance = this;
    }
}