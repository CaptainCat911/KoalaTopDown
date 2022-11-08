using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxPlayer : MonoBehaviour
{
    Player player;
    public WeaponHolder weaponHolder;                   // ������ �� ������ weaponHolder (��� �����)

    public int damage = 10;                             // ����
    public float pushForce = 1;                         // ���� ������
    public float radius = 1;                            // ������
    public float cooldown = 1f;                         // ����������� �����
    float lastAttack;                                   // ����� ���������� ����� (��� ����������� �����)

    void Start()
    {
        player = GameManager.instance.player;        
    }

    
    void Update()
    {
        if (weaponHolder.attackHitBoxStart && Time.time - lastAttack > cooldown)          // ���� ������ ��������� � �� ������
        {
            //Debug.Log("Attack!");
            lastAttack = Time.time;                             // ����������� ����� �����
            player.animator.SetTrigger("AttackHit");

            Collider2D[] collidersHits = Physics2D.OverlapCircleAll(transform.position, radius);     // ������� ���� � ������� ������� � ��������
            foreach (Collider2D coll in collidersHits)
            {
                if (coll == null)
                {
                    continue;
                }

                if (coll.gameObject.TryGetComponent<Enemy>(out Enemy enemy))                            // ���� ������ �����
                {
                    enemy.TakeDamage(damage);                                                           // ������� ����
                    Vector2 vec2 = (enemy.transform.position - player.transform.position).normalized;          // ��������� ������ ����������� �����
                    enemy.rb2D.AddForce(vec2 * pushForce, ForceMode2D.Impulse);                         // ��� �������                                                                
                }
                collidersHits = null;
            }
        }
        
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
