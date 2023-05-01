using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletRifle : Bullet
{

    public override void OnTriggerEnter2D(Collider2D collision)
    {        
        if (collision.gameObject.TryGetComponent<Fighter>(out Fighter fighter))
        {
            Vector2 vec2 = (collision.transform.position - GameManager.instance.player.transform.position).normalized;
            fighter.TakeDamage(damage, vec2, pushForce);
            enemyDamaged++;
        }

        base.OnTriggerEnter2D(collision);           // ��� ����� ���� ���

        if (enemyDamaged >= enemyToDamageCount || collision.tag == "Wall")      // ���� ������� ������ ��� ������ � �����
            Explosion();        
    }

    public void FindNewTarget()
    {
        Collider2D[] collidersHits = Physics2D.OverlapCircleAll(transform.position, 5, layerExplousion);     // ������� ���� � ������� ������� � ��������
        foreach (Collider2D coll in collidersHits)
        {
            if (coll == null)
            {
                continue;
            }

            if (coll.gameObject.TryGetComponent<Fighter>(out Fighter fighter))
            {
                Vector2 vec2 = (coll.transform.position - transform.position).normalized;
                rb.AddForce(vec2 * 15, ForceMode2D.Impulse);    // ��� �������
            }
            collidersHits = null;
        }
    }
}
