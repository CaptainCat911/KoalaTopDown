using UnityEngine;
using UnityEngine.AI;

public class Grave : Fighter
{
    [Header("Параметры предмета")]
    public bool destroyed;                      // разрушен
    public bool withObstacle;                   // с навМешОбстикл
    public GameObject itemToSpawn;              // предмет для спауна
    public GameObject expEffect;                // эффект разрушения
    SpriteRenderer spriteRenderer;              // спрайт    
    public Sprite[] m_spriteDestroyed;          // массив разрушенных спрайтов
    BoxCollider2D boxCollider;


    AudioSource audioSource;

    public override void Awake()
    {
        base.Awake();
        spriteRenderer = GetComponent<SpriteRenderer>();        
        boxCollider = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
        if (destroyed)
            Death();
    }



    protected override void Death()
    {
        base.Death();
        if (itemToSpawn)
            Instantiate(itemToSpawn, transform.position, Quaternion.identity);                      // создаем предмет

        if (!destroyed)
        {
            GameObject effect = Instantiate(expEffect, transform.position, Quaternion.identity);    // создаем эффект
            Destroy(effect, 0.5f);                                                                  // уничтожаем эффект через .. сек
        }                                                                 

        if (audioSource && !destroyed)                  // звук
            audioSource.Play();

        int spriteNumber = Random.Range(0, m_spriteDestroyed.Length);           // выбираем рандомно спрайт 
        spriteRenderer.sprite = m_spriteDestroyed[spriteNumber];                // заменяем спрайт на разрушенный
        boxCollider.enabled = false;                                            // убираем коллайдер
        if (withObstacle)
        {
            GetComponent<NavMeshObstacle>().enabled = false;
        }
    }
}