using UnityEngine;
using UnityEngine.AI;
public class BotAIWeaponMelee : MonoBehaviour
{
    BotAI botAI;
    BotAIMeleeWeaponHolder weaponHolderMelee;   // ссылка на скрипт weaponHolder (дл€ стрельбы)
    Animator animator;    

    public string weaponName;
    public Transform hitBox;
    public int damage = 10;                             // урон
    public float pushForce = 1;                         // сила толчка
    public float radius = 1;                            // радиус
    public float cooldown = 1f;                         // перезард€ка атаки
    float lastAttack;                                   // врем€ последнего удара (дл€ перезар€дки удара)
    [HideInInspector] public LayerMask layerHit;        // слои дл€ бить€ (берем из ботј»)

    public GameObject bulletPrefab;                     // префаб снар€да дл€ посоха
    public string attackClass;                          // тип атаки оружи€ (1 - мили, 2 - ренж, 3 - призыв)
    //public bool demon;
    public GameObject[] prefabEnemies;                  // массив префабов со скелетами
    NavMeshAgent agent;                                 // их агент

    [Header("ѕараметры взрыва")]    
    public int explousionDamage;
    public float explousionForce;
    public float explousionRadius;

    // “реил 
    public TrailRenderer trail;

    void Start()
    {
        botAI = GetComponentInParent<BotAI>();
        animator = GetComponentInParent<BotAIAnimator>().GetComponent<Animator>();
        weaponHolderMelee = GetComponentInParent<BotAIMeleeWeaponHolder>();
        layerHit = botAI.layerHit;
    }


    void Update()
    {
        // јтака
/*        if (!weaponHolderMelee.fireStart)                       // если не готовы стрел€ть
        {
            return;                                             // выходим
        }*/


    }

    public void Attack(string type)
    {
        if (Time.time - lastAttack > cooldown)                  // если готовы атаковать и кд готово
        {
            lastAttack = Time.time;                             // присваиваем врем€ атаки

            switch (type)
            {
                case "1":
                    {
                        animator.SetTrigger("HitMelee");
                    }
                    break;

                case "2":
                    {
                        animator.SetTrigger("HitRange");
                    }
                    break;

                case "3":
                    {
                        animator.SetTrigger("HitSpawn");
                    }
                    break;
                case "4":
                    {
                        animator.SetTrigger("HitExplousion");
                    }
                    break;
            }
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
        GameObject bullet = Instantiate(bulletPrefab, hitBox.transform.position, hitBox.transform.rotation);              // создаем префаб снар€да с позицией и поворотом €кор€
        bullet.GetComponent<Bullet>().damage = damage;                                                      // присваиваем урон снар€ду
        bullet.GetComponent<Bullet>().pushForce = pushForce;                                                // присваиваем силу толчка снар€ду
        bullet.GetComponent<Rigidbody2D>().AddForce(hitBox.transform.right * 4, ForceMode2D.Impulse);              // даЄм импульс        
        //botAI.ForceBackFire(firePoint.transform.position, forceBackFire);                                   // даЄм отдачу оружи€
        //firePoint.Rotate(0, 0, -randomBulletX);                                                             // и тупо возвращаем поворот
        if (botAI.isFriendly)
        {
            bullet.layer = LayerMask.NameToLayer("BulletPlayer");           // слой пули
            bullet.GetComponent<Bullet>().layerExplousion = LayerMask.GetMask("Enemy", "ObjectsDestroyble", "Default");
        }
    }

    public void SpawnAttack()
    {
        int ndx = Random.Range(0, prefabEnemies.Length);            // выбираем рандом из массива врагов
        GameObject go = Instantiate(prefabEnemies[ndx]);            // создаЄм префаб
        //go.transform.SetParent(transform, false);                   // назначаем этот спавнер родителем
        agent = go.GetComponentInChildren<NavMeshAgent>();                    // находим Ќавћешјгент
        agent.Warp(transform.position);                             // перемещаем префаб к спавнеру
        go.GetComponentInChildren<BotAI>().target = GameManager.instance.player.gameObject;
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
