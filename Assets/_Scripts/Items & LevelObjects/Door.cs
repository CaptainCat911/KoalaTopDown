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

    public bool openedDoor;                 // открыть дверь при старте
    public int doorTypeNumber;              // тип двери
    public Sprite openedDoorSprite;         // спрайт открыйтой двери
    Sprite closedDoorSprite;                // спрайт закрытой двери
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
        if (GameManager.instance.keys[doorTypeNumber] > 0 && !isOpened)         // если ключей больше 0 и дверь ещё не открыта
        {
            OpenDoor();
            GameManager.instance.keys[doorTypeNumber]--;                        // забираем ключ
        }
        else
        {
            //GameManager.instance.CreateFloatingMessage("Нужен ключ!", Color.white, transform.position);
        }
    }

    public void OpenDoor()
    {
        spriteRenderer.sprite = openedDoorSprite;           // меняем спрайт закрытой двери на спрайт открытой
        boxCollider2D.enabled = false;                      // убираем коллайдер
        //GetComponent<NavMeshObstacle>().enabled = false;
        shadow2D.enabled = false;                           // убираем тень
        if (effect)
            effect.Stop();
        if (audioSource && !openedDoor)
            audioSource.Play();
        isOpened = true;                                    // дверь открыта
    }

    public void CloseDoor()
    {
        spriteRenderer.sprite = closedDoorSprite;           // меняем спрайт закрытой двери на спрайт открытой
        boxCollider2D.enabled = true;                       // включаем коллайдер
        shadow2D.enabled = true;                            // добавляем тень
        if(effect)
            effect.Play();
        if (audioSource)
            audioSource.Play();
        //GetComponent<NavMeshObstacle>().enabled = true;
        isOpened = false;                                    // дверь открыта
    }
}
