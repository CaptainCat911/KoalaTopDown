using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
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
        if (GameManager.instance.keys > 0 && !isOpened)         // ���� ������ ������ 0 � ����� ��� �� �������
        {
            spriteRenderer.sprite = openedDoorSprite;           // ������ ������ �������� ����� �� ������ ��������
            boxCollider2D.enabled = false;                      // ������� ���������
            GameManager.instance.TakeKey(false);                // �������� ����
            isOpened = true;                                    // ����� �������
        }
    }
}
