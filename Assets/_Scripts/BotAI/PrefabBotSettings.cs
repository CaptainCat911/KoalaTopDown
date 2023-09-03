using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabBotSettings : MonoBehaviour
{
    [Header("��� �����")]
    public bool withChat;
    public bool withChatAudio;
    public bool noPatrol;
    public bool noTriggerAgro;
    public bool turnLeft;
    BotAI botAI;

    [Header("��� �������")]
    public bool changeLight;        // �������� ���� ������
    public Color color;             // ��� ����
    public float intensity;         // �������������
    Torch torch;

    public void SetSettingsBot()
    {
        botAI = GetComponentInChildren<BotAI>();
        if (botAI)
        {
            botAI.withChat = withChat;
            botAI.withAudioChat = withChatAudio;
            botAI.noPatrol = noPatrol;
            botAI.noTriggerAgro = noTriggerAgro;
            botAI.makeLeft = turnLeft;
            return;
        }

        torch = GetComponentInChildren<Torch>();
        if (torch)
        {
            if (changeLight)
            {
                torch.SetColorTorch(color, intensity);
            }
            return;
        }
    }
}
