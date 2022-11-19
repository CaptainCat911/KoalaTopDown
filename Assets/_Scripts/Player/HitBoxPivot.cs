using UnityEngine;

/// <summary>
/// ������������ ������� (��� ����� �����)
/// </summary>

public class HitBoxPivot : MonoBehaviour
{
    public WeaponHolder weaponHolder;
    void Start()
    {
        //weaponHolder = GetComponentInParent<WeaponHolder>();
    }
   
    void Update()
    {
        Quaternion qua1 = Quaternion.Euler(0, 0, weaponHolder.aimAngle);                                // ������� ���� ���� � Quaternion
        transform.rotation = Quaternion.Lerp(transform.rotation, qua1, Time.fixedDeltaTime * 15);       // ������ Lerp ����� weaponHoder � ����� �����
    }
}
