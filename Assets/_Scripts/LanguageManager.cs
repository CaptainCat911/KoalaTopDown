using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager instance;          // инстанс

    [HideInInspector] public bool eng;          // язык
    public bool hardCoreMode;                   // хардкор мод

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;                                // присваем instance (?) этому обьекту и по ивенту загрузки запускаем функцию загрузки   
    }

    public void MakeEng(bool status)
    {
        eng = status;
        if (status)
            PlayerPrefs.SetInt("Language", 1);      // язык англ
        else
            PlayerPrefs.SetInt("Language", 2);      // язык ру
    }
}
