using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabBotSettings : MonoBehaviour
{
    BotAI botAI;

    public bool withChat;
    public bool noPatrol;

    public void SetSettingsBot()
    {
        botAI = GetComponentInChildren<BotAI>();
        botAI.withChat = withChat;
        botAI.noPatrol = noPatrol;
    }
}
