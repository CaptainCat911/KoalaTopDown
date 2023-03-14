using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreFabOptimaser : MonoBehaviour
{
    
    public GameObject go;           // ������ �� ������ �����������
    public GameObject point;        // ����� ��� �������������� �����/������ ����������
    public GameObject prefab;       // ���� ������
    

    private void Awake()
    {
        DisActiveGo();              // ��� ������ ��������� ������
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
        if (go)
        {
            point.transform.position = go.transform.position;       // ���������� ����� � ��������
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
