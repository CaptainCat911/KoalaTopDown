using UnityEngine;

public class BotAIWeaponRange : MonoBehaviour
{
    [Header("Ссылки")]                      // почему-то не отображается
    BotAI botAI;
    BotAIRangeWeaponHolder weaponHolderRange;   // ссылка на скрипт weaponHolder (для стрельбы)
    public WeaponClass weaponClass;         // ссылка на класс оружия
    public Transform firePoint;             // якорь для снарядов
    public Transform pivot;                 // якорь weaponHolder (используется для прицеливания)

    [Header("Параметры оружия")]
    bool rayCastWeapon;                     // рейкаст оружие
    [HideInInspector] public string weaponName;     // название оружия
    GameObject bulletPrefab;                // префаб снаряда
    float bulletSpeed;                      // скорость снаряда
    int damage;                             // урон (возможно нужно сделать на снаряде)
    float pushForce;                        // сила толчка (возможно нужно сделать на снаряде)
    [HideInInspector] public float fireRate;                // скорострельность оружия (10 - 0,1 выстрелов в секунду)
    [HideInInspector] public float nextTimeToFire;          // для стрельбы (когда стрелять в след раз)
    float forceBackFire;                    // отдача оружия
    float recoilX;                          // разброс стрельбы
    LayerMask layerRayCast;                 // слои для рейкастов

/*    // Для флипа оружия
    bool needFlip;                          // нужен флип (для правильного отображения оружия)    
    bool leftFlip;                          // оружие слева
    bool rightFlip = true;                  // оружие справа
*/
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

    void Awake()
    {
        
        //weaponHolderGO = GetComponentInParent<WeaponHolder>().gameObject;       // находим объект weaponHolder
        weaponName = weaponClass.weaponName;                                    // имя оружия
        //rayCastWeapon = weaponClass.rayCastWeapon;                              // рейкаст оружие
        layerRayCast = weaponClass.layerRayCast;                                       // слои к рейкастам
        if (weaponClass.bulletPrefab)
            bulletPrefab = weaponClass.bulletPrefab;                                  // тип снаряда (если не рейкаст оружие)
        bulletSpeed = weaponClass.bulletSpeed;                                  // скорость
        damage = weaponClass.damage;                                            // урон
        pushForce = weaponClass.pushForce;                                      // сила толчка
        fireRate = weaponClass.fireRate;                                        // скорострельность
        forceBackFire = weaponClass.forceBackFire;                              // сила отдачи
        recoilX = weaponClass.recoil;                                           // разброс стрельбы
        //flashEffect = weaponClass.flashEffect;                                  // эффект вспышки при выстреле (флэш) 
    }

    private void Start()
    {
        botAI = GetComponentInParent<BotAI>();
        weaponHolderRange = GetComponentInParent<BotAIRangeWeaponHolder>();                    // находим скрипт weaponHolder      
    }

    private void Update()
    {
/*        // Флип оружия
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
        }*/

        // Стирать рендер лазера (возможно стоит переделать)
        if (!singleFlash && Time.time >= nextTimeToFire + 0.1f)
        {
            flashEffectAnimator.SetBool("Fire", false);
            flashActive = false;
            if (lineRaycast)
                lineRaycast.enabled = false;
        }
    }



    private void FixedUpdate()
    {
        // Стрельба
        if (!weaponHolderRange.fireStart)                        // если не готовы стрелять
        {
            return;                                         // выходим
        }

        if (Time.time >= nextTimeToFire)                    // если начинаем стрелять и кд готово
        {
            nextTimeToFire = Time.time + 1f / fireRate;                                     // вычисляем кд
            if (!rayCastWeapon)
                FireProjectile();                                                           // выстрел пулей
            if (rayCastWeapon)
                FireRayCast();                                                              // выстрел рейкастом
            CMCameraShake.Instance.ShakeCamera(cameraAmplitudeShake, cameraTimedeShake);    // тряска камеры
            if (flashEffectAnimator != null)                                                // если флэшэффект есть
                Flash();
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
/*    void Flip()
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
    }*/


    public void FireProjectile()
    {
        float randomBulletX = Random.Range(-recoilX, recoilX);                                              // разброс стрельбы
        firePoint.Rotate(0, 0, randomBulletX);                                                              // тупо вращаем
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);              // создаем префаб снаряда с позицией и поворотом якоря
        bullet.GetComponent<Bullet>().damage = damage;                                                      // присваиваем урон снаряду
        bullet.GetComponent<Bullet>().pushForce = pushForce;                                                // присваиваем силу толчка снаряду
        bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.right * bulletSpeed, ForceMode2D.Impulse);    // даём импульс        
        botAI.ForceBackFire(firePoint.transform.position, forceBackFire);                                   // даём отдачу оружия
        firePoint.Rotate(0, 0, -randomBulletX);                                                             // и тупо возвращаем поворот
        if (botAI.isFriendly)
        {
            bullet.layer = LayerMask.NameToLayer("BulletPlayer");           // слой пули
            bullet.GetComponent<Bullet>().layerExplousion = LayerMask.GetMask("Enemy", "ObjectsDestroyble", "Default");
        }
    }

    public void FireRayCast()
    {
        // Настройки для трасеров
        TrailRenderer tracer = Instantiate(tracerEffect, firePoint.position, Quaternion.identity);          // создаем трасер
        tracer.AddPosition(firePoint.position);                                                             // начальная позиция
        //tracer.transform.SetParent(transform, true); 

        // Разброс
        float randomBulletX = Random.Range(-recoilX, recoilX);

        // Рейкаст2Д
        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, firePoint.right + new Vector3(randomBulletX, 0, 0), Mathf.Infinity, layerRayCast);
        if (hit.collider != null)
        {
            //Debug.Log("Hit!");
            if (hit.collider.TryGetComponent<Fighter>(out Fighter fighter))
            {
                Vector2 vec2 = (fighter.transform.position - botAI.transform.position).normalized;
                fighter.TakeDamage(damage, vec2, pushForce);
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
