using UnityEngine;

public class Teleport : MonoBehaviour
{
    Animator animator;
    public GameObject exitTeleport;         // ������ ��������
    public float timeToTeleport;            // ����� ����� ������� ���������� ������������
    bool isInRange;                         // � ����� �� ��� ���
    float timerCount;                       // ������
    GameObject goToTeleport;                // ������, ������� ����� ���������������

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Update()
    {
        if (isInRange)                      // ���� � �����
        {
            Timer();                        // ����������� ������
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            isInRange = true;
            goToTeleport = player.gameObject;
            timerCount = timeToTeleport;                // ���� ����� � �������� ����������� ����� �������
            animator.SetBool("Teleporting", true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            isInRange = false;
            goToTeleport = null;
            animator.SetBool("Teleporting", false);
        }
    }

    void Timer()
    {
        if (timerCount > 0)                             // ���� ������ ������ 
            timerCount -= Time.deltaTime;
        if (timerCount <= 0)                            // ���� ������ ����������
            TeleportObject(goToTeleport);               // �������������
    }

    public void TeleportObject(GameObject go)
    {
        go.transform.position = exitTeleport.transform.position;
    }
}
