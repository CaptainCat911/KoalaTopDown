using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BulletBlackBomb : Bullet
{
    [Header("��������� ������")]
    public float expRadius = 3;         // ������ ������
    public float timeToExpl = 1f;       // ����� �� ������ (������)
    public float slow = 2f;             // ����������

    [Header("� ����")]
    public bool withFire;
    public int damageBurn = 8;
    public float cooldownBurn = 0.5f;
    public float durationBurn = 5f;

    [Header("������� ���-��")]
    public bool withSpawn;
    public GameObject[] goToSpawn;      // ������ �������� � ��������� ��� ������

    [Header("������ ������ ��� ��������")]
    public float cameraAmplitudeShake = 3f;     // ���������
    public float cameraTimedeShake = 0.1f;      // ������������


    private void Start()
    {
        Invoke("Explosion", timeToExpl);                   // ����� ����� �������
    }
    public override void OnTriggerEnter2D(Collider2D collision)
    {
/*        if (collision.gameObject.TryGetComponent<Fighter>(out Fighter fighter))
        {            
            Vector2 vec2 = (collision.transform.position - GameManager.instance.player.transform.position).normalized;
            fighter.rb2D.AddForce(vec2 * pushForce, ForceMode2D.Impulse);
        }*/
        base.OnTriggerEnter2D(collision);           // ��� ����� ���� ���
        //Explosion();
    }

    public override void Explosion()
    {
        Collider2D[] collidersHits = Physics2D.OverlapCircleAll(transform.position, expRadius, layerExplousion);     // ������� ���� � ������� ������� � ��������
        foreach (Collider2D coll in collidersHits)
        {
            if (coll == null)
            {
                continue;
            }

            if (coll.gameObject.TryGetComponent<Fighter>(out Fighter fighter))
            {
                Vector2 vec2 = -(coll.transform.position - transform.position).normalized;  // ����������� ������
                fighter.TakeDamage(damage, vec2, pushForce);                                // ����

/*                if (coll.gameObject.TryGetComponent(out BotAI botAI))                       // ��� ���������� (����� ����������)
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
            int ndx = Random.Range(0, goToSpawn.Length);                            // �������� ������ �� ������� ������
            GameObject enemyPref = Instantiate(goToSpawn[ndx], transform.position, Quaternion.identity);                     // ������ ������            
            NavMeshAgent agent = enemyPref.GetComponent<NavMeshAgent>();            // ������� �����������            
            agent.Warp(transform.position);                                         // ���������� ������ � ��������
            //enemyPref.GetComponentInChildren<BotAI>().triggerLenght = chaseDistance;    // ������������� ��������� ��������
        }

        CMCameraShake.Instance.ShakeCamera(cameraAmplitudeShake, cameraTimedeShake);            // ������ ������
        base.Explosion();                                       // ������ ������ � ���������� ��� � ������
    }




    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;        
        Gizmos.DrawWireSphere(transform.position, expRadius);
    }
}
