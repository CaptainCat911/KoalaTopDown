using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseHelp : MonoBehaviour
{
    //public GameObject[] helps;
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
        helpOn = true;
        animator.SetTrigger("HelpOn");
        //helps[number].SetActive(true);
    }

    public void HideHelpPause()
    {
        //helps[number].SetActive(false);
        animator.SetTrigger("HelpOff");
        helpOn = false;
        GameManager.instance.playerAtTarget = false;
        GameManager.instance.isPlayerEnactive = false;
        GameManager.instance.helpOn = false;
    }
}
