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

    private void Update()
    {
/*        if (textDisplay.text == sentences[index])       // если написали всё предложение
        {
            continueButton.SetActive(true);             // показываем кнопку продолжения
        }*/
    }

/*    public void TakeDialog(DialogStore dialog)
    {
        sentences = dialog.sentences;
    }*/

    public void StartDialog(int numberDialog)
    {
        GameManager.instance.isPlayerEnactive = true;               // отключаем управление игроком
        GameManager.instance.EnemyResetAndNeutral(true);            // сбрасываем ботов
        blackImagesAnim.SetTrigger("In");                           // запускаем чёрные полосы
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
