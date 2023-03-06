using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    public int damage;                              // урон
    public float pushForce;                         // сила толчка
    public float cooldown = 3f;                     // перезардяка атаки    
    float lastAttack;                               // время последнего удара (для перезарядки удара)    

    private void Update()
    {
        if (Time.time - lastAttack > cooldown)
        {
            DamageAll();
            lastAttack = Time.time;                 // присваиваем время атаки
        }
    }

    void DamageAll()
    {
        Collider2D[] collidersHits = Physics2D.OverlapBoxAll(transform.position, new Vector2(1, 1), 0f);     // создаем квадрат в позиции объекта с радиусом
        foreach (Collider2D coll in collidersHits)
        {
            if (coll == null)
            {
                continue;
            }

            if (coll.gameObject.TryGetComponent<Fighter>(out Fighter fighter))
            {
                fighter.TakeDamage(damage,new Vector2(0,0), 0);
                /*                Vector2 vec2 = (fighter.transform.position - transform.position).normalized;
                                fighter.rb2D.AddForce(vec2 * pushForce, ForceMode2D.Impulse);*/
            }
            collidersHits = null;
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(1, 1, 0f));
    }
}
