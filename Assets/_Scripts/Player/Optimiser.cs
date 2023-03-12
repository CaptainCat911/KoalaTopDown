using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Optimiser : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);

        PreFabOptimaser preFabOptimaser = collision.GetComponentInParent<PreFabOptimaser>();
        
        if (preFabOptimaser)
        {
            preFabOptimaser.ActiveGo();
        }       
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log(collision.name);

        PreFabOptimaser preFabOptimaser = collision.GetComponentInParent<PreFabOptimaser>();

        if (preFabOptimaser)
        {
            preFabOptimaser.DisActiveGo();
        }                
    }
}
