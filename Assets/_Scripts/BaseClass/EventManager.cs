using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{

    // ���� ��� 3-�� ���
    public void LightOff(bool status)
    {
        GameManager.instance.LightsOff(status);
    }

    public void PlayerMiniLightOn(bool status)
    {
        GameManager.instance.player.MiniLightOn(status);
    }
}
