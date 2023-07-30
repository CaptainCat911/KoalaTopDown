using UnityEngine;
using UnityEngine.AI;
public class BotAIWeaponMelee : MonoBehaviour
{
    BotAI botAI;
    //BotAIMeleeWeaponHolder weaponHolderMelee;   // ссылка на скрипт weaponHolder (для стрельбы)
    Animator animator;    
    [HideInInspector] public LayerMask layerHit;    // слои для битья (берем из ботАИ)    

    public string weaponName;
    public Transform hitBox;
    //public string attackClass;                      // тип атаки оружия (1 - мили, 2 - ренж, 3 - призыв)
    public float cooldown = 1f;                     // перезардяка атаки
    float lastAttack;                               // время последнего удара (для перезарядки удара)

    [Header("Параметры мили атаки")]
    public int damage = 10;                         // урон
    public float pushForce = 1;                     // сила толчка
    public float radius = 1;                        // радиус
    public TrailRenderer trail;
    public bool hitShield;                          // Пробивает щит

    [Header("Параметры ренж атаки (слабая)")]
    public GameObject bulletPrefab;                 // префаб снаряда
    public int damageRange;                         // урон ренж
    public float pushForceRange;                    // сила толчка ренж
    public float projctleSpeed;                     // скорость снаряда    

    [Header("Параметры ренж атаки (сильная)")]
    public GameObject bulletPrefabBig;              // префаб снаряда
    public int damageRangeBig;                      // урон ренж
    public float pushForceRangeBig;                 // сила толчка ренж
    public float projctleSpeedBig;                  // скорость снаряда
    public bool withSplit;                          // с разлётом на мелкие снаряды

    [Header("Параметры взрыва")]
    public int explousionDamage;                    // урон взрыва
    public float explousionForce;                   // сила толчка взрыва
    public float explousionRadius;                  // радиус взрыва
    //public GameObject explousionEffect;             // эффект взрыва


    [Header("Параметры призыва")]
    public EnemySpawner[] enemySpawners;            // массив префабов со спавнерами
    public int spawnTimes;                          // сколько призывать за раз
    //public GameObject[] prefabEnemies;            // массив префабов со скелетами
    //NavMeshAgent agent;                           // их агент

    [Header("Параметры мультиатаки")]
    public int splitTimes;                          // урон взрыва
    public float splitRecoil;                       // сила толчка взрыва    

    [Header("Параметры лазера")]
    public int laserDamage;                         // урон взрыва
    public float laserForce;                        // сила толчка взрыва 
    [HideInInspector] public bool laserStart;       // начало лазерной атаки
    public int damageBurn;
    public float cooldownBurn;
    public float durationBurn;
    public ParticleSystem laserEffectParticles;     // префаб системы частиц для лазера    

    [Header("Параметры гравитационной атаки")]
    public int gravityDamage;                       // урон 
    public float gravityForce;                      // сила притягивания 
    public float gravityRadius;                     // радиус гравитации
    public float gravityDistanceToDamage;           // дистанция, с которой будет наноситься урон
    public float gravityCooldown;                   // перезардяка гравитации
    float gravityLastAttack;                        // время последнего удара
    [HideInInspector] public bool gravityStart;     // начало гравитации
    public ParticleSystem gravityEffectParticles;   // префаб системы частиц для лазера
    public GameObject effectExplGravity;            // префаб эффекта взрыва гравитации

    [Header("Параметры телепорта")]
    public Transform[] teleports;
    public float teleportForceRadius;
    public float teleportInForce;
    public float teleportOutForce;

    [Header("Параметры возвращения во времени")]
    public int sceneNumberStuff;

    [Header("Для щита")]
    public LayerMask layerRayCastTarget;

    [Header("Звуки")]
    [HideInInspector] public AudioSource audioSource;
    public AudioWeaponMelee audioWeapon;
    

    //public bool demon;


    void Start()
    {
        botAI = GetComponentInParent<BotAI>();
        animator = GetComponentInParent<BotAIAnimator>().GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        //weaponHolderMelee = GetComponentInParent<BotAIMeleeWeaponHolder>();
        layerHit = botAI.layerHit;
    }


    void Update()
    {
        // Атака
/*        if (!weaponHolderMelee.fireStart)                       // если не готовы стрелять
        {
            return;                                             // выходим
        }*/


    }

    private void FixedUpdate()
    {
        if (laserStart)
        {
            LaserAttack();
        }
        if (gravityStart)
        {
            if (Time.time - gravityLastAttack > gravityCooldown)            // если готовы атаковать и кд готово
            {
                gravityLastAttack = Time.time;                             // присваиваем время атаки
                GravityAttack();
            }
        }
    }   

    public void Attack(int type)
    {
        if (Time.time - lastAttack > cooldown)      // если готовы атаковать и кд готово
        {
            lastAttack = Time.time;                 // присваиваем время атаки

            animator.SetFloat("HitType", type);     // тип атаки
            animator.SetTrigger("Hit");             // тригер для начала анимации
            if (audioWeapon && !botAI.newNpcSystem)
            {
                audioSource.clip = audioWeapon.hitStart;            // звук взмаха
                audioSource.Play();
            }
        }  
    }

    public void LaserOn(int number)
    {
        if (number == 1)
            laserStart = true;
        if (number == 0)
            laserStart = false;

        if (laserEffectParticles)
        {
            if (number == 1)
                laserEffectParticles.Play();
            if (number == 0)
                laserEffectParticles.Stop();
        }
    }    
    
    public void GravityOn(int number)
    {
        if (number == 1)
            gravityStart = true;
        if (number == 0)
            gravityStart = false;

        if (gravityEffectParticles)
        {
            if (number == 1)
                gravityEffectParticles.Play();
            if (number == 0)
                gravityEffectParticles.Stop();
        }
    }


    // Для щита игрока
    public bool RayCastToTarget(Transform fromTrans, Transform toTrans, float distance)
    {
        if (hitShield)
        {
            return true;
        }
        Vector2 vec2 = (toTrans.transform.position - fromTrans.transform.position).normalized;

        RaycastHit2D hit = Physics2D.Raycast(fromTrans.position, vec2, distance, layerRayCastTarget);

        if (hit.collider != null)
        {
            if (hit.collider.TryGetComponent<Shield>(out Shield shield))
            {
                shield.TakeDamage();
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            return true;
        }
        //Debug.DrawRay(fromTrans.position, vec2, Color.yellow);
    }

    public void MeleeAttack()
    {
        Collider2D[] collidersHits = Physics2D.OverlapCircleAll(hitBox.position, radius, layerHit);     // создаем круг в позиции объекта с радиусом
        foreach (Collider2D coll in collidersHits)
        {
            if (coll == null)
            {
                continue;
            }

            if (coll.gameObject.TryGetComponent<Fighter>(out Fighter fighter))
            {
                float distance = Vector2.Distance(botAI.transform.position, coll.transform.position);

                Vector2 vec2 = (coll.transform.position - botAI.transform.position).normalized;
                fighter.TakeDamage(0, vec2, pushForce);

                if (RayCastToTarget(botAI.transform, coll.transform, distance))     // проверка на щит
                {
                    fighter.TakeDamage(damage, new Vector2(0, 0), 0);
                }               

                if (audioWeapon)                    
                {
                    audioSource.clip = audioWeapon.hitDone;                 // звук попадания
                    audioSource.Play();                        
                }
            }
            collidersHits = null;
        }        
    }

    public void RangeAttack()
    {
        //float randomBulletX = Random.Range(-recoilX, recoilX);                                              // разброс стрельбы
        //firePoint.Rotate(0, 0, randomBulletX);                                                              // тупо вращаем
        GameObject bullet = Instantiate(bulletPrefab, hitBox.transform.position, hitBox.transform.rotation);              // создаем префаб снаряда с позицией и поворотом якоря
        bullet.GetComponent<Bullet>().damage = damageRange;                                                      // присваиваем урон снаряду
        bullet.GetComponent<Bullet>().pushForce = pushForceRange;                                                // присваиваем силу толчка снаряду
        bullet.GetComponent<Rigidbody2D>().AddForce(hitBox.transform.right * projctleSpeed, ForceMode2D.Impulse);              // даём импульс        
        //botAI.ForceBackFire(firePoint.transform.position, forceBackFire);                                   // даём отдачу оружия
        //firePoint.Rotate(0, 0, -randomBulletX);                                                             // и тупо возвращаем поворот
        if (botAI.isFriendly)
        {
            bullet.layer = LayerMask.NameToLayer("BulletPlayer");           // слой пули
            bullet.GetComponent<Bullet>().layerExplousion = LayerMask.GetMask("Enemy", "ObjectsDestroyble", "Default");
        }
        if (audioWeapon && !botAI.newNpcSystem)
        {
            audioSource.clip = audioWeapon.hitRange;                 // звук попадания
            audioSource.Play();
        }
    }

    public void RangeAttackBig()
    {
        //float randomBulletX = Random.Range(-recoilX, recoilX);                                              // разброс стрельбы
        //firePoint.Rotate(0, 0, randomBulletX);                                                              // тупо вращаем
        GameObject bullet = Instantiate(bulletPrefabBig, hitBox.transform.position, hitBox.transform.rotation);              // создаем префаб снаряда с позицией и поворотом якоря
        bullet.GetComponent<Bullet>().damage = damageRangeBig;                                                      // присваиваем урон снаряду
        bullet.GetComponent<Bullet>().pushForce = pushForceRangeBig;                                                // присваиваем силу толчка снаряду
        bullet.GetComponent<BulletRocket>().withSplit = withSplit;
        bullet.GetComponent<Rigidbody2D>().AddForce(hitBox.transform.right * projctleSpeedBig, ForceMode2D.Impulse);              // даём импульс        
        //botAI.ForceBackFire(firePoint.transform.position, forceBackFire);                                   // даём отдачу оружия
        //firePoint.Rotate(0, 0, -randomBulletX);                                                             // и тупо возвращаем поворот
        if (botAI.isFriendly)
        {
            bullet.layer = LayerMask.NameToLayer("BulletPlayer");           // слой пули
            bullet.GetComponent<Bullet>().layerExplousion = LayerMask.GetMask("Enemy", "ObjectsDestroyble", "Default");
        }
        if (audioWeapon)
        {
            audioSource.clip = audioWeapon.hitRangeBig;                 // звук попадания
            audioSource.Play();
        }
    }

    public void MultiRangeAttack()
    {
        for (int i = 0; i < splitTimes; i++)                            // кол-во снарядов (разделений)
        {
            //Debug.Log("Fire");

            // Вращаем
            if (i % 2 == 1)                                         // если делится без остатка (?)
                hitBox.Rotate(0, 0, (splitRecoil * (i + 1)));       // вращаем на сплитРекоил                                                           
            else
                hitBox.Rotate(0, 0, (-splitRecoil * (i)));

            /*            if (!raycast)
                            FireProjectile();
                        if (raycast)
                            FireRayCast();*/

            RangeAttack();

            // Возвращаем
            if (i % 2 == 1)
                hitBox.Rotate(0, 0, (-splitRecoil * (i + 1)));
            else
                hitBox.Rotate(0, 0, (splitRecoil * (i)));
        }

        if (audioWeapon)
        {
            audioSource.clip = audioWeapon.hitMultiRange;                 // звук попадания
            audioSource.Play();
        }
    }


    public void SpawnAttack()
    {
        if (enemySpawners.Length < 1)
            return;

        foreach(EnemySpawner enemySpawner in enemySpawners)
        {
            enemySpawner.enemysHowMuch += spawnTimes;
        }

        if (audioWeapon)
        {
            audioSource.clip = audioWeapon.hitSpawn;                 // звук попадания
            audioSource.Play();
        }

        /*        int ndx = Random.Range(0, prefabEnemies.Length);            // выбираем рандом из массива врагов
                GameObject go = Instantiate(prefabEnemies[ndx]);            // создаём префаб
                //go.transform.SetParent(transform, false);                   // назначаем этот спавнер родителем
                agent = go.GetComponentInChildren<NavMeshAgent>();                    // находим НавМешАгент
                agent.Warp(transform.position);                             // перемещаем префаб к спавнеру
                go.GetComponentInChildren<BotAI>().target = GameManager.instance.player.gameObject;*/
    }

    public void ExplousionAttack()
    {
        Collider2D[] collidersHits = Physics2D.OverlapCircleAll(hitBox.position, explousionRadius, layerHit);     // создаем круг в позиции объекта с радиусом
        foreach (Collider2D coll in collidersHits)
        {
            if (coll == null)
            {
                continue;
            }

            if (coll.gameObject.TryGetComponent<Fighter>(out Fighter fighter))
            {
                float distance = Vector2.Distance(botAI.transform.position, coll.transform.position);

                Vector2 vec2 = (coll.transform.position - botAI.transform.position).normalized;
                fighter.TakeDamage(0, vec2, explousionForce);

                if (RayCastToTarget(botAI.transform, coll.transform, distance))
                {
                    fighter.TakeDamage(explousionDamage, new Vector2(0, 0), 0);
                }
            }
            collidersHits = null;
        }
        GameObject effect = Instantiate(GameAssets.instance.explousionRedEffect, hitBox.position, Quaternion.identity);     // создаем эффект убийства
        Destroy(effect, 1);                                                             // уничтожаем эффект через .. сек
    }


    public void LaserAttack()
    {
        // Рейкаст2Д        
        RaycastHit2D[] hits = Physics2D.BoxCastAll(hitBox.position, new Vector2(0.8f, 0.8f), 0f, hitBox.right, 14, layerHit);
        if (hits != null)
        {
            foreach (RaycastHit2D hit in hits)
            {
                //Debug.Log("Hit!");
                if (hit.collider.TryGetComponent<Fighter>(out Fighter fighter))
                {
                    float distance = Vector2.Distance(hitBox.transform.position, fighter.transform.position);

                    Vector2 vec2 = (fighter.transform.position - hitBox.transform.position).normalized;
                    fighter.TakeDamage(0, vec2, laserForce);

                    if (RayCastToTarget(hitBox.transform, fighter.transform, distance))
                    {
                        fighter.TakeDamage(laserDamage, new Vector2(0, 0), 0);

                        if (hit.collider.TryGetComponent<Ignitable>(out Ignitable ignitable))
                        {
                            //Vector2 vec2 = (fighter.transform.position - player.transform.position).normalized;
                            ignitable.Ignite(damageBurn, cooldownBurn, durationBurn);
                        }
                    }
                }               
            }
        }
    }

    public void GravityAttack()
    {
        Collider2D[] collidersHits = Physics2D.OverlapCircleAll(hitBox.position, gravityRadius, layerHit);     // создаем круг в позиции объекта с радиусом
        foreach (Collider2D coll in collidersHits)
        {
            if (coll == null)
            {
                continue;
            }

            if (coll.gameObject.TryGetComponent<Fighter>(out Fighter fighter))
            {
                float distance = Vector2.Distance(hitBox.transform.position, fighter.transform.position);
                Vector2 vec2 = (coll.transform.position - hitBox.transform.position).normalized;
                if (distance > gravityDistanceToDamage)
                    fighter.TakeDamage(0, -vec2, gravityForce);
                if (distance <= gravityDistanceToDamage)
                    fighter.TakeDamage(gravityDamage, -vec2, gravityForce);
            }
            collidersHits = null;
        }
        if (effectExplGravity)
        {
            GameObject effect = Instantiate(effectExplGravity, hitBox.position, Quaternion.identity);       // создаем эффект 
            Destroy(effect, 0.5f);          // уничтожаем эффект через .. сек
        }
        if (audioWeapon)
        {
            audioSource.clip = audioWeapon.hitGravity;                 // звук попадания
            audioSource.Play();
        }
    }

    public void Teleport()
    {
        //Debug.Log(teleports.Length);
        if (teleports.Length < 1)
            return;

        Transform point = teleports[Random.Range(0, teleports.Length)];
        float distance = Vector2.Distance(point.position, botAI.transform.position);
        if (distance > 5)
        {
            GameObject effect = Instantiate(GameAssets.instance.explousionTeleportIn, botAI.transform.position, Quaternion.identity);    // создаем эффект
            Destroy(effect, 0.5f);                          // уничтожаем эффект через .. сек
            // Создаём втягивание
            Collider2D[] collidersHitsIn = Physics2D.OverlapCircleAll(botAI.transform.position, teleportForceRadius, layerHit);     // создаем круг в позиции объекта с радиусом
            foreach (Collider2D coll in collidersHitsIn)
            {
                if (coll == null)
                {
                    continue;
                }

                if (coll.gameObject.TryGetComponent<Fighter>(out Fighter fighter))
                {
                    Vector2 vec2 = (coll.transform.position - botAI.transform.position).normalized;
                    fighter.TakeDamage(0, -vec2, teleportInForce);
                }
                collidersHitsIn = null;
            }

            botAI.agent.Warp(point.position);               // тут телепортируем босса

            GameObject effectExit = Instantiate(GameAssets.instance.explousionTeleportOut, botAI.transform.position, Quaternion.identity);    // создаем эффект
            Destroy(effectExit, 0.5f);                      // уничтожаем эффект через .. сек     
            // Создаём взрыв
            Collider2D[] collidersHitsOut = Physics2D.OverlapCircleAll(botAI.transform.position, teleportForceRadius, layerHit);     // создаем круг в позиции объекта с радиусом
            foreach (Collider2D coll in collidersHitsOut)
            {
                if (coll == null)
                {
                    continue;
                }

                if (coll.gameObject.TryGetComponent<Fighter>(out Fighter fighter))
                {
                    Vector2 vec2 = (coll.transform.position - botAI.transform.position).normalized;
                    fighter.TakeDamage(0, vec2, teleportOutForce);
                }
                collidersHitsOut = null;
            }
        }
        else
        {
            Teleport();
        }

        if (audioWeapon)
        {
            audioSource.clip = audioWeapon.hitTeleport;                 // звук попадания
            audioSource.Play();
        }
    }

    public void TimeReverceAttack()
    {
        GameManager.instance.NextScene(sceneNumberStuff);
    }


    public void TrailOn(int number)
    {
        if (trail)
        {
            if (number == 1)
                trail.emitting = true;
            if (number == 0)
                trail.emitting = false;
        }
    }



    

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(hitBox.position, radius);
    }
}
