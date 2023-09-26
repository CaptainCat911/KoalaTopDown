using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;
using UnityEngine.Events;

public class BotAI : Fighter
{
    // Ссылки
    //EnemyThinker enemyThinker;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Animator animator;
    [HideInInspector] public BotAIAnimator animatorWeapon;
    [HideInInspector] public SpriteRenderer spriteRenderer;    
    BotAIHitBoxPivot pivot;
    [HideInInspector] public BotAIMeleeWeaponHolder botAIMeleeWeaponHolder;
    [HideInInspector] public BotAIRangeWeaponHolder botAIRangeWeaponHolder;
    BotAIHitbox hitBox;                                     // хитбокс (для атаки)
    Ignitable ignitable;
    ShadowCaster2D shadow;                                  // тень
    AudioSource audioSource;
    //public Animator animatorHit;                            // аниматор мили оружия

    [Header("Параметры бота")]
    public bool newNpcSystem;                               // босс или сложный нпс
    public bool isNeutral;                                  // не будет никого атаковать
    public bool isFriendly;                                 // союзный бот
    public bool isEnemy;                                    // несоюзный бот
    public bool isArenaEnemy;                               // бот для арены
    public bool isArenaBoss;                                // босс для арены    
    public bool skeletonKing;                               // король скелетов
    public bool skeletonResble;                             // бот возрождаается
    public int resTimes;                                    // сколько раз возрождаться
    int resCount;
    //public bool reaper;

    //public bool isFollow;                                   // следовать

    [Header("Параметры тригера")]
    public float triggerLenght;                             // дистанция тригера
    float startTriigerLenght;                               // запоминаем стартовую дистанцию тригера
    public float chaseLeght;                                // дальность преследования 
    public float distanceToChangeTarget = 3f;               // дистанция при которой бот будет менять цель, если целей больше 1
    [HideInInspector] public GameObject target;             // цель
    [HideInInspector] public LayerMask layerTarget;         // слой для поиска 
    [HideInInspector] public LayerMask layerHit;            // слой для оружия
    [HideInInspector] public bool chasing;                  // статус преследования
    [HideInInspector] public Vector3 startPosition;         // позиция для охраны
    [HideInInspector] public bool targetVisible;            // видим мы цель или нет
    [HideInInspector] public bool closeToTarget;            // можно атаковать (текущей атакой)
    [HideInInspector] public bool targetInRange;            // можно атаковать (общее)

    [Header("Тип атаки бота")]
    public bool meleeAttackType;                            // устанавливаем тип атаки мили
    public bool rangeAttackType;                            // ... ренж
    public bool twoWeapons;                                 // если есть 2 оружия
    public float defaultRangeToTarget;                      // дистанция для атакой мили
    public float distanceToAttackMelee;                     // дистанция для атакой мили
    public float distanceToAttackRange;                     // дистанция для атаки ренж
    public float pivotSpeedKoef = 1f;                       // скорость поворота держателя оружия
    public float distanceToAttack;                          // дистанция, с которой можно атаковать
    [HideInInspector] public float distanceToTarget;        // дистанция до цели
    [HideInInspector] public bool nowAttacking;             // дистанция до цели

    [Header("Предмет")]
    public bool noHealBox;
    public GameObject itemToSpawn;

    //public bool switchMelee;

    // Замедление
    bool slowed;    
    float maxSpeed;

    [Header("Поведение")]
    public bool stayOnGround;                       // стоять на месте и охранять
    public bool noPatrol;                           // без патрулирования
    public bool goTo;                               // двигаться к точке
    public bool followPlayer;                       // следовать за игроком
    public Transform destinationPoint;              // точка назначения
    public bool noTriggerAgro;                      // без агро при попадании в соседнего монстра

    [Header("Ивенты при старте")]
    public UnityEvent eventsStart;                  // ивенты при старте

    [Header("Параметры при смерти")]
    public UnityEvent eventsDeath;                  // ивенты
    public bool withBigExplousion;                  // с большим хлопком
    public int damageBigExplousion;                 // урон хлопка
    public bool fastDeathAnim;
    public float timeForDeath = 2.5f;
    int deathCount;

    [Header("Анимации и эффекты")]
    public GameObject deathEffect;                  // эффект (потом сделать его в аниматоре (или  нет))
    public float deathCameraShake;                  // мощность тряски камеры при убийстве
    public ParticleSystem darkEffect;               // эффект тьмы
    public ParticleSystem resMagicEffect;           // эффект магии времени
    public bool makeLeft;                           // повернуть налево
    [HideInInspector] public float aimAnglePivot;   // угол поворота хитбокспивота
    [HideInInspector] public bool flipLeft;         // для флипа
    [HideInInspector] public bool flipRight;        //    
    bool pivotZero;                                 // оружие не вращается

    [Header("Остальное")]
    // Таймер для цветов при уроне
    float timerForColor;
    bool red;
    Color originColor;

    // Для триггера
    public LayerMask layerTrigger;

    [Header("Смотреть на цель")]
    public Transform friendTarget;
    public bool lookAtPlayer;

    [Header("Диалоги (баблчат)")]
    public string[] bubbleTexts;
    public string[] bubbleTextsEng;
    public bool withChat;               // с чатом
    bool sayedChat;

    [Header("Аудио")]    
    public AudioEnemy audioEnemy;       // набор звуков
    public bool withAudioChat;          // с репликой
    bool sayedAudoiChat;

    // Дебаг
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
        agent.updateRotation = false;           // для навмеш2д
        agent.updateUpAxis = false;             //
        agent.ResetPath();                      // сбрасываем путь, потому что он при старте есть

        startPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);      // стартовая позиция

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

        startTriigerLenght = triggerLenght;                     // стартовая длинна тригера



        /*        agent.updateRotation = false;           // для навмеш2д
                agent.updateUpAxis = false;             //
                agent.ResetPath();                      // сбрасываем путь, потому что он при старте есть*/

        eventsStart.Invoke();                   // запускаем ивент

        if (meleeAttackType)
            SwitchAttackType(1);
        if (rangeAttackType)
            SwitchAttackType(2);

        if (makeLeft)
            Invoke(nameof(MakeLeft), 0.5f);     // повернуть налево

/*        if (reaper)
            target = GameManager.instance.player.gameObject;*/
    }

    public override void Update()
    {
/*        if (debug)
            Debug.Log(agent.updateRotation);*/

        /*        if (agent.updateRotation || agent.updateUpAxis)
                {
                    agent.updateRotation = false;           // для навмеш2д
                    agent.updateUpAxis = false;             //
                }*/

/*        if (Input.GetKeyDown(KeyCode.H))
        {
            agent.updateRotation = false;           // для навмеш2д
            agent.updateUpAxis = false;             //
        }*/

        // Выбор цвета при получении урона и его сброс
        SetColorTimer();

        if (!isAlive)
            return;

        // Поворот хитбокса
        if (pivotZero)
        {
            if (flipRight)
                pivot.transform.rotation = Quaternion.Lerp(pivot.transform.rotation, Quaternion.Euler(0, 0, 0), Time.fixedDeltaTime * 15);   // делаем Lerp между weaponHoder и нашим углом
            if (flipLeft)
                pivot.transform.rotation = Quaternion.Lerp(pivot.transform.rotation, Quaternion.Euler(0, 0, 180), Time.fixedDeltaTime * 15);   // делаем Lerp между weaponHoder и нашим углом
        }
        else if (target && chasing && targetVisible)
        {
            Vector3 aimDirection = target.transform.position - pivot.transform.position;                            // угол между положением мыши и pivot оружия          
            aimAnglePivot = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;                            // находим угол в градусах             
            Quaternion qua1 = Quaternion.Euler(0, 0, aimAnglePivot);                                                // создаем этот угол в Quaternion
            pivot.transform.rotation = Quaternion.Lerp(pivot.transform.rotation, qua1, Time.fixedDeltaTime * 5 * pivotSpeedKoef);    // делаем Lerp между weaponHoder и нашим углом
        }
        else 
        {
            if (flipRight)
                pivot.transform.rotation = Quaternion.Lerp(pivot.transform.rotation, Quaternion.Euler(0, 0, 0), Time.fixedDeltaTime * 15);   // делаем Lerp между weaponHoder и нашим углом
            if (flipLeft)
                pivot.transform.rotation = Quaternion.Lerp(pivot.transform.rotation, Quaternion.Euler(0, 0, 180), Time.fixedDeltaTime * 15);   // делаем Lerp между weaponHoder и нашим углом
        }

        // поворот спрайта (Flip)
        if (target && targetVisible)                           // (ещё это дублируется в хитбокспивот)
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

        // Замедление
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


        // Дебаг
/*        if (debug)
        {
            Debug.Log(target);            
            Debug.Log(chasing);
            Debug.Log(targetVisible);            
        }*/
    }

    // Сделать нейтральным или нет
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

    // Назначить точку назначения
    public void SetPointBot(Transform transform)
    {
        agent.SetDestination(transform.position);
    }

    // Переместить (если живой)
    public void WarpBot(Transform transform)
    {
        if (isAlive)
        {
            agent.Warp(transform.position);
        }
    }

    // Дружеская цель
    public void LookAt(Transform friendTarget)
    {
        Vector3 targetDirection = friendTarget.transform.position - pivot.transform.position;           // угол между целью и pivot оружия          
        float targetAnglePivot = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;     // находим угол в градусах             
        
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
        if (type == 1)          // мили
        {
            meleeAttackType = true;
            rangeAttackType = false;
            distanceToAttack = distanceToAttackMelee;
        }
        if (type == 2)          // ренж
        {
            meleeAttackType = false;
            rangeAttackType = true;
            distanceToAttack = distanceToAttackRange;
        }
        
/*        else
        {
            if (type == 1)          // мили
            {
                botAIMeleeWeaponHolder.SelectCurrentWeapon(0);
                distanceToAttack = distanceToAttackMelee;
            }
            if (type == 2)          // ренж
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
            targetVisible = true;                                           // цели видима из нашей позиции
        }
        else
        {
            // тут добавить проверку какой объект попал под рейкаст (стена или снаряд например или враг)
            targetVisible = false;
        }
    }


    public void FindTarget()
    {                                                        
        Collider2D[] collidersHitbox = Physics2D.OverlapCircleAll(transform.position, triggerLenght, layerTarget);    // создаем круг в позиции объекта с радиусом 
        List<GameObject> targets = new List<GameObject>();
        foreach (Collider2D enObjectBox in collidersHitbox)
        {
            if (enObjectBox == null)
            {
                continue;
            }

            if (enObjectBox.gameObject.TryGetComponent(out Fighter fighter))        // ищем скрипт файтер
            {
                if (fighter.noAgro)
                {
                    continue;
                }
                NavMeshRayCast(fighter.gameObject);
                float distance = Vector3.Distance(fighter.transform.position, transform.position);   // считаем дистанцию 
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
            collidersHitbox = null;                 // сбрасываем все найденные объекты (на самом деле непонятно как это работает)
        }
        if (targets.Count > 0)
        {
            target = targets[Random.Range(0, targets.Count)];       // выбираем рандомно цель

            if (withChat && !sayedChat)             // чат, когда нашли цель
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

        distanceToTarget = Vector3.Distance(transform.position, target.transform.position);     // считаем дистанцию до цели

        // Дистанция до цели
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

        // Дистанция для текущей атаки
        if (distanceToTarget <= distanceToAttack && targetVisible)      // если дошли до цели и видим её
        {
            if (!closeToTarget)                                                 // 
            {
                agent.ResetPath();                                              // сбрасываем путь       
                closeToTarget = true;                                           // готов стрелять
            }
        }
        else
        {
            agent.SetDestination(target.transform.position);                    // перемещаемся к цели
            if (closeToTarget)
            {
                closeToTarget = false;                                          // не готов стрелять
            }
        }
    }

    public void ResetTarget()
    {
        //isFindTarget = false;
        target = null;
        chasing = false;                    // преследование отключено            
        targetVisible = false;              // цель не видима
        closeToTarget = false;              // далеко от цели
        //agent.ResetPath();                  // сбрасываем путь
        agent.SetDestination(startPosition);
        triggerLenght = startTriigerLenght;
        animator.SetFloat("Speed", 0);      // сбрасываем анимацию бега
    }

    public void StayOnGround()
    {
        stayOnGround = false;                               // стоять на месте и охранять
        goTo = false;                                       // двигаться к точке
        followPlayer = false;                               // следовать за игроком
    }
    public void ActiveGoTo()
    {
        stayOnGround = false;                               // стоять на месте и охранять
        goTo = true;                                        // двигаться к точке
        followPlayer = false;                               // следовать за игроком
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

    // Фукция для ивента анимации (потом как-нибудь сделать по нормальному)
    public void AttacHitBox()
    {
        hitBox.Attack();
    }
    public void EffectRangeAttackHitBox()
    {
        hitBox.EffectRangeAttack();
    }


    // Атаки милихолдера (ссылка с аниматора на милхолдер)
    public void AttackMeleeHolder(int type)
    {
        if (botAIMeleeWeaponHolder.currentWeapon)
            botAIMeleeWeaponHolder.currentWeapon.Attack(type);
    }

    public void ForceBackFire(Vector3 forceDirection, float forceBack)
    {
        Vector2 vec2 = (transform.position - forceDirection).normalized;        // направление отдачи нормализированное
        rb2D.AddForce(vec2 * forceBack, ForceMode2D.Impulse);                   // толкаем импульсом
    }

    public override void TakeDamage(int dmg, Vector2 vec2, float pushForce)
    {
        if (currentHealth == maxHealth)
        {            
            if (isEnemy)
            {
                TriggerEnemy();                 // добавляем длину триггера, чтобы агрился если получил урон
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

    // Триггер для противников
    public void TriggerEnemy()
    {
        Collider2D[] collidersHits = Physics2D.OverlapCircleAll(transform.position, 10, layerTrigger);     // создаем круг в позиции объекта с радиусом
        foreach (Collider2D coll in collidersHits)
        {
            if (coll == null)
            {
                continue;
            }

            if (coll.gameObject.TryGetComponent<BotAI>(out BotAI enemy))        // (потом переделать на скрипт Enemy)
            {
                if (!enemy.noTriggerAgro)
                    enemy.triggerLenght = 30;                                       // (потом поменять на таргет = плеер)          
            }
            collidersHits = null;
        }
    }


    // Смена цветов при уроне
    void SetColorTimer()
    {
        if (timerForColor > 0)                  // таймер для отображения урона
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

    // Поворот спрайта
    public void FaceTargetRight()                                  // поворот направо
    {
        spriteRenderer.flipX = false;
        flipLeft = false;
        flipRight = true;        
    }
    public void FaceTargetLeft()                                   // поворот налево
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
            eventsDeath.Invoke();                        // ивенты
            SayText("Хорошо, ты доказал, что достоин второго шанса");
            return;
        }*/

        base.Death();

        //GameManager.instance.enemyCount--;                                                          // -1 к счётчику врагов

        CMCameraShake.Instance.ShakeCamera(deathCameraShake, 0.2f);                                 // тряска камеры
        if (deathEffect)
        {
            GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);  // создаем эффект убийства
            Destroy(effect, 1);                                                                     // уничтожаем эффект через .. сек
        }        
        animator.SetTrigger("Death");               // тригер 
        spriteRenderer.color = Color.white;         // возвращем цвета на белый
        hpBarGO.SetActive(false);                   // убираем хп бар
        agent.ResetPath();                          // сбрасываем путь        

        if (resCount == resTimes)
        {
            if (itemToSpawn)
                Instantiate(itemToSpawn, transform.position, Quaternion.identity);          // создаем предмет

            if (!noHealBox)
            {
                int random = Random.Range(0, 101);
                if (random >= 96)
                    Instantiate(GameAssets.instance.healBox, transform.position, Quaternion.identity);          // создаем аптечку
            }
        }

 

        botAIMeleeWeaponHolder.HideWeapons();       // прячем оружия
        botAIRangeWeaponHolder.HideWeapons();
        animatorWeapon.animator.enabled = false;    // отключаем аниматор оружия
        if (shadow)
            shadow.enabled = false;                 // отключаем тень
        //animatorWeapon.animator.StopPlayback();
        //gameObject.layer = LayerMask.NameToLayer("Item");                            // слой самого бота

        if (audioEnemy)
        {
            int random = Random.Range(0, audioEnemy.audioClipsDeath.Length);        // выбираем рандомно звук
            float audioPitch = Random.Range(0.9f, 1.1f);                            // рандомный питч
            audioSource.pitch = audioPitch;                                         // устанавливаем питч
            audioSource.clip = audioEnemy.audioClipsDeath[random];                  // устанавливаем выбранный звук в аудиоСоурс
            audioSource.Play();                                                     // воспроизводим
            audioSource.pitch = 1f;                                                 // возвращаем обычный питч 
        }

        eventsDeath.Invoke();                        // ивенты


        if (withBigExplousion)
        {
            Collider2D[] collidersHits = Physics2D.OverlapCircleAll(transform.position, 25, LayerMask.GetMask("Enemy"));     // создаем круг в позиции объекта с радиусом
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

            GameObject effect = Instantiate(GameAssets.instance.bigDarkExpliusion, transform.position, Quaternion.identity);  // создаем эффект убийства
            Destroy(effect, 1);

            CMCameraShake.Instance.ShakeCamera(5, 0.3f);            // тряска камеры            
        }




        if (!fastDeathAnim)
            Invoke("AfterDeath", timeForDeath);         // 0.8f
        else
        {
            agent.enabled = false;              // выключаем агента
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




        // Для арены
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
        agent.enabled = false;                  // выключаем агента

/*        if (skeletonResble)                     // если возрождающийся скелет
        {
            StartResSkeleton();                 // запускаем возрождение
        }*/

        deathCount++;                           // для способностей короля скелетов



        if (skeletonKing || skeletonResble)     // если возрождающийся скелет
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

    // Анимация воскрешения
    void ResAnimator()  
    {
        //animatorWeapon.animator.StopPlayback();         // НЕ РАБОТАЕТ
        animatorWeapon.ResetAllParam();
        animatorWeapon.animator.Play("Base Layer.NullState", 0, 0.0f);
        animator.SetTrigger("Res");
    }

    // Воскрешение
    void Res()
    {
        isAlive = true;                             // жив
        if (capsuleCollider2D)
            capsuleCollider2D.enabled = true;       // коллайдер включаем
        currentHealth = maxHealth;                  // здоров
        agent.enabled = true;                       // выключаем агента
        if (darkEffect)
            darkEffect.Play();
        if (resMagicEffect && resTimes > resCount)
            resMagicEffect.Play();
        botAIMeleeWeaponHolder.SelectWeapon();      // достаём оружия
        botAIRangeWeaponHolder.SelectWeapon();
        animatorWeapon.animator.enabled = true;     // включаем аниматор оружия
        if (shadow)
            shadow.enabled = true;                  // включаем тень
        if (ignitable)
        {
            //Debug.Log("Ign!");    
            ignitable.stopBurn = false;             // возвращаем горение
        }
        //hpBarGO.SetActive(true);                   // включем хп бар
    }

    public void MakeFriendly()
    {
        layerTarget = LayerMask.GetMask("Enemy");                                   // слой поиска цели
        gameObject.layer = LayerMask.NameToLayer("NPC");                            // слой самого бота
        layerHit = LayerMask.GetMask("Enemy", "ObjectsDestroyble", "Default");      // слой для оружия        
    }

    public void MakeEnemyly()
    {
        layerTarget = LayerMask.GetMask("Player", "NPC");
        gameObject.layer = LayerMask.NameToLayer("Enemy");                            // слой самого бота
        layerHit = LayerMask.GetMask("Player", "NPC", "ObjectsDestroyble", "Default");
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, triggerLenght);
    }
}