using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletRocket : Bullet
{
    public float expRadius = 3;
    public bool withTimer;
    public float timeToExpl;

    [Header("������ ������ ��� ������")]
    public float cameraAmplitudeShake = 3f; // ���������
    public float cameraTimedeShake = 0.1f;  // ������������


    private void Start()
    {
        if (withTimer)
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
        Explosion();
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
                Vector2 vec2 = (coll.transform.position - transform.position).normalized;
                fighter.TakeDamage(damage, vec2, pushForce);                
            }
            collidersHits = null;
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
