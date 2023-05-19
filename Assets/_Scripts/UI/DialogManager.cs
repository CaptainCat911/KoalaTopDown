using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class DialogManager : MonoBehaviour
{
    public static DialogManager instance;   // �������

    public TextMeshProUGUI textDisplay;     // ������ �� �����
    public Animator animator;               // ������ �� ��������
    public GameObject blackImages;          // �������� ������ �����

    public DialogStore[] dialogStore;       // �������

    public float typingSpeed;               // �������� ��������� ����
    string[] sentences;                     // �����������
    string[] characterName;                 // ��� ���������, ������� �������
    UnityEvent startInteractAction;         // ����� �� ������ �������
    UnityEvent goInteractAction;            // ����� ��� ������ �������
    UnityEvent interactAction;              // ����� �� ���������� �������

    int index;                              // ������
    public GameObject continueButton;       // ������ �� ������ �����������     

    GameObject npcImage;                    // ������� ���, � ������� �������
    public GameObject heroImage;            // ������� �����    

    [HideInInspector] public bool startEvent;   // ��������� ����� ������
    int dialogeNumber;                      // ��������� ���������� ��� ������ �������


    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (startEvent)         // ���� ����� �����
        {
            if (!GameManager.instance.playerAtTarget && dialogStore[dialogeNumber].targetToDialoge)       // ���� ����� �� ����� �� ���� � ���� ����
            {                       
                GameManager.instance.MovePlayer(dialogStore[dialogeNumber].targetToDialoge.position);       // ������� ������ � ����[����� �������]
            }                
            else                // ���� ����� ��� ���� ���
            {
                StartDialog();                                  // �������� ������
                startEvent = false;                             // ����� ����������
                GameManager.instance.playerAtTarget = false;    // ����������, ��� ����� ����� ����
                GameManager.instance.player.animator.SetFloat("Speed", 0);  // ���� ��� ���� ��� ��������� �������� ���� ������ (�������� ����� ����������� � ������ ������)
            }                
        }

/*        if (textDisplay.text == sentences[index])       // ���� �������� �� �����������
        {
            continueButton.SetActive(true);             // ���������� ������ �����������
        }*/
    }


    // �������� ����� (���������� �� Gamemanager)
    public void StartEvent(int numberDialog)
    {
        GameManager.instance.dialogeStart = true;
        startEvent = true;                                                      // ����� �����        
        dialogeNumber = numberDialog;                                           // ����� �������
        GameManager.instance.isPlayerEnactive = true;                           // ��������� ���������� �������
        GameManager.instance.EnemyResetAndNeutral(true);                        // ���������� �����
        BlackTapes(true);                                                       // ������ ������
        GameManager.instance.cameraOnPlayer = true;                             // ������ �� ������

        sentences = dialogStore[numberDialog].sentences;                        // ����� ����������� �� ������ ����� numberDialog
        characterName = dialogStore[numberDialog].characterName;                // ��� (��������) ���������, ������� ����� ��������
        npcImage = dialogStore[numberDialog].imageNpc;                          // ��� �������
        startInteractAction = dialogStore[numberDialog].awakeInteractAction;    // ����� �� ������ �������
        goInteractAction = dialogStore[numberDialog].goInteractAction;          // ����� ��� ������ �������
        interactAction = dialogStore[numberDialog].interactAction;              // ����� �� ���������� �������
        startInteractAction.Invoke();
    }

    public void StartDialog()
    {
        goInteractAction.Invoke();
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
        else                                    // ���� ����������� �����������
        {
            index = 0;
            textDisplay.text = "";
            heroImage.SetActive(false);                         // ������� ��������
            npcImage.SetActive(false);          
            BlackTapes(false);                                  // ������� ������ ������
            GameManager.instance.cameraOnPlayer = false;        // ��������� ������
            GameManager.instance.isPlayerEnactive = false;      // �������� ���������� �������
            GameManager.instance.EnemyResetAndNeutral(false);   // �������� �����
            interactAction.Invoke();                            // �������� ����� (���� ����)
            GameManager.instance.dialogeStart = false;
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

    // ׸���� ������
    public void BlackTapes(bool status)
    {
        if (status)
            blackImages.GetComponent<Animator>().SetTrigger("In");                   // ��������� ������ ������
        else
            blackImages.GetComponent<Animator>().SetTrigger("Out");                  // ������� ������ ������
    }
}
