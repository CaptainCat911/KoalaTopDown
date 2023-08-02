using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletRifle : Bullet
{

    public LayerMask layerRayCastTarget;            // для щита
                        
    [Header("Параметры поджога")]
    public bool ignite;                         
    public int damageBurn;
    public float cooldownBurn;
    public float durationBurn;

    [Header("Параметры взрыва")]
    //public float expRadius = 3;         // радиус взрыва
    public float timeToExpl = 1f;       // время до взрыва (таймер)


    private void Start()
    {
        Invoke("Explosion", timeToExpl);                   // взрыв после времени
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {        
        base.OnTriggerEnter2D(collision);           // там пусто пока что

        if (collision.gameObject.TryGetComponent<Fighter>(out Fighter fighter))
        {
            Vector2 vec2 = (collision.transform.position - GameManager.instance.player.transform.position).normalized;
            fighter.TakeDamage(0, vec2, pushForce);

            float distance = Vector2.Distance(transform.position, fighter.transform.position);

            if (RayCastToTarget(transform, fighter.transform, distance))
            {
                fighter.TakeDamage(damage, new Vector2(0, 0), 0);
            }
            enemyDamaged++;

            if (ignite)
            {
                if (collision.gameObject.TryGetComponent<Ignitable>(out Ignitable ignitable))
                {
                    //Vector2 vec2 = (fighter.transform.position - player.transform.position).normalized;
                    ignitable.Ignite(damageBurn, cooldownBurn, durationBurn);
                }
            }
        }



        if (collision.TryGetComponent<Shield>(out Shield shield))
        {
            shield.TakeDamage();            
        }

        if (enemyDamaged >= enemyToDamageCount || collision.tag == "Wall")      // если пробили врагов или попали в стену
            Explosion();

        if (sparksEffect)
        {
            GameObject effect = Instantiate(sparksEffect, transform.position, Quaternion.identity);                                      // создаем эффект убийства
            Destroy(effect, 1);
        }
    }





    public void FindNewTarget()
    {
        Collider2D[] collidersHits = Physics2D.OverlapCircleAll(transform.position, 5, layerExplousion);     // создаем круг в позиции объекта с радиусом
        foreach (Collider2D coll in collidersHits)
        {
            if (coll == null)
            {
                continue;
            }

            if (coll.gameObject.TryGetComponent<Fighter>(out Fighter fighter))
            {
                Vector2 vec2 = (coll.transform.position - transform.position).normalized;
                rb.AddForce(vec2 * 15, ForceMode2D.Impulse);    // даём импульс
            }
            collidersHits = null;
        }
    }




    // Для щита игрока (временно тут)
    public bool RayCastToTarget(Transform fromTrans, Transform toTrans, float distance)
    {
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
}
