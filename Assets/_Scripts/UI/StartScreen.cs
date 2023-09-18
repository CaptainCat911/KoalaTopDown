using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{
    public AudioSource audioSource;             // ���������

    public GameObject startLanguageMenu;        // ��������� ���� ��� ������

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
    public GameObject loadingEng;                  // �������� ��������
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
        if (PlayerPrefs.GetInt("Language") == 0)
        {
            startLanguageMenu.SetActive(true);
        }
        if (PlayerPrefs.GetInt("Language") == 1)        // ����
        {
            menuEng.SetActive(true);
            StartMusic();
            StartScreenAnimation();
            LanguageManager.instance.MakeEng(true);
        }
        if (PlayerPrefs.GetInt("Language") == 2)        // ��
        {
            menuRu.SetActive(true);
            StartMusic();
            StartScreenAnimation();
            LanguageManager.instance.MakeEng(false);
        }


        /*        if (eng)
                    menuEng.SetActive(true);
                else
                    menuRu.SetActive(true);*/
        //StartScreenAnimation();
    }
    

    public void StartMusic()
    {
        audioSource.Play();
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
        loadingEng.SetActive(true);                             // ����� ��������        
        SceneManager.LoadScene(sceneLoad);                      // ��������� �����
    }

    void ClearPrefs()
    {
        PlayerPrefs.DeleteAll();                    // ������� ����������, �� ���������� ����
        if (LanguageManager.instance.eng)
            PlayerPrefs.SetInt("Language", 1);      // ���� ����
        else
            PlayerPrefs.SetInt("Language", 2);      // ���� ��

    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
