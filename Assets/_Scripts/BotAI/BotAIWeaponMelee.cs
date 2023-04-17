using UnityEngine;
using UnityEngine.AI;
public class BotAIWeaponMelee : MonoBehaviour
{
    BotAI botAI;
    BotAIMeleeWeaponHolder weaponHolderMelee;   // ссылка на скрипт weaponHolder (для стрельбы)
    Animator animator;    

    public string weaponName;
    public Transform hitBox;
    public string attackClass;                          // тип атаки оружия (1 - мили, 2 - ренж, 3 - призыв)
    public float cooldown = 1f;                         // перезардяка атаки
    float lastAttack;                                   // время последнего удара (для перезарядки удара)
    [HideInInspector] public LayerMask layerHit;        // слои для битья (берем из ботАИ)    

    [Header("Параметры мили атаки")]
    public int damage = 10;                             // урон
    public float pushForce = 1;                         // сила толчка
    public float radius = 1;                            // радиус

    [Header("Параметры ренж атаки")]
    public GameObject bulletPrefab;                     // префаб снаряда для посоха
    public int damageRange;                             // урон ренж
    public float pushForceRange;                        // сила толчка ренж
    public float projctleSpeed = 4f;                    // скорость снаряда

    [Header("Параметры взрыва")]    
    public int explousionDamage;                        // урон взрыва
    public float explousionForce;                       // сила толчка взрыва
    public float explousionRadius;                      // радиус взрыва

    [Header("Параметры призыва")]
    public EnemySpawner[] enemySpawners;
    //public GameObject[] prefabEnemies;                  // массив префабов со скелетами
    //NavMeshAgent agent;                                 // их агент

    [Header("Параметры мультиатаки")]
    public int splitTimes;                      // урон взрыва
    public float splitRecoil;                   // сила толчка взрыва    

    [Header("Параметры лазера")]
    public int laserDamage;                        // урон взрыва
    public float laserForce;                       // сила толчка взрыва 
    [HideInInspector] public bool laserStart;



    //public bool demon;
    [Header("Визуальные эффекты")]
    // Треил 
    public TrailRenderer trail;
    public ParticleSystem effectParticles;      // префаб системы частиц пламени




    void Start()
    {
        botAI = GetComponentInParent<BotAI>();
        animator = GetComponentInParent<BotAIAnimator>().GetComponent<Animator>();
        weaponHolderMelee = GetComponentInParent<BotAIMeleeWeaponHolder>();
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
    }   

    public void Attack(int type)
    {
        if (Time.time - lastAttack > cooldown)                  // если готовы атаковать и кд готово
        {
            lastAttack = Time.time;                             // присваиваем время атаки

            animator.SetFloat("HitType", type);
            animator.SetTrigger("Hit");
        }  
    }

    public void LaserOn(int number)
    {
        if (number == 1)
            laserStart = true;
        if (number == 0)
            laserStart = false;

        if (effectParticles)
        {
            if (number == 1)
                effectParticles.Play();
            if (number == 0)
                effectParticles.Stop();
        }
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
                Vector2 vec2 = (coll.transform.position - botAI.transform.position).normalized;
                fighter.TakeDamage(damage, vec2, pushForce);                
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
    }


    public void SpawnAttack()
    {
        foreach(EnemySpawner enemySpawner in enemySpawners)
        {
            enemySpawner.enemysHowMuch++;
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
                Vector2 vec2 = (coll.transform.position - botAI.transform.position).normalized;
                fighter.TakeDamage(explousionDamage, vec2, explousionForce);
            }
            collidersHits = null;
        }
        GameObject effect = Instantiate(GameAssets.instance.explousionStaffEffect,
            hitBox.position, Quaternion.identity);                                      // создаем эффект убийства
        Destroy(effect, 1);                                                             // уничтожаем эффект через .. сек
    }


    public void LaserAttack()
    {
        // Рейкаст2Д        
        RaycastHit2D[] hits = Physics2D.BoxCastAll(hitBox.position, new Vector2(0.8f, 0.8f), 0f, hitBox.right, 15, layerHit);
        if (hits != null)
        {
            foreach (RaycastHit2D hit in hits)
            {
                //Debug.Log("Hit!");
                if (hit.collider.TryGetComponent<Fighter>(out Fighter fighter))
                {
                    Vector2 vec2 = (fighter.transform.position - transform.position).normalized;
                    fighter.TakeDamage(laserDamage, vec2, laserForce);
                }
            }
        }
    }

    public void TimeReverceAttack()
    {
        GameManager.instance.NextScene(0);
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
