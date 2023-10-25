using UnityEngine;
using UnityEngine.Rendering.Universal;      // для 2д света

public class Weapon : MonoBehaviour
{
    [Header("Ссылки")]                      // почему-то не отображается
    Player player;
    AmmoPackStore[] ammoWeapons;
    public WeaponClass weaponClass;         // ссылка на класс оружия
    public Transform firePoint;             // якорь для снарядов
    public Transform pivot;                 // якорь weaponHolder (используется для прицеливания)
    WeaponHolder weaponHolder;              // ссылка на скрипт weaponHolder (для стрельбы)

    [Header("Параметры оружия")]
    public int weaponIndexForAmmo;                  // индекс оружия (для патронов)
    [HideInInspector] public int ammo;

    [HideInInspector] public string weaponName;     // название оружия
    [HideInInspector] public string weaponNameEng;     // название оружия
    [HideInInspector] public float fireRate;        // скорострельность оружия (10 - 0,1 выстрелов в секунду)
    [HideInInspector] public float nextTimeToFire;  // для стрельбы (когда стрелять в след раз)       
    float currentDelay;

    // Для флипа оружия
    bool needFlip;                          // нужен флип (для правильного отображения оружия)    
    bool leftFlip;                          // оружие слева
    bool rightFlip = true;                  // оружие справа

    [Header("Эффекты")]
    public Animator flashEffectAnimator;    // флеш при стрельбе
    Light2D light2DFlash;                   // 2д свет флеш
    Color originFlashColor;
    public bool singleFlash;                // одиночный флеш
    bool flashActive;                       // флеш активен (для мультифлеша)
    public LineRenderer lineRenderer;       // линия для лазера (префаб)
    LineRenderer lineRaycast;               // линия для лазера (создаём)
    public TrailRenderer tracerEffect;      // трасер
    public ParticleSystem effectParticles;  // префаб системы частиц стрельбы
    public ParticleSystem startParticles;   // префаб системы частиц при старте огня


    [Header("Тряска камеры при выстреле")]
    public float cameraAmplitudeShake = 1f; // амплитуда
    public float cameraTimedeShake = 0.1f;  // длительность

    [Header("Звук")]
    public bool singleShot;                 // одиночный звук
    public bool multiShot;                  // круговой звук
    bool soundStarted;                      // звук начался
    public AudioSource audioSource;         // основной звук
    public AudioSource audioSourceTail;     // "хвост"

    [Header("Для сообщений")]
    float cooldownMessage = 1f;             // перезардяка атаки
    float lastMessage;                      // время последнего удара (для перезарядки удара)

    Vector3 originPosition;

    public bool debug;

    void Awake()
    {
        player = GameManager.instance.player;                           // игрок
        ammoWeapons = GameManager.instance.ammoManager.ammoWeapons;     // оружия
        weaponName = weaponClass.weaponName;                            // имя оружия
        weaponNameEng = weaponClass.weaponNameEng;                      // имя оружия на англ
        ammo = ammoWeapons[weaponIndexForAmmo].allAmmo;                 // патроны

        audioSource = GetComponent<AudioSource>();

        light2DFlash = flashEffectAnimator.gameObject.GetComponentInChildren<Light2D>();    // свет флеш
        if (light2DFlash)
            originFlashColor = light2DFlash.color;

        if (singleShot)
        {
            audioSource.loop = false;
        }
        if (multiShot)
        {
            audioSource.loop = true;
        }
    }

    private void Start()
    {
        weaponHolder = GetComponentInParent<WeaponHolder>();                    // находим скрипт weaponHolder
        originPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
    }

    private void OnEnable()
    {        
        currentDelay = weaponClass.delayFire;

        if (light2DFlash)                                   // если есть свет для флеш
        {
            if (player.darkPlace)                           // если место темное 
            {
                light2DFlash.color = originFlashColor;      // ставим оригинальный цвет
            }
            else                                            // если светлое место
            {
                light2DFlash.color = new Color(originFlashColor.r, originFlashColor.g, originFlashColor.b, 0.2f);   // уменьшаем альфу
            }
        }
    }

    private void OnDisable()
    {
        soundStarted = false;               
    }

    private void Update()
    {
        //Debug.Log(currentDelay);

        // Флип оружия
        if (GameManager.instance.player.leftFlip && rightFlip)
        {
            needFlip = true;
            leftFlip = true;
            rightFlip = false;
        }
        if (GameManager.instance.player.rightFlip && leftFlip)
        {
            needFlip = true;
            rightFlip = true;
            leftFlip = false;
        }
        if (needFlip)
        {
            Flip();
        }

        // Эффекты выстрелов
        if (!flashEffectAnimator)
        {
            return;
        }

        // Стирать рендер лазера и мультифлеш (возможно стоит переделать)
        if (!singleFlash && Time.time >= nextTimeToFire + 0.1f)
        {
            flashEffectAnimator.SetBool("Fire", false);
            flashActive = false;

            if (lineRaycast)
                lineRaycast.enabled = false;
        }

        // Звук
        if (multiShot)
        {
            if (ammoWeapons[weaponIndexForAmmo].allAmmo > 0)            // если патронов больше 0
            {
                if (weaponHolder.fireStart && !soundStarted)            // если готовы стрелять
                {
                    audioSource.Play();
                    soundStarted = true;
                }
                if (!weaponHolder.fireStart && soundStarted)            // если не готовы стрелять
                {
                    audioSource.Stop();
                    if (audioSourceTail)
                        audioSourceTail.Play();
                    soundStarted = false;
                }
            }
            else                                    // если патроны закончились
            {
                if (soundStarted)                   // и играл звук
                {
                    audioSource.Stop();             // останавливаем звук
                    if (audioSourceTail)
                        audioSourceTail.Play();     // воспроизводим "хваост"
                    soundStarted = false;           // звук остановлен
                }
            }
        }        
    }


    private void FixedUpdate()
    {
        // Стрельба
        //Debug.Log(originPosition.x);

        // Возвращение отдачи
        if (transform.localPosition.x < originPosition.x)       
        {
            //Debug.Log("Recoil!");
            transform.localPosition = new Vector3(transform.localPosition.x + 0.05f, transform.localPosition.y, transform.localPosition.z);
        }

        // Задержка перед выстрелом
        if (weaponHolder.fireStart && ammoWeapons[weaponIndexForAmmo].allAmmo > 0)          // если готовы стрелять и есть патроны
        {
            currentDelay -= 0.02f;

            if (startParticles)
            {
                if (currentDelay > 0)
                {
                    startParticles.Play();
                }
                else
                {
                    startParticles.Stop();
                }
            }
        }

        // Сообщение, что закончились патроны
        if (weaponHolder.fireStart && ammoWeapons[weaponIndexForAmmo].allAmmo <= 0 && Time.fixedTime - lastMessage > cooldownMessage)
        {
            if (LanguageManager.instance.eng)
            {
                GameManager.instance.CreateFloatingMessage("No ammo!", Color.white, GameManager.instance.player.transform.position);
            }
            else
            {
                GameManager.instance.CreateFloatingMessage("Нет патронов!", Color.white, GameManager.instance.player.transform.position);
            }            

            lastMessage = Time.fixedTime;
        }

        // Если закончились патроны
        if (!weaponHolder.fireStart || ammoWeapons[weaponIndexForAmmo].allAmmo <= 0)        // если не готовы стрелять или закончились патроны
        {
            currentDelay = weaponClass.delayFire;

            if (startParticles)
                startParticles.Stop();
            if (effectParticles)
                effectParticles.Stop();
            return;
        }

        // Задержка перед стрельбой
        if (currentDelay > 0)
            return;                

        if (Time.time >= nextTimeToFire)     // если начинаем стрелять и кд готово
        {
/*            if (ammoWeapons[weaponIndexForAmmo].allAmmo <= 0)
            {                
                if (startParticles)
                    startParticles.Stop();
                if (effectParticles)
                    effectParticles.Stop();
                return;
            }*/

            transform.localPosition = new Vector3(transform.localPosition.x - weaponClass.effectBackFire, transform.localPosition.y, transform.localPosition.z);    // отдача (эффект)

            if (!GameManager.instance.infiniteAmmo)
                ammoWeapons[weaponIndexForAmmo].allAmmo--;                  // - патроны
            nextTimeToFire = Time.time + 1f / weaponClass.fireRate;         // вычисляем кд           

            if (((int)weaponClass.weaponType) == 0)
                FireProjectile();                       // выстрел пулей
            if (((int)weaponClass.weaponType) == 1)
                FireRayCast();                          // выстрел рейкастом
            if (((int)weaponClass.weaponType) == 2)
                FireBoxCast();                          // выстрел бокскастом
            if (((int)weaponClass.weaponType) == 3)
                FireSplit(false);                       // выстрел "дробью"
            if (((int)weaponClass.weaponType) == 4)
                FireSplit(true);                        // выстрел "дробью" рейкастов
            if (((int)weaponClass.weaponType) == 5)
                FireRayCastAll();                       // выстрел рейкастом по всем (просторел)
            if (((int)weaponClass.weaponType) == 6)
                FireBoxCastAll();                       // выстрел бокскастом по всем (просторел)

            CMCameraShake.Instance.ShakeCamera(cameraAmplitudeShake, cameraTimedeShake);    // тряска камеры

            // Флэш
            if (flashEffectAnimator != null)        // если флэшэффект есть
                Flash();

            if (effectParticles)
            {
                effectParticles.Play();
                //flameParticles.transform.position = firePoint.transform.position;              
            }

            // Аудио
            if (singleShot)
            {
                audioSource.Play();
            }
        }

        // Находим угол для флипа холдера и спрайта игрока
        //mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);                // положение мыши                  
        //Vector3 aimDirection = mousePosition - transform.position;                          // угол между положением мыши и pivot оружия          
        //float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;       // находим угол в градусах             
        //Quaternion qua1 = Quaternion.Euler(0, 0, aimAngle);                                 // создаем этот угол в Quaternion
        //weaponHolderGO.transform.rotation = Quaternion.Lerp(weaponHolderGO.transform.rotation, qua1, Time.fixedDeltaTime * 15); // делаем Lerp между weaponHoder и нашим углом



        // Отображение оружия перед или позади игрока
        /*        if (aimAngle > 0)
                {
                    spriteRenderer.sortingOrder = -1;
                }
                else
                {
                    spriteRenderer.sortingOrder = 1;
                }*/
    }

    // Флэш
    void Flash()
    {
        if (!singleFlash)
        {
            if (!flashActive)
            {
                flashEffectAnimator.SetBool("Fire", true);
                flashActive = true;
            }
        }
        else
        {
            flashEffectAnimator.SetTrigger("Fire");
        }
    }

    public void SetLightFlash(Color color)
    {        
        light2DFlash.color = color;
        //light2DFlash.intensity = intensity;        
    }

    // Флип
    void Flip()
    {
        if (leftFlip)                                                                                   // разворот налево
        {
            transform.localScale = new Vector3(transform.localScale.x, -1, transform.localScale.z);     // поворачиваем оружие через scale
        }
        if (rightFlip)
        {
            transform.localScale = new Vector3(transform.localScale.x, 1, transform.localScale.z);
        }
        needFlip = false;
    }

    void FireProjectile()
    {
        float randomBulletX = Random.Range(-weaponClass.recoil, weaponClass.recoil);                            // разброс стрельбы
        firePoint.Rotate(0, 0, randomBulletX);                                                                  // тупо вращаем
        GameObject bullet = Instantiate(weaponClass.bulletPrefab, firePoint.position, firePoint.rotation);      // создаем префаб снаряда с позицией и поворотом якоря
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.damage = weaponClass.damage;                                              // присваиваем урон снаряду
        bulletScript.pushForce = weaponClass.pushForce;                                        // присваиваем силу толчка снаряду
        bulletScript.enemyToDamageCount = weaponClass.enemyToDamageCount;                      // сколько врагов пробьёт снаряд
        bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.right * weaponClass.bulletSpeed, ForceMode2D.Impulse);    // даём импульс
        player.ForceBackFire(firePoint.transform.position, weaponClass.forceBackFire);                          // даём отдачу оружия
        firePoint.Rotate(0, 0, -randomBulletX);                                                                 // и тупо возвращаем поворот
    }

    void FireSplit(bool raycast)
    {
        for (int i = 0; i < weaponClass.splitTimes; i++)                            // кол-во снарядов (разделений)
        {
            //Debug.Log("Fire");

            // Вращаем
            if (i % 2 == 1)                                                         // если делится без остатка (?)
                firePoint.Rotate(0, 0, (weaponClass.splitRecoil * (i + 1)));        // вращаем на сплитРекоил                                                           
            else
                firePoint.Rotate(0, 0, (-weaponClass.splitRecoil * (i)));

            if (!raycast)
                FireProjectile();
            if (raycast)
                FireRayCast();

            // Возвращаем
            if (i % 2 == 1)
                firePoint.Rotate(0, 0, (-weaponClass.splitRecoil * (i + 1)));
            else
                firePoint.Rotate(0, 0, (weaponClass.splitRecoil * (i)));
        }
    }


    void FireRayCast()
    {
        // Разброс
        float randomBulletX = Random.Range(-weaponClass.recoil, weaponClass.recoil);

        // Рейкаст2Д
        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, firePoint.right + new Vector3(randomBulletX, 0, 0), weaponClass.range, weaponClass.layerRayCast);
        if (hit.collider != null)
        {
            //Debug.Log("Hit!");
            if (hit.collider.TryGetComponent<Fighter>(out Fighter fighter))
            {
                Vector2 vec2 = (fighter.transform.position - player.transform.position).normalized;
                fighter.TakeDamage(weaponClass.damage, vec2, weaponClass.pushForce);                
            }

            // если со взрывом
            if (weaponClass.withExplousion)
            {
                Explousion(hit.point);
            }

            // Настройки для трасеров
            if (tracerEffect)
            {
                TrailRenderer tracer = Instantiate(tracerEffect, firePoint.position, Quaternion.identity);          // создаем трасер
                tracer.AddPosition(firePoint.position);                                                             // начальная позиция
                //tracer.transform.SetParent(transform, true); 
                tracer.transform.position = hit.point;                      // конечная позиция трасера рейкаста
            }


            if (weaponClass.ignite)
            {
                if (hit.collider.TryGetComponent<Ignitable>(out Ignitable ignitable))
                {
                    //Vector2 vec2 = (fighter.transform.position - player.transform.position).normalized;
                    ignitable.Ignite(weaponClass.damageBurn, weaponClass.cooldownBurn, weaponClass.durationBurn);
                }
            }


/*            if (!lineRaycast)
            {
                lineRaycast = Instantiate(lineRenderer, firePoint.position, Quaternion.identity);
            }
            lineRaycast.enabled = true;
            lineRaycast.SetPosition(0, firePoint.position);
*//*            lineRaycast.SetPosition(1, (hit.point - new Vector2(firePoint.position.x, firePoint.position.y))/2);            
            lineRaycast.SetPosition(2, (hit.point + new Vector2(firePoint.position.x, firePoint.position.y)) / 2);
            lineRaycast.SetPosition(3, hit.point - (hit.point - new Vector2(firePoint.position.x, firePoint.position.y)) / 1.5f);*//*
            lineRaycast.SetPosition(1, hit.point);*/



            if (debug)
                Debug.DrawRay(firePoint.position, firePoint.right * weaponClass.range, Color.yellow);
        }

        //Debug.Log("Hit");

        /*        if (Physics2D.Raycast(ray, out hit, Mathf.Infinity, layerRayCast))
                {
                    Debug.DrawLine(ray.origin, hit.point, Color.red, 1.0f);                                     // дебаг, красные линии

                    //tracer.transform.position = hit.point;                                                    // конечная позиция трасера пули 

                    if (hit.collider.TryGetComponent<Fighter>(out Fighter fighter))
                    {
                        fighter.TakeDamage(damage);
                        Vector2 vec2 = (player.transform.position - GameManager.instance.player.transform.position).normalized;
                        fighter.rb2D.AddForce(vec2 * pushForce, ForceMode2D.Impulse);
                    }
                }*/
    }

    void Explousion(Vector3 explPosition)
    {
        Collider2D[] collidersHits = Physics2D.OverlapCircleAll(explPosition, weaponClass.radiusExpl, weaponClass.layerExplousion);     // создаем круг в позиции объекта с радиусом
        foreach (Collider2D coll in collidersHits)
        {
            if (coll == null)
            {
                continue;
            }

            if (coll.gameObject.TryGetComponent<Fighter>(out Fighter fighter))
            {
                float distance = Vector2.Distance(transform.position, coll.transform.position);

                Vector2 vec2 = (coll.transform.position - explPosition).normalized;
                fighter.TakeDamage(weaponClass.damageExpl, vec2, weaponClass.pushForceExpl);
            }
            collidersHits = null;
        }

        if (weaponClass.expEffect)
        {
            GameObject effect = Instantiate(weaponClass.expEffect, explPosition, Quaternion.identity);          // создаем эффект
            Destroy(effect, 0.5f);                                                                              // уничтожаем эффект через .. сек    
        }
        if (weaponClass.sparksEffect)
        {
            GameObject effect = Instantiate(weaponClass.sparksEffect, explPosition, Quaternion.identity);       // создаем эффект
            Destroy(effect, 1);
        }

        CMCameraShake.Instance.ShakeCamera(cameraAmplitudeShake, cameraTimedeShake);            // тряска камеры
    }


    void FireRayCastAll()
    {
        // Разброс
        float randomBulletX = Random.Range(-weaponClass.recoil, weaponClass.recoil);

        // Рейкаст2Д
        RaycastHit2D[] hits = Physics2D.RaycastAll(firePoint.position, firePoint.right + new Vector3(randomBulletX, 0, 0), weaponClass.range, weaponClass.layerRayCast);
        if (hits != null)
        {
            foreach (RaycastHit2D hit in hits)
            {
                //Debug.Log("Hit!");
                if (hit.collider.TryGetComponent<Fighter>(out Fighter fighter))
                {
                    Vector2 vec2 = (fighter.transform.position - player.transform.position).normalized;
                    fighter.TakeDamage(weaponClass.damage, vec2, weaponClass.pushForce);
                }

                // Настройки для трасеров
                if (tracerEffect)
                {
                    TrailRenderer tracer = Instantiate(tracerEffect, firePoint.position, Quaternion.identity);          // создаем трасер
                    tracer.AddPosition(firePoint.position);                                                             // начальная позиция
                    //tracer.transform.SetParent(transform, true); 
                    tracer.transform.position = hit.point;                      // конечная позиция трасера рейкаста
                }

                if (weaponClass.ignite)
                {
                    if (hit.collider.TryGetComponent<Ignitable>(out Ignitable ignitable))
                    {
                        //Vector2 vec2 = (fighter.transform.position - player.transform.position).normalized;
                        ignitable.Ignite(weaponClass.damageBurn, weaponClass.cooldownBurn, weaponClass.durationBurn);
                    }
                }
            }

            if (debug)
                Debug.DrawRay(firePoint.position, firePoint.right * weaponClass.range, Color.yellow);
        }

        player.ForceBackFire(firePoint.transform.position, weaponClass.forceBackFire);                          // даём отдачу оружия
    }



    void FireBoxCast()      // (пока не использую)
    {
        // Разброс
        float randomBulletX = Random.Range(-weaponClass.recoil, weaponClass.recoil);

        // Рейкаст2Д
        //RaycastHit2D hit = Physics2D.Raycast(firePoint.position, firePoint.right + new Vector3(randomBulletX, 0, 0), weaponClass.range, weaponClass.layerRayCast);

        RaycastHit2D hit = Physics2D.BoxCast(firePoint.position, new Vector2(1f, 1f), 0f, firePoint.right + new Vector3(randomBulletX, 0, 0), weaponClass.range, weaponClass.layerRayCast);

        if (hit.collider != null)
        {
            //Debug.Log("Hit!");
            if (hit.collider.TryGetComponent<Fighter>(out Fighter fighter))
            {
                Vector2 vec2 = (fighter.transform.position - player.transform.position).normalized;
                fighter.TakeDamage(weaponClass.damage, vec2, weaponClass.pushForce);
            }

            // Настройки для трасеров
            if (tracerEffect)
            {
                TrailRenderer tracer = Instantiate(tracerEffect, firePoint.position, Quaternion.identity);          // создаем трасер
                tracer.AddPosition(firePoint.position);                                                             // начальная позиция
                //tracer.transform.SetParent(transform, true); 
                tracer.transform.position = hit.point;                      // конечная позиция трасера рейкаста
            }


            if (weaponClass.ignite)
            {
                if (hit.collider.TryGetComponent<Ignitable>(out Ignitable ignitable))
                {
                    //Vector2 vec2 = (fighter.transform.position - player.transform.position).normalized;
                    ignitable.Ignite(weaponClass.damageBurn, weaponClass.cooldownBurn, weaponClass.durationBurn);
                }
            }

            //BoxCast(Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance = Mathf.Infinity, int layerMask = Physics2D.AllLayers, float minDepth = -Mathf.Infinity, float maxDepth = Mathf.Infinity);
        }
    }

    void FireBoxCastAll()
    {
        // Разброс
        float randomBulletX = Random.Range(-weaponClass.recoil, weaponClass.recoil);

        // Рейкаст2Д        
        RaycastHit2D[] hits = Physics2D.BoxCastAll(firePoint.position, new Vector2(weaponClass.boxSize, weaponClass.boxSize), 0f, firePoint.right + new Vector3(randomBulletX, 0, 0), weaponClass.range, weaponClass.layerRayCast);
        if (hits != null)                       // если во что-то попали
        {
            foreach (RaycastHit2D hit in hits)
            {
                bool stop = false;
                if (hit.collider.tag == "Wall")
                {
                    if (tracerEffect)
                    {
                        TrailRenderer tracer = Instantiate(tracerEffect, firePoint.position, Quaternion.identity);          // создаем трасер
                        tracer.AddPosition(firePoint.position);                                                             // начальная позиция
                        //tracer.transform.SetParent(transform, true); 
                        tracer.transform.position = hit.point;            // конечная позиция трасера рейкаста
                    }
                    stop = true;
                }
                if (stop)
                    break;

                //Debug.Log("Hit!");
                if (hit.collider.TryGetComponent<Fighter>(out Fighter fighter))
                {
                    Vector2 vec2 = (fighter.transform.position - player.transform.position).normalized;
                    fighter.TakeDamage(weaponClass.damage, vec2, weaponClass.pushForce);
                }

                if (weaponClass.ignite)
                {
                    if (hit.collider.TryGetComponent<Ignitable>(out Ignitable ignitable))
                    {
                        //Vector2 vec2 = (fighter.transform.position - player.transform.position).normalized;
                        ignitable.Ignite(weaponClass.damageBurn, weaponClass.cooldownBurn, weaponClass.durationBurn);
                    }
                }
                hits = null;

                /*                if (tracerEffect)
                                {
                                    TrailRenderer tracer = Instantiate(tracerEffect, firePoint.position, Quaternion.identity);          // создаем трасер
                                    tracer.AddPosition(firePoint.position);                                                             // начальная позиция
                                    //tracer.transform.SetParent(transform, true); 
                                    tracer.transform.position = tracer.transform.position + firePoint.right * 20;                      // конечная позиция трасера рейкаста
                                }*/
            }

            if (debug)
            {
                //Debug.Log("Ok!");
                Debug.DrawRay(firePoint.position, firePoint.right * weaponClass.range, Color.yellow);
            }
        }

        else                // если ни во что не попали
        {
            // Настройки для трасеров
            if (tracerEffect)
            {
                TrailRenderer tracer = Instantiate(tracerEffect, firePoint.position, Quaternion.identity);          // создаем трасер
                tracer.AddPosition(firePoint.position);                                                             // начальная позиция
                //tracer.transform.SetParent(transform, true); 
                tracer.transform.position = tracer.transform.position + firePoint.right * 20;                      // конечная позиция трасера рейкаста
            }
        }

        player.ForceBackFire(firePoint.transform.position, weaponClass.forceBackFire);                          // даём отдачу оружия
    }
}