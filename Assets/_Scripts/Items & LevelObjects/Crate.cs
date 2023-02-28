using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : Fighter
{
    //public bool isSpawnItem;
    public GameObject itemToSpawn;
    public GameObject expEffect;
    protected override void Death()
    {
        if(itemToSpawn)
            Instantiate(itemToSpawn, transform.position, Quaternion.identity);                  // создаем предмет
        GameObject effect = Instantiate(expEffect, transform.position, Quaternion.identity);    // создаем эффект
        Destroy(effect, 0.5f);                                                                  // уничтожаем эффект через .. сек     
        Destroy(gameObject);                                                                    // уничтожаем пулю
    }
}

