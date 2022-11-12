using UnityEngine;

public class Teleport : MonoBehaviour
{
    Animator animator;
    public GameObject exitTeleport;         // ������ ��������
    public float timeToTeleport;            // ����� ����� ������� ���������� ������������    
    bool isInRange;                         // � ����� �� ��� ���
    bool startLoadTeleport;                 // ������������ ������
    float timerCount;                       // ������
    GameObject goToTeleport;                // ������, ������� ����� ���������������

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Update()
    {
        if (isInRange && !startLoadTeleport && Input.GetKeyDown(GameManager.instance.keyToUse))
        {            
            startLoadTeleport = true;
            timerCount = timeToTeleport;            // ���� ����� � �������� ����������� ����� �������
            animator.SetBool("Teleporting", true);
        }
        if (startLoadTeleport)
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
        startLoadTeleport = false;
    }
}
