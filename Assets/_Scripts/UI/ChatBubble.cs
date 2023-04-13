using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChatBubble : MonoBehaviour
{
    public static void Create(Transform parent, Vector3 localPosition, string text, float timeToDestroy)                 // ������� ������
    {
        Transform chatBubbleTransform = Instantiate(GameAssets.instance.chatBubblePrefab, parent);  // ������ ������ ������� � ����������� ��������
        chatBubbleTransform.localPosition = localPosition;                                          // ����� ������� ������� ����� �� ��� � � ��������
        chatBubbleTransform.GetComponent<ChatBubble>().Setup(text);                                 // �������� ������ �������� ����� � �������
        Destroy(chatBubbleTransform.gameObject, timeToDestroy);
    }

    public static void Clear(GameObject go )                                                        // �������� ������
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
        textMeshPro.ForceMeshUpdate();                                  // ���������� ������ (����� �� ���� �����)
        Vector2 textSize = textMeshPro.GetRenderedValues(false);        // �������� ������� ������

        Vector2 padding = new Vector2(1f, 0.5f);                          // ������ 
        backgroundSpriteRenderer.size = textSize + padding;
    }
}
