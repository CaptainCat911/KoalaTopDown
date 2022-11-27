using UnityEngine;
using UnityEngine.AI;

public class Enemy : Fighter
{
    // ������
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Animator animator;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public EnemyHitbox hitBox;
    EnemyHitBoxPivot pivot;
    EnemyThinker enemyThinker;


    // �������������
    public bool isNeutral;                                  // �� ����� ������ ���������
    //[HideInInspector] public GameObject target;             // ����
    [HideInInspector] public bool chasing;                  // ������ �������������
    Vector3 startPosition;                                  // ������� ��� ������
    public float chaseLeght;                                // ��������� �������������    
    public float triggerLenght;                             // ��������� �������
    [HideInInspector] public bool targetVisible;            // ����� �� ���� ��� ���
    public bool readyToAttack;            // ����� ���������
    public float distanceToAttack;                          // ���������, � ������� ����� ��������� (0.8 ��� �������)    

    // ��� ��������
    [HideInInspector] public float aimAnglePivot;           // ���� �������� �������������
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
        enemyThinker = GetComponentInChildren<EnemyThinker>();
    }

    void Start()
    {
        startPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        agent.updateRotation = false;                       // ��� ������2�
        agent.updateUpAxis = false;                         //
        agent.ResetPath();                                  // ���������� ����, ������ ��� �� ��� ������ ����
    }

    private void Update()
    {
        // ������� ��������
        if (enemyThinker.target && chasing && targetVisible)
        {
            Vector3 aimDirection = enemyThinker.target.transform.position - pivot.transform.position;               // ���� ����� ���������� ���� � pivot ������          
            aimAnglePivot = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;                            // ������� ���� � ��������             
            Quaternion qua1 = Quaternion.Euler(0, 0, aimAnglePivot);                                                // ������� ���� ���� � Quaternion
            pivot.transform.rotation = Quaternion.Lerp(pivot.transform.rotation, qua1, Time.fixedDeltaTime * 15);   // ������ Lerp ����� weaponHoder � ����� �����
        }

        // ������� ������� (Flip)       
        if (enemyThinker.target && targetVisible)                           // (����� chasing �������� �� target � ��� ��� ����������� � ������������)
        {
            if (Mathf.Abs(aimAnglePivot) > 90 && !flipLeft)
            {
                FaceTargetLeft();
            }
            if (Mathf.Abs(aimAnglePivot) <= 90 && !flipRight)
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


    public void NavMeshRayCast(GameObject target)
    {
        NavMeshHit hit;
        if (!agent.Raycast(target.transform.position, out hit))
        {
            //Debug.Log("Visible");            
            targetVisible = true;                                           // ���� ������ �� ����� �������
        }
        else
        {
            // ��� �������� �������� ����� ������ ����� ��� ������� (����� ��� ������ �������� ��� ����)
            targetVisible = false;
        }
    }


/*    public void ChaseAndAttack(GameObject target)
    {        
        float distance = Vector3.Distance(transform.position, target.transform.position);       // ������� ��������� �� ����
        if (distance < distanceToAttack && targetVisible)                                       // ���� ����� �� ���� � ����� �
        {
            if (!readyToAttack)
            {
                agent.ResetPath();                                                              // ���������� ����            
                readyToAttack = true;                                                           // ����� ��������
                //Debug.Log("Ready Attack");
            }
        }
        else 
        {
            agent.SetDestination(target.transform.position);                                    // ������������ � ����
            if (readyToAttack)
                readyToAttack = false;                                                          // �� ����� ��������                
        }
    }*/

    public void SetDestination(Vector3 destination)
    {
        agent.SetDestination(destination);
    }

    public void SayText(string text)
    {
        ChatBubble.Clear(gameObject);
        ChatBubble.Create(transform, new Vector3(0.2f, 0.2f), text);
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





    public void ForceBackFire(Vector3 forceDirection, float forceBack)
    {
        Vector2 vec2 = (transform.position - forceDirection).normalized;        // ����������� ������ �����������������
        rb2D.AddForce(vec2 * forceBack, ForceMode2D.Impulse);                   // ������� ���������
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