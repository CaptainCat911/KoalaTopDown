using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeNew : MonoBehaviour
{
    Animator animator;      
    public int damage;                      // урон
    public float pushForce;                 // отталкивание
    public float timeToActive;              // через сколько сработают
    public Animator[] animators;

    [HideInInspector] public bool isSpikeActive;        // активны сейчас или нет (не использую, но есть в ивентах аниматора)

    public bool isSpikeWork;                // шипы работают постоянно
    public float cooldown = 3f;             // перезардяка атаки
    float lastAttack;                       // время последнего удара (для перезарядки удара)
    public LayerMask layerHit;              // слой 


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }


    private void Update()
    {
        if (isSpikeWork && Time.time - lastAttack > cooldown)
        {
            lastAttack = Time.time;                         // присваиваем время атаки
            ActiveSpike();
        }
    }

/*    public void SpikeActiveEvent()                          // ивент запускает инвок
    {
        if (isSpikeWork)
            return;
        Invoke("SpikeActivate", timeToActive);
    }

    void SpikeActivate()
    {
        isSpikeActive = true;
        animator.SetTrigger("SpikeHit");
        DamageAll();
    }*/


    private void OnTriggerEnter2D(Collider2D collision)
    {
/*        if (collision.gameObject.TryGetComponent<Player>(out Player player))
        {            
            if (!isSpikeActive)
            {
                isSpikeActive = true;
                Invoke("ActiveSpike", timeToActive);                
            }
        }*/
    }

    void DamageAll()
    {
        Collider2D[] collidersHits = Physics2D.OverlapBoxAll(new Vector2(transform.position.x, transform.position.y + 0.15f), new Vector2 (1.5f, 1.25f), 0f, layerHit);       // создаем квадрат в позиции объекта с радиусом
        foreach (Collider2D coll in collidersHits)
        {
            if (coll == null)
            {
                continue;
            }

            if (coll.gameObject.TryGetComponent<Fighter>(out Fighter fighter))
            {
                Vector2 vec2 = (fighter.transform.position - transform.position).normalized;
                fighter.TakeDamage(damage, vec2, pushForce);
                /*                
                                fighter.rb2D.AddForce(vec2 * pushForce, ForceMode2D.Impulse);*/
            }
            collidersHits = null;
        }
    }

    void ActiveSpike()
    {
        animator.SetTrigger("SpikeHit");
        foreach (Animator animator in animators)
        {
            animator.SetTrigger("SpikeHit");
        }
    }

    void DisActiveSpike()
    {
        isSpikeActive = false;
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(new Vector2(transform.position.x, transform.position.y + 0.15f), new Vector3(1.5f, 1.25f, 0f));
    }
}
