using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager instance;          // инстанс

    public bool eng;

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
    }
}
