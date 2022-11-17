using UnityEngine;

/// <summary>
/// ������ weaponHolder � hitboxpivot ������
/// </summary>

public class EnemyHitBoxPivot : MonoBehaviour
{
    Enemy enemy;    
    [HideInInspector] public float aimAngle;                // ���� �������� ��� �������� ������� � ������� � �������������

    void Start()
    {
        enemy = GetComponentInParent<Enemy>();
    }
    
    void Update()
    {
        // ������� ��������
        if (enemy.chasing && enemy.targetVisible)
        {
            Vector3 aimDirection = enemy.target.transform.position - transform.position;                // ���� ����� ���������� ���� � pivot ������          
            aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;                     // ������� ���� � ��������             
            Quaternion qua1 = Quaternion.Euler(0, 0, aimAngle);                                         // ������� ���� ���� � Quaternion
            transform.rotation = Quaternion.Lerp(transform.rotation, qua1, Time.fixedDeltaTime * 15);   // ������ Lerp ����� weaponHoder � ����� �����
        }        
    }
}
