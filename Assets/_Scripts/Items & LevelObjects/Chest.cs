using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : ItemPickUp
{
    BoxCollider2D boxCollider2d;
    Animator animator;
    AudioSource audioSource;
    SpriteRenderer spriteRenderer;
    bool isOpened;

    [Header("���������� ������")]
    public bool magicChest;
    public GameObject prefab;
    public ParticleSystem particleEffect;
    public GameObject startEffect;
    public GameObject endEffect;
    public float cooldown = 1f;                     // ����������� 
    float lastAttack;                               // ����� ��������� ����� �����

    [Header("������� ��� ������")]
    public GameObject itemToSpawn;      



    public override void Awake()
    {
        base.Awake();
        boxCollider2d = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public override void Start()
    {
        base.Start();
        if (magicChest)
        {
            GameObject effect = Instantiate(startEffect, transform.position, Quaternion.identity);          // ������� ������ 
            Destroy(effect, 1);
        }
    }

    private void Update()
    {
        if (magicChest && Time.time - lastAttack > cooldown)
        {
            lastAttack = Time.time;

            Color randomColor = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
            spriteRenderer.color = randomColor;
        }
    }

    private void OnEnable()
    {
        if (isOpened)
            animator.SetTrigger("OpenChest");
    }

    public void OpenChest()
    {
        if (itemToSpawn)
            Instantiate(itemToSpawn, transform.position, Quaternion.identity);                  // ������� �������  
        boxCollider2d.enabled = false;
        isOpened = true;

        if (audioSource)                    // ����
            audioSource.Play();

        animator.SetTrigger("OpenChest");

        if (magicChest)
        {
            Invoke(nameof(TeleportOutMagicChest), 1);         
        }
    }

    void TeleportOutMagicChest()        // ������� ��� ����������� �������
    {
        particleEffect.Stop();
        GameObject effect = Instantiate(endEffect, transform.position, Quaternion.identity);          // ������� ������
        Destroy(effect, 1);
        Destroy(gameObject);
        Destroy(prefab, 2);
    }
}
