using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChatBubble : MonoBehaviour
{
    public static void Create(Transform parent, Vector3 localPosition, string text)                 // создать диалог
    {
        Transform chatBubbleTransform = Instantiate(GameAssets.instance.chatBubblePrefab, parent);  // создаём префаб диалога и присваиваем родителя
        chatBubbleTransform.localPosition = localPosition;                                          // задаём позицию диалога такую же как и у родителя
        chatBubbleTransform.GetComponent<ChatBubble>().Setup(text);                                 // вызываем фукцию написать текст у префаба
        Destroy(chatBubbleTransform.gameObject, 4f);
    }

    public static void Clear(GameObject go )                                                        // очистить диалог
    {
        ChatBubble[] chats = go.GetComponentsInChildren<ChatBubble>();
        foreach (ChatBubble chat in chats)
        {
            chat.gameObject.SetActive(false);
        }
    }


    SpriteRenderer backgroundSpriteRenderer;
    //SpriteRenderer iconSpriteRenderer;
    TextMeshPro textMeshPro;

    private void Awake()
    {
        backgroundSpriteRenderer = transform.Find("Background").GetComponent<SpriteRenderer>();
        //iconSpriteRenderer = transform.Find("Icon").GetComponent<SpriteRenderer>();
        textMeshPro = transform.Find("Text").GetComponent<TextMeshPro>();
    }

    private void Start()
    {
        //Setup("Hi");
    }

    void Setup(string text)
    {
        textMeshPro.SetText(text);
        textMeshPro.ForceMeshUpdate();                                  // обновление текста (чтобы не было багов)
        Vector2 textSize = textMeshPro.GetRenderedValues(false);        // получаем размеры текста

        Vector2 padding = new Vector2(1f, 0.5f);                          // оффсет 
        backgroundSpriteRenderer.size = textSize + padding;
    }
}
