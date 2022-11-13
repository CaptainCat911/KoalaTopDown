using UnityEngine;
using UnityEngine.Events;

public class Teleport : MonoBehaviour
{
    Player player;
    Animator animator;
    public GameObject exitTeleport;         // ������ ��������
    public float timeToTeleport;            // ����� ����� ������� ���������� ������������    
    public bool actived;                    // �������� �����������
    public int batteryToActivate;           // ������� ������� �����, ����� ������������ ��������
    //public UnityEvent interactAction;       // �����

    bool isInRange;                         // � ����� �� ��� ���
    bool startedLoadTeleport;               // ������������ ������
    float timerCount;                       // ������
    GameObject goToTeleport;                // ������, ������� ����� ���������������

    private void Start()
    {
        player = GameManager.instance.player;
        animator = GetComponent<Animator>();
        if (actived)
        {
            animator.SetBool("Activated", true);
        }
    }

    public void ActivateTeleportWithDelay()
    {
        if (GameManager.instance.battery >= batteryToActivate && !actived)
        {
            Invoke("ActivateTeleport", 1);
            animator.SetBool("Activated", true);
            GameManager.instance.battery -= batteryToActivate;                      // �������� �������
            player.SayText("������ �������� �����������");
        }
        else if (GameManager.instance.battery < batteryToActivate && !actived)
        {
            player.SayText("� ���� ������������ ������� ��� ��������� ���������");
        }
    }
    void ActivateTeleport()
    {
        actived = true;
        
    }

    void Update()
    {
        if (actived && isInRange && !startedLoadTeleport && Input.GetKeyDown(GameManager.instance.keyToUse))
        {            
            startedLoadTeleport = true;
            timerCount = timeToTeleport;            // ���� ����� � �������� ����������� ����� �������
            animator.SetBool("Teleporting", true);
        }
        if (startedLoadTeleport)
        {
            Timer();                                // ����������� ������
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            isInRange = true;
            goToTeleport = player.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            isInRange = false;
            goToTeleport = null;            
        }
    }

    void Timer()
    {
        if (timerCount > 0)                             // ���� ������ ������ 
            timerCount -= Time.deltaTime;
        if (timerCount <= 0)                            // ���� ������ ���������� � ����� � �����
            TeleportObject(goToTeleport);               // �������������
    }

    public void TeleportObject(GameObject go)
    {
        if (isInRange)
            go.transform.position = exitTeleport.transform.position;
        animator.SetBool("Teleporting", false);
        startedLoadTeleport = false;
    }
}
