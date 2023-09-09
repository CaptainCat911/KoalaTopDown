using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using YG;

public class DialogManager : MonoBehaviour
{
    public static DialogManager instance;   // �������

    public TextMeshProUGUI textDisplay;     // ������ �� �����
    public Animator textAnimator;           // ������ �� �������� ������
    public GameObject blackImages;          // �������� ������ �����

    public DialogStore[] dialogStore;       // �������

    public float typingSpeed;               // �������� ��������� ����
    string[] sentences;                     // �����������
    string[] characterName;                 // ��� ���������, ������� �������
    UnityEvent startInteractAction;         // ����� �� ������ �������
    UnityEvent goInteractAction;            // ����� ��� ������ �������
    UnityEvent interactAction;              // ����� �� ���������� �������

    int index;                              // ������
    public GameObject continueButton;       // ������ �����������
    public GameObject skipButton;           // ������ �������� �������

    GameObject npcImage;                    // ������� ���, � ������� �������
    public GameObject heroImage;            // ������� �����    

    [HideInInspector] public bool startEvent;   // ��������� ����� ������
    int dialogeNumber;                      // ��������� ���������� ��� ������ �������

    //IEnumerator coroutine;


    private void Awake()
    {
        instance = this;
        //coroutine = Type(0f);
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
                Invoke(nameof(StartDialog), dialogStore[dialogeNumber].dialogeDelay);                 // �������� ������
                //StartDialog();                                  
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


    // �������� �����
    public void StartEvent(int numberDialog)
    {
        GameManager.instance.dialogeStart = true;
        startEvent = true;                                                      // ����� �����        
        dialogeNumber = numberDialog;                                           // ����� �������
        GameManager.instance.isPlayerEnactive = true;                           // ��������� ���������� �������
        GameManager.instance.player.isImmortal = true;                          // ����� ����������
        TextUI.instance.CursorVisibleOnOff(true);                               // ����������� ������ �� ������
        GameManager.instance.EnemyResetAndNeutral(true);                        // ���������� �����
        BlackTapes(true);                                                       // ������ ������
        GameManager.instance.cameraOnPlayer = true;                             // ������ �� ������
        
        if (LanguageManager.instance.eng)
            sentences = dialogStore[numberDialog].sentencesEng;                 // ����� ����������� �� ������ ����� numberDialog (����)
        else
            sentences = dialogStore[numberDialog].sentences;                    // ����� ����������� �� ������ ����� numberDialog
        characterName = dialogStore[numberDialog].characterName;                // ��� (��������) ���������, ������� ����� ��������
        npcImage = dialogStore[numberDialog].imageNpc;                          // ��� �������
        startInteractAction = dialogStore[numberDialog].awakeInteractAction;    // ����� �� ������ �������
        goInteractAction = dialogStore[numberDialog].goInteractAction;          // ����� ��� ������ �������
        interactAction = dialogStore[numberDialog].interactAction;              // ����� �� ���������� �������
        startInteractAction.Invoke();
    }

    public void StartDialog()
    {
        if (GameManager.instance.forYG)
            YandexGame.FullscreenShow();
        goInteractAction.Invoke();          // ����� ��� ������ �������
        skipButton.SetActive(true);         // �������� ������ �������� �������
        StartCoroutine(Type(0.5f));         // ��������� ������ ����
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
        textAnimator.SetBool("Change", false);          // ������� ������ ��������� ����
        continueButton.SetActive(true);             // ���������� ������ �����������
    }

    public void NextSentence()
    {        
        continueButton.SetActive(false);        // ������� ������ �����������
        textAnimator.SetBool("Change", true);       // ������ ��������� ������

        if (index < sentences.Length - 1)       // ���� ��� ���� �����������
        {
            index++;
            textDisplay.text = "";              // ������� �����
            StartCoroutine(Type(0f));
        }
        else                                    // ���� ����������� �����������
        {
            skipButton.SetActive(false);
            index = 0;
            textDisplay.text = "";
            heroImage.SetActive(false);                         // ������� ��������
            npcImage.SetActive(false);          
            BlackTapes(false);                                  // ������� ������ ������
            GameManager.instance.cameraOnPlayer = false;        // ��������� ������
            GameManager.instance.player.isImmortal = false;     // ����� �� ����������
            GameManager.instance.isPlayerEnactive = false;      // �������� ���������� �������
            TextUI.instance.CursorVisibleOnOff(false);
            GameManager.instance.EnemyResetAndNeutral(false);   // �������� �����
            interactAction.Invoke();                            // �������� ����� (���� ����)
            GameManager.instance.dialogeStart = false;
        }
    }

    public void SkipDialoge()
    {
        skipButton.SetActive(false);
        continueButton.SetActive(false);                    // ������� ������ �����������
        StopAllCoroutines();
        index = 0;
        textDisplay.text = "";
        heroImage.SetActive(false);                         // ������� ��������
        npcImage.SetActive(false);
        BlackTapes(false);                                  // ������� ������ ������
        GameManager.instance.cameraOnPlayer = false;        // ��������� ������
        GameManager.instance.player.isImmortal = false;     // ����� �� ����������
        GameManager.instance.isPlayerEnactive = false;      // �������� ���������� �������
        TextUI.instance.CursorVisibleOnOff(false);          // ������ ������ �� ������
        GameManager.instance.EnemyResetAndNeutral(false);   // �������� �����
        interactAction.Invoke();                            // �������� ����� (���� ����)
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

    // ׸���� ������
    public void BlackTapes(bool status)
    {
        if (status)
            blackImages.GetComponent<Animator>().SetTrigger("In");                   // ��������� ������ ������
        else
            blackImages.GetComponent<Animator>().SetTrigger("Out");                  // ������� ������ ������
    }
}
