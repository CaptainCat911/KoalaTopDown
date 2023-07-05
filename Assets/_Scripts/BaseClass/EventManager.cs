using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{

    // Свет для 3-го лвл
    public void LightOff(bool status)
    {
        GameManager.instance.LightsOff(status);
    }

    public void PlayerMiniLightOn(bool status)
    {
        GameManager.instance.player.MiniLightOn(status);
    }
}
