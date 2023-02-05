using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBlackBomb : Bullet
{
    public float expRadius = 3;
    public float timeToExpl = 1f;
    public float force = 2f;

    private void Start()
    {
        Invoke("Explosion", timeToExpl);                   // взрыв после времени
    }
    public override void OnTriggerEnter2D(Collider2D collision)
    {
/*        if (collision.gameObject.TryGetComponent<Fighter>(out Fighter fighter))
        {            
            Vector2 vec2 = (collision.transform.position - GameManager.instance.player.transform.position).normalized;
            fighter.rb2D.AddForce(vec2 * pushForce, ForceMode2D.Impulse);
        }*/
        base.OnTriggerEnter2D(collision);           // там пусто пока что
        //Explosion();
    }

    public override void Explosion()
    {
        Collider2D[] collidersHits = Physics2D.OverlapCircleAll(transform.position, expRadius, layerExplousion);     // создаем круг в позиции объекта с радиусом
        foreach (Collider2D coll in collidersHits)
        {
            if (coll == null)
            {
                continue;
            }

            if (coll.gameObject.TryGetComponent<Fighter>(out Fighter fighter))
            {
                Vector2 vec2 = -(coll.transform.position - transform.position).normalized;  // направление внутрь
                fighter.TakeDamage(damage, vec2, pushForce);
                if (coll.gameObject.TryGetComponent(out BotAI botAI))                       // для замедления (потом переделать)
                {
                    botAI.SlowSpeed(force);
                }
            }
            collidersHits = null;
        }
        CMCameraShake.Instance.ShakeCamera(3, 0.1f);            // тряска камеры
        base.Explosion();                                       // создаёт эффект и уничтожает его и объект
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;        
        Gizmos.DrawWireSphere(transform.position, expRadius);
    }
}
