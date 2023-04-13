using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    public static GameAssets instance;         // инстанс

    [Header("“екст")]
    public Transform chatBubblePrefab;
    public GameObject floatingText;
    public GameObject floatingMessage;

    [Header("Ёффекты")]
    public GameObject explousionStaffEffect;



    private void Awake()
    {
        instance = this;
    }
}
