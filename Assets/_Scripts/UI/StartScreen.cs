using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine.UI;
using YG;

public class StartScreen : MonoBehaviour
{
    public bool forYG;
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

    public Text textTest;



    // ������������� �� ������� GetDataEvent � OnEnable
    private void OnEnable()
    {
        YandexGame.GetDataEvent += GetLoad;       
    }
    // ������������ �� ������� GetDataEvent � OnDisable
    private void OnDisable() 
    { 
        YandexGame.GetDataEvent -= GetLoad;         
    }


    private void Awake()
    {

    }

    private void Start()
    {
        if (forYG)
        {
            if (YandexGame.SDKEnabled == true)
            {
                // ���� ����������, �� ��������� ��� ����� ��� ��������
                GetLoad();
            }
        }
        else
        {
            if (PlayerPrefs.GetInt("GameContinue") == 1)        // ���� ���� ����������
            {
                continueButton.SetActive(true);                 // ��� ������ ����������
                continueButtonEng.SetActive(true);              // ��� ������ ����������
            }
        }



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
    }

    public void GetLoad()
    {
        if (YandexGame.savesData.gameContinue)
        {
            continueButton.SetActive(true);                 // ��� ������ ����������
            continueButtonEng.SetActive(true);              // ��� ������ ����������
        }
        textTest.text = YandexGame.savesData.numberStartScene;

        //textMoney.text = YandexGame.savesData.money.ToString();
    }

    public void MySave()
    {
        // ���������� ������ � ������
        // ��������, �� ����� ��������� ���������� ����� ������:
        YandexGame.savesData.numberStartScene = textTest.text;

        // ������ ������� ��������� ������
        YandexGame.SaveProgress();
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

        if (forYG)
        {
            // ����� ����������
            YandexGame.savesData.gameContinue = false;
        }
        else
        {
            ClearPrefs();                                   // ���������� ����������
        }

       

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
        if (forYG)
        {
            string sceneLoad = YandexGame.savesData.sceneNameToLoad;
            YandexGame.savesData.loadPlayerData = true;
            SceneManager.LoadScene(sceneLoad);                      // ��������� �����
        }
        else
        {
            string sceneLoad = PlayerPrefs.GetString("SceneName");  // ��������� �������� ����� �� ����������
            PlayerPrefs.SetInt("LoadPlayerData", 1);                // ��� �������� ���-�� ������
            SceneManager.LoadScene(sceneLoad);                      // ��������� �����
        }

        loading.SetActive(true);                                // ����� ��������        
        loadingEng.SetActive(true);                             // ����� ��������        
        
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
