using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Ссылки")]                      // почему-то не отображается
    Player player;
    AmmoPackStore[] ammoWeapons;
    public WeaponClass weaponClass;         // ссылка на класс оружия
    public Transform firePoint;             // якорь для снарядов
    public Transform pivot;                 // якорь weaponHolder (используется для прицеливания)
    WeaponHolder weaponHolder;              // ссылка на скрипт weaponHolder (для стрельбы)

    //GameObject weaponHolderGO;              // ссылка на объект weaponHolder (для поворота)
    //Vector3 mousePosition;                  // положение мыши

    [Header("Параметры оружия")]
    public int weaponIndexForAmmo;                  // индекс оружия (для патронов)
    bool projectileWeapon;                          // оружие со снарядами
    bool splitProjectileWeapon;                           // дробовики
    bool rayCastWeapon;                             // рейкаст оружие
    [HideInInspector] public string weaponName;     // название оружия
    [HideInInspector] public float fireRate;        // скорострельность оружия (10 - 0,1 выстрелов в секунду)
    [HideInInspector] public float nextTimeToFire;  // для стрельбы (когда стрелять в след раз)
    //public int ammo;                                       // патроны

/*    GameObject bulletPrefab;                // префаб снаряда
    float bulletSpeed;                      // скорость снаряда
    int damage;                             // урон (возможно нужно сделать на снаряде)
    float pushForce;                        // сила толчка (возможно нужно сделать на снаряде)
    float forceBackFire;                    // отдача оружия
    float recoil;                          // разброс стрельбы
    LayerMask layerRayCast;                 // слои для рейкастов*/

    // Для флипа оружия
    bool needFlip;                          // нужен флип (для правильного отображения оружия)    
    bool leftFlip;                          // оружие слева
    bool rightFlip = true;                  // оружие справа

    [Header("Эффекты")]
    public Animator flashEffectAnimator;    // флеш при стрельбе
    public bool singleFlash;                // одиночный флеш
    bool flashActive;                       // флеш активен (для мультифлеша)
    public LineRenderer lineRenderer;       // линия для лазера (префаб)
    LineRenderer lineRaycast;               // линия для лазера (создаём)
    public TrailRenderer tracerEffect;      // трасер (пока не используется)

    [Header("Тряска камеры при выстреле")]
    public float cameraAmplitudeShake = 1f; // амплитуда
    public float cameraTimedeShake = 0.1f;  // длительность

    [Header("Звук")]
    public bool singleShot;                 // одиночный звук
    public bool multiShot;                  // круговой звук
    bool soundStarted;                      // звук начался
    public AudioSource audioSource;         // основной звук
    public AudioSource audioSourceTail;     // "хвост"

    void Awake()
    {
        player = GameManager.instance.player;
        ammoWeapons = GameManager.instance.ammoPack.ammoWeapons;

        weaponName = weaponClass.weaponName;                                    // имя оружия
        projectileWeapon = weaponClass.projectileWeapon;                        // оружие снарядами
        splitProjectileWeapon = weaponClass.splitProjectileWeapon;                        // оружие снарядами
        rayCastWeapon = weaponClass.rayCastWeapon;                              // рейкаст оружие

        audioSource = GetComponent<AudioSource>();

        if (singleShot)
        {
            audioSource.loop = false;
        }
        if (multiShot)
        {
            audioSource.loop = true;
        }

        /*        layerRayCast = weaponClass.layerRayCast;                                       // слои к рейкастам
                if (weaponClass.bulletPrefab)
                    bulletPrefab = weaponClass.bulletPrefab;                                  // тип снаряда (если не рейкаст оружие)
                bulletSpeed = weaponClass.bulletSpeed;                                  // скорость
                damage = weaponClass.damage;                                            // урон
                pushForce = weaponClass.pushForce;                                      // сила толчка
                fireRate = weaponClass.fireRate;                                        // скорострельность
                forceBackFire = weaponClass.forceBackFire;                              // сила отдачи
                recoil = weaponClass.recoil;                                           // разброс стрельбы*/
        //flashEffect = weaponClass.flashEffect;                                  // эффект вспышки при выстреле (флэш) 
    }

    private void Start()
    {
        weaponHolder = GetComponentInParent<WeaponHolder>();                    // находим скрипт weaponHolder
        //ammo = GameManager.instance.ammoPack.ammoTompson;
    }

    private void Update()
    {
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
        
        if (!flashEffectAnimator)
        {
            return;
        }

        // Стирать рендер лазера (возможно стоит переделать)
        if (!singleFlash && Time.time >= nextTimeToFire + 0.1f)
        {
            flashEffectAnimator.SetBool("Fire", false);
            flashActive = false;
            if (lineRaycast)
                lineRaycast.enabled = false;
        }

        // Звук
        if (multiShot && ammoWeapons[weaponIndexForAmmo].allAmmo > 0)
        {
            if (weaponHolder.fireStart && !soundStarted)                        // если не готовы стрелять
            {                
                audioSource.Play();
                soundStarted = true;
            }
            if (!weaponHolder.fireStart && soundStarted)                        // если не готовы стрелять
            {                 
                audioSource.Stop();
                if (audioSourceTail)
                    audioSourceTail.Play();
                soundStarted = false;
            }
        }
    }


    private void FixedUpdate()
    {
        // Стрельба
        if (!weaponHolder.fireStart)        // если не готовы стрелять
        {
            return;                         // выходим
        }

        if (Time.time >= nextTimeToFire && ammoWeapons[weaponIndexForAmmo].allAmmo > 0)     // если начинаем стрелять и кд готово
        {
            ammoWeapons[weaponIndexForAmmo].allAmmo--;                      // - патроны
            nextTimeToFire = Time.time + 1f / weaponClass.fireRate;         // вычисляем кд           
            
            if (projectileWeapon)
                FireProjectile();       // выстрел пулей
            if (splitProjectileWeapon)
                FireSplit();            // выстрел "дробью"
            if (rayCastWeapon)
                FireRayCast();          // выстрел рейкастом

            CMCameraShake.Instance.ShakeCamera(cameraAmplitudeShake, cameraTimedeShake);    // тряска камеры

            // Флэш
            if (flashEffectAnimator != null)        // если флэшэффект есть
                Flash();

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
        bullet.GetComponent<Bullet>().damage = weaponClass.damage;                                              // присваиваем урон снаряду
        bullet.GetComponent<Bullet>().pushForce = weaponClass.pushForce;                                        // присваиваем силу толчка снаряду
        bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.right * weaponClass.bulletSpeed, ForceMode2D.Impulse);    // даём импульс
        player.ForceBackFire(firePoint.transform.position, weaponClass.forceBackFire);                          // даём отдачу оружия
        firePoint.Rotate(0, 0, -randomBulletX);                                                                 // и тупо возвращаем поворот
    }

    void FireSplit()
    {
        for (int i = 0; i < 7; i++)
        {
            Debug.Log("Fire");
            //firePoint.Rotate(0, 0, (1.5f * i));                                                                  // тупо вращаем
            FireProjectile();
            //firePoint.Rotate(0, 0, (-1.5f * i));                                                                  // тупо вращаем
        }
    }    

    void FireRayCast()
    {
        // Настройки для трасеров
        TrailRenderer tracer = Instantiate(tracerEffect, firePoint.position, Quaternion.identity);          // создаем трасер
        tracer.AddPosition(firePoint.position);                                                             // начальная позиция
        //tracer.transform.SetParent(transform, true); 

        // Разброс
        float randomBulletX = Random.Range(-weaponClass.recoil, weaponClass.recoil);
        
        // Рейкаст2Д
        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, firePoint.right + new Vector3(randomBulletX, 0, 0), Mathf.Infinity, weaponClass.layerRayCast);        
        if (hit.collider != null)
        {
            //Debug.Log("Hit!");
            if (hit.collider.TryGetComponent<Fighter>(out Fighter fighter))
            {
                Vector2 vec2 = (fighter.transform.position - player.transform.position).normalized;
                fighter.TakeDamage(weaponClass.damage, vec2, weaponClass.pushForce);                
            }

            tracer.transform.position = hit.point;                      // конечная позиция трасера рейкаста             

            /*            if (!lineRaycast)
                        {
                            lineRaycast = Instantiate(lineRenderer, firePoint.position, Quaternion.identity);
                        }
                        lineRaycast.enabled = true;
                        lineRaycast.SetPosition(0, firePoint.position);
                        lineRaycast.SetPosition(1, hit.point);*/

            //Debug.DrawRay(firePoint.position, firePoint.right * 100f, Color.yellow);
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
}
