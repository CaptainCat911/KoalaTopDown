using UnityEngine;
using UnityEngine.AI;


public class Player : Fighter
{
    // Ссылки    
    //NavMeshAgent agent;
    [HideInInspector] public Animator animator;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public WeaponHolder weaponHolder;
    [HideInInspector] public WeaponHolderMelee weaponHolderMelee;
    [HideInInspector] public BombWeaponHolder bombWeaponHolder;
    [HideInInspector] public HitBoxPivot hitBoxPivot;
    [HideInInspector] public bool playerWeaponReady;    // игрок готов (стрелять (это для блинка))
    Ignitable ignitable;

    [Header("Параметры перемещения")]    
    [HideInInspector] public Vector2 moveDirection;     // вектор для перемещения (направление)
    Vector2 movementVector;                             // вектор перещение (добавляем скорость)
    public float moveSpeed = 5f;                        // скорость передвижения

    [Header("Рывок")]
    public float dashForce;                             // сила рывка
    public float dashRate;                              // как часто можно делать рывок 
    float nextTimeToDash;                               // когда в следующий раз готов рывок

    [Header("Блинк")]
    public bool blink;                  // блинк 
    public float blinkForce;            // сила (дальность) блинка
    public float blinkRate;             // кд блинка
    public float blinkOutTime;          // время в межпространстве
    [Space(2)]
    public bool blinkWithExplousion;    // с хлопком
    public int blinkOutDamage;          // урон
    public float blinkOutExpRadius;     // радиус
    public float blinkOutPushForce;     // толчек
    public LayerMask layerExplBlink;    // слой
    public TrailRenderer blinkTrail;    // треил
    public GameObject antiBugCircle;    // круг от застревания

    [Header("Реактивные ботинки")]
    public bool bootsMod;
    public float bootsSpeed;
    public float bootsEnergy;
    public float minusEnergy;
    public float plusEnergy;
    bool bootsOn;

    [Header("Флеш мод")]
    public bool flashMod;
    public float flashSpeed;
    bool inBlinkSpace;

    [Header("Параметры энергощита")]
    public EnergyShield shield;

    [Header("Параметры магнита для монеток")]
    public bool withGoldMagnet;
    public float speedMagnet;

    // Для флипа игрока
    [HideInInspector] public bool needFlip;             // нужен флип (для игрока и оружия)    
    [HideInInspector] public bool leftFlip;             // оружие слева
    [HideInInspector] public bool rightFlip = true;     // оружие справа

    // Таймер для цветов при уроне
    float timerForColor;        // сколько времени он будет красным
    bool red;                   // красный (-_-)







    //---------------------------------------------------------------------------------------------------------------------------------------------------------\\

    public override void Start()
    {
        base.Start();
        playerWeaponReady = true;
        animator = GetComponent<Animator>();
        //agent = GetComponent<NavMeshAgent>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        weaponHolder = GetComponentInChildren<WeaponHolder>();
        weaponHolderMelee = GetComponentInChildren<WeaponHolderMelee>();
        bombWeaponHolder = GetComponentInChildren<BombWeaponHolder>();
        hitBoxPivot = GetComponentInChildren<HitBoxPivot>();
        ignitable = GetComponent<Ignitable>();

        //agent.updateRotation = false;               // для навМеш2д
        //agent.updateUpAxis = false;                 //        
    }

    public override void Update()
    {
        base.Update();

        //Debug.Log(rightFlip);

        // Таймеры
        if (bootsMod)
        {
            if (bootsOn)
            {
                bootsEnergy -= Time.deltaTime * minusEnergy;
                if (bootsEnergy < 0)
                    bootsEnergy = 0;
            }
            if (!bootsOn && bootsEnergy < 100)
            {
                bootsEnergy += Time.deltaTime * plusEnergy;
                if (bootsEnergy > 100)
                    bootsEnergy = 100;
            }
        }

        if (GameManager.instance.isPlayerEnactive)              // если игрок не активен
        {
            moveDirection = new Vector2(0, 0).normalized;       // сбрасываем вектор для скорости
            if (bootsOn)
                BootsMode(false);                               // выключаем реактивные ботинки
            return;
        }

        // Перемещение и направление
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");        
        moveDirection = new Vector2(moveX, moveY).normalized;               // скорость нормализированная 

        // Рывок
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= nextTimeToDash)
        {
            if (blink && !inBlinkSpace)
            {
                nextTimeToDash = Time.time + 1f / blinkRate;                    // вычисляем кд
                BlinkIn();
            }
            else
            {
                nextTimeToDash = Time.time + 1f / dashRate;                     // вычисляем кд
                Dash();
            }
        }

        if (bootsMod)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && bootsEnergy >= 20 && !bootsOn)
            {
                BootsMode(true);
            }
            if ((Input.GetKeyUp(KeyCode.LeftShift) || bootsEnergy <= 0) && bootsOn)
            {
                BootsMode(false);
            }
        }


        // Флеш мод
        /*        if (flashMod)
                {
                    if (Input.GetKeyDown(KeyCode.LeftShift) && !inBlinkSpace)
                    {
                        RunFlashMode(true);
                    }
                    if (Input.GetKeyUp(KeyCode.LeftShift))
                    {
                        RunFlashMode(false);
                    }
                }*/



        // Анимации 
        animator.SetFloat("Speed", movementVector.magnitude);
        //Debug.Log(movementVector.magnitude);

        // Флип спрайта игрока
        if (Mathf.Abs(weaponHolder.aimAngle) > 90 && rightFlip)
        {
            needFlip = true;
            leftFlip = true;
            rightFlip = false;
        }
        if (Mathf.Abs(weaponHolder.aimAngle) <= 90 && leftFlip)
        {
            needFlip = true;
            rightFlip = true;
            leftFlip = false;
        }
        if (needFlip)
        {
            Flip();
            hitBoxPivot.Flip();
        }

        // Выбор цвета при получении урона и его сброс
        SetColorTimer();
    }


    private void FixedUpdate()
    {
        // Перемещение
        UpdateMotor(moveDirection);             // запускаем мотор

        //rb2D.AddForce(moveDirection * moveSpeed);
        //rb2D.MovePosition(rb2D.position + moveDirection * moveSpeed * Time.deltaTime);                // скорость полная  
        //rb2D.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);        // скорость полная        
    }

    // Мотор
    void UpdateMotor(Vector2 input)
    {
        movementVector = new Vector2(input.x * moveSpeed, input.y * moveSpeed);     // добавляем скорость к направлению
        transform.Translate(movementVector * Time.deltaTime);                       // перемещаем с учётом дельтаТайм

        /*movementVector = new Vector2(input.x * agent.speed, input.y * agent.speed);                      // создаем вектор куда нужно переместится        
        agent.Move(movementVector * Time.deltaTime);                                                        // перемещаем с учётом дельтаТайм
        Debug.Log(movementVector);*/
    }
    void Dash()
    {
        rb2D.AddForce(moveDirection * dashForce, ForceMode2D.Impulse);              // даём импульс
    }

    void BlinkIn()
    {
        inBlinkSpace = true;
        rb2D.AddForce(moveDirection * blinkForce, ForceMode2D.Impulse);              // даём импульс
        gameObject.layer = LayerMask.NameToLayer("BlinkSpace");                     // слой самого бота

        GameObject effect = Instantiate(GameAssets.instance.playerBlinkIn,
            transform.position, Quaternion.identity);                               // создаем эффект убийства
        Destroy(effect, 0.5f);                                                      // уничтожаем эффект через .. сек
        blinkTrail.emitting = true;                                                 // включаем треил
        spriteRenderer.enabled = false;                                             // отключаем спрайт
        //spriteRenderer.color = new Color(1, 0, 0, 0.5f);                            
        //playerWeaponReady = false;
        //weaponHolder.HideWeapon(true);
        weaponHolder.gameObject.SetActive(false);               // отключаем оружия 
        weaponHolderMelee.gameObject.SetActive(false);          //
        ignitable.flames.gameObject.SetActive(false);           // отключаем горение

        Invoke("BlinkOut", blinkOutTime);
        Invoke("AntiBugCircleOn", blinkOutTime - 0.04f);
    }
    void BlinkOut()
    {        
        gameObject.layer = LayerMask.NameToLayer("Player");                     // слой самого бота

        if (blinkWithExplousion)
            BlinkExplousion();

        spriteRenderer.enabled = true;
        //spriteRenderer.color = Color.white;
        GameObject effect = Instantiate(GameAssets.instance.playerBlinkOut,
            transform.position, Quaternion.identity);                               // создаем эффект убийства
        Destroy(effect, 0.5f);                                                      // уничтожаем эффект через .. сек
        blinkTrail.emitting = false;        
        //playerWeaponReady = true;
        //weaponHolder.HideWeapon(false);
        weaponHolder.gameObject.SetActive(true);
        weaponHolderMelee.gameObject.SetActive(true);
        ignitable.flames.gameObject.SetActive(true);
        inBlinkSpace = false;
    }

    void RunFlashMode(bool status)
    {
        if (status)
        {
            inBlinkSpace = true;
            gameObject.layer = LayerMask.NameToLayer("BlinkSpace");                     // слой самого бота
            moveSpeed += flashSpeed;
            GameObject effect = Instantiate(GameAssets.instance.playerBlinkIn,
                transform.position, Quaternion.identity);                               // создаем эффект убийства
            Destroy(effect, 0.5f);                                                      // уничтожаем эффект через .. сек
            blinkTrail.emitting = true;                                                 // включаем треил
            spriteRenderer.color = new Color(1, 0, 0, 0.5f);   
            weaponHolder.gameObject.SetActive(false);               // отключаем оружия 
            weaponHolderMelee.gameObject.SetActive(false);          //
            ignitable.flames.gameObject.SetActive(false);           // отключаем горение
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Player");                     // слой самого бота
            moveSpeed -= flashSpeed;
            spriteRenderer.enabled = true;
            GameObject effect = Instantiate(GameAssets.instance.playerBlinkOut,
                transform.position, Quaternion.identity);                               // создаем эффект убийства
            Destroy(effect, 0.5f);                                                      // уничтожаем эффект через .. сек
            blinkTrail.emitting = false;
            spriteRenderer.color = Color.white;
            weaponHolder.gameObject.SetActive(true);
            weaponHolderMelee.gameObject.SetActive(true);
            ignitable.flames.gameObject.SetActive(true);
            inBlinkSpace = false;
        }        
    }

    void BootsMode(bool status)
    {
        if (status)
        {
            bootsOn = true;
            moveSpeed += bootsSpeed;
            bootsEnergy -= 10;
            GameObject effect = Instantiate(GameAssets.instance.explousionSmall,
                transform.position, Quaternion.identity);                               // создаем эффект убийства
            Destroy(effect, 0.5f);                                                      // уничтожаем эффект через .. сек
            blinkTrail.emitting = true;                                                 // включаем треил
        }
        else
        {            
            moveSpeed -= bootsSpeed;            
            //GameObject effect = Instantiate(GameAssets.instance.explousionSmall,
                //transform.position, Quaternion.identity);                               // создаем эффект убийства
            //Destroy(effect, 0.5f);                                                      // уничтожаем эффект через .. сек
            blinkTrail.emitting = false;
            bootsOn = false;
        }
    }

    void AntiBugCircleOn()
    {
        antiBugCircle.SetActive(true);
        Invoke("AntiBugCircleOff", 0.02f);
    }
    void AntiBugCircleOff()
    {
        antiBugCircle.SetActive(false);        
    }

    void BlinkExplousion()
    {
        Collider2D[] collidersHits = Physics2D.OverlapCircleAll(transform.position, blinkOutExpRadius, layerExplBlink);     // создаем круг в позиции объекта с радиусом
        foreach (Collider2D coll in collidersHits)
        {
            if (coll == null)
            {
                continue;
            }

            if (coll.gameObject.TryGetComponent<Fighter>(out Fighter fighter))
            {
                Vector2 vec2 = (coll.transform.position - transform.position).normalized;
                fighter.TakeDamage(blinkOutDamage, vec2, blinkOutPushForce);
            }
            collidersHits = null;
        }
        CMCameraShake.Instance.ShakeCamera(2, 0.2f);            // тряска камеры

        GameObject effect = Instantiate(GameAssets.instance.explousionRedEffect,
            transform.position, Quaternion.identity);                                      // создаем эффект убийства
        Destroy(effect, 1);                                                             // уничтожаем эффект через .. сек
    }

    public void Move(Vector3 targetPosition, bool moving)
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, 3 * Time.deltaTime);   // перемещаем игрока

        if (targetPosition.x > transform.position.x && leftFlip)        // если игрок повернут влево, а цель справа
        {
            rightFlip = true;   
            leftFlip = false;
            Flip();
            hitBoxPivot.Flip();
        }
        if (targetPosition.x < transform.position.x && rightFlip)
        {
            rightFlip = false;
            leftFlip = true;
            Flip();
            hitBoxPivot.Flip();
        }

        if (moving)                             // если двигаемся
            animator.SetFloat("Speed", 1);      
        else
            animator.SetFloat("Speed", 0);
    }


    // Флип игрока
    void Flip()
    {
        if (leftFlip)                               // разворот налево
        {
            spriteRenderer.flipX = true;            // поворачиваем спрайт игрока
        }
        if (rightFlip)
        {
            spriteRenderer.flipX = false;
        }
        needFlip = false;
    }

    // Отдача
    public void ForceBackFire(Vector3 forceDirection, float forceBack)
    {
        Vector2 vec2 = (transform.position - forceDirection).normalized;        // направление отдачи нормализированное
        rb2D.AddForce(vec2 * forceBack, ForceMode2D.Impulse);                   // толкаем импульсом
    }

    // Получение урона
    public override void TakeDamage(int dmg, Vector2 vec2, float pushForce)
    {
        if (shield.shieldOn)
        {
            shield.TakeDamage(dmg);
        }
        else
        {
            base.TakeDamage(dmg, vec2, pushForce);
            if (dmg == 0)
                return;
            animator.SetTrigger("TakeHit");
            ColorRed(0.1f);                         // делаем спрайт красным
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


    // Что-то сказать
    public void SayText(string text)
    {
        ChatBubble.Clear(gameObject);
        ChatBubble.Create(transform, new Vector3(0.2f, 0.2f), text, 2f);
    }

    protected override void Death()
    {
        base.Death();
        spriteRenderer.color = Color.white;
        GameManager.instance.isPlayerEnactive = true;
    }
}