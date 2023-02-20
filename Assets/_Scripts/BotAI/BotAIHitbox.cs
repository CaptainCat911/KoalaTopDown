using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotAIHitbox : MonoBehaviour
{    
    BotAI botAI;
    BotAIHitBoxPivot pivot;

    [Header("Параметры атаки")]
    [HideInInspector] public LayerMask layer;
    [HideInInspector] public float lastAttack;              // время последнего удара (для перезарядки удара)
    public float hitBoxRadius;                              // радиус атаки                                                               
    public float cooldown = 0.5f;                           // перезардяка атаки
    public int damage = 1;                                  // урон
    public float pushForce = 10;                            // сила толчка
    // Если ренж
    //public bool isRange;                                    // ренж атака
    public GameObject bulletPrefab;
    public GameObject effectRangeAttack;                    // эффект ренж атаки
    public float bulletSpeed = 10;                          // скорость снаряда
    public float forceBackFire = 10;                        // отдача

/*    // Для флипа оружия
    bool needFlip;                          // нужен флип (для правильного отображения оружия)    
    bool leftFlip;                          // оружие слева
    bool rightFlip = true;                  // оружие справа*/



    private void Awake()
    {
        botAI = GetComponentInParent<BotAI>();
        pivot = GetComponentInParent<BotAIHitBoxPivot>();
    }

    void Start()
    {
        layer = LayerMask.GetMask("Player", "ObjectsDestroyble", "Default", "NPC");
    }

    
    void Update()
    {
        //Debug.Log(enemy.lastAttack);

        // Атака
        if (botAI.closeToTarget && Time.time - lastAttack > cooldown)               // если готовы атаковать и кд готово
        {
            lastAttack = Time.time;                                                 // присваиваем время атаки
            botAI.animator.SetTrigger("Attack");                                    // начинаем анимацию атаки
        }

/*        // Флип оружия
        if (Mathf.Abs(enemy.aimAnglePivot) > 90 && rightFlip)
        {
            needFlip = true;
            leftFlip = true;
            rightFlip = false;
        }
        if (Mathf.Abs(enemy.aimAnglePivot) <= 90 && leftFlip)
        {
            needFlip = true;
            rightFlip = true;
            leftFlip = false;
        }
        if (needFlip)
        {
            Flip();
        }*/
    }

    public void Attack()
    {
        if (botAI.meleeAttackType)
        {
            MeleeAttack();                                                      // мили атака
        }
        else if (botAI.rangeAttackType)
        {
            RangeAttack();                                                      // ренж атака
        }        
    }

    void MeleeAttack()
    {
        Collider2D[] collidersHitbox = Physics2D.OverlapCircleAll(transform.position, hitBoxRadius, layer);    // создаем круг в позиции объекта с радиусом
        foreach (Collider2D enObjectBox in collidersHitbox)
        {
            if (enObjectBox == null)
            {
                continue;
            }

            if (enObjectBox.gameObject.TryGetComponent<Fighter>(out Fighter fighter))               // ищем скрипт файтер
            {
                Vector2 vec2 = (fighter.transform.position - transform.position).normalized;        // вычисляем вектор направления удара
                fighter.TakeDamage(damage, vec2, pushForce);                                        // наносим урон                                                                                                                                  
            }
            collidersHitbox = null;                                                                 // сбрасываем все найденные объекты (на самом деле непонятно как это работает)
        }
    }


    public void EffectRangeAttack()
    {
        if (effectRangeAttack && botAI.rangeAttackType)
        {
            GameObject effect = Instantiate(effectRangeAttack, pivot.transform.position + new Vector3(0, 0.1f, 0), Quaternion.identity);                  // создаем эффект выстрела (если он есть)
            Destroy(effect, 1);
        }
    }

    void RangeAttack()
    {        
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);              // создаем префаб снаряда с позицией и поворотом якоря
        bullet.GetComponent<Bullet>().damage = damage;                                                      // присваиваем урон снаряду
        bullet.GetComponent<Bullet>().pushForce = pushForce;                                                // присваиваем урон снаряду
        bullet.GetComponent<Rigidbody2D>().AddForce(transform.right * bulletSpeed, ForceMode2D.Impulse);    // даём импульс
        //bullet.transform.Rotate(0.0f, 0.0f, -90.0f, Space.Self);                                          // поворачиваем снаряд
        botAI.ForceBackFire(transform.position, forceBackFire);                                             // даём отдачу         
    }

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

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;        
        Gizmos.DrawWireSphere(transform.position, hitBoxRadius);
    }
}
