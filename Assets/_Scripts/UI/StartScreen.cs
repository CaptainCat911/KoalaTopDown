using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{
    public string[] sceneNames;                 // ��� �����
    public int sceneNumber;                     // ����� �����
    public int delay;                           // ��������
    public Animator animator;                   // �������� �������
    public GameObject kontrakt;                 // �������� ���������
    public GameObject loading;                  // �������� ��������
    public Animator screenAnimator;

    private void Start()
    {
        screenAnimator.SetTrigger("StartScreenNormal");
    }

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
        string sceneName = sceneNames[sceneNumber];     // �������� �����
        SceneManager.LoadScene(sceneName);              // ��������� �����
    }
    public void SetSceneStart(int number)
    {
        //GameManager.instance.SaveState();
        //string sceneName = sceneNames[Random.Range(0, sceneNames.Length)];        
        loading.SetActive(true);
        string sceneName = sceneNames[number];          // �������� �����
        SceneManager.LoadScene(sceneName);              // ��������� �����
    }
}
