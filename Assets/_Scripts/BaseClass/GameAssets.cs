using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    public static GameAssets instance;         // �������

    [Header("�������")]
    public GameObject fireBallSmall;
    public GameObject fireBallBig;

    [Header("�����")]
    public Transform chatBubblePrefab;
    public GameObject floatingText;
    public GameObject floatingMessage;

    [Header("�������")]
    public GameObject explousionRedEffect;
    public GameObject explousionBlue;
    public GameObject explousionGravity;
    public GameObject explousionTeleportIn;
    public GameObject explousionTeleportOut;
    public GameObject playerBlinkIn;
    public GameObject playerBlinkOut;



    private void Awake()
    {
        instance = this;
    }
}
