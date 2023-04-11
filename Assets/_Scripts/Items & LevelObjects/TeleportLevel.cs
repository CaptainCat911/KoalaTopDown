using UnityEngine;
using UnityEngine.Events;

public class TeleportLevel : MonoBehaviour
{
    //public bool teleportFromShop;
    public bool teleportToShop;             // телепорт в магазин
    public GameObject exitTeleport;         // второй телепорт      
    public GameObject exit;
    public bool actived;                    // телепорт активирован
    //bool isInRange;                       // в ренже мы или нет    
    GameObject gameObjectToTeleport;        // объект, который нужно телепортировать

    void ActivateTeleport()
    {
        actived = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Player player))
        {
            //isInRange = true;
            gameObjectToTeleport = player.gameObject;
            if (actived)
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
        if (teleportToShop)
        {
            TeleportShop.instance.SetExitShopPortal(exit);
        }
    }

/*    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }*/
}
