using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportManager : MonoBehaviour
{
    public static TeleportManager instance;     // �������

    public TeleportLevel shopPortal;            // ������ �� ��������
    //public TeleportLevel resPortal;             // ������ �� ������� ������������

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
