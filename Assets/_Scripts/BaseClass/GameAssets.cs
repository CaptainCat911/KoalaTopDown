using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    public static GameAssets instance;         // �������

    [Header("�����")]
    public Transform chatBubblePrefab;
    public GameObject floatingText;

    [Header("�������")]
    public GameObject explousionStaffEffect;



    private void Awake()
    {
        instance = this;
    }
}
