using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    //public bool rockDoor;
    public int doorTypeNumber;              // тип двери
    public Sprite openedDoorSprite;         // спрайт открыйтой двери
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
        if (GameManager.instance.keys[doorTypeNumber] > 0 && !isOpened)         // если ключей больше 0 и дверь ещё не открыта
        {
            OpenDoor();
            GameManager.instance.keys[doorTypeNumber]--;                        // забираем ключ
        }
    }

    public void OpenDoor()
    {
        spriteRenderer.sprite = openedDoorSprite;           // меняем спрайт закрытой двери на спрайт открытой
        boxCollider2D.enabled = false;                      // убираем коллайдер
        isOpened = true;                                    // дверь открыта
    }
}
