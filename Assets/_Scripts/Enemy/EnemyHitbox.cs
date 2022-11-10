using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
    Enemy enemy;
    //public WeaponHolder weaponHolder;
    public float attackRadius;                              // ������ �����
    
    void Start()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    
    void Update()
    {
        //Debug.Log(enemy.lastAttack);
        // �����
        if (enemy.readyToAttack && Time.time - enemy.lastAttack > enemy.cooldown)           // ���� ������ ��������� � �� ������
        {
            //Debug.Log("Attack!");
            enemy.lastAttack = Time.time;                                                   // ����������� ����� �����

            Collider2D[] collidersHitbox = Physics2D.OverlapCircleAll(transform.position, attackRadius);  // ������� ���� � ������� ������� � ��������
            foreach (Collider2D enObjectBox in collidersHitbox)
            {
                if (enObjectBox == null)
                {
                    continue;
                }

                if (enObjectBox.gameObject.TryGetComponent<Player>(out Player player))                  // ���� ������ ������
                {
                    player.TakeDamage(enemy.attackDamage);                                              // ������� ����
                    Vector2 vec2 = (player.transform.position - transform.position).normalized;         // ��������� ������ ����������� �����
                    player.rb.AddForce(vec2 * enemy.pushForce, ForceMode2D.Impulse);                  // ��� �������
                    enemy.animator.SetTrigger("Attack");                                                // �������� ��������
                    //Debug.Log("Player!");
                }
                collidersHitbox = null;                                                                 // ���������� ��� ��������� ������� (�� ����� ���� ��������� ��� ��� ��������)
            }            
        }        
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;        
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
