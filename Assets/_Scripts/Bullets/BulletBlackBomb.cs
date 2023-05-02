using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BulletBlackBomb : Bullet
{
    [Header("Параметры взрыва")]
    public float expRadius = 3;         // радиус взрыва
    public float timeToExpl = 1f;       // время до взрыва (таймер)
    public float slow = 2f;             // замедление

    [Header("С огнём")]
    public bool withFire;
    public int damageBurn = 8;
    public float cooldownBurn = 0.5f;
    public float durationBurn = 5f;

    [Header("Создать что-то")]
    public bool withSpawn;
    public GameObject[] goToSpawn;      // массив префабов с объектами для спауна

    [Header("Тряска камеры при выстреле")]
    public float cameraAmplitudeShake = 3f;     // амплитуда
    public float cameraTimedeShake = 0.1f;      // длительность


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
                fighter.TakeDamage(damage, vec2, pushForce);                                // урон

/*                if (coll.gameObject.TryGetComponent(out BotAI botAI))                       // для замедления (потом переделать)
                {
                    botAI.SlowSpeed(slow);
                }*/

                if (withFire)
                {
                    if (fighter.TryGetComponent<Ignitable>(out Ignitable ignitable))
                    {                        
                        ignitable.Ignite(damageBurn, cooldownBurn, durationBurn);
                    }
                }
            }
            collidersHits = null;
        }

        if (withSpawn)
        {
            int ndx = Random.Range(0, goToSpawn.Length);                            // выбираем рандом из массива врагов
            GameObject enemyPref = Instantiate(goToSpawn[ndx], transform.position, Quaternion.identity);                     // создаём префаб            
            NavMeshAgent agent = enemyPref.GetComponent<NavMeshAgent>();            // находим НавМешАгент            
            agent.Warp(transform.position);                                         // перемещаем префаб к спавнеру
            //enemyPref.GetComponentInChildren<BotAI>().triggerLenght = chaseDistance;    // устанавливаем дистанцию триггера
        }

        CMCameraShake.Instance.ShakeCamera(cameraAmplitudeShake, cameraTimedeShake);            // тряска камеры
        base.Explosion();                                       // создаёт эффект и уничтожает его и объект
    }




    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;        
        Gizmos.DrawWireSphere(transform.position, expRadius);
    }
}
