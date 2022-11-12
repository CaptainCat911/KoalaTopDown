using UnityEngine;
using UnityEngine.Events;

public class NPC : MonoBehaviour
{    
    public string[] textToSay;      // ����� ��� �������
    int dialogeNumber;              // ����� �������
    bool isTextDone;                // ����������� ���� �����


    public void Speak()
    {
        if (!isTextDone)                            // ���� �� ����������� ���� �����
        {
            ChatBubble.Clear(gameObject);           // ������� ������
            ChatBubble.Create(transform, new Vector3(-1f, 0.2f), textToSay[dialogeNumber]);     // �������     

            dialogeNumber++;                        // + � ������ �������

            if (dialogeNumber >= textToSay.Length)  // ���� ����� ������� ���������
            {
                isTextDone = true;                  // ����������� ���� �����
            }
        }
        else
        {
            ChatBubble.Clear(gameObject);           // ������� ������ ���� �� �����������
        }
    }

    public void SpeakText(string text)
    {
        ChatBubble.Clear(gameObject);
        ChatBubble.Create(transform, new Vector3(-1f, 0.2f), text);        
    }

/*    void ClearDialoge()
    {
        ChatBubble[] chats = GetComponentsInChildren<ChatBubble>();
        foreach (ChatBubble chat in chats)
        {
            chat.gameObject.SetActive(false);
        }
    }*/
}
