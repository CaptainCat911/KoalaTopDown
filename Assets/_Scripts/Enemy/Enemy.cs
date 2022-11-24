using UnityEngine;
using UnityEngine.AI;

public class Enemy : Fighter
{
    // ������
    NavMeshAgent agent;
    [HideInInspector] public Animator animator;
    SpriteRenderer spriteRenderer;
    EnemyHitBoxPivot pivot;
    EnemyHitbox hitBox;

    // �������������
    public bool isNeutral;                                  // �� ����� ������ ���������
    [HideInInspector] public GameObject target;             // ����
    [HideInInspector] public bool chasing;                  // ������ �������������
    public float triggerLenght;                             // ��������� �������
    public float distanceToAttack;                          // ���������, � ������� ����� ��������� (0.8 ��� �������)
    [HideInInspector] public bool targetVisible;            // ����� �� ���� ��� ���
    [HideInInspector] public bool readyToAttack;            // ����� ���������

    
    //public float attackSpeed = 1;                           // �������� �����

    // ��� ��������
    public GameObject deathEffect;                          // ������ (����� ������� ��� � ��������� (���  ���))
    public float deathCameraShake;                          // �������� ������ ������ ��� ��������
    bool flipLeft;                                          // ��� �����
    bool flipRight;                                         //    

    // ������ ��� ������ ��� �����
    float timerForColor;
    bool red;

    // �����
    public bool debug;

    public override void Awake()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        pivot = GetComponentInChildren<EnemyHitBoxPivot>();
        hitBox = GetComponentInChildren<EnemyHitbox>();
    }

    void Start()
    {
        //target = GameManager.instance.player.gameObject;    // ���� ��� ���� ������ �����

        agent.updateRotation = false;                       // ��� ������2�
        agent.updateUpAxis = false;                         //
    }

    private void Update()
    {
        // ������� ������� (Flip)       
        if (chasing && targetVisible)                           // (����� chasing �������� �� target � ��� ��� ����������� � ������������)
        {
            if (Mathf.Abs(pivot.aimAngle) > 90 && !flipLeft)
            {
                FaceTargetLeft();
            }
            if (Mathf.Abs(pivot.aimAngle) <= 90 && !flipRight)
            {
                FaceTargetRight();
            }
        }
        else
        {
            if (agent.velocity.x < -0.2 && !flipLeft)
            {
                FaceTargetLeft();
            }
            if (agent.velocity.x > 0.2 && !flipRight)
            {
                FaceTargetRight();
            }
        }

        animator.SetFloat("Speed", agent.velocity.magnitude);

        // ����� ����� ��� ��������� ����� � ��� �����
        SetColorTimer();

        // �����
        if (debug)
            Debug.Log(targetVisible);
    }

    void FixedUpdate()
    {
        if (!target)
        {            
            return;
        }
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
        if (Vector3.Distance(target.transform.position, transform.position) < triggerLenght && targetVisible)       // ���� ��������� �� ������ < ������ ���������
        {
            chasing = true;                                                 // ������������� �������� 
        }

        if (chasing)                                                        // ���� ����������
        {
            Chase(target);
        }

        if (debug)
        {
            //Debug.Log(chasing);
        }
    }

    public void Chase(GameObject target)
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

    public void SetDestination(Vector3 destination)
    {
        agent.SetDestination(destination);
    }


    // ������ ��� ������ �������� (����� ���-������ ������� �� �����������)
    public void AttacHitBox()               
    {
        hitBox.Attack();
    }   

    public void EffectRangeAttackHitBox()
    {
        hitBox.EffectRangeAttack();
    }



    public void SayText(string text)
    {
        ChatBubble.Clear(gameObject);
        ChatBubble.Create(transform, new Vector3(0.2f, 0.2f), text);
    }

    public void ForceBackFire(Vector3 forceDirection, float forceBack)
    {
        Vector2 vec2 = (transform.position - forceDirection).normalized;        // ����������� ������ �����������������
        rb2D.AddForce(vec2 * forceBack, ForceMode2D.Impulse);                   // ������� ���������
    }


    public override void TakeDamage(int dmg)
    {
        if (currentHealth == maxHealth)             // ���� �������� ����, �� ����� ���� ������
            chasing = true;
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
        CMCameraShake.Instance.ShakeCamera(deathCameraShake, 0.2f);                             // ������ ������
        GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);  // ������� ������ ��������
        //effect.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        Destroy(effect, 1);                                                                     // ���������� ������ ����� .. ���
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanceToAttack);
    }
}