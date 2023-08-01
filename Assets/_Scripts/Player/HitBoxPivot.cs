using UnityEngine;

/// <summary>
/// ����� ��� ���� ������, ����� �������������� ��� �������� ������ � �����
/// </summary>

public class HitBoxPivot : MonoBehaviour
{
    Player player;
    public WeaponHolder weaponHolder;

    public Shield shield;       // (�������� ���� ������� ���� �����)

    void Start()
    {
        player = GameManager.instance.player;
        //weaponHolder = GetComponentInParent<WeaponHolder>();
    }
   
    void Update()
    {
        if (!GameManager.instance.isPlayerEnactive && player.noAim)
        {
            Quaternion qua1 = Quaternion.Euler(0, 0, weaponHolder.aimAngle);                                // ������� ���� ���� � Quaternion
            transform.rotation = Quaternion.Lerp(transform.rotation, qua1, Time.fixedDeltaTime * 15);       // ������ Lerp ����� weaponHoder � ����� �����
        }        
        else
        {
            
            if (player.rightFlip)
            {                
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 0), Time.fixedDeltaTime * 15);   // ������ Lerp ����� weaponHoder � ����� �����
            }
            if (player.leftFlip)
            {                
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 180), Time.fixedDeltaTime * 15);   // ������ Lerp ����� weaponHoder � ����� �����
            }
        }
    }

    public void Flip()
    {
        shield.Flip();

        if (player.leftFlip)                               // �������� ������
        {
            transform.localScale = new Vector3(transform.localScale.x, -1, transform.localScale.z);     // ������������ ������ ����� scale
        }
        if (player.rightFlip)
        {
            transform.localScale = new Vector3(transform.localScale.x, 1, transform.localScale.z);     // ������������ ������ ����� scale
        }        
    }
}
