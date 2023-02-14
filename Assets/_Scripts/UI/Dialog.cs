using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialog : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;     // ссылка на текст
    public DialogStore[] dialogStore;       // диалоги
    string[] sentences;                     // предложения
    string[] characterName;                 // имя персонажа, который говорит
    public float typingSpeed;               // скорость появления букв
    int index;                              // индекс
    public GameObject continueButton;       // ссылка на кнопку продолжения     
    public Animator animator;               // ссылка на аниматор
    GameObject npcImage;
    public GameObject blackImages;    
    public GameObject heroImage;    

    private void Update()
    {
/*        if (textDisplay.text == sentences[index])       // если написали всё предложение
        {
            continueButton.SetActive(true);             // показываем кнопку продолжения
        }*/
    }

    public void TakeDialog(DialogStore dialog)
    {
        sentences = dialog.sentences;
    }

    public void StartDialog(int numberDialog)
    {
        GameManager.instance.isPlayerEnactive = true;               // отключаем управление игроком
        blackImages.SetActive(true);
        sentences = dialogStore[numberDialog].sentences;
        characterName = dialogStore[numberDialog].characterName;
        npcImage = dialogStore[numberDialog].imageNpc;
        StartCoroutine(Type());                                     // запускаем печать букв
    }

    public IEnumerator Type()
    {
        ChangeCharacterImage(characterName[index]);
        foreach (char letter in sentences[index].ToCharArray())      // для каждой буквы в предложении 
        {
            textDisplay.text += letter;                             // выводим текст по букве
            yield return new WaitForSeconds(typingSpeed);           // задержка
        }
        continueButton.SetActive(true);             // показываем кнопку продолжения
    }

    public void NextSentence()
    {        
        animator.SetTrigger("Change");          // эффект появления текста
        continueButton.SetActive(false);        // убираем кнопку продолжения

        if (index < sentences.Length - 1)       // если ещё есть предложения
        {
            index++;
            textDisplay.text = "";              // стираем текст
            StartCoroutine(Type());
        }
        else
        {
            index = 0;
            textDisplay.text = "";
            heroImage.SetActive(false);
            npcImage.SetActive(false);
            blackImages.SetActive(false);
            GameManager.instance.isPlayerEnactive = false;      // включаем управление игроком
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
