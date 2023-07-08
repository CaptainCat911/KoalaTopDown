using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeNew : MonoBehaviour
{
    Animator animator;      
    public int damage;                      // ����
    public float pushForce;                 // ������������
    public float timeToActive;              // ����� ������� ���������
    public Animator[] animators;

    [HideInInspector] public bool isSpikeActive;        // ������� ������ ��� ��� (�� ���������, �� ���� � ������� ���������)

    public bool isSpikeWork;                // ���� �������� ���������
    public float cooldown = 3f;             // ����������� �����
    float lastAttack;                       // ����� ���������� ����� (��� ����������� �����)
    public LayerMask layerHit;              // ���� 


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }


    private void Update()
    {
        if (isSpikeWork && Time.time - lastAttack > cooldown)
        {
            lastAttack = Time.time;                         // ����������� ����� �����
            ActiveSpike();
        }
    }

/*    public void SpikeActiveEvent()                          // ����� ��������� �����
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
        Collider2D[] collidersHits = Physics2D.OverlapBoxAll(new Vector2(transform.position.x, transform.position.y + 0.15f), new Vector2 (1.5f, 1.25f), 0f, layerHit);       // ������� ������� � ������� ������� � ��������
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
