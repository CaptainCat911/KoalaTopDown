using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    Player player;
    //WeaponHolder weaponHolder;                          // ссылка на скрипт weaponHolder (для удара)
    WeaponHolderMelee weaponHolderMelee;
    Animator animator;
    public LayerMask layer;                             // слои для битья

    public string weaponName;
    public bool swoard;
    public bool spear;
    public Transform hitBox;                            // положение хитбокса
    public int damage = 10;                             // урон
    public float pushForce = 1;                         // сила толчка
    public float radius = 1;                            // радиус
    public float cooldown = 1f;                         // перезардяка атаки
    float lastAttack;                                   // время последнего удара (для перезарядки удара)

    // Треил 
    public TrailRenderer trail;

    void Start()
    {
        player = GameManager.instance.player;
        //weaponHolder = GameManager.instance.player.weaponHolder;            // находим скрипт weaponHolder
        weaponHolderMelee = GameManager.instance.player.weaponHolderMelee;      // находим скрипт weaponHolder
        animator = GetComponentInParent<Animator>();        
    }

    
    void Update()
    {
        if (weaponHolderMelee.attackHitBoxStart && Time.time - lastAttack > cooldown)          // если готовы атаковать и кд готово
        {
            //Debug.Log("Attack!");
            lastAttack = Time.time;                             // присваиваем время атаки
            if (swoard)
                animator.SetTrigger("HitSwoard");
            if (spear)
                animator.SetTrigger("HitSpear");
        }
    }

    public void MeleeAttack()
    {
        Collider2D[] collidersHits = Physics2D.OverlapCircleAll(hitBox.position, radius, layer);     // создаем круг в позиции объекта с радиусом
        foreach (Collider2D coll in collidersHits)
        {
            if (coll == null)
            {
                continue;
            }

            if (coll.gameObject.TryGetComponent<Fighter>(out Fighter fighter))
            {
                Vector2 vec2 = (coll.transform.position - player.transform.position).normalized;
                fighter.TakeDamage(damage, vec2, pushForce);                
            }
            collidersHits = null;
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
