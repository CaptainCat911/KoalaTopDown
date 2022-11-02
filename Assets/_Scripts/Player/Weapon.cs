using UnityEngine;

public class Weapon : MonoBehaviour
{
    // ������
    Player player;
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
    float pushForce;                        // ���� ������ (�������� ����� ������� �� �������)
    [HideInInspector] public float fireRate;                // ���������������� ������ (10 - 0,1 ��������� � �������)
    [HideInInspector] public float nextTimeToFire;          // ��� �������� (����� �������� � ���� ���)
    [HideInInspector] public bool fireStart;

    // ��� ����� ������
    bool needFlip;                          // ����� ���� (��� ����������� ����������� ������)    
    bool leftFlip;                          // ������ �����
    bool rightFlip = true;                  // ������ ������

    // �������
    public GameObject flashEffect;
    bool flashEffectActive;


    private void Start()
    {
        player = GameManager.instance.player;
        weaponHolder = GetComponentInParent<WeaponHolder>().gameObject;         // ������� weaponHolder
        spriteRenderer = GetComponent<SpriteRenderer>();
        weaponName = weaponClass.name;                                          // ���
        bulletPrefab = weaponClass.bullet;                                      // ��� �������
        bulletSpeed = weaponClass.bulletSpeed;                                  // ��������
        damage = weaponClass.damage;                                            // ����
        pushForce = weaponClass.pushForce;                                      // ���� ������
        fireRate = weaponClass.fireRate;                                        // ����������������        
    }

    private void Update()
    {
        // ��������
        if (fireStart && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Fire();
        }

        // ������ ����
        if (fireStart && !flashEffectActive)
        {
            if (flashEffect == null)
                return;
            flashEffect.SetActive(true);
            flashEffectActive = true;
        }
        if (!fireStart)
        {
            if (flashEffect == null)
                return;
            flashEffect.SetActive(false);
            flashEffectActive = false;
        }

    }

    private void FixedUpdate()
    {
        // ������� ������
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);                                                    // ��������� ����                  
        Vector3 aimDirection = mousePosition - pivot.transform.position;                                                        // ���� ����� ���������� ���� � weaponHolder (�� ����� ����� firePoint)          
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;                                           // ������� ���� � ��������             
        Quaternion qua1 = Quaternion.Euler(0, 0, aimAngle);                                                                     // ������� ���� ���� � Quaternion
        weaponHolder.transform.rotation = Quaternion.Lerp(weaponHolder.transform.rotation, qua1, Time.fixedDeltaTime * 15);     // ������ Lerp ����� weaponHoder � ����� �����

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

         // ����������� ������ ����� ��� ������ ������
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
        if (leftFlip)                                                                                   // �������� ������
        {
            transform.localScale = new Vector3(transform.localScale.x, -1, transform.localScale.z);     // ������������ ������ ����� scale
            player.spriteRenderer.flipX = true;                                                         // ������������ ������ ������
        }
        if (rightFlip)
        {
            transform.localScale = new Vector3(transform.localScale.x, 1, transform.localScale.z);          
            player.spriteRenderer.flipX = false;
        }
        needFlip = false;
    }


    public void Fire()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);              // ������� ������ ������� � �������� � ��������� �����
        bullet.GetComponent<Bullet>().damage = damage;                                                      // ����������� ���� �������
        bullet.GetComponent<Bullet>().pushForce = pushForce;                                                // ����������� ���� �������
        bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.right * bulletSpeed, ForceMode2D.Impulse);    // ��� �������
        //bullet.transform.Rotate(0.0f, 0.0f, -90.0f, Space.Self);                                             // ������������ ������ (��� ������)
    }
}
