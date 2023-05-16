using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    //public bool rockDoor;
    BoxCollider2D boxCollider2D;
    SpriteRenderer spriteRenderer;

    public bool openedDoor;
    public int doorTypeNumber;              // тип двери
    public Sprite openedDoorSprite;         // спрайт открыйтой двери
    Sprite closedDoorSprite;

    bool isOpened;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        closedDoorSprite = spriteRenderer.sprite;
    }


    private void Start()
    {
        if (openedDoor)
            OpenDoor();
    }

    public void OpenDoorWithKey()
    {   
        if (GameManager.instance.keys[doorTypeNumber] > 0 && !isOpened)         // если ключей больше 0 и дверь ещё не открыта
        {
            OpenDoor();
            GameManager.instance.keys[doorTypeNumber]--;                        // забираем ключ
        }
        else
        {
            //GameManager.instance.CreateFloatingMessage("Нужен ключ!", Color.white, transform.position);
        }
    }

    public void OpenDoor()
    {
        spriteRenderer.sprite = openedDoorSprite;           // меняем спрайт закрытой двери на спрайт открытой
        boxCollider2D.enabled = false;                      // убираем коллайдер
        isOpened = true;                                    // дверь открыта
    }

    public void CloseDoor()
    {
        spriteRenderer.sprite = closedDoorSprite;           // меняем спрайт закрытой двери на спрайт открытой
        boxCollider2D.enabled = true;                      // убираем коллайдер
        isOpened = false;                                    // дверь открыта
    }
}
