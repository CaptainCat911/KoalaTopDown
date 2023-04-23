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
    public float dashForce;                             // сила рывка
    public float dashRate;                              // как часто можно делать рывок 
    float nextTimeToDash;                               // когда в следующий раз готов рывок

    public bool blink;
    public float blinkForce;
    public float blinkRate;
    public float blinkOutTime;
    public TrailRenderer blinkTrail;

    [Header("Параметры энергощита")]
    public EnergyShield shield;

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

    void Update()
    {
        //Debug.Log(rightFlip);

        if (GameManager.instance.isPlayerEnactive)              // если игрок не активен
        {
            moveDirection = new Vector2(0, 0).normalized;       // сбрасываем вектор для скорости 
            return;
        }

        // Перемещение и направление
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");        
        moveDirection = new Vector2(moveX, moveY).normalized;               // скорость нормализированная 

        // Рывок
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= nextTimeToDash)
        {
            if (blink)
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
        rb2D.AddForce(moveDirection * blinkForce, ForceMode2D.Impulse);              // даём импульс

        GameObject effect = Instantiate(GameAssets.instance.playerBlinkIn,
            transform.position, Quaternion.identity);                               // создаем эффект убийства
        Destroy(effect, 0.5f);                                                      // уничтожаем эффект через .. сек
        blinkTrail.emitting = true;

        gameObject.layer = LayerMask.NameToLayer("BlinkSpace");                     // слой самого бота
        spriteRenderer.enabled = false;
        spriteRenderer.color = new Color(1, 0, 0, 0.5f);
        //playerWeaponReady = false;
        //weaponHolder.HideWeapon(true);
        weaponHolder.gameObject.SetActive(false);
        weaponHolderMelee.gameObject.SetActive(false);
        ignitable.flames.gameObject.SetActive(false);

        Invoke("BlinkOut", blinkOutTime);
    }
    void BlinkOut()
    {        
        gameObject.layer = LayerMask.NameToLayer("Player");                     // слой самого бота

        spriteRenderer.enabled = true;
        spriteRenderer.color = Color.white;
        GameObject effect = Instantiate(GameAssets.instance.playerBlinkOut,
            transform.position, Quaternion.identity);                               // создаем эффект убийства
        Destroy(effect, 0.5f);                                                      // уничтожаем эффект через .. сек
        blinkTrail.emitting = false;

        //playerWeaponReady = true;
        //weaponHolder.HideWeapon(false);
        weaponHolder.gameObject.SetActive(true);
        weaponHolderMelee.gameObject.SetActive(true);
        ignitable.flames.gameObject.SetActive(true);
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