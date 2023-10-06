using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

public class StartScreen : MonoBehaviour
{
    public bool forYG;                          // для яндекс игр
    public AudioSource audioSource;             // аудиосорс
    public Animator screenAnimator;             // аниматор для стартового затемнения

    public GameObject startLanguageMenu;        // стартовое меню для языков

    public GameObject menuEng;
    public GameObject menuRu;
    public string[] sceneNames;                 // все сцены
    public int sceneNumber;                     // номер сцены
    public int delay;                           // задержка
    public Animator animator;                   // аниматор подписи
    public Animator animatorEng;                // аниматор подписи
    public GameObject continueButton;           // кнопка продолжения игры
    public GameObject continueButtonEng;        // кнопка продолжения игры
    public GameObject kontrakt;                 // страница контракта
    public GameObject loading;                  // картинка загрузки
    public GameObject loadingEng;               // картинка загрузки

    public Text textTest;



    // Подписываемся на событие GetDataEvent в OnEnable
    private void OnEnable()
    {
        YandexGame.GetDataEvent += GetLoad;       
    }
    // Отписываемся от события GetDataEvent в OnDisable
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
                // Если запустился, то выполняем Ваш метод для загрузки
                GetLoad();
            }
        }
        else
        {            
            if (PlayerPrefs.GetInt("Language") == 1)        // англ
            {
                menuEng.SetActive(true);
                StartMusic();
                StartScreenAnimation();
                LanguageManager.instance.MakeEng(true);
            }
            else if (PlayerPrefs.GetInt("Language") == 2)        // ру
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

        if (PlayerPrefs.GetInt("GameContinue") == 1)        // если есть сохранение
        {
            continueButton.SetActive(true);                 // вкл кнопку продолжить
            continueButtonEng.SetActive(true);              // вкл кнопку продолжить
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
            continueButton.SetActive(true);                 // вкл кнопку продолжить
            continueButtonEng.SetActive(true);              // вкл кнопку продолжить
        }
        textTest.text = YandexGame.savesData.numberStartScene;  */
    }

    public void MySave()
    {
/*        // Записываем данные в плагин
        // Например, мы хотил сохранить количество монет игрока:
        YandexGame.savesData.numberStartScene = textTest.text;

        // Теперь остаётся сохранить данные
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

    // Новая игра
    public void NextSceneStart()
    {
        //GameManager.instance.SaveState();
        //string sceneName = sceneNames[Random.Range(0, sceneNames.Length)];

        //if (forYG)
        //{
            // сброс сохранений
            /*YandexGame.savesData.gameContinue = false;*/
        //}
        //else
        //{
            ClearPrefs();                                   // сбрасываем сохранения
        //}       

       

        //kontrakt.SetActive(false);
        //loading.SetActive(true);
        string sceneName = sceneNames[sceneNumber];     // выбираем сцену        
        SceneManager.LoadScene(sceneName);              // загружаем сцену
    }

    // Выбрать уровень
    public void SetSceneStart(int number)
    {
        //GameManager.instance.SaveState();
        //string sceneName = sceneNames[Random.Range(0, sceneNames.Length)];        
        loading.SetActive(true);
        string sceneName = sceneNames[number];          // выбираем сцену        
        SceneManager.LoadScene(sceneName);              // загружаем сцену
    }

    // Продолжить игру
    public void LoadData()
    {
        //if (forYG)
        //{
/*            string sceneLoad = YandexGame.savesData.sceneNameToLoad;
            YandexGame.savesData.loadPlayerData = true;
            SceneManager.LoadScene(sceneLoad);                      // загружаем сцену*/
        //}
       // else
        //{
            string sceneLoad = PlayerPrefs.GetString("SceneName");      // загружаем название сцены из сохранения
            PlayerPrefs.SetInt("LoadPlayerData", 1);                    // для загрузки пар-ов игрока

            if (PlayerPrefs.GetInt("HardCore") == 1)
            {
                LanguageManager.instance.hardCoreMode = true;
            }
            else
            {
                LanguageManager.instance.hardCoreMode = false;
            }            

            SceneManager.LoadScene(sceneLoad);                          // загружаем сцену
        //}

        loading.SetActive(true);                                        // пошла загрузка        
        loadingEng.SetActive(true);                                     // пошла загрузка         
    }

    void ClearPrefs()
    {
        //Debug.Log("Clear!");
        PlayerPrefs.DeleteAll();                    // стираем сохранения, но запоминаем язык

        if (LanguageManager.instance.eng)
            PlayerPrefs.SetInt("Language", 1);      // язык англ
        else
            PlayerPrefs.SetInt("Language", 2);      // язык ру

        if (LanguageManager.instance.hardCoreMode)
            PlayerPrefs.SetInt("HardCore", 1);      // хардкор вкл
        else
            PlayerPrefs.SetInt("HardCore", 0);      // хардкор выкл
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
