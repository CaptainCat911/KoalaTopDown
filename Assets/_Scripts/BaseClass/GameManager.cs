using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;         // �������

    [Header("������")]
    public Player player;                       // ������ �� ������

    [Header("��������")]
    public int keys;                            // �����

    private void Awake()
    {
        instance = this;
    }

    public void TakeKey(bool findKey)
    {
        if (findKey)
            keys++;
        else if (!findKey && keys > 0)
            keys--;
    }
}
