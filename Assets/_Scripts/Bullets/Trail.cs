using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trail : MonoBehaviour
{    
    void Start()
    {
        Destroy(gameObject, 0.5f);
    }
}
