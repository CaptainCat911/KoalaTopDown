using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Optimiser : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.name);

        PreFabOptimaser preFabOptimaser = collision.GetComponentInChildren<PreFabOptimaser>();      // ищем оптимизатор
        
        if (preFabOptimaser)
        {
            preFabOptimaser.ActiveGo();         // если есть - включаем объект            
        }       
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Debug.Log(collision.name);

        PreFabOptimaser preFabOptimaser = collision.GetComponentInChildren<PreFabOptimaser>();

        if (preFabOptimaser)
        {
            preFabOptimaser.DisActiveGo();
        }                
    }
}
