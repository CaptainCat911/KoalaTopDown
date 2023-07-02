using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : ItemPickUp
{
    [Header("Предмет для спауна")]
    public GameObject itemToSpawn;

    BoxCollider2D boxCollider2d;
    Animator animator;
    bool isOpened;

    public override void Awake()
    {
        boxCollider2d = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        if (isOpened)
            animator.SetTrigger("OpenChest");
    }

    public void OpenChest()
    {
        if (itemToSpawn)
            Instantiate(itemToSpawn, transform.position, Quaternion.identity);                  // создаем предмет  
        boxCollider2d.enabled = false;
        isOpened = true;
        animator.SetTrigger("OpenChest");
    }
}
