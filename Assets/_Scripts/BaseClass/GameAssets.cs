using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    public static GameAssets instance;         // инстанс

    [Header("Префабы")]
    public GameObject fireBallSmall;
    public GameObject fireBallBig;

    [Header("Текст")]
    public Transform chatBubblePrefab;
    public GameObject floatingText;
    public GameObject floatingMessage;

    [Header("Эффекты")]
    public GameObject explousionRedEffect;
    public GameObject explousionGravity;
    public GameObject explousionTeleportIn;
    public GameObject explousionTeleportOut;



    private void Awake()
    {
        instance = this;
    }
}
