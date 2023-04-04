using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class Dialog : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;     // ссылка на текст
    public Animator animator;               // ссылка на аниматор
    
    public DialogStore[] dialogStore;       // диалоги

    public float typingSpeed;               // скорость по€влени€ букв
    string[] sentences;                     // предложени€
    string[] characterName;                 // им€ персонажа, который говорит
    UnityEvent interactAction;              // ивент по завершении диалога

    int index;                              // индекс
    public GameObject continueButton;       // ссылка на кнопку продолжени€     

    GameObject npcImage;                    // портрет нпс, с которым говорим
    public GameObject heroImage;            // портрет геро€    

    public bool startEvent;                 // состо€ние этого ивента
    int dialogeNumber;                      // временна€ переменна€ дл€ номера диалога
    



    private void Awake()
    {
        
    }

    private void Update()
    {
        if (startEvent)                 // если ивент начат
        {
            if (!GameManager.instance.playerAtTarget && dialogStore[dialogeNumber].targetToDialoge)       // если игрок не дошЄл до цели и цель есть
            {                       
                GameManager.instance.MovePlayer(dialogStore[dialogeNumber].targetToDialoge.position);       // двигаем игрока к цели[номер диалога]
            }                
            else        // если дошЄл или цели нет
            {
                StartDialog(dialogeNumber);                 // начинаем диалог
                startEvent = false;                         // ивент закончилс€
                GameManager.instance.playerAtTarget = false;    // сбрасываем, что игрок возле цели
                GameManager.instance.player.animator.SetFloat("Speed", 0);  // если нет цели тут сбрасываю анимацию бега игрока (возможно стоит переместить в другой скрипт)
            }                
        }




/*        if (textDisplay.text == sentences[index])       // если написали всЄ предложение
        {
            continueButton.SetActive(true);             // показываем кнопку продолжени€
        }*/
    }

/*    public void TakeDialog(DialogStore dialog)
    {
        sentences = dialog.sentences;
    }*/


    // Ќачинаем ивент (вызываетс€ из Gamemanager)
    public void StartEvent(int numberDialog)
    {
        startEvent = true;                                          // ивент начат
        dialogeNumber = numberDialog;                               // номер диалога
        GameManager.instance.isPlayerEnactive = true;               // отключаем управление игроком
        GameManager.instance.EnemyResetAndNeutral(true);            // сбрасываем ботов
        GameManager.instance.BlackTapes(true);                      // черные полосы
        
    }

    public void StartDialog(int numberDialog)
    {
        sentences = dialogStore[numberDialog].sentences;            // берем предложени€ из диалга номер numberDialog
        characterName = dialogStore[numberDialog].characterName;    // им€ (название) персонажа, который будет говорить
        npcImage = dialogStore[numberDialog].imageNpc;              // его портрет
        interactAction = dialogStore[numberDialog].interactAction;  // ивент
        StartCoroutine(Type(0.5f));                                 // запускаем печать букв
    }

    public IEnumerator Type(float delay)
    {
        yield return new WaitForSeconds(delay);                     // задержка
        ChangeCharacterImage(characterName[index]);                 // выбираем портрет кто говорит
        foreach (char letter in sentences[index].ToCharArray())     // дл€ каждой буквы в предложении 
        {
            textDisplay.text += letter;                             // выводим текст по букве
            yield return new WaitForSeconds(typingSpeed);           // задержка
        }
        animator.SetBool("Change", false);          // убираем эффект по€влени€ букв
        continueButton.SetActive(true);             // показываем кнопку продолжени€
    }

    public void NextSentence()
    {        
        continueButton.SetActive(false);        // убираем кнопку продолжени€
        animator.SetBool("Change", true);       // эффект по€влени€ текста

        if (index < sentences.Length - 1)       // если ещЄ есть предложени€
        {
            index++;
            textDisplay.text = "";              // стираем текст
            StartCoroutine(Type(0f));
        }
        else                                    // если предложени€ закончились
        {
            index = 0;
            textDisplay.text = "";
            heroImage.SetActive(false);                         // убираем портреты
            npcImage.SetActive(false);          
            GameManager.instance.BlackTapes(false);             // убираем черные полосы                                        
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
