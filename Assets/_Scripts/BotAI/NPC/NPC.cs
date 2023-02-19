using UnityEngine;
using UnityEngine.Events;

public class NPC : BotAI
{    
    public string[] textToSay;      // текст для диалога
    int dialogeNumber;              // номер диалога
    bool isTextDone;                // проговорили весь текст


    public void Speak()
    {
        if (!isTextDone)                            // если не проговорили весь текст
        {
            ChatBubble.Clear(gameObject);           // очищаем диалог
            ChatBubble.Create(transform, new Vector3(-1f, 0.2f), textToSay[dialogeNumber]);     // говорим     

            dialogeNumber++;                        // + к номеру диалога

            if (dialogeNumber >= textToSay.Length)  // если номер диалога последний
            {
                isTextDone = true;                  // проговорили весь текст
            }
        }
        else
        {
            ChatBubble.Clear(gameObject);           // очищаем диалог если всё проговорили
        }
    }

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
