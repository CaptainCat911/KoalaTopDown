using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class Dialog : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;     // ссылка на текст
    public Animator animator;               // ссылка на аниматор
    public Animator blackImagesAnim;        // аниматор чёрных полос
    public DialogStore[] dialogStore;       // диалоги

    public float typingSpeed;               // скорость появления букв
    string[] sentences;                     // предложения
    string[] characterName;                 // имя персонажа, который говорит
    UnityEvent interactAction;              // ивент по завершении диалога

    int index;                              // индекс
    public GameObject continueButton;       // ссылка на кнопку продолжения     

    GameObject npcImage;                    // портрет нпс, с которым говорим
    public GameObject heroImage;            // портрет героя    

    public bool startEvent;                 // состояние этого ивента
    int dialogeNumber;                      // временная переменная для номера диалога
    Player player;



    private void Awake()
    {
        player = GameManager.instance.player;
    }

    private void Update()
    {
        if (startEvent)                 // если ивент начат
        {
            if (!player.atTarget)       // если игрок не дошёл до цели
            {
                player.Move(dialogStore[dialogeNumber].targetToDialoge.position);       // двигаем игрока к цели[номер диалога]
            }                
            if (player.atTarget)        // если дошёл
            {
                StartDialog(dialogeNumber);     // начинаем диалог
                startEvent = false;             // ивент закончился
            }                
        }




/*        if (textDisplay.text == sentences[index])       // если написали всё предложение
        {
            continueButton.SetActive(true);             // показываем кнопку продолжения
        }*/
    }

/*    public void TakeDialog(DialogStore dialog)
    {
        sentences = dialog.sentences;
    }*/


    // Начинаем ивент (вызывается из Gamemanager)
    public void StartEvent(int numberDialog)
    {
        startEvent = true;                                          // ивент начат
        dialogeNumber = numberDialog;                               // номер диалога
        GameManager.instance.isPlayerEnactive = true;               // отключаем управление игроком
        GameManager.instance.EnemyResetAndNeutral(true);            // сбрасываем ботов
        blackImagesAnim.SetTrigger("In");                           // запускаем чёрные полосы
    }

    public void StartDialog(int numberDialog)
    {
        sentences = dialogStore[numberDialog].sentences;            // берем предложения из диалга номер numberDialog
        characterName = dialogStore[numberDialog].characterName;    // имя (название) персонажа, который будет говорить
        npcImage = dialogStore[numberDialog].imageNpc;              // его портрет
        interactAction = dialogStore[numberDialog].interactAction;  // ивент
        StartCoroutine(Type(0.5f));                                 // запускаем печать букв
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
        animator.SetBool("Change", false);          // убираем эффект появления букв
        continueButton.SetActive(true);             // показываем кнопку продолжения
    }

    public void NextSentence()
    {        
        continueButton.SetActive(false);        // убираем кнопку продолжения
        animator.SetBool("Change", true);       // эффект появления текста

        if (index < sentences.Length - 1)       // если ещё есть предложения
        {
            index++;
            textDisplay.text = "";              // стираем текст
            StartCoroutine(Type(0f));
        }
        else
        {
            index = 0;
            textDisplay.text = "";
            heroImage.SetActive(false);                         // убираем портреты
            npcImage.SetActive(false);
            blackImagesAnim.SetTrigger("Out");                  // убираем чёрные полосы
            player.atTarget = false;                            // сбрасываем, что игрок у цели
            GameManager.instance.isPlayerEnactive = false;      // включаем управление игроком
            GameManager.instance.EnemyResetAndNeutral(false);   // включаем ботов
            interactAction.Invoke();                            // вызываем ивент (если есть)
        }
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
}
