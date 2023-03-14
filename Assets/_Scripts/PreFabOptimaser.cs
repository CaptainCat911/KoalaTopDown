using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreFabOptimaser : MonoBehaviour
{
    
    public GameObject go;           // ссылка на объект оптимизации
    public GameObject point;        // точка для детектирования входа/выхода коллайдера
    public GameObject prefab;       // весь префаб
    

    private void Awake()
    {
        DisActiveGo();              // при старте отключаем объект
    }

    private void Update()
    {
        if (!go)                        // если объект разрушен    
        {            
            Destroy(gameObject, 1);     // через секунду убираем этот объек с этим скриптом
            Destroy(prefab, 2);         // через 2 секунды убираем весь префаб
            return;
        }
    }

    private void FixedUpdate()
    {
        if (go)
        {
            point.transform.position = go.transform.position;       // перемещаем точку с объектом
        }
    }

    public void ActiveGo()
    {
        if (go)
            go.SetActive(true);
    }

    public void DisActiveGo()
    {
        if (go)
            go.SetActive(false);
    }



    IEnumerator EnterDelay()
    {
        yield return new WaitForSeconds(1f);

    }

    IEnumerator ExitDelay()
    {
        yield return new WaitForSeconds(1f);

    }
}
