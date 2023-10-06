using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

public class StartScreen : MonoBehaviour
{
    public bool forYG;                          // ��� ������ ���
    public AudioSource audioSource;             // ���������
    public Animator screenAnimator;             // �������� ��� ���������� ����������

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
    public GameObject loadingEng;               // �������� ��������

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
            if (PlayerPrefs.GetInt("Language") == 1)        // ����
            {
                menuEng.SetActive(true);
                StartMusic();
                StartScreenAnimation();
                LanguageManager.instance.MakeEng(true);
            }
            else if (PlayerPrefs.GetInt("Language") == 2)        // ��
            {
                menuRu.SetActive(true);
                StartMusic();
                StartScreenAnimation();
                LanguageManager.instance.MakeEng(false);
            }
            else
            {                
                startLanguageMenu.SetActive(true);
            }
        }

        if (PlayerPrefs.GetInt("GameContinue") == 1)        // ���� ���� ����������
        {
            continueButton.SetActive(true);                 // ��� ������ ����������
            continueButtonEng.SetActive(true);              // ��� ������ ����������
        }
    }

    public void GetLoad()
    {
        if (YandexGame.EnvironmentData.language == "ru")
        {
            //Debug.Log("RU!");
            menuRu.SetActive(true);
            StartMusic();
            StartScreenAnimation();
            LanguageManager.instance.MakeEng(false);
        }
        if (YandexGame.EnvironmentData.language == "en")
        {
            menuEng.SetActive(true);
            StartMusic();
            StartScreenAnimation();
            LanguageManager.instance.MakeEng(true);
        }

/*        if (YandexGame.savesData.gameContinue)
        {
            continueButton.SetActive(true);                 // ��� ������ ����������
            continueButtonEng.SetActive(true);              // ��� ������ ����������
        }
        textTest.text = YandexGame.savesData.numberStartScene;  */
    }

    public void MySave()
    {
/*        // ���������� ������ � ������
        // ��������, �� ����� ��������� ���������� ����� ������:
        YandexGame.savesData.numberStartScene = textTest.text;

        // ������ ������� ��������� ������
        YandexGame.SaveProgress();*/
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
    public void NextSceneStart()
    {
        //GameManager.instance.SaveState();
        //string sceneName = sceneNames[Random.Range(0, sceneNames.Length)];

        //if (forYG)
        //{
            // ����� ����������
            /*YandexGame.savesData.gameContinue = false;*/
        //}
        //else
        //{
            ClearPrefs();                                   // ���������� ����������
        //}       

       

        //kontrakt.SetActive(false);
        //loading.SetActive(true);
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
        //if (forYG)
        //{
/*            string sceneLoad = YandexGame.savesData.sceneNameToLoad;
            YandexGame.savesData.loadPlayerData = true;
            SceneManager.LoadScene(sceneLoad);                      // ��������� �����*/
        //}
       // else
        //{
            string sceneLoad = PlayerPrefs.GetString("SceneName");      // ��������� �������� ����� �� ����������
            PlayerPrefs.SetInt("LoadPlayerData", 1);                    // ��� �������� ���-�� ������

            if (PlayerPrefs.GetInt("HardCore") == 1)
            {
                LanguageManager.instance.hardCoreMode = true;
            }
            else
            {
                LanguageManager.instance.hardCoreMode = false;
            }            

            SceneManager.LoadScene(sceneLoad);                          // ��������� �����
        //}

        loading.SetActive(true);                                        // ����� ��������        
        loadingEng.SetActive(true);                                     // ����� ��������         
    }

    void ClearPrefs()
    {
        //Debug.Log("Clear!");
        PlayerPrefs.DeleteAll();                    // ������� ����������, �� ���������� ����

        if (LanguageManager.instance.eng)
            PlayerPrefs.SetInt("Language", 1);      // ���� ����
        else
            PlayerPrefs.SetInt("Language", 2);      // ���� ��

        if (LanguageManager.instance.hardCoreMode)
            PlayerPrefs.SetInt("HardCore", 1);      // ������� ���
        else
            PlayerPrefs.SetInt("HardCore", 0);      // ������� ����
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
