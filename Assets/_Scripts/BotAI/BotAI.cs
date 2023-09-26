using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;
using UnityEngine.Events;

public class BotAI : Fighter
{
    // ������
    //EnemyThinker enemyThinker;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Animator animator;
    [HideInInspector] public BotAIAnimator animatorWeapon;
    [HideInInspector] public SpriteRenderer spriteRenderer;    
    BotAIHitBoxPivot pivot;
    [HideInInspector] public BotAIMeleeWeaponHolder botAIMeleeWeaponHolder;
    [HideInInspector] public BotAIRangeWeaponHolder botAIRangeWeaponHolder;
    BotAIHitbox hitBox;                                     // ������� (��� �����)
    Ignitable ignitable;
    ShadowCaster2D shadow;                                  // ����
    AudioSource audioSource;
    //public Animator animatorHit;                            // �������� ���� ������

    [Header("��������� ����")]
    public bool newNpcSystem;                               // ���� ��� ������� ���
    public bool isNeutral;                                  // �� ����� ������ ���������
    public bool isFriendly;                                 // ������� ���
    public bool isEnemy;                                    // ��������� ���
    public bool isArenaEnemy;                               // ��� ��� �����
    public bool isArenaBoss;                                // ���� ��� �����    
    public bool skeletonKing;                               // ������ ��������
    public bool skeletonResble;                             // ��� �������������
    public int resTimes;                                    // ������� ��� ������������
    int resCount;
    //public bool reaper;

    //public bool isFollow;                                   // ���������

    [Header("��������� �������")]
    public float triggerLenght;                             // ��������� �������
    float startTriigerLenght;                               // ���������� ��������� ��������� �������
    public float chaseLeght;                                // ��������� ������������� 
    public float distanceToChangeTarget = 3f;               // ��������� ��� ������� ��� ����� ������ ����, ���� ����� ������ 1
    [HideInInspector] public GameObject target;             // ����
    [HideInInspector] public LayerMask layerTarget;         // ���� ��� ������ 
    [HideInInspector] public LayerMask layerHit;            // ���� ��� ������
    [HideInInspector] public bool chasing;                  // ������ �������������
    [HideInInspector] public Vector3 startPosition;         // ������� ��� ������
    [HideInInspector] public bool targetVisible;            // ����� �� ���� ��� ���
    [HideInInspector] public bool closeToTarget;            // ����� ��������� (������� ������)
    [HideInInspector] public bool targetInRange;            // ����� ��������� (�����)

    [Header("��� ����� ����")]
    public bool meleeAttackType;                            // ������������� ��� ����� ����
    public bool rangeAttackType;                            // ... ����
    public bool twoWeapons;                                 // ���� ���� 2 ������
    public float defaultRangeToTarget;                      // ��������� ��� ������ ����
    public float distanceToAttackMelee;                     // ��������� ��� ������ ����
    public float distanceToAttackRange;                     // ��������� ��� ����� ����
    public float pivotSpeedKoef = 1f;                       // �������� �������� ��������� ������
    public float distanceToAttack;                          // ���������, � ������� ����� ���������
    [HideInInspector] public float distanceToTarget;        // ��������� �� ����
    [HideInInspector] public bool nowAttacking;             // ��������� �� ����

    [Header("�������")]
    public bool noHealBox;
    public GameObject itemToSpawn;

    //public bool switchMelee;

    // ����������
    bool slowed;    
    float maxSpeed;

    [Header("���������")]
    public bool stayOnGround;                       // ������ �� ����� � ��������
    public bool noPatrol;                           // ��� ��������������
    public bool goTo;                               // ��������� � �����
    public bool followPlayer;                       // ��������� �� �������
    public Transform destinationPoint;              // ����� ����������
    public bool noTriggerAgro;                      // ��� ���� ��� ��������� � ��������� �������

    [Header("������ ��� ������")]
    public UnityEvent eventsStart;                  // ������ ��� ������

    [Header("��������� ��� ������")]
    public UnityEvent eventsDeath;                  // ������
    public bool withBigExplousion;                  // � ������� �������
    public int damageBigExplousion;                 // ���� ������
    public bool fastDeathAnim;
    public float timeForDeath = 2.5f;
    int deathCount;

    [Header("�������� � �������")]
    public GameObject deathEffect;                  // ������ (����� ������� ��� � ��������� (���  ���))
    public float deathCameraShake;                  // �������� ������ ������ ��� ��������
    public ParticleSystem darkEffect;               // ������ ����
    public ParticleSystem resMagicEffect;           // ������ ����� �������
    public bool makeLeft;                           // ��������� ������
    [HideInInspector] public float aimAnglePivot;   // ���� �������� �������������
    [HideInInspector] public bool flipLeft;         // ��� �����
    [HideInInspector] public bool flipRight;        //    
    bool pivotZero;                                 // ������ �� ���������

    [Header("���������")]
    // ������ ��� ������ ��� �����
    float timerForColor;
    bool red;
    Color originColor;

    // ��� ��������
    public LayerMask layerTrigger;

    [Header("�������� �� ����")]
    public Transform friendTarget;
    public bool lookAtPlayer;

    [Header("������� (�������)")]
    public string[] bubbleTexts;
    public string[] bubbleTextsEng;
    public bool withChat;               // � �����
    bool sayedChat;

    [Header("�����")]    
    public AudioEnemy audioEnemy;       // ����� ������
    public bool withAudioChat;          // � ��������
    bool sayedAudoiChat;

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
        originColor = spriteRenderer.color;
        pivot = GetComponentInChildren<BotAIHitBoxPivot>();
        botAIMeleeWeaponHolder = GetComponentInChildren<BotAIMeleeWeaponHolder>();
        botAIRangeWeaponHolder = GetComponentInChildren<BotAIRangeWeaponHolder>();
        hitBox = GetComponentInChildren<BotAIHitbox>();
        ignitable = GetComponent<Ignitable>();
        shadow = GetComponentInChildren<ShadowCaster2D>();
        audioSource = GetComponent<AudioSource>();

        layerTarget = LayerMask.GetMask("Player", "NPC");
        layerHit = LayerMask.GetMask("Player", "NPC", "ObjectsDestroyble", "Default");
        if (isFriendly)
        {
            MakeFriendly();
        }
        
        maxSpeed = agent.speed;
        agent.updateRotation = false;           // ��� ������2�
        agent.updateUpAxis = false;             //
        agent.ResetPath();                      // ���������� ����, ������ ��� �� ��� ������ ����

        startPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);      // ��������� �������

        if (LanguageManager.instance.hardCoreMode && isEnemy)
        {
            maxHealth *= 3;
            currentHealth = maxHealth;
            agent.speed += 1f;
            skeletonResble = true;
            resTimes += 1;
        }

        //MakeLeft();

        //distanceToAttack = defaultRangeToTarget;
    }

    private void OnEnable()
    {
        if (resMagicEffect)
        {
            if (skeletonResble && resTimes > 0)
                resMagicEffect.Play();
            else
                resMagicEffect.Stop();
        }
    }

    public override void Start()
    {
        base.Start();

        startTriigerLenght = triggerLenght;                     // ��������� ������ �������



        /*        agent.updateRotation = false;           // ��� ������2�
                agent.updateUpAxis = false;             //
                agent.ResetPath();                      // ���������� ����, ������ ��� �� ��� ������ ����*/

        eventsStart.Invoke();                   // ��������� �����

        if (meleeAttackType)
            SwitchAttackType(1);
        if (rangeAttackType)
            SwitchAttackType(2);

        if (makeLeft)
            Invoke(nameof(MakeLeft), 0.5f);     // ��������� ������

/*        if (reaper)
            target = GameManager.instance.player.gameObject;*/
    }

    public override void Update()
    {
/*        if (debug)
            Debug.Log(agent.updateRotation);*/

        /*        if (agent.updateRotation || agent.updateUpAxis)
                {
                    agent.updateRotation = false;           // ��� ������2�
                    agent.updateUpAxis = false;             //
                }*/

/*        if (Input.GetKeyDown(KeyCode.H))
        {
            agent.updateRotation = false;           // ��� ������2�
            agent.updateUpAxis = false;             //
        }*/

        // ����� ����� ��� ��������� ����� � ��� �����
        SetColorTimer();

        if (!isAlive)
            return;

        // ������� ��������
        if (pivotZero)
        {
            if (flipRight)
                pivot.transform.rotation = Quaternion.Lerp(pivot.transform.rotation, Quaternion.Euler(0, 0, 0), Time.fixedDeltaTime * 15);   // ������ Lerp ����� weaponHoder � ����� �����
            if (flipLeft)
                pivot.transform.rotation = Quaternion.Lerp(pivot.transform.rotation, Quaternion.Euler(0, 0, 180), Time.fixedDeltaTime * 15);   // ������ Lerp ����� weaponHoder � ����� �����
        }
        else if (target && chasing && targetVisible)
        {
            Vector3 aimDirection = target.transform.position - pivot.transform.position;                            // ���� ����� ���������� ���� � pivot ������          
            aimAnglePivot = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;                            // ������� ���� � ��������             
            Quaternion qua1 = Quaternion.Euler(0, 0, aimAnglePivot);                                                // ������� ���� ���� � Quaternion
            pivot.transform.rotation = Quaternion.Lerp(pivot.transform.rotation, qua1, Time.fixedDeltaTime * 5 * pivotSpeedKoef);    // ������ Lerp ����� weaponHoder � ����� �����
        }
        else 
        {
            if (flipRight)
                pivot.transform.rotation = Quaternion.Lerp(pivot.transform.rotation, Quaternion.Euler(0, 0, 0), Time.fixedDeltaTime * 15);   // ������ Lerp ����� weaponHoder � ����� �����
            if (flipLeft)
                pivot.transform.rotation = Quaternion.Lerp(pivot.transform.rotation, Quaternion.Euler(0, 0, 180), Time.fixedDeltaTime * 15);   // ������ Lerp ����� weaponHoder � ����� �����
        }

        // ������� ������� (Flip)
        if (target && targetVisible)                           // (��� ��� ����������� � ������������)
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

        // ����������
        if (slowed)
        {
            if (agent.speed < maxSpeed)
                agent.speed += 0.005f;
            else
                slowed = false;
        }        
    

        if (friendTarget)
        {
            LookAt(friendTarget);
        }
                
        if (lookAtPlayer)
        {
            LookAt(GameManager.instance.player.transform);
        }


        // �����
/*        if (debug)
        {
            Debug.Log(target);            
            Debug.Log(chasing);
            Debug.Log(targetVisible);            
        }*/
    }

    // ������� ����������� ��� ���
    public void SetNeutral(bool status)
    {
        if (status)
        {
            isNeutral = true;
            noAgro = true;
        }
        if (!status)
        {
            isNeutral = false;
            noAgro = false;
        }
    }

    // ��������� ����� ����������
    public void SetPointBot(Transform transform)
    {
        agent.SetDestination(transform.position);
    }

    // ����������� (���� �����)
    public void WarpBot(Transform transform)
    {
        if (isAlive)
        {
            agent.Warp(transform.position);
        }
    }

    // ��������� ����
    public void LookAt(Transform friendTarget)
    {
        Vector3 targetDirection = friendTarget.transform.position - pivot.transform.position;           // ���� ����� ����� � pivot ������          
        float targetAnglePivot = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;     // ������� ���� � ��������             
        
        if (Mathf.Abs(targetAnglePivot) > 90 && !flipLeft)
        {
            FaceTargetLeft();
            pivot.Flip();
        }
        if (Mathf.Abs(targetAnglePivot) <= 90 && !flipRight)
        {
            FaceTargetRight();
            pivot.Flip();
        }
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
                if (fighter.noAgro)
                {
                    continue;
                }
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
                    if (targetVisible && distance < 10)
                    {
                        targets.Add(fighter.gameObject);
                    }
                }
            }
            collidersHitbox = null;                 // ���������� ��� ��������� ������� (�� ����� ���� ��������� ��� ��� ��������)
        }
        if (targets.Count > 0)
        {
            target = targets[Random.Range(0, targets.Count)];       // �������� �������� ����

            if (withChat && !sayedChat)             // ���, ����� ����� ����
            {
                if (LanguageManager.instance.eng)
                {
                    SayText(bubbleTextsEng[Random.Range(0, bubbleTextsEng.Length)]);
                }
                else
                {
                    SayText(bubbleTexts[Random.Range(0, bubbleTexts.Length)]);
                }
                
                sayedChat = true;
            }
            if (withAudioChat && !sayedAudoiChat)
            {
                int random = Random.Range(0, audioEnemy.audioClipsTaunt.Length);
                audioSource.clip = audioEnemy.audioClipsTaunt[random];
                audioSource.Play();
                sayedAudoiChat = true;
            }
        }
    }


    public void Chase()
    {
        if (isNeutral || !isAlive || !target)
            return;

        if (debug)
        {
            //Debug.Log(nowAttacking);            
        }            

        distanceToTarget = Vector3.Distance(transform.position, target.transform.position);     // ������� ��������� �� ����

        // ��������� �� ����
        if (distanceToTarget <= defaultRangeToTarget && targetVisible)
        {
            if (!targetInRange)
            {
                targetInRange = true;
            }
        }
        else
        {
            if (targetInRange)
            {
                targetInRange = false;
            }
        }

        if (nowAttacking)
        {
            agent.ResetPath();
            return;
        }

        // ��������� ��� ������� �����
        if (distanceToTarget <= distanceToAttack && targetVisible)      // ���� ����� �� ���� � ����� �
        {
            if (!closeToTarget)                                                 // 
            {
                agent.ResetPath();                                              // ���������� ����       
                closeToTarget = true;                                           // ����� ��������
            }
        }
        else
        {
            agent.SetDestination(target.transform.position);                    // ������������ � ����
            if (closeToTarget)
            {
                closeToTarget = false;                                          // �� ����� ��������
            }
        }
    }

    public void ResetTarget()
    {
        //isFindTarget = false;
        target = null;
        chasing = false;                    // ������������� ���������            
        targetVisible = false;              // ���� �� ������
        closeToTarget = false;              // ������ �� ����
        //agent.ResetPath();                  // ���������� ����
        agent.SetDestination(startPosition);
        triggerLenght = startTriigerLenght;
        animator.SetFloat("Speed", 0);      // ���������� �������� ����
    }

    public void StayOnGround()
    {
        stayOnGround = false;                               // ������ �� ����� � ��������
        goTo = false;                                       // ��������� � �����
        followPlayer = false;                               // ��������� �� �������
    }
    public void ActiveGoTo()
    {
        stayOnGround = false;                               // ������ �� ����� � ��������
        goTo = true;                                        // ��������� � �����
        followPlayer = false;                               // ��������� �� �������
    }
    public void ActiveLookAtPlayer()
    {
        lookAtPlayer = true;
    }


    public void SetDestination(Vector3 destination)
    {
        agent.SetDestination(destination);
    }

    public void SayText(string text)
    {
        ChatBubble.Clear(gameObject);
        ChatBubble.Create(transform, new Vector3(0.2f, 0.2f), text, 4f);
    }

    public void PoliceSayChat()
    {
        if (LanguageManager.instance.eng)
        {
            SayText(bubbleTextsEng[Random.Range(0, bubbleTextsEng.Length)]);
        }
        else
        {
            SayText(bubbleTexts[Random.Range(0, bubbleTexts.Length)]);
        }       
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


    // ����� ����������� (������ � ��������� �� ���������)
    public void AttackMeleeHolder(int type)
    {
        if (botAIMeleeWeaponHolder.currentWeapon)
            botAIMeleeWeaponHolder.currentWeapon.Attack(type);
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
        Collider2D[] collidersHits = Physics2D.OverlapCircleAll(transform.position, 10, layerTrigger);     // ������� ���� � ������� ������� � ��������
        foreach (Collider2D coll in collidersHits)
        {
            if (coll == null)
            {
                continue;
            }

            if (coll.gameObject.TryGetComponent<BotAI>(out BotAI enemy))        // (����� ���������� �� ������ Enemy)
            {
                if (!enemy.noTriggerAgro)
                    enemy.triggerLenght = 30;                                       // (����� �������� �� ������ = �����)          
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
        spriteRenderer.color = originColor;
        red = false;
    }

    // ������� �������
    public void FaceTargetRight()                                  // ������� �������
    {
        spriteRenderer.flipX = false;
        flipLeft = false;
        flipRight = true;        
    }
    public void FaceTargetLeft()                                   // ������� ������
    {
        spriteRenderer.flipX = true;
        flipRight = false;
        flipLeft = true;        
    }

    public void MakeLeft()
    {
        FaceTargetLeft();
        pivot.Flip();
    }

    public void PivotZero(int number)
    {
        if (number == 0)
        {
            pivotZero = false;
        }
        if (number == 1)
        {
            pivotZero = true;
        }
    }

    protected override void Death()
    {
 /*       if (skeletonKing)
        {
            MakeFriendly();
            currentHealth = maxHealth;
            eventsDeath.Invoke();                        // ������
            SayText("������, �� �������, ��� ������� ������� �����");
            return;
        }*/

        base.Death();

        //GameManager.instance.enemyCount--;                                                          // -1 � �������� ������

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

        if (resCount == resTimes)
        {
            if (itemToSpawn)
                Instantiate(itemToSpawn, transform.position, Quaternion.identity);          // ������� �������

            if (!noHealBox)
            {
                int random = Random.Range(0, 101);
                if (random >= 96)
                    Instantiate(GameAssets.instance.healBox, transform.position, Quaternion.identity);          // ������� �������
            }
        }

 

        botAIMeleeWeaponHolder.HideWeapons();       // ������ ������
        botAIRangeWeaponHolder.HideWeapons();
        animatorWeapon.animator.enabled = false;    // ��������� �������� ������
        if (shadow)
            shadow.enabled = false;                 // ��������� ����
        //animatorWeapon.animator.StopPlayback();
        //gameObject.layer = LayerMask.NameToLayer("Item");                            // ���� ������ ����

        if (audioEnemy)
        {
            int random = Random.Range(0, audioEnemy.audioClipsDeath.Length);        // �������� �������� ����
            float audioPitch = Random.Range(0.9f, 1.1f);                            // ��������� ����
            audioSource.pitch = audioPitch;                                         // ������������� ����
            audioSource.clip = audioEnemy.audioClipsDeath[random];                  // ������������� ��������� ���� � ����������
            audioSource.Play();                                                     // �������������
            audioSource.pitch = 1f;                                                 // ���������� ������� ���� 
        }

        eventsDeath.Invoke();                        // ������


        if (withBigExplousion)
        {
            Collider2D[] collidersHits = Physics2D.OverlapCircleAll(transform.position, 25, LayerMask.GetMask("Enemy"));     // ������� ���� � ������� ������� � ��������
            foreach (Collider2D coll in collidersHits)
            {
                if (coll == null)
                {
                    continue;
                }

                if (coll.gameObject.TryGetComponent<Fighter>(out Fighter fighter))
                {

                    Vector2 vec2 = (coll.transform.position - transform.position).normalized;
                    fighter.TakeDamage(damageBigExplousion, vec2, 60);
                }
                collidersHits = null;
            }

            GameObject effect = Instantiate(GameAssets.instance.bigDarkExpliusion, transform.position, Quaternion.identity);  // ������� ������ ��������
            Destroy(effect, 1);

            CMCameraShake.Instance.ShakeCamera(5, 0.3f);            // ������ ������            
        }




        if (!fastDeathAnim)
            Invoke("AfterDeath", timeForDeath);         // 0.8f
        else
        {
            agent.enabled = false;              // ��������� ������
            Destroy(gameObject);
        }
        if (ignitable)
        {
            //Debug.Log("Ign!");
            ignitable.stopBurn = true;
        }

        if (darkEffect)
            darkEffect.Stop();
        if (resMagicEffect)
            resMagicEffect.Stop();




        // ��� �����
        if (resCount == resTimes)
        {
            if (isArenaEnemy)
            {
                ArenaManager.instance.arenaEnemyCount--;
                ArenaManager.instance.arenaEnemyKilled++;
            }
            if (isArenaBoss)
            {
                ArenaManager.instance.arenaBossCount--;
                ArenaManager.instance.arenaBossKilled++;
            }
        }
    }

    void AfterDeath()
    {
        agent.enabled = false;                  // ��������� ������

/*        if (skeletonResble)                     // ���� �������������� ������
        {
            StartResSkeleton();                 // ��������� �����������
        }*/

        deathCount++;                           // ��� ������������ ������ ��������



        if (skeletonKing || skeletonResble)     // ���� �������������� ������
        {
            if (skeletonResble && resCount < resTimes)
            {
                resCount++;
                Invoke(nameof(StartResSkeleton), 0);
                return;
            }
            else
            {
                //animator.ResetTrigger("Res");
            }

            if (skeletonKing)
            {
                return;
            }
            
        }

        Destroy(gameObject, 1f);
    }

    public void StartResSkeleton()
    {
/*        if (debug)
            Debug.Log("Res!");*/
        Invoke(nameof(ResAnimator), 2);
        Invoke(nameof(Res), 2.6f);
    }

    public void StartRes()
    {
        Invoke(nameof(ResAnimator), 7); 
        Invoke(nameof(Res), 7.6f);
    }

    // �������� �����������
    void ResAnimator()  
    {
        //animatorWeapon.animator.StopPlayback();         // �� ��������
        animatorWeapon.ResetAllParam();
        animatorWeapon.animator.Play("Base Layer.NullState", 0, 0.0f);
        animator.SetTrigger("Res");
    }

    // �����������
    void Res()
    {
        isAlive = true;                             // ���
        if (capsuleCollider2D)
            capsuleCollider2D.enabled = true;       // ��������� ��������
        currentHealth = maxHealth;                  // ������
        agent.enabled = true;                       // ��������� ������
        if (darkEffect)
            darkEffect.Play();
        if (resMagicEffect && resTimes > resCount)
            resMagicEffect.Play();
        botAIMeleeWeaponHolder.SelectWeapon();      // ������ ������
        botAIRangeWeaponHolder.SelectWeapon();
        animatorWeapon.animator.enabled = true;     // �������� �������� ������
        if (shadow)
            shadow.enabled = true;                  // �������� ����
        if (ignitable)
        {
            //Debug.Log("Ign!");    
            ignitable.stopBurn = false;             // ���������� �������
        }
        //hpBarGO.SetActive(true);                   // ������� �� ���
    }

    public void MakeFriendly()
    {
        layerTarget = LayerMask.GetMask("Enemy");                                   // ���� ������ ����
        gameObject.layer = LayerMask.NameToLayer("NPC");                            // ���� ������ ����
        layerHit = LayerMask.GetMask("Enemy", "ObjectsDestroyble", "Default");      // ���� ��� ������        
    }

    public void MakeEnemyly()
    {
        layerTarget = LayerMask.GetMask("Player", "NPC");
        gameObject.layer = LayerMask.NameToLayer("Enemy");                            // ���� ������ ����
        layerHit = LayerMask.GetMask("Player", "NPC", "ObjectsDestroyble", "Default");
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, triggerLenght);
    }
}