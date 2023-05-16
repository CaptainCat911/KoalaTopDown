using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PortalExplousion : MonoBehaviour
{
    //Animator animator;      
    SpriteRenderer spriteRenderer;

    public int damage;                  // ���� ��� ��������� �������
    public float pushForce;             // ���
    public float expRadius = 3;         // ������
    public LayerMask layerExplousion;   // ���� ��� �����
    public GameObject expEffect;        // ������ ������
    public BotAI targetTeleport;        // ��� ��� ��������
    public GameObject targetHome;       // ����� ����
    public UnityEvent interactAction;   // ����� �� ���������� �������

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

        CMCameraShake.Instance.ShakeCamera(3, 0.1f);    // ������ ������

        GameObject effect = Instantiate(expEffect, transform.position, Quaternion.identity);    // ������� ������
        Destroy(effect, 0.5f);                          // ���������� ������ ����� .. ���        
        targetTeleport.agent.Warp(transform.position);  // ��� ������ (����������) ��� ����
        spriteRenderer.enabled = false;                 // ��������� ������
        interactAction.Invoke();                        // ����� (��� �������)
        //Destroy(gameObject);                          // ���������� ������
    }


    public void ClosePortal()
    {
        GameObject effect = Instantiate(expEffect, targetTeleport.transform.position, Quaternion.identity);    // ������� ������
        targetTeleport.agent.Warp(targetHome.transform.position);           // ��� ������ (����������) ��� ����
        targetTeleport.startPosition = targetHome.transform.position;       // ������ ���� ��������� �������
        targetTeleport.ResetTarget();                                       
        Destroy(effect, 0.5f);                                  // ���������� ������ ����� .. ���
        Destroy(gameObject, 0.5f);                              // ���������� ������ ����� .. ���
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, expRadius);
    }
}
