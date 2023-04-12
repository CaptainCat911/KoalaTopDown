using UnityEngine;
using UnityEngine.Events;

public class NPC : BotAI
{
    [Header("��������� �����")]
    public float meleeDistanceToAttack;
    public float rangeDistanceToAttack;

    [Header("����� ����")]
    public string[] textToSay;      // ����� ��� �������
    int dialogeNumber;              // ����� �������
    bool isTextDone;                // ����������� ���� �����


    public void Speak()
    {
        if (!isTextDone)                            // ���� �� ����������� ���� �����
        {
            ChatBubble.Clear(gameObject);           // ������� ������
            ChatBubble.Create(transform, new Vector3(-1f, 0.2f), textToSay[dialogeNumber], 4f);     // �������     

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

/*    public void NpcMagazine()
    {
        GameManager.instance.OpenCloseMagazine();
    }*/

/*    protected override void Death()
    {
        base.Death();
        Destroy(gameObject);
    }*/

    /*    void ClearDialoge()
        {
            ChatBubble[] chats = GetComponentsInChildren<ChatBubble>();
            foreach (ChatBubble chat in chats)
            {
                chat.gameObject.SetActive(false);
            }
        }*/
}
