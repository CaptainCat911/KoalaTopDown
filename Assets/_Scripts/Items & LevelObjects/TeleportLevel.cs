using UnityEngine;
using UnityEngine.Events;

public class TeleportLevel : MonoBehaviour
{
    //public bool teleportFromShop;
    public bool teleportToShop;             // �������� � �������
    public GameObject exitTeleport;         // ������ ��������      
    public GameObject exit;                 // ����� �� �������� ���������
    //public bool actived;                    // �������� �����������
    //bool isInRange;                       // � ����� �� ��� ���    
    GameObject gameObjectToTeleport;        // ������, ������� ����� ���������������

    [Header("��������� ������� (�������� �����)")]
    public bool teleportFromRes;            // �������� �� �������
    public bool teleportPozor;              // �������� ��������
    public GameObject doorRes;              // ����� 

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
            // ��� �������, �������� �����
            if (teleportFromRes)
            {
                GameManager.instance.playerInResroom = false;               // ����� �� �������
                doorRes.GetComponent<Door>().CloseDoor();                   // ������� �����
                ResRoomManager.instance.ResetMonsterAndChest();           // ���������� ������ � ������
                if (teleportPozor)                                          // ���� ������ ������
                {
                    GameManager.instance.pozorCount++;                      // + � ������
                    TextUI.instance.pozorGo.SetActive(true);                // �������� UI ������
                }
            }


            //isInRange = true;
            gameObjectToTeleport = player.gameObject;
            //if (actived)
            TeleportObject(gameObjectToTeleport);                   // �������������
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
            GameManager.instance.player.Invoke(nameof(GameManager.instance.player.ExplousionPlayer), 0.2f);     // (�������� �����)
        }

        if (audioSource)                    // ����
            audioSource.Play();
    }

    /*    void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, 0.5f);
        }*/
}
