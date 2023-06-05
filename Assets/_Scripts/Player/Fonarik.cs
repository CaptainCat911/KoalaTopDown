using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fonarik : MonoBehaviour
{
    public GameObject spotLightNormal;
    public GameObject spotLightMini;
    public GameObject fonarikHorror;
    public GameObject spotLightHorror;
    bool horrorMode = true;
    bool bigLight = true;

    void Start()
    {
        
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (horrorMode)
            {
                spotLightMini.SetActive(false);
                spotLightNormal.SetActive(true);
                fonarikHorror.SetActive(false);
                spotLightHorror.SetActive(false);
                horrorMode = false;
                bigLight = true;
            }
            else
            {
                if (bigLight)
                {
                    spotLightNormal.SetActive(false);                     
                    spotLightMini.SetActive(true);
                    bigLight = false;
                }
                else
                {
                    spotLightMini.SetActive(false);
                    fonarikHorror.SetActive(true);
                    spotLightHorror.SetActive(true);                    
                    horrorMode = true;
                }
            }
        }
    }
}
