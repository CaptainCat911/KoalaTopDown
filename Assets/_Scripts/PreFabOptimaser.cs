using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreFabOptimaser : MonoBehaviour
{
    
    public GameObject go;           // ссылка на объект оптимизации
    public GameObject point;        // точка для детектирования входа/выхода коллайдера
    public GameObject prefab;       // весь префаб
    public bool pointMove;

    PrefabBotSettings prefabBotSettings;        // установки настроек бота в префабе


    private void Awake()
    {
        DisActiveGo();              // при старте отключаем объект
        prefabBotSettings = GetComponentInParent<PrefabBotSettings>();
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
        if (go && !pointMove)
        {
            point.transform.position = go.transform.position;       // перемещаем точку с объектом
        }
    }

    public void ActiveGo()
    {        
        //Debug.Break();

        if (go)
        {
            go.SetActive(true);
            if(prefabBotSettings)
                prefabBotSettings.SetSettingsBot();         // устанавливаем настройки префаба
            //Debug.Log("Active!");
        }
    }

    public void DisActiveGo()
    {
        //Debug.Break();

        if (go)
        {
            go.SetActive(false);
            //Debug.Log("Disactive!");
        }
    }


/*    IEnumerator EnterDelay()
    {
        yield return new WaitForSeconds(1f);

    }

    IEnumerator ExitDelay()
    {
        yield return new WaitForSeconds(1f);

    }*/
}
