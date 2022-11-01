using UnityEngine;
using UnityEngine.AI;

public class Enemy : Fighter
{
    // ������
    NavMeshAgent agent;
    [HideInInspector] public Animator animator;
    SpriteRenderer spriteRenderer;
    [HideInInspector] public Rigidbody2D rb2D;

    // �������������
    [HideInInspector] public GameObject target;             // ����
    bool chasing;                                           // ������ �������������
    public float triggerLenght;                             // ��������� �������
    public float distanceToAttack;                          // ���������, � ������� ����� ���������
    [HideInInspector] public bool targetVisible;            // ����� �� ���� ��� ���
    [HideInInspector] public bool readyToAttack;            // ����� ���������

    // �����
    [HideInInspector] public float lastAttack;              // ����� ���������� ����� (��� ����������� �����)
    public float cooldown = 0.5f;                           // ����������� �����
    public int attackDamage;                                // ����
    public float pushForce;                                 // ���� ������
    //public float attackSpeed = 1;                           // �������� �����

    // ��� ��������
    bool flipLeft;                                          // ��� �����
    bool flipRight;                                         //    


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb2D = GetComponent<Rigidbody2D>();

        target = GameManager.instance.player.gameObject;    // ���� ��� ���� ������ �����

        agent.updateRotation = false;                       // ��� ������2�
        agent.updateUpAxis = false;                         //
    }

    private void Update()
    {
        // ������� ������� (Flip)       (����� ����� �������� �� ����� ����������)
        if (agent.velocity.x < -0.2 && !flipLeft)
        {
            FaceTargetLeft();
        }
        if (agent.velocity.x > 0.2 && !flipRight)
        {
            FaceTargetRight();
        }

        animator.SetFloat("Speed", agent.velocity.magnitude);
    }

    void FixedUpdate()
    {
        if (!target)
            return;

        // �������������
        if (Vector3.Distance(target.transform.position, transform.position) < triggerLenght)       // ���� ��������� �� ������ < ������ ���������
        {
            chasing = true;                                                 // ������������� �������� 
        }

        if (chasing)                                                        // ���� ����������
        {
            //agent.SetDestination(target.transform.position);                    // ������������ � ����
            NavMeshHit hit;
            if (!agent.Raycast(target.transform.position, out hit))
            {
                //Debug.Log("Visible");            
                targetVisible = true;                                       // Target is "visible" from our position.
            }
            else
            {
                // ��� �������� �������� ����� ������ ����� ��� ������� (����� ��� ������ �������� ��� ����)
                targetVisible = false;
            }

            float distance = Vector3.Distance(transform.position, target.transform.position);       // ������� ��������� �� ����
            if (distance < distanceToAttack && targetVisible)                                       // ���� ����� �� ���� � ����� �
            {
                agent.ResetPath();                                                                  // ���������� ����            
                readyToAttack = true;                                                               // ����� ��������
                //Debug.Log("Ready Attack");
            }
            else
            {
                agent.SetDestination(target.transform.position);                                    // ������������ � ����
                readyToAttack = false;                                                              // �� ����� ��������                
            }
        }
    }


    void FaceTargetRight()                                          // ������� �������
    {
        spriteRenderer.flipX = false;
        flipLeft = false;
        flipRight = true;
    }
    void FaceTargetLeft()                                           // ������� ������
    {
        spriteRenderer.flipX = true;
        flipRight = false;
        flipLeft = true;
    }

    protected override void Death()
    {
        base.Death();
        Destroy(gameObject);
    }
}
