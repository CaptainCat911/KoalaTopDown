using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreFabOptimaser : MonoBehaviour
{
    
    public GameObject go;           // ������ �� ������ �����������
    public GameObject point;        // ����� ��� �������������� �����/������ ����������
    public GameObject prefab;       // ���� ������
    public bool pointMove;

    PrefabBotSettings prefabBotSettings;        // ��������� �������� ���� � �������


    private void Awake()
    {
        DisActiveGo();              // ��� ������ ��������� ������
        prefabBotSettings = GetComponentInParent<PrefabBotSettings>();
    }

    private void Update()
    {
        if (!go)                        // ���� ������ ��������    
        {            
            Destroy(gameObject, 1);     // ����� ������� ������� ���� ����� � ���� ��������
            Destroy(prefab, 2);         // ����� 2 ������� ������� ���� ������
            return;
        }
    }

    private void FixedUpdate()
    {
        if (go && !pointMove)
        {
            point.transform.position = go.transform.position;       // ���������� ����� � ��������
        }
    }

    public void ActiveGo()
    {        
        //Debug.Break();

        if (go)
        {
            go.SetActive(true);
            if(prefabBotSettings)
                prefabBotSettings.SetSettingsBot();         // ������������� ��������� �������
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
