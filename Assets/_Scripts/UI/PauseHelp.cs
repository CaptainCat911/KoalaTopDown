using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseHelp : MonoBehaviour
{
    //public GameObject[] helps;
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void StartHelpPause()
    {
        GameManager.instance.isPlayerEnactive = true;
        animator.SetTrigger("HelpOn");
        //helps[number].SetActive(true);
    }

    public void HideHelpPause()
    {
        //helps[number].SetActive(false);
        animator.SetTrigger("HelpOff");
        GameManager.instance.isPlayerEnactive = false;
    }
}
