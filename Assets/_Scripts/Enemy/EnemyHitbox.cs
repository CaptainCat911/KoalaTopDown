using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{    
    Enemy enemy;
    EnemyHitBoxPivot pivot;

    [Header("��������� �����")]
    [HideInInspector] public float lastAttack;              // ����� ���������� ����� (��� ����������� �����)
    public float hitBoxRadius;                              // ������ �����                                                               
    public float cooldown = 0.5f;                           // ����������� �����
    public int damage = 1;                                  // ����
    public float pushForce = 10;                            // ���� ������
    // ���� ����
    public bool isRange;                                    // ���� �����
    public GameObject bulletPrefab;
    public GameObject effectRangeAttack;                    // ������ ���� �����
    public float bulletSpeed = 10;                          // �������� �������
    public float forceBackFire = 10;                        // ������

    // ��� ����� ������
    bool needFlip;                          // ����� ���� (��� ����������� ����������� ������)    
    bool leftFlip;                          // ������ �����
    bool rightFlip = true;                  // ������ ������



    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
        pivot = GetComponentInParent<EnemyHitBoxPivot>();
    }

    void Start()
    {

    }

    
    void Update()
    {
        //Debug.Log(enemy.lastAttack);

        // �����
        if (enemy.readyToAttack && Time.time - lastAttack > cooldown)               // ���� ������ ��������� � �� ������
        {
            lastAttack = Time.time;                                                 // ����������� ����� �����
            enemy.animator.SetTrigger("Attack");                                    // �������� �������� �����
        }

        // ���� ������
        if (Mathf.Abs(pivot.aimAngle) > 90 && rightFlip)
        {
            needFlip = true;
            leftFlip = true;
            rightFlip = false;
        }
        if (Mathf.Abs(pivot.aimAngle) <= 90 && leftFlip)
        {
            needFlip = true;
            rightFlip = true;
            leftFlip = false;
        }
        if (needFlip)
        {
            Flip();
        }
    }

    public void Attack()
    {
        if (!isRange)
        {
            MeleeAttack();                                                      // ���� �����
        }
        else if (isRange)
        {
            RangeAttack();                                                      // ���� �����
        }        
    }

    void MeleeAttack()
    {
        Collider2D[] collidersHitbox = Physics2D.OverlapCircleAll(transform.position, hitBoxRadius);    // ������� ���� � ������� ������� � ��������
        foreach (Collider2D enObjectBox in collidersHitbox)
        {
            if (enObjectBox == null)
            {
                continue;
            }

            if (enObjectBox.gameObject.TryGetComponent<Player>(out Player player))                  // ���� ������ ������
            {
                player.TakeDamage(damage);                                                          // ������� ����
                Vector2 vec2 = (player.transform.position - transform.position).normalized;         // ��������� ������ ����������� �����
                player.rb2D.AddForce(vec2 * pushForce, ForceMode2D.Impulse);                        // ��� �������                                                                                                                  
            }
            collidersHitbox = null;                                                                 // ���������� ��� ��������� ������� (�� ����� ���� ��������� ��� ��� ��������)
        }
    }


    public void EffectRangeAttack()
    {
        if (effectRangeAttack)
            Instantiate(effectRangeAttack, pivot.transform.position + new Vector3(0, 0.1f, 0), Quaternion.identity);                  // ������� ������ �������� (���� �� ����)
    }

    void RangeAttack()
    {        
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);              // ������� ������ ������� � �������� � ��������� �����
        bullet.GetComponent<Bullet>().damage = damage;                                                      // ����������� ���� �������
        bullet.GetComponent<Bullet>().pushForce = pushForce;                                                // ����������� ���� �������
        bullet.GetComponent<Rigidbody2D>().AddForce(transform.right * bulletSpeed, ForceMode2D.Impulse);    // ��� �������
        //bullet.transform.Rotate(0.0f, 0.0f, -90.0f, Space.Self);                                          // ������������ ������
        enemy.ForceBackFire(transform.position, forceBackFire);                                             // ��� ������         
    }

    void Flip()
    {
        if (leftFlip)                                                                                   // �������� ������
        {
            transform.localScale = new Vector3(transform.localScale.x, -1, transform.localScale.z);     // ������������ ������ ����� scale
        }
        if (rightFlip)
        {
            transform.localScale = new Vector3(transform.localScale.x, 1, transform.localScale.z);
        }
        needFlip = false;
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;        
        Gizmos.DrawWireSphere(transform.position, hitBoxRadius);
    }
}
