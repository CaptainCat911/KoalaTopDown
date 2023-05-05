using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public bool withAnimation;      // с анимацией выпадания
    public bool delayTake;          // с задержкой после анимации
    public GameObject trigger;  
    Animator itemAnimator;


    public virtual void Awake()
    {
        itemAnimator = GetComponent<Animator>();

        if (delayTake)
            trigger.SetActive(false);
    }

    public virtual void Start()
    {
        if (withAnimation)
            itemAnimator.SetTrigger("ItemDrop");
    }

    public void EnableTrigger()
    {
        trigger.SetActive(true);
    }

    public void DisableTrigger()
    {
        trigger.SetActive(false);
    }



    public virtual void OnTriggerEnter2D(Collider2D collision)
    {

    }
}
