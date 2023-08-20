using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseHelp : MonoBehaviour
{
    public GameObject[] helps;
    int i;
    Animator animator;
    bool helpOn;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (helpOn)
            GameManager.instance.MovePlayer(GameManager.instance.player.transform.position);
    }

    public void StartHelpPause()
    {
        GameManager.instance.helpOn = true;
        GameManager.instance.isPlayerEnactive = true;
        TextUI.instance.CursorVisibleOnOff(true);
        helpOn = true;
        animator.SetTrigger("HelpOn");
        //helps[number].SetActive(true);
    }

    public void NextHelp()
    {
        helps[i].SetActive(false);
        i++;
        helps[i].SetActive(true);
    }

    public void HideHelpPause()
    {
        //helps[number].SetActive(false);
        animator.SetTrigger("HelpOff");
        helpOn = false;
        GameManager.instance.playerAtTarget = false;
        GameManager.instance.isPlayerEnactive = false;
        TextUI.instance.CursorVisibleOnOff(false);
        GameManager.instance.helpOn = false;
    }
}
