using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager instance;          // �������

    [HideInInspector] public bool eng;          // ����
    public bool hardCoreMode;                   // ������� ���

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
        if (status)
            PlayerPrefs.SetInt("Language", 1);      // ���� ����
        else
            PlayerPrefs.SetInt("Language", 2);      // ���� ��
    }
}
