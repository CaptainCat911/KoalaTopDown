using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{
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
    public Animator screenAnimator;

    private void Awake()
    {
        if (PlayerPrefs.GetInt("GameContinue") == 1)        // если есть сохранение
        {
            continueButton.SetActive(true);                 // вкл кнопку продолжить
            continueButtonEng.SetActive(true);              // вкл кнопку продолжить
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

    // Новая игра
    void NextSceneStart()
    {
        //GameManager.instance.SaveState();
        //string sceneName = sceneNames[Random.Range(0, sceneNames.Length)];
        ClearPrefs();                                   // сбрасываем сохранения
        kontrakt.SetActive(false);
        loading.SetActive(true);
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
        string sceneLoad = PlayerPrefs.GetString("SceneName");  // загружаем название сцены из сохранения
        PlayerPrefs.SetInt("LoadPlayerData", 1);                // для загрузки пар-ов игрока
        loading.SetActive(true);                                // пошла загрузка        
        SceneManager.LoadScene(sceneLoad);                      // загружаем сцену
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
