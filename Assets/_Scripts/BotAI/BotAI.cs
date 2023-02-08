using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotAI : Fighter
{
    // ������
    //EnemyThinker enemyThinker;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Animator animator;
    BotAIAnimator animatorWeapon;
    [HideInInspector] public SpriteRenderer spriteRenderer;    
    BotAIHitBoxPivot pivot;
    [HideInInspector] public BotAIMeleeWeaponHolder botAIMeleeWeaponHolder;
    [HideInInspector] public BotAIRangeWeaponHolder botAIRangeWeaponHolder;
    BotAIHitbox hitBox;
    //public Animator animatorHit;                            // �������� ���� ������

    // ��� ����
    public bool isNeutral;                                  // �� ����� ������ ���������
    public bool isFriendly;                                 // ������� ���
    public bool isEnemy;                                    // ��������� ���

    // �������������
    [HideInInspector] public GameObject target;             // ����
    [HideInInspector] public LayerMask layerTarget;         // ���� ��� ������ 
    [HideInInspector] public LayerMask layerHit;            // ���� ��� ������
    [HideInInspector] public bool chasing;                  // ������ �������������
    [HideInInspector] public Vector3 startPosition;         // ������� ��� ������
    public float chaseLeght;                                // ��������� �������������    
    public float triggerLenght;                             // ��������� �������
    [HideInInspector] public bool targetVisible;            // ����� �� ���� ��� ���
    public bool readyToAttack;                              // ����� ���������
    public float distanceToAttackMelee;                     // ��������� ��� ������ ����
    public float distanceToAttackRange;                     // ��������� ��� ����� ����
    [HideInInspector] public float distanceToAttack;        // ���������, � ������� ����� ���������

    public bool meleeAttackType;                            // ������������� ��� ����� ����
    public bool rangeAttackType;                            // ... ����
    public bool twoWeapons;                                 // ���� ���� 2 ������
    //public bool switchMelee;

    bool slowed;
    float maxSpeed;

    // ��� ��������
    [HideInInspector] public float aimAnglePivot;           // ���� �������� �������������
    public GameObject deathEffect;                          // ������ (����� ������� ��� � ��������� (���  ���))
    public float deathCameraShake;                          // �������� ������ ������ ��� ��������
    [HideInInspector] public bool flipLeft;                  // ��� �����
    [HideInInspector] public bool flipRight;                 //    

    // ������ ��� ������ ��� �����
    float timerForColor;
    bool red;

    // ����
    [HideInInspector] public GameObject hpBarGO;

    // ��� ��������
    public LayerMask layerTrigger;

    // �����
    public bool debug;

    public override void Awake()
    {
        base.Awake();
        //enemyThinker = GetComponentInChildren<EnemyThinker>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        animatorWeapon = GetComponentInChildren<BotAIAnimator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        pivot = GetComponentInChildren<BotAIHitBoxPivot>();
        botAIMeleeWeaponHolder = GetComponentInChildren<BotAIMeleeWeaponHolder>();
        botAIRangeWeaponHolder = GetComponentInChildren<BotAIRangeWeaponHolder>();
        hitBox = GetComponentInChildren<BotAIHitbox>();

        layerTarget = LayerMask.GetMask("Player", "NPC");
        layerHit = LayerMask.GetMask("Player", "NPC", "ObjectsDestroyble", "Default");
        if (isFriendly)
        {
            layerTarget = LayerMask.GetMask("Enemy");                                   // ���� ������ ����
            gameObject.layer = LayerMask.NameToLayer("NPC");                            // ���� ������ ����
            layerHit = LayerMask.GetMask("Enemy", "ObjectsDestroyble", "Default");      // ���� ��� ������
        }
        hpBarGO = transform.GetChild(0).gameObject;         // ������� �� ��� (������ ��� � ������ ��������)

        maxSpeed = agent.speed;
    }

    public override void Start()
    {
        base.Start();
        startPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        agent.updateRotation = false;                       // ��� ������2�
        agent.updateUpAxis = false;                         //
        agent.ResetPath();                                  // ���������� ����, ������ ��� �� ��� ������ ����

        if (meleeAttackType)
            SwitchAttackType(1);
        if (rangeAttackType)
            SwitchAttackType(2);

        hpBarGO.SetActive(false);           // ������ �� ���, ���� �� ������� ����
    }

    private void Update()
    {
        // ����� ����� ��� ��������� ����� � ��� �����
        SetColorTimer();

        if (!isAlive)
            return;

        // ������� ��������
        if (target && chasing && targetVisible)
        {
            Vector3 aimDirection = target.transform.position - pivot.transform.position;               // ���� ����� ���������� ���� � pivot ������          
            aimAnglePivot = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;                            // ������� ���� � ��������             
            Quaternion qua1 = Quaternion.Euler(0, 0, aimAnglePivot);                                                // ������� ���� ���� � Quaternion
            pivot.transform.rotation = Quaternion.Lerp(pivot.transform.rotation, qua1, Time.fixedDeltaTime * 5);   // ������ Lerp ����� weaponHoder � ����� �����
        }
        else 
        {
            if (flipRight)
                pivot.transform.rotation = Quaternion.Lerp(pivot.transform.rotation, Quaternion.Euler(0, 0, 0), Time.fixedDeltaTime * 15);   // ������ Lerp ����� weaponHoder � ����� �����
            if (flipLeft)
                pivot.transform.rotation = Quaternion.Lerp(pivot.transform.rotation, Quaternion.Euler(0, 0, 180), Time.fixedDeltaTime * 15);   // ������ Lerp ����� weaponHoder � ����� �����
        }

        // ������� ������� (Flip)       
        if (target && targetVisible)                           // (����� chasing �������� �� target � ��� ��� ����������� � ������������)
        {
            if (Mathf.Abs(aimAnglePivot) > 90 && !flipLeft)
            {
                FaceTargetLeft();
                pivot.Flip();
            }
            if (Mathf.Abs(aimAnglePivot) <= 90 && !flipRight)
            {
                FaceTargetRight();
                pivot.Flip();
            }
        }
        else
        {
            if (agent.velocity.x < -0.2 && !flipLeft)
            {
                FaceTargetLeft();
                pivot.Flip();
            }
            if (agent.velocity.x > 0.2 && !flipRight)
            {
                FaceTargetRight();
                pivot.Flip();
            }
        }

        animator.SetFloat("Speed", agent.velocity.magnitude);


        if (slowed)
        {
            if (agent.speed < maxSpeed)
                agent.speed += 0.005f;
            else
                slowed = false;
        }


        // �����
        if (debug)
            //Debug.Log(targetVisible);
            Debug.Log(maxSpeed);
    }

    public void SwitchAttackType(int type)
    {        
        if (type == 1)          // ����
        {
            meleeAttackType = true;
            rangeAttackType = false;
            distanceToAttack = distanceToAttackMelee;
        }
        if (type == 2)          // ����
        {
            meleeAttackType = false;
            rangeAttackType = true;
            distanceToAttack = distanceToAttackRange;
        }
        
/*        else
        {
            if (type == 1)          // ����
            {
                botAIMeleeWeaponHolder.SelectCurrentWeapon(0);
                distanceToAttack = distanceToAttackMelee;
            }
            if (type == 2)          // ����
            {
                botAIMeleeWeaponHolder.SelectCurrentWeapon(1);
                distanceToAttack = distanceToAttackRange;
            }
            switchMelee = true;
        }*/
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


    public void FindTarget()
    {
                                                        // ����������� ����� �����
        Collider2D[] collidersHitbox = Physics2D.OverlapCircleAll(transform.position, triggerLenght, layerTarget);    // ������� ���� � ������� ������� � �������� 
        List<GameObject> targets = new List<GameObject>();
        foreach (Collider2D enObjectBox in collidersHitbox)
        {
            if (enObjectBox == null)
            {
                continue;
            }

            if (enObjectBox.gameObject.TryGetComponent(out Fighter fighter))        // ���� ������ ������
            {
                NavMeshRayCast(fighter.gameObject);
                float distance = Vector3.Distance(fighter.transform.position, transform.position);   // ������� ��������� 
                if (!target)
                {
                    if (targetVisible)
                    {
                        targets.Add(fighter.gameObject);
                    }
                }
                else
                {
                    if (targetVisible && distance < 3)
                    {
                        targets.Add(fighter.gameObject);
                    }
                }
            }
            collidersHitbox = null;                         // ���������� ��� ��������� ������� (�� ����� ���� ��������� ��� ��� ��������)
        }
        if (targets.Count > 0)
            target = targets[Random.Range(0, targets.Count)];
    }


    public void Chase()
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
    }

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

    public override void TakeDamage(int dmg, Vector2 vec2, float pushForce)
    {
        if (currentHealth == maxHealth)
        {
            hpBarGO.SetActive(true);
            if (isEnemy)
            {
                TriggerEnemy();                 // ��������� ����� ��������, ����� ������� ���� ������� ����
            }
        }

        base.TakeDamage(dmg, vec2, pushForce);
        //animator.SetTrigger("TakeHit");
        ColorRed(0.05f);                    
    }

    public void SlowSpeed(float slowValue)
    {
        agent.speed -= slowValue;
        slowed = true;
    }

    // ������� ��� �����������
    public void TriggerEnemy()
    {
        Collider2D[] collidersHits = Physics2D.OverlapCircleAll(transform.position, 2, layerTrigger);     // ������� ���� � ������� ������� � ��������
        foreach (Collider2D coll in collidersHits)
        {
            if (coll == null)
            {
                continue;
            }

            if (coll.gameObject.TryGetComponent<BotAI>(out BotAI enemy))        // (����� ���������� �� ������ Enemy)
            {
                enemy.triggerLenght = 25;                                       // (����� �������� �� ������ = �����)          
            }
            collidersHits = null;
        }
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
    void FaceTargetRight()                                  // ������� �������
    {
        spriteRenderer.flipX = false;
        flipLeft = false;
        flipRight = true;
    }
    void FaceTargetLeft()                                   // ������� ������
    {
        spriteRenderer.flipX = true;
        flipRight = false;
        flipLeft = true;
    }

    protected override void Death()
    {
        base.Death();

        GameManager.instance.enemyCount--;                                                          // -1 � �������� ������

        CMCameraShake.Instance.ShakeCamera(deathCameraShake, 0.2f);                                 // ������ ������
        if (deathEffect)
        {
            GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);  // ������� ������ ��������
            Destroy(effect, 1);                                                                     // ���������� ������ ����� .. ���
        }        
        animator.SetTrigger("Death");               // ������ 
        spriteRenderer.color = Color.white;         // ��������� ����� �� �����
        hpBarGO.SetActive(false);                   // ������� �� ���
        agent.ResetPath();                          // ���������� ����
        agent.enabled = false;                      // ��������� ������

        //botAIMeleeWeaponHolder.gameObject.SetActive(false);
        //botAIRangeWeaponHolder.gameObject.SetActive(false);

        botAIMeleeWeaponHolder.HideWeapons();       // ������ ������
        botAIRangeWeaponHolder.HideWeapons();
        //animatorWeapon.animator.StopPlayback();
        animatorWeapon.animator.enabled = false;

        Destroy(gameObject, 0.8f);
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanceToAttack);
    }
}