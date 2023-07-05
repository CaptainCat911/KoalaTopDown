using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabBotSettings : MonoBehaviour
{
    BotAI botAI;

    public bool withChat;
    public bool withChatAudio;
    public bool noPatrol;
    public bool noTriggerAgro;

    public void SetSettingsBot()
    {
        botAI = GetComponentInChildren<BotAI>();
        botAI.withChat = withChat;
        botAI.withAudioChat = withChatAudio;
        botAI.noPatrol = noPatrol;
        botAI.noTriggerAgro = noTriggerAgro;
    }
}
