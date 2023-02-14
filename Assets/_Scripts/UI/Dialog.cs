using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialog : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;     // ������ �� �����
    public DialogStore[] dialogStore;       // �������
    string[] sentences;                     // �����������
    string[] characterName;                 // ��� ���������, ������� �������
    public float typingSpeed;               // �������� ��������� ����
    int index;                              // ������
    public GameObject continueButton;       // ������ �� ������ �����������     
    public Animator animator;               // ������ �� ��������
    GameObject npcImage;
    public GameObject blackImages;    
    public GameObject heroImage;    

    private void Update()
    {
/*        if (textDisplay.text == sentences[index])       // ���� �������� �� �����������
        {
            continueButton.SetActive(true);             // ���������� ������ �����������
        }*/
    }

    public void TakeDialog(DialogStore dialog)
    {
        sentences = dialog.sentences;
    }

    public void StartDialog(int numberDialog)
    {
        GameManager.instance.isPlayerEnactive = true;               // ��������� ���������� �������
        blackImages.SetActive(true);
        sentences = dialogStore[numberDialog].sentences;
        characterName = dialogStore[numberDialog].characterName;
        npcImage = dialogStore[numberDialog].imageNpc;
        StartCoroutine(Type());                                     // ��������� ������ ����
    }

    public IEnumerator Type()
    {
        ChangeCharacterImage(characterName[index]);
        foreach (char letter in sentences[index].ToCharArray())      // ��� ������ ����� � ����������� 
        {
            textDisplay.text += letter;                             // ������� ����� �� �����
            yield return new WaitForSeconds(typingSpeed);           // ��������
        }
        continueButton.SetActive(true);             // ���������� ������ �����������
    }

    public void NextSentence()
    {        
        animator.SetTrigger("Change");          // ������ ��������� ������
        continueButton.SetActive(false);        // ������� ������ �����������

        if (index < sentences.Length - 1)       // ���� ��� ���� �����������
        {
            index++;
            textDisplay.text = "";              // ������� �����
            StartCoroutine(Type());
        }
        else
        {
            index = 0;
            textDisplay.text = "";
            heroImage.SetActive(false);
            npcImage.SetActive(false);
            blackImages.SetActive(false);
            GameManager.instance.isPlayerEnactive = false;      // �������� ���������� �������
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
