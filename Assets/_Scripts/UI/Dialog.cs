using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class Dialog : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;     // ������ �� �����
    public Animator animator;               // ������ �� ��������
    public Animator blackImagesAnim;        // �������� ������ �����
    public DialogStore[] dialogStore;       // �������

    public float typingSpeed;               // �������� ��������� ����
    string[] sentences;                     // �����������
    string[] characterName;                 // ��� ���������, ������� �������
    UnityEvent interactAction;              // ����� �� ���������� �������

    int index;                              // ������
    public GameObject continueButton;       // ������ �� ������ �����������     

    GameObject npcImage;                    // ������� ���, � ������� �������
    public GameObject heroImage;            // ������� �����

    private void Update()
    {
/*        if (textDisplay.text == sentences[index])       // ���� �������� �� �����������
        {
            continueButton.SetActive(true);             // ���������� ������ �����������
        }*/
    }

/*    public void TakeDialog(DialogStore dialog)
    {
        sentences = dialog.sentences;
    }*/

    public void StartDialog(int numberDialog)
    {
        GameManager.instance.isPlayerEnactive = true;               // ��������� ���������� �������
        GameManager.instance.EnemyResetAndNeutral(true);            // ���������� �����
        blackImagesAnim.SetTrigger("In");                           // ��������� ������ ������
        sentences = dialogStore[numberDialog].sentences;            // ����� ����������� �� ������ ����� numberDialog
        characterName = dialogStore[numberDialog].characterName;    // ��� (��������) ���������, ������� ����� ��������
        npcImage = dialogStore[numberDialog].imageNpc;              // ��� �������
        interactAction = dialogStore[numberDialog].interactAction;  // �����
        StartCoroutine(Type(0.5f));                                 // ��������� ������ ����
    }

    public IEnumerator Type(float delay)
    {
        yield return new WaitForSeconds(delay);                     // ��������
        ChangeCharacterImage(characterName[index]);                 // �������� ������� ��� �������
        foreach (char letter in sentences[index].ToCharArray())     // ��� ������ ����� � ����������� 
        {
            textDisplay.text += letter;                             // ������� ����� �� �����
            yield return new WaitForSeconds(typingSpeed);           // ��������
        }
        animator.SetBool("Change", false);          // ������� ������ ��������� ����
        continueButton.SetActive(true);             // ���������� ������ �����������
    }

    public void NextSentence()
    {        
        continueButton.SetActive(false);        // ������� ������ �����������
        animator.SetBool("Change", true);       // ������ ��������� ������

        if (index < sentences.Length - 1)       // ���� ��� ���� �����������
        {
            index++;
            textDisplay.text = "";              // ������� �����
            StartCoroutine(Type(0f));
        }
        else
        {
            index = 0;
            textDisplay.text = "";
            heroImage.SetActive(false);                         // ������� ��������
            npcImage.SetActive(false);
            blackImagesAnim.SetTrigger("Out");                  // ������� ������ ������
            GameManager.instance.isPlayerEnactive = false;      // �������� ���������� �������
            GameManager.instance.EnemyResetAndNeutral(false);   // �������� �����
            interactAction.Invoke();                            // �������� ����� (���� ����)
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
