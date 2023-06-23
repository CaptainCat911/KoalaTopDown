using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.Serialization;
using UnityEngine.Scripting.APIUpdating;
using System;

public class Torch : MonoBehaviour
{
    Animator animator;
    bool lightIsDark;
    bool lightIsOff;
    public bool svecha;
    

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {

    }

   
    void Update()
    {
        if (svecha)
        {
            return;
        }

        if (GameManager.instance.lightDark && !lightIsDark)
        {
            SetLightType(true);
        }
        if (!GameManager.instance.lightDark && lightIsDark)
        {
            SetLightType(false);
        }

        if (GameManager.instance.lightOff && !lightIsOff)
        {
            SetLightOff(true);
        }
        if (!GameManager.instance.lightOff && lightIsOff)
        {
            SetLightOff(false);
        }
    }

    public void SetLightType(bool status)
    {
        lightIsDark = status;
        animator.SetBool("DarkLight", status);
    }

    public void SetLightOff(bool status)
    {
        lightIsOff = status;
        animator.SetBool("LightOff", status);
    }
}
