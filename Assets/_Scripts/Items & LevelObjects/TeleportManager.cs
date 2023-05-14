using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportManager : MonoBehaviour
{
    public static TeleportManager instance;     // инстанс

    public TeleportLevel shopPortal;            // портал из магазина
    //public TeleportLevel resPortal;             // Портал из комнаты воскерешения

    private void Awake()
    {
        instance = this;
    }

    public void SetExitShopPortal(GameObject newTeleport)
    {
        shopPortal.exitTeleport = newTeleport;
    }
/*    public void SetExitResPortal(GameObject newTeleport)
    {
        resPortal.exitTeleport = newTeleport;
    }*/
}
