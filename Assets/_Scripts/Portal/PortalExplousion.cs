using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PortalExplousion : MonoBehaviour
{
    //Animator animator;      
    SpriteRenderer spriteRenderer;

    public int damage;                  // урон при появлении портала
    public float pushForce;             // пуш
    public float expRadius = 3;         // радиус
    public LayerMask layerExplousion;   // слои для битья
    public GameObject expEffect;        // эффект взрыва
    public BotAI targetTeleport;        // бот для портации
    public GameObject targetHome;       // домик мага
    public UnityEvent interactAction;   // ивент по завершении диалога

    private void Awake()
    {
        //animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void StartPortal()
    {
        //animator.SetTrigger("Start");
        Explosion();
        targetTeleport.startPosition = transform.position;
        targetTeleport.ResetTarget();
    }

    public void Explosion()
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
            }
            collidersHits = null;
        }

        CMCameraShake.Instance.ShakeCamera(3, 0.1f);    // тряска камеры

        GameObject effect = Instantiate(expEffect, transform.position, Quaternion.identity);    // создаем эффект
        Destroy(effect, 0.5f);                          // уничтожаем эффект через .. сек        
        targetTeleport.agent.Warp(transform.position);  // тут создаём (перемещаем) нпс мага
        spriteRenderer.enabled = false;                 // выключаем спрайт
        interactAction.Invoke();                        // ивент (для диалога)
        //Destroy(gameObject);                          // уничтожаем портал
    }


    public void ClosePortal()
    {
        GameObject effect = Instantiate(expEffect, targetTeleport.transform.position, Quaternion.identity);    // создаем эффект
        targetTeleport.agent.Warp(targetHome.transform.position);           // тут создаём (перемещаем) нпс мага
        targetTeleport.startPosition = targetHome.transform.position;       // меняем магу стартовую позицию
        targetTeleport.ResetTarget();                                       
        Destroy(effect, 0.5f);                                  // уничтожаем эффект через .. сек
        Destroy(gameObject, 0.5f);                              // уничтожаем портал через .. сек
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, expRadius);
    }
}
