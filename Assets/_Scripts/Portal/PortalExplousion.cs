using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalExplousion : MonoBehaviour
{
    public int damage;
    public float pushForce;    
    public float expRadius = 3;
    public LayerMask layerExplousion;
    public GameObject expEffect;
    public BotAI botAI;

    public void Explosion()
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

        CMCameraShake.Instance.ShakeCamera(3, 0.1f);                                            // ������ ������

        GameObject effect = Instantiate(expEffect, transform.position, Quaternion.identity);    // ������� ������
        Destroy(effect, 0.5f);                                                                  // ���������� ������ ����� .. ���
        // ��� ������ (����������) ��� ����
        botAI.agent.Warp(transform.position);
        Destroy(gameObject);                                                                    // ���������� ����
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, expRadius);
    }
}
