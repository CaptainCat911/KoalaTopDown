using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grave : Fighter
{
    [Header("Параметры предмета")]
    public GameObject itemToSpawn;
    public GameObject expEffect;
    SpriteRenderer spriteRenderer;
    //public SpriteRenderer spriteDestroyed;
    public Sprite spriteDestroyed;
    BoxCollider2D boxCollider;

    public override void Awake()
    {
        base.Awake();
        spriteRenderer = GetComponent<SpriteRenderer>();        
        boxCollider = GetComponent<BoxCollider2D>();
    }

    protected override void Death()
    {
        base.Death();
        if (itemToSpawn)
            Instantiate(itemToSpawn, transform.position, Quaternion.identity);                  // создаем предмет
        GameObject effect = Instantiate(expEffect, transform.position, Quaternion.identity);    // создаем эффект
        Destroy(effect, 0.5f);                                                                  // уничтожаем эффект через .. сек        
        spriteRenderer.sprite = spriteDestroyed;        // заменяем спрайт на разрушенный
        boxCollider.enabled = false;                    // убираем коллайдер
    }
}