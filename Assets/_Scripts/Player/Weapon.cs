using UnityEngine;

public class Weapon : MonoBehaviour
{
    // ������
    SpriteRenderer spriteRenderer;          
    public WeaponClass weaponClass;         // ������ �� ����� ������
    public Transform firePoint;             // ����� ��� ��������
    public Transform pivot;                 // ����� weaponHolder
    GameObject weaponHolder;                // ������ �� weaponHolder (��� ��������)

    Vector3 mousePosition;                  // ��������� ����

    // ��������� ������ (�� ������ ������)
    string weaponName;                      // �������� ������
    GameObject bulletPrefab;                // ������ �������
    float bulletSpeed;                      // �������� �������
    int damage;                             // ���� (�������� ����� ������� �� �������)
    [HideInInspector]
    public float fireRate;                  // ���������������� ������ (10 - 0,1 ��������� � �������)
    [HideInInspector]
    public float nextTimeToFire = 0f;       // ��� �������� (����� �������� � ���� ���)

    bool needFlip;                          // ����� ���� (��� ����������� ����������� ������)    
    bool leftFlip;                          // ������ �����
    bool rightFlip = true;                  // ������ ������


    private void Awake()
    {
        
    }

    private void Start()
    {
        weaponName = weaponClass.name;                                          // ���
        bulletPrefab = weaponClass.bullet;                                      // ��� �������
        bulletSpeed = weaponClass.bulletSpeed;                                  // ��������
        damage = weaponClass.damage;                                            // ����
        fireRate = weaponClass.fireRate;                                        // ����������������
        //GetComponent<Renderer>().material.color = weaponClass.color;            // ����
        weaponHolder = GetComponentInParent<WeaponHolder>().gameObject;         // ������� weaponHolder
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        // ������� ������
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);                                                            // ��������� ����                  
        Vector3 aimDirection = mousePosition - pivot.transform.position;                                                         // ���� ����� ���������� ���� � weaponHolder (�� ����� ����� firePoint)          
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;                                                   // ������� ���� � ��������             
        Quaternion qua1 = Quaternion.Euler(0, 0, aimAngle);                                                                             // ������� ���� ���� � Quaternion
        weaponHolder.transform.rotation = Quaternion.Lerp(weaponHolder.transform.rotation, qua1, Time.fixedDeltaTime * 15);             // ������ Lerp ����� weaponHoder � ����� �����

        // ���� ������
        if (Mathf.Abs(aimAngle) > 90 && rightFlip)
        {
            needFlip = true;
            leftFlip = true;
            rightFlip = false;
        }
        if (Mathf.Abs(aimAngle) <= 90 && leftFlip)
        {
            needFlip = true;
            rightFlip = true;
            leftFlip = false;
        }
        if (needFlip)
        {
            Flip();
        }

        /*        if (aimAngle > 0)
                {
                    spriteRenderer.sortingOrder = -1;
                }
                else
                {
                    spriteRenderer.sortingOrder = 1;
                }*/
        
        
        


    }

    void Flip()
    {
        if (leftFlip)
            transform.localScale = new Vector3(transform.localScale.x, -1, transform.localScale.z);
        if (rightFlip)
            transform.localScale = new Vector3(transform.localScale.x, 1, transform.localScale.z);          
        needFlip = false;
    }


    public void Fire()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);                      // ������� ������ ������� � �������� � ��������� �����
        //bullet.layer = 7;                                                                                           // ��������� ������� ���� "BulletPlayer"
        bullet.GetComponent<Bullet>().damage = damage;                                                              // ����������� ���� �������
        //bullet.transform.Rotate(0.0f, 0.0f, 0.0f, Space.Self);                                                     // ������������ ������ (��� ������)
        bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.right * bulletSpeed, ForceMode2D.Impulse);            // ��� �������
    }
}
