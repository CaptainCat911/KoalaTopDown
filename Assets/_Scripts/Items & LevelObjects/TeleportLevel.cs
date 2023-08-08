using UnityEngine;
using UnityEngine.Events;

public class TeleportLevel : MonoBehaviour
{
    //public bool teleportFromShop;
    public bool teleportToShop;             // телепорт в магазин
    public GameObject exitTeleport;         // второй телепорт      
    public GameObject exit;                 // выход из входного телепорта
    //public bool actived;                    // телепорт активирован
    //bool isInRange;                       // в ренже мы или нет    
    GameObject gameObjectToTeleport;        // объект, который нужно телепортировать

    [Header("Настройки РесРума (временно здесь)")]
    public bool teleportFromRes;            // телепорт из ресрума
    public bool teleportPozor;              // позорный телепорт
    public GameObject doorRes;              // дверь 

    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }


    /*    void ActivateTeleport()
        {
            actived = true;
        }*/

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Player player))
        {
            // Для ресрума, временно здесь
            if (teleportFromRes)
            {
                GameManager.instance.playerInResroom = false;               // вышли из ресрума
                doorRes.GetComponent<Door>().CloseDoor();                   // закрыли дверь
                ResRoomManager.instance.ResetMonsterAndChest();           // уничтожили сундук и мостра
                if (teleportPozor)                                          // если портал позора
                {
                    GameManager.instance.pozorCount++;                      // + к позору
                    TextUI.instance.pozorGo.SetActive(true);                // включаем UI позора
                }
            }


            //isInRange = true;
            gameObjectToTeleport = player.gameObject;
            //if (actived)
            TeleportObject(gameObjectToTeleport);                   // телепортируем
        }
    }
/*    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            isInRange = false;
            goToTeleport = null;
        }
    }*/

    public void TeleportObject(GameObject go)
    {
        //if (isInRange)        
        go.transform.position = exitTeleport.transform.position;
        //go.GetComponent<Rigidbody2D>(). = 0;

        if (teleportToShop)
        {
            TeleportManager.instance.SetExitShopPortal(exit);
        }
        
        if (teleportFromRes)
        {
            GameManager.instance.player.Invoke(nameof(GameManager.instance.player.ExplousionPlayer), 0.2f);     // (Временно здесь)
        }

        if (audioSource)                    // звук
            audioSource.Play();
    }

    /*    void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, 0.5f);
        }*/
}
