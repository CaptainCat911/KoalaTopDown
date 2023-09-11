using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseHelp : MonoBehaviour
{
    public GameObject[] helps;          // подсказки
    public GameObject[] helpsEng;       // подсказки англ
    int i;                              // счетчик
    Animator animator;
    bool helpOn;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (LanguageManager.instance.eng)
        {
            foreach (GameObject help in helps)
            {
                help.SetActive(false);
            }
        }
        else
        {
            foreach (GameObject help in helpsEng)
            {
                help.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (helpOn)
            GameManager.instance.MovePlayer(GameManager.instance.player.transform.position);        // при подсказке стоим на месте (сброс анимации бега)
    }

    // Запускаем подсказку
    public void StartHelpPause()
    {
        GameManager.instance.helpOn = true;
        GameManager.instance.isPlayerEnactive = true;
        TextUI.instance.CursorVisibleOnOff(true);           // вкл курсор
        helpOn = true;                                      // подсказка запущена
        animator.SetTrigger("HelpOn");                      // показываем подсказки
        //helps[number].SetActive(true);
    }

    public void NextHelp()
    {
        if (LanguageManager.instance.eng)
        {
            helpsEng[i].SetActive(false);
            i++;
            helpsEng[i].SetActive(true);
        }
        else
        {
            helps[i].SetActive(false);
            i++;
            helps[i].SetActive(true);
        }
    }

    public void HideHelpPause()
    {
        //helps[number].SetActive(false);
        animator.SetTrigger("HelpOff");                     // убираем подсказки
        helpOn = false;
        GameManager.instance.playerAtTarget = false;
        GameManager.instance.isPlayerEnactive = false;
        TextUI.instance.CursorVisibleOnOff(false);          // откл курсор
        GameManager.instance.helpOn = false;
    }
}
