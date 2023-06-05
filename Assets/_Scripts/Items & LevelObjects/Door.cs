using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;

public class Door : MonoBehaviour
{
    //public bool rockDoor;
    BoxCollider2D boxCollider2D;
    SpriteRenderer spriteRenderer;
    ShadowCaster2D shadow2D;

    public bool openedDoor;                 // ������� ����� ��� ������
    public int doorTypeNumber;              // ��� �����
    public Sprite openedDoorSprite;         // ������ ��������� �����
    Sprite closedDoorSprite;

    bool isOpened;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        shadow2D = GetComponent<ShadowCaster2D>();
        closedDoorSprite = spriteRenderer.sprite;
    }


    private void Start()
    {
        if (openedDoor)
            OpenDoor();
    }

    public void OpenDoorWithKey()
    {   
        if (GameManager.instance.keys[doorTypeNumber] > 0 && !isOpened)         // ���� ������ ������ 0 � ����� ��� �� �������
        {
            OpenDoor();
            GameManager.instance.keys[doorTypeNumber]--;                        // �������� ����
        }
        else
        {
            //GameManager.instance.CreateFloatingMessage("����� ����!", Color.white, transform.position);
        }
    }

    public void OpenDoor()
    {
        spriteRenderer.sprite = openedDoorSprite;           // ������ ������ �������� ����� �� ������ ��������
        boxCollider2D.enabled = false;                      // ������� ���������
        //GetComponent<NavMeshObstacle>().enabled = false;
        shadow2D.enabled = false;                           // ������� ����
        isOpened = true;                                    // ����� �������
    }

    public void CloseDoor()
    {
        spriteRenderer.sprite = closedDoorSprite;           // ������ ������ �������� ����� �� ������ ��������
        boxCollider2D.enabled = true;                       // �������� ���������
        shadow2D.enabled = true;                            // ��������� ����
        //GetComponent<NavMeshObstacle>().enabled = true;
        isOpened = false;                                    // ����� �������
    }
}
