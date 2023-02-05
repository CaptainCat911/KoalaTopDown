using UnityEngine;
using UnityEngine.AI;


public class Player : Fighter
{
    // Ссылки    
    [HideInInspector] public Animator animator;
    NavMeshAgent agent;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public WeaponHolder weaponHolder;
    [HideInInspector] public BombWeaponHolder bombWeaponHolder;
    HitBoxPivot hitBoxPivot;

    // Передвижение
    [HideInInspector] public Vector2 moveDirection;     // вектор для перемещения (направление)
    Vector2 movementVector;                             // вектор перещение (добавляем скорость)
    [Header("Параметры перемещения")]
    public float moveSpeed = 5f;                        // скорость передвижения
    public float dashForce;                             // сила рывка
    public float dashRate;                              // как часто можно делать рывок 
    float nextTimeToDash;                               // когда в следующий раз готов рывок

    // Для флипа игрока
    [HideInInspector] public bool needFlip;             // нужен флип (для игрока и оружия)    
    [HideInInspector] public bool leftFlip;             // оружие слева
    [HideInInspector] public bool rightFlip = true;     // оружие справа

    // Таймер для цветов при уроне
    float timerForColor;        // сколько времени он будет красным
    bool red;                   // красный (-_-)

    public EnergyShield shield;



    //---------------------------------------------------------------------------------------------------------------------------------------------------------\\


    public override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        weaponHolder = GetComponentInChildren<WeaponHolder>();
        bombWeaponHolder = GetComponentInChildren<BombWeaponHolder>();
        hitBoxPivot = GetComponentInChildren<HitBoxPivot>();

        agent.updateRotation = false;               // для навМеш2д
        agent.updateUpAxis = false;                 //        
    }

    void Update()
    {
        // Перемещение и направление
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");        
        moveDirection = new Vector2(moveX, moveY).normalized;                       // скорость нормализированная 

        // Рывок
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= nextTimeToDash)
        {
            nextTimeToDash = Time.time + 1f / dashRate;                 // вычисляем кд
            Dash();
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
        rb2D.AddForce(moveDirection * dashForce, ForceMode2D.Impulse);                // даём импульс
    }




    // Фукция для ивента анимации (потом как-нибудь сделать по нормальному и через ивент)
/*    public void AttacHitBox()
    {
        hitBox.HitBoxAttack();
    }

    public void TrailHitbox(int isOn)
    {
        hitBox.TrailOn(isOn);
    }
*/





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
        ChatBubble.Create(transform, new Vector3(0.2f, 0.2f), text);
    }
}