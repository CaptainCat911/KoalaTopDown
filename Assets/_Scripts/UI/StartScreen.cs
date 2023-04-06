using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{
    public string[] sceneNames;                 // все сцены
    public int sceneNumber;                     // номер сцены
    public int delay;                           // задержка
    public Animator animator;                   // аниматор подписи
    public GameObject kontrakt;
    public GameObject loading;



    public void StartNextScene()
    {
        animator.SetTrigger("Start");
        Invoke("NextSceneStart", delay);
    }

    void NextSceneStart()
    {
        //GameManager.instance.SaveState();
        //string sceneName = sceneNames[Random.Range(0, sceneNames.Length)];
        kontrakt.SetActive(false);
        loading.SetActive(true);
        string sceneName = sceneNames[sceneNumber];     // выбираем сцену
        SceneManager.LoadScene(sceneName);              // загружаем сцену
    }
}
