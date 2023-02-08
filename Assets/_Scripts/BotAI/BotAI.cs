using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotAI : Fighter
{
    // Ссылки
    //EnemyThinker enemyThinker;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Animator animator;
    BotAIAnimator animatorWeapon;
    [HideInInspector] public SpriteRenderer spriteRenderer;    
    BotAIHitBoxPivot pivot;
    [HideInInspector] public BotAIMeleeWeaponHolder botAIMeleeWeaponHolder;
    [HideInInspector] public BotAIRangeWeaponHolder botAIRangeWeaponHolder;
    BotAIHitbox hitBox;
    //public Animator animatorHit;                            // аниматор мили оружия

    // Тип бота
    public bool isNeutral;                                  // не будет никого атаковать
    public bool isFriendly;                                 // союзный бот
    public bool isEnemy;                                    // несоюзный бот

    // Преследование
    [HideInInspector] public GameObject target;             // цель
    [HideInInspector] public LayerMask layerTarget;         // слой для поиска 
    [HideInInspector] public LayerMask layerHit;            // слой для оружия
    [HideInInspector] public bool chasing;                  // статус преследования
    [HideInInspector] public Vector3 startPosition;         // позиция для охраны
    public float chaseLeght;                                // дальность преследования    
    public float triggerLenght;                             // дистанция тригера
    [HideInInspector] public bool targetVisible;            // видим мы цель или нет
    public bool readyToAttack;                              // можно атаковать
    public float distanceToAttackMelee;                     // дистанция для атакой мили
    public float distanceToAttackRange;                     // дистанция для атаки ренж
    [HideInInspector] public float distanceToAttack;        // дистанция, с которой можно атаковать

    public bool meleeAttackType;                            // устанавливаем тип атаки мили
    public bool rangeAttackType;                            // ... ренж
    public bool twoWeapons;                                 // если есть 2 оружия
    //public bool switchMelee;

    bool slowed;
    float maxSpeed;

    // Для анимации
    [HideInInspector] public float aimAnglePivot;           // угол поворота хитбокспивота
    public GameObject deathEffect;                          // эффект (потом сделать его в аниматоре (или  нет))
    public float deathCameraShake;                          // мощность тряски камеры при убийстве
    [HideInInspector] public bool flipLeft;                  // для флипа
    [HideInInspector] public bool flipRight;                 //    

    // Таймер для цветов при уроне
    float timerForColor;
    bool red;

    // Бары
    [HideInInspector] public GameObject hpBarGO;

    // Для триггера
    public LayerMask layerTrigger;

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
        pivot = GetComponentInChildren<BotAIHitBoxPivot>();
        botAIMeleeWeaponHolder = GetComponentInChildren<BotAIMeleeWeaponHolder>();
        botAIRangeWeaponHolder = GetComponentInChildren<BotAIRangeWeaponHolder>();
        hitBox = GetComponentInChildren<BotAIHitbox>();

        layerTarget = LayerMask.GetMask("Player", "NPC");
        layerHit = LayerMask.GetMask("Player", "NPC", "ObjectsDestroyble", "Default");
        if (isFriendly)
        {
            layerTarget = LayerMask.GetMask("Enemy");                                   // слой поиска цели
            gameObject.layer = LayerMask.NameToLayer("NPC");                            // слой самого бота
            layerHit = LayerMask.GetMask("Enemy", "ObjectsDestroyble", "Default");      // слой для оружия
        }
        hpBarGO = transform.GetChild(0).gameObject;         // находим хп бар (ставлю его в начало иерархии)

        maxSpeed = agent.speed;
    }

    public override void Start()
    {
        base.Start();
        startPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        agent.updateRotation = false;                       // для навмеш2д
        agent.updateUpAxis = false;                         //
        agent.ResetPath();                                  // сбрасываем путь, потому что он при старте есть

        if (meleeAttackType)
            SwitchAttackType(1);
        if (rangeAttackType)
            SwitchAttackType(2);

        hpBarGO.SetActive(false);           // прячем хп бар, пока не получим урон
    }

    private void Update()
    {
        // Выбор цвета при получении урона и его сброс
        SetColorTimer();

        if (!isAlive)
            return;

        // Поворот хитбокса
        if (target && chasing && targetVisible)
        {
            Vector3 aimDirection = target.transform.position - pivot.transform.position;               // угол между положением мыши и pivot оружия          
            aimAnglePivot = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;                            // находим угол в градусах             
            Quaternion qua1 = Quaternion.Euler(0, 0, aimAnglePivot);                                                // создаем этот угол в Quaternion
            pivot.transform.rotation = Quaternion.Lerp(pivot.transform.rotation, qua1, Time.fixedDeltaTime * 5);   // делаем Lerp между weaponHoder и нашим углом
        }
        else 
        {
            if (flipRight)
                pivot.transform.rotation = Quaternion.Lerp(pivot.transform.rotation, Quaternion.Euler(0, 0, 0), Time.fixedDeltaTime * 15);   // делаем Lerp между weaponHoder и нашим углом
            if (flipLeft)
                pivot.transform.rotation = Quaternion.Lerp(pivot.transform.rotation, Quaternion.Euler(0, 0, 180), Time.fixedDeltaTime * 15);   // делаем Lerp между weaponHoder и нашим углом
        }

        // поворот спрайта (Flip)       
        if (target && targetVisible)                           // (потом chasing заменить на target и ещё это дублируется в хитбокспивот)
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


        // Дебаг
        if (debug)
            //Debug.Log(targetVisible);
            Debug.Log(maxSpeed);
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
                                                        // присваиваем время атаки
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
                    if (targetVisible && distance < 3)
                    {
                        targets.Add(fighter.gameObject);
                    }
                }
            }
            collidersHitbox = null;                         // сбрасываем все найденные объекты (на самом деле непонятно как это работает)
        }
        if (targets.Count > 0)
            target = targets[Random.Range(0, targets.Count)];
    }


    public void Chase()
    {
        float distance = Vector3.Distance(transform.position, target.transform.position);       // считаем дистанцию до цели
        if (distance < distanceToAttack && targetVisible)                                       // если дошли до цели и видим её
        {
            if (!readyToAttack)
            {
                agent.ResetPath();                                                              // сбрасываем путь            
                readyToAttack = true;                                                           // готов стрелять
                //Debug.Log("Ready Attack");
            }
        }
        else
        {
            agent.SetDestination(target.transform.position);                                    // перемещаемся к цели
            if (readyToAttack)
                readyToAttack = false;                                                          // не готов стрелять                
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




    // Фукция для ивента анимации (потом как-нибудь сделать по нормальному)
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
        Vector2 vec2 = (transform.position - forceDirection).normalized;        // направление отдачи нормализированное
        rb2D.AddForce(vec2 * forceBack, ForceMode2D.Impulse);                   // толкаем импульсом
    }

    public override void TakeDamage(int dmg, Vector2 vec2, float pushForce)
    {
        if (currentHealth == maxHealth)
        {
            hpBarGO.SetActive(true);
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
        Collider2D[] collidersHits = Physics2D.OverlapCircleAll(transform.position, 2, layerTrigger);     // создаем круг в позиции объекта с радиусом
        foreach (Collider2D coll in collidersHits)
        {
            if (coll == null)
            {
                continue;
            }

            if (coll.gameObject.TryGetComponent<BotAI>(out BotAI enemy))        // (потом переделать на скрипт Enemy)
            {
                enemy.triggerLenght = 25;                                       // (потом поменять на таргет = плеер)          
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
        spriteRenderer.color = Color.white;
        red = false;
    }

    // Поворот спрайта
    void FaceTargetRight()                                  // поворот направо
    {
        spriteRenderer.flipX = false;
        flipLeft = false;
        flipRight = true;
    }
    void FaceTargetLeft()                                   // поворот налево
    {
        spriteRenderer.flipX = true;
        flipRight = false;
        flipLeft = true;
    }

    protected override void Death()
    {
        base.Death();

        GameManager.instance.enemyCount--;                                                          // -1 к счётчику врагов

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
        agent.enabled = false;                      // выключаем агента

        //botAIMeleeWeaponHolder.gameObject.SetActive(false);
        //botAIRangeWeaponHolder.gameObject.SetActive(false);

        botAIMeleeWeaponHolder.HideWeapons();       // прячем оружия
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