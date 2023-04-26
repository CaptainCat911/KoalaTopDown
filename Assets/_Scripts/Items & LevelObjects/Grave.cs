using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grave : Fighter
{
    [Header("Параметры предмета")]
    public bool destroyed;
    public GameObject itemToSpawn;              // предмет для спауна
    public GameObject expEffect;                // эффект разрушения
    SpriteRenderer spriteRenderer;              // спрайт    
    public Sprite[] m_spriteDestroyed;          // массив разрушенных спрайтов
    BoxCollider2D boxCollider;

    public override void Awake()
    {
        base.Awake();
        spriteRenderer = GetComponent<SpriteRenderer>();        
        boxCollider = GetComponent<BoxCollider2D>();
        if (destroyed)
            Death();
    }



    protected override void Death()
    {
        base.Death();
        if (itemToSpawn)
            Instantiate(itemToSpawn, transform.position, Quaternion.identity);                  // создаем предмет
        GameObject effect = Instantiate(expEffect, transform.position, Quaternion.identity);    // создаем эффект
        Destroy(effect, 0.5f);                                                                  // уничтожаем эффект через .. сек

        int spriteNumber = Random.Range(0, m_spriteDestroyed.Length);       // выбираем рандомно спрайт 
        spriteRenderer.sprite = m_spriteDestroyed[spriteNumber];                // заменяем спрайт на разрушенный
        boxCollider.enabled = false;                                            // убираем коллайдер
    }
}