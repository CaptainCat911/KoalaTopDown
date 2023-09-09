using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager instance;          // �������

    public bool eng;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;                                // �������� instance (?) ����� ������� � �� ������ �������� ��������� ������� ��������   
    }

    public void MakeEng(bool status)
    {
        eng = status;
    }
}
