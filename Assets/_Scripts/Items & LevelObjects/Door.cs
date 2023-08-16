using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;

public class Door : MonoBehaviour
{
    //public bool rockDoor;
    BoxCollider2D boxCollider2D;
    SpriteRenderer spriteRenderer;
    ShadowCaster2D shadow2D;
    AudioSource audioSource;

    public bool openedDoor;                 // ������� ����� ��� ������
    public int doorTypeNumber;              // ��� �����
    public Sprite openedDoorSprite;         // ������ ��������� �����
    Sprite closedDoorSprite;                // ������ �������� �����
    public ParticleSystem effect;

    bool isOpened;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        shadow2D = GetComponent<ShadowCaster2D>();
        closedDoorSprite = spriteRenderer.sprite;
        audioSource = GetComponent<AudioSource>();
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
        if (effect)
            effect.Stop();
        if (audioSource && !openedDoor)
            audioSource.Play();
        isOpened = true;                                    // ����� �������
    }

    public void CloseDoor()
    {
        spriteRenderer.sprite = closedDoorSprite;           // ������ ������ �������� ����� �� ������ ��������
        boxCollider2D.enabled = true;                       // �������� ���������
        shadow2D.enabled = true;                            // ��������� ����
        if(effect)
            effect.Play();
        if (audioSource)
            audioSource.Play();
        //GetComponent<NavMeshObstacle>().enabled = true;
        isOpened = false;                                    // ����� �������
    }
}
