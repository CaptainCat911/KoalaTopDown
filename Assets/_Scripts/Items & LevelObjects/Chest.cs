using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : ItemPickUp
{
    [Header("Предмет для спауна")]
    public GameObject itemToSpawn;

    BoxCollider2D boxCollider2d;
    Animator animator;

    public override void Awake()
    {
        boxCollider2d = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    public void OpenChest()
    {
        if (itemToSpawn)
            Instantiate(itemToSpawn, transform.position, Quaternion.identity);                  // создаем предмет  
        boxCollider2d.enabled = false;
        animator.SetTrigger("OpenChest");
    }
}
