using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{
    public GameObject menuEng;
    public GameObject menuRu;
    public string[] sceneNames;                 // ��� �����
    public int sceneNumber;                     // ����� �����
    public int delay;                           // ��������
    public Animator animator;                   // �������� �������
    public Animator animatorEng;                // �������� �������
    public GameObject continueButton;           // ������ ����������� ����
    public GameObject continueButtonEng;        // ������ ����������� ����
    public GameObject kontrakt;                 // �������� ���������
    public GameObject loading;                  // �������� ��������
    public Animator screenAnimator;

    private void Awake()
    {
        if (PlayerPrefs.GetInt("GameContinue") == 1)        // ���� ���� ����������
        {
            continueButton.SetActive(true);                 // ��� ������ ����������
            continueButtonEng.SetActive(true);              // ��� ������ ����������
        }
    }

    private void Start()
    {      
        /*        if (eng)
                    menuEng.SetActive(true);
                else
                    menuRu.SetActive(true);*/        
    }

    public void StartScreenAnimation()
    {        
        screenAnimator.SetTrigger("StartScreenNormal");
    }

    public void StartNextScene()
    {
        if (LanguageManager.instance.eng)
            animatorEng.SetTrigger("Start");
        else
            animator.SetTrigger("Start");

        Invoke("NextSceneStart", delay);
    }

    // ����� ����
    void NextSceneStart()
    {
        //GameManager.instance.SaveState();
        //string sceneName = sceneNames[Random.Range(0, sceneNames.Length)];
        ClearPrefs();                                   // ���������� ����������
        kontrakt.SetActive(false);
        loading.SetActive(true);
        string sceneName = sceneNames[sceneNumber];     // �������� �����        
        SceneManager.LoadScene(sceneName);              // ��������� �����
    }

    // ������� �������
    public void SetSceneStart(int number)
    {
        //GameManager.instance.SaveState();
        //string sceneName = sceneNames[Random.Range(0, sceneNames.Length)];        
        loading.SetActive(true);
        string sceneName = sceneNames[number];          // �������� �����        
        SceneManager.LoadScene(sceneName);              // ��������� �����
    }

    // ���������� ����
    public void LoadData()
    {
        string sceneLoad = PlayerPrefs.GetString("SceneName");  // ��������� �������� ����� �� ����������
        PlayerPrefs.SetInt("LoadPlayerData", 1);                // ��� �������� ���-�� ������
        loading.SetActive(true);                                // ����� ��������        
        SceneManager.LoadScene(sceneLoad);                      // ��������� �����
    }

    void ClearPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
