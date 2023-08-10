using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPlasma : Bullet
{

    public float expRadius = 1;
    GameObject targetToTeleport;        // для портальной пушки полиции

    private void Start()
    {
        //Invoke("Explosion", 1);                   // взрыв после времени
    }
    public override void OnTriggerEnter2D(Collider2D collision)
    {
/*        if (collision.gameObject.TryGetComponent<Fighter>(out Fighter fighter))
        {
            Vector2 vec2 = (collision.transform.position - transform.position).normalized;
            fighter.rb2D.AddForce(vec2 * pushForce, ForceMode2D.Impulse);
        }*/
        base.OnTriggerEnter2D(collision);           // там пусто пока что
        Explosion();            
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
                Vector2 vec2 = (coll.transform.position - transform.position).normalized;
                fighter.TakeDamage(damage, vec2, pushForce);  

                if (portalBullet)
                {
                    //Debug.Log("Expl!");
                    targetToTeleport = fighter.gameObject;
                    StartCoroutine(TeleportTarget());                    
                    //targetToTeleport.transform.position = GameObject.Find("SpawnPoint").transform.position;
                }
            }
            collidersHits = null;
        }
        CMCameraShake.Instance.ShakeCamera(1, 0.1f);            // тряска камеры
        base.Explosion();                                       // создаёт эффект и уничтожает его и объект
    }


    IEnumerator TeleportTarget()
    {
        //Debug.Log("Cor!");
        yield return new WaitForSeconds(0.1f);
        targetToTeleport.transform.position = GameObject.Find("SpawnPoint").transform.position;
    }  
    
    
    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, expRadius);
    }
}
