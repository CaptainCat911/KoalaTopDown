using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportShop : MonoBehaviour
{
    public static TeleportShop instance;         // инстанс

    public TeleportLevel shopPortal;

    private void Awake()
    {
        instance = this;
    }


    public void SetExitShopPortal(GameObject newTeleport)
    {
        shopPortal.exitTeleport = newTeleport;
    }
}
