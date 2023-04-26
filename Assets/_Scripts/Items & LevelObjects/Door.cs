using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    //public bool rockDoor;
    public int doorTypeNumber;              // ��� �����
    public Sprite openedDoorSprite;         // ������ ��������� �����
    SpriteRenderer spriteRenderer;
    BoxCollider2D boxCollider2D;
    bool isOpened;
    

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    public void OpenDoorWithKey()
    {   
        if (GameManager.instance.keys[doorTypeNumber] > 0 && !isOpened)         // ���� ������ ������ 0 � ����� ��� �� �������
        {
            OpenDoor();
            GameManager.instance.keys[doorTypeNumber]--;                        // �������� ����
        }
    }

    public void OpenDoor()
    {
        spriteRenderer.sprite = openedDoorSprite;           // ������ ������ �������� ����� �� ������ ��������
        boxCollider2D.enabled = false;                      // ������� ���������
        isOpened = true;                                    // ����� �������
    }
}
