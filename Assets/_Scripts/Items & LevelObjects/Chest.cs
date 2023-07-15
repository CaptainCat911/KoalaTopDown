using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : ItemPickUp
{
    [Header("������� ��� ������")]
    public GameObject itemToSpawn;

    BoxCollider2D boxCollider2d;
    Animator animator;
    bool isOpened;

     AudioSource audioSource; 

    public override void Awake()
    {
        boxCollider2d = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
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
    }
}
