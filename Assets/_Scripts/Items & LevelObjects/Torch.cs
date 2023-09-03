using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.Serialization;
using UnityEngine.Scripting.APIUpdating;
using System;
using UnityEngine.Rendering.Universal;

public class Torch : MonoBehaviour
{
    Animator animator;
    bool lightIsDark;
    bool lightIsOff;
    public bool svecha;

    Light2D light2D;
/*    [HideInInspector] public Color color;
    [HideInInspector] public float intensity;*/
    

    private void Awake()
    {
        animator = GetComponent<Animator>();
        light2D = GetComponentInChildren<Light2D>();
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

    public void SetColorTorch(Color color, float intensity)
    {
        //Debug.Log("!");
        light2D.color = color;
        light2D.intensity = intensity;
        //Debug.Log(light2D.intensity);
    }
}
