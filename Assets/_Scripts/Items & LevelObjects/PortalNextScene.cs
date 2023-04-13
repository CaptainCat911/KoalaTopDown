using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public string[] sceneNames;                 // все сцены

    public void NextScene(int sceneNumber)
    {
        //GameManager.instance.SaveState();
        //string sceneName = sceneNames[Random.Range(0, sceneNames.Length)];
        string sceneName = sceneNames[sceneNumber];     // выбираем сцену
        SceneManager.LoadScene(sceneName);              // загружаем сцену
    }    
}
