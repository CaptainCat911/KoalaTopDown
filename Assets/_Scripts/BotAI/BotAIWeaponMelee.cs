using UnityEngine;

public class BotAIWeaponMelee : MonoBehaviour
{
    BotAI botAI;
    BotAIMeleeWeaponHolder weaponHolderMelee;   // ссылка на скрипт weaponHolder (для стрельбы)
    Animator animator;

    public string weaponName;
    public Transform hitBox;
    public int damage = 10;                             // урон
    public float pushForce = 1;                         // сила толчка
    public float radius = 1;                            // радиус
    public float cooldown = 1f;                         // перезардяка атаки
    float lastAttack;                                   // время последнего удара (для перезарядки удара)
    [HideInInspector] public LayerMask layerHit;        // слои для битья (берем из ботАИ)

    public GameObject bulletPrefab;
    //public bool demon;

    // Треил 
    public TrailRenderer trail;

    void Start()
    {
        botAI = GetComponentInParent<BotAI>();        
        animator = GetComponentInParent<Animator>();
        weaponHolderMelee = GetComponentInParent<BotAIMeleeWeaponHolder>();
        layerHit = botAI.layerHit;
    }


    void Update()
    {
        // Атака
        if (!weaponHolderMelee.fireStart)                        // если не готовы стрелять
        {
            return;                                         // выходим
        }

        if (Time.time - lastAttack > cooldown)          // если готовы атаковать и кд готово
        {
            //Debug.Log("Attack!");
            lastAttack = Time.time;                             // присваиваем время атаки
            animator.SetTrigger("Hit");
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
        bullet.GetComponent<Bullet>().damage = damage;                                                      // присваиваем урон снаряду
        bullet.GetComponent<Bullet>().pushForce = pushForce;                                                // присваиваем силу толчка снаряду
        bullet.GetComponent<Rigidbody2D>().AddForce(hitBox.transform.right * 5, ForceMode2D.Impulse);              // даём импульс        
        //botAI.ForceBackFire(firePoint.transform.position, forceBackFire);                                   // даём отдачу оружия
        //firePoint.Rotate(0, 0, -randomBulletX);                                                             // и тупо возвращаем поворот
        if (botAI.isFriendly)
        {
            bullet.layer = LayerMask.NameToLayer("BulletPlayer");           // слой пули
            bullet.GetComponent<Bullet>().layerExplousion = LayerMask.GetMask("Enemy", "ObjectsDestroyble", "Default");
        }
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
