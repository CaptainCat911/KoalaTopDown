using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreFabOptimaser : MonoBehaviour
{
    
    public GameObject go;
    public GameObject point;
    

    private void Awake()
    {
        go.SetActive(false);
    }

    private void Update()
    {
        if (!go)
        {            
            Destroy(point, 1);
            Destroy(gameObject, 2);
            return;
        }
    }

    private void FixedUpdate()
    {
        if (go)
        {
            point.transform.position = go.transform.position;
        }
    }

    public void ActiveGo()
    {
        StartCoroutine(EnterDelay());
    }

    public void DisActiveGo()
    {
        StartCoroutine(ExitDelay());
    }

    IEnumerator EnterDelay()
    {
        yield return new WaitForSeconds(1f);
        if (go)
            go.SetActive(true);
    }

    IEnumerator ExitDelay()
    {
        yield return new WaitForSeconds(1f);
        if (go)
            go.SetActive(false);
    }
}
