using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using YG;

public class DialogManager : MonoBehaviour
{
    public static DialogManager instance;   // инстанс

    public TextMeshProUGUI textDisplay;     // ссылка на текст
    public Animator textAnimator;           // ссылка на аниматор текста
    public GameObject blackImages;          // аниматор чёрных полос

    public DialogStore[] dialogStore;       // диалоги

    public float typingSpeed;               // скорость появления букв
    string[] sentences;                     // предложения
    string[] characterName;                 // имя персонажа, который говорит
    UnityEvent startInteractAction;         // ивент до начала диалога
    UnityEvent goInteractAction;            // ивент при старте диалога
    UnityEvent interactAction;              // ивент по завершении диалога

    int index;                              // индекс
    public GameObject continueButton;       // кнопка продолжения
    public GameObject skipButton;           // кнопка пропуска диалога

    GameObject npcImage;                    // портрет нпс, с которым говорим
    public GameObject heroImage;            // портрет героя    

    [HideInInspector] public bool startEvent;   // состояние этого ивента
    int dialogeNumber;                      // временная переменная для номера диалога

    //IEnumerator coroutine;


    private void Awake()
    {
        instance = this;
        //coroutine = Type(0f);
    }

    private void Update()
    {
        if (startEvent)         // если ивент начат
        {
            if (!GameManager.instance.playerAtTarget && dialogStore[dialogeNumber].targetToDialoge)       // если игрок не дошёл до цели и цель есть
            {                       
                GameManager.instance.MovePlayer(dialogStore[dialogeNumber].targetToDialoge.position);       // двигаем игрока к цели[номер диалога]
            }                
            else                // если дошёл или цели нет
            {
                Invoke(nameof(StartDialog), dialogStore[dialogeNumber].dialogeDelay);                 // начинаем диалог
                //StartDialog();                                  
                startEvent = false;                             // ивент закончился
                GameManager.instance.playerAtTarget = false;    // сбрасываем, что игрок возле цели
                GameManager.instance.player.animator.SetFloat("Speed", 0);  // если нет цели тут сбрасываю анимацию бега игрока (возможно стоит переместить в другой скрипт)
            }                
        }

/*        if (textDisplay.text == sentences[index])       // если написали всё предложение
        {
            continueButton.SetActive(true);             // показываем кнопку продолжения
        }*/
    }


    // Начинаем ивент
    public void StartEvent(int numberDialog)
    {
        GameManager.instance.dialogeStart = true;
        startEvent = true;                                                      // ивент начат        
        dialogeNumber = numberDialog;                                           // номер диалога
        GameManager.instance.isPlayerEnactive = true;                           // отключаем управление игроком
        GameManager.instance.player.isImmortal = true;                          // игрок бессмертен
        TextUI.instance.CursorVisibleOnOff(true);                               // переключаем прицел на курсор
        GameManager.instance.EnemyResetAndNeutral(true);                        // сбрасываем ботов
        BlackTapes(true);                                                       // черные полосы
        GameManager.instance.cameraOnPlayer = true;                             // камера на игрока
        
        if (LanguageManager.instance.eng)
            sentences = dialogStore[numberDialog].sentencesEng;                 // берем предложения из диалга номер numberDialog (англ)
        else
            sentences = dialogStore[numberDialog].sentences;                    // берем предложения из диалга номер numberDialog
        characterName = dialogStore[numberDialog].characterName;                // имя (название) персонажа, который будет говорить
        npcImage = dialogStore[numberDialog].imageNpc;                          // его портрет
        startInteractAction = dialogStore[numberDialog].awakeInteractAction;    // ивент до начала диалога
        goInteractAction = dialogStore[numberDialog].goInteractAction;          // ивент при старте диалога
        interactAction = dialogStore[numberDialog].interactAction;              // ивент по завершению диалога
        startInteractAction.Invoke();
    }

    public void StartDialog()
    {
        if (GameManager.instance.forYG)
            YandexGame.FullscreenShow();
        goInteractAction.Invoke();          // ивент при старте диалога
        skipButton.SetActive(true);         // включаем кнопку пропуска диалога
        StartCoroutine(Type(0.5f));         // запускаем печать букв
    }

    public IEnumerator Type(float delay)
    {
        yield return new WaitForSeconds(delay);                     // задержка
        ChangeCharacterImage(characterName[index]);                 // выбираем портрет кто говорит
        foreach (char letter in sentences[index].ToCharArray())     // для каждой буквы в предложении 
        {
            textDisplay.text += letter;                             // выводим текст по букве
            yield return new WaitForSeconds(typingSpeed);           // задержка
        }
        textAnimator.SetBool("Change", false);          // убираем эффект появления букв
        continueButton.SetActive(true);             // показываем кнопку продолжения
    }

    public void NextSentence()
    {        
        continueButton.SetActive(false);        // убираем кнопку продолжения
        textAnimator.SetBool("Change", true);       // эффект появления текста

        if (index < sentences.Length - 1)       // если ещё есть предложения
        {
            index++;
            textDisplay.text = "";              // стираем текст
            StartCoroutine(Type(0f));
        }
        else                                    // если предложения закончились
        {
            skipButton.SetActive(false);
            index = 0;
            textDisplay.text = "";
            heroImage.SetActive(false);                         // убираем портреты
            npcImage.SetActive(false);          
            BlackTapes(false);                                  // убираем черные полосы
            GameManager.instance.cameraOnPlayer = false;        // отпускаем камеру
            GameManager.instance.player.isImmortal = false;     // игрок не бессмертен
            GameManager.instance.isPlayerEnactive = false;      // включаем управление игроком
            TextUI.instance.CursorVisibleOnOff(false);
            GameManager.instance.EnemyResetAndNeutral(false);   // включаем ботов
            interactAction.Invoke();                            // вызываем ивент (если есть)
            GameManager.instance.dialogeStart = false;
        }
    }

    public void SkipDialoge()
    {
        skipButton.SetActive(false);
        continueButton.SetActive(false);                    // убираем кнопку продолжения
        StopAllCoroutines();
        index = 0;
        textDisplay.text = "";
        heroImage.SetActive(false);                         // убираем портреты
        npcImage.SetActive(false);
        BlackTapes(false);                                  // убираем черные полосы
        GameManager.instance.cameraOnPlayer = false;        // отпускаем камеру
        GameManager.instance.player.isImmortal = false;     // игрок не бессмертен
        GameManager.instance.isPlayerEnactive = false;      // включаем управление игроком
        TextUI.instance.CursorVisibleOnOff(false);          // меняем курсор на прицел
        GameManager.instance.EnemyResetAndNeutral(false);   // включаем ботов
        interactAction.Invoke();                            // вызываем ивент (если есть)
        GameManager.instance.dialogeStart = false;
    }

    void ChangeCharacterImage(string name)
    {
        if (name == "Hero")
        {
            heroImage.SetActive(true);
            npcImage.SetActive(false);
        }
        if (name == "Npc")
        {
            heroImage.SetActive(false);
            npcImage.SetActive(true);
        }
    }

    // Чёрные полосы
    public void BlackTapes(bool status)
    {
        if (status)
            blackImages.GetComponent<Animator>().SetTrigger("In");                   // запускаем чёрные полосы
        else
            blackImages.GetComponent<Animator>().SetTrigger("Out");                  // убираем чёрные полосы
    }
}
