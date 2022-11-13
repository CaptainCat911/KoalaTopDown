using UnityEngine;
using UnityEngine.AI;

public class Enemy : Fighter
{
    // ������
    NavMeshAgent agent;
    [HideInInspector] public Animator animator;
    SpriteRenderer spriteRenderer;

    // �������������
    public bool isNeutral;                                   // �� ����� ������ ���������
    [HideInInspector] public GameObject target;             // ����
    [HideInInspector] public bool chasing;                  // ������ �������������
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
    public GameObject deathEffect;                          // ������ (����� ������� ��� � ��������� (���  ���))
    bool flipLeft;                                          // ��� �����
    bool flipRight;                                         //    

    // ������ ��� ������ ��� �����
    float timerForColor;
    bool red;

    public override void Awake()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
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

        // ����� ����� ��� ��������� ����� � ��� �����
        SetColorTimer();
    }

    void FixedUpdate()
    {
        if (!target)
            return;
        if (isNeutral)
            return;

        NavMeshHit hit;
        if (!agent.Raycast(target.transform.position, out hit))
        {
            //Debug.Log("Visible");            
            targetVisible = true;                                           // Target is "visible" from our position.
        }
        else
        {
            // ��� �������� �������� ����� ������ ����� ��� ������� (����� ��� ������ �������� ��� ����)
            targetVisible = false;
        }

        // �������������
        if (Vector3.Distance(target.transform.position, transform.position) < triggerLenght && targetVisible || currentHealth != maxHealth)       // ���� ��������� �� ������ < ������ ���������
        {
            chasing = true;                                                 // ������������� �������� 
        }

        if (chasing)                                                        // ���� ����������
        {
            //agent.SetDestination(target.transform.position);                    // ������������ � ����


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

    public override void TakeDamage(int dmg)
    {
        base.TakeDamage(dmg);
        //animator.SetTrigger("TakeHit");
        ColorRed(0.05f);
    }

    // ����� ������ ��� �����
    void SetColorTimer()
    {
        if (timerForColor > 0)                  // ������ ��� ����������� �����
            timerForColor -= Time.deltaTime;
        if (red && timerForColor <= 0)
            ColorWhite();
    }
    void ColorRed(float time)
    {
        timerForColor = time;
        spriteRenderer.color = Color.red;
        red = true;
    }
    void ColorWhite()
    {
        spriteRenderer.color = Color.white;
        red = false;
    }

    // ������� �������
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
        CMCameraShake.Instance.ShakeCamera(3, 0.2f);                                            // ������ ������
        GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);  // ������� ������
        Destroy(effect, 1);                                                                     // ���������� ������ ����� .. ���
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanceToAttack);
    }

    public void SayText(string text)
    {
        ChatBubble.Clear(gameObject);
        ChatBubble.Create(transform, new Vector3(0.2f, 0.2f), text);
    }
}
