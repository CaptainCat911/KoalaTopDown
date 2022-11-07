using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("������")]
    Player player;
    SpriteRenderer spriteRenderer;
    public WeaponClass weaponClass;         // ������ �� ����� ������
    public Transform firePoint;             // ����� ��� ��������
    public Transform pivot;                 // ����� weaponHolder (������������ ��� ������������)
    GameObject weaponHolderGO;              // ������ �� ������ weaponHolder (��� ��������)
    WeaponHolder weaponHolder;              // ������ �� ������ weaponHolder (��� ��������)

    Vector3 mousePosition;                  // ��������� ����

    // ��������� ������ (�� ������ ������)
    string weaponName;                      // �������� ������
    GameObject bulletPrefab;                // ������ �������
    float bulletSpeed;                      // �������� �������
    int damage;                             // ���� (�������� ����� ������� �� �������)
    float pushForce;                        // ���� ������ (�������� ����� ������� �� �������)
    [HideInInspector] public float fireRate;                // ���������������� ������ (10 - 0,1 ��������� � �������)
    [HideInInspector] public float nextTimeToFire;          // ��� �������� (����� �������� � ���� ���)    

    // ��� ����� ������
    bool needFlip;                          // ����� ���� (��� ����������� ����������� ������)    
    bool leftFlip;                          // ������ �����
    bool rightFlip = true;                  // ������ ������

    // �������
    public Animator flashEffectAnimator;
    public bool singleFlash;
    bool flashEffectActive;

    [Header("������ ������ ��� ��������")]
    public float cameraAmplitudeShake = 1f;     // ���������
    public float cameraTimedeShake = 0.1f;      // ������������





    private void Start()
    {
        player = GameManager.instance.player;
        spriteRenderer = GetComponent<SpriteRenderer>();
        weaponHolderGO = GetComponentInParent<WeaponHolder>().gameObject;       // ������� ������ weaponHolder
        weaponHolder = GetComponentInParent<WeaponHolder>();                    // ������� ������ weaponHolder
        weaponName = weaponClass.name;                                          // ���
        bulletPrefab = weaponClass.bullet;                                      // ��� �������
        bulletSpeed = weaponClass.bulletSpeed;                                  // ��������
        damage = weaponClass.damage;                                            // ����
        pushForce = weaponClass.pushForce;                                      // ���� ������
        fireRate = weaponClass.fireRate;                                        // ����������������        
        //flashEffect = weaponClass.flashEffect;                                  // ������ ������� ��� �������� (����)

    }

    private void Update()
    {
        // ��������
        if (weaponHolder.fireStart && Time.time >= nextTimeToFire)                          // ���� �������� �������� � �� ������
        {
            nextTimeToFire = Time.time + 1f / fireRate;                                     // ��������� ��
            Fire();                                                                         // �����
            CMCameraShake.Instance.ShakeCamera(cameraAmplitudeShake, cameraTimedeShake);    // ������ ������

            // ������ ���� ��� ���������� ������� (����� ��� ��� ������� �������� ����)
            if (singleFlash)
                FlashSingle();
        }

        // ������ ����
        if (flashEffectAnimator != null && !singleFlash)        // ���� ���������� ����
            Flash();        
    }

    void Flash()
    {
        if (weaponHolder.fireStart && !flashEffectActive)
        {
            flashEffectAnimator.SetBool("Fire", true);
            flashEffectActive = true;
        }
        if (!weaponHolder.fireStart)
        {
            flashEffectAnimator.SetBool("Fire", false);
            flashEffectActive = false;
        }
    }
    void FlashSingle()
    {
        flashEffectAnimator.SetTrigger("Fire");
    }


        private void FixedUpdate()
    {
        // ������� ������
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);                                                    // ��������� ����                  
        Vector3 aimDirection = mousePosition - pivot.transform.position;                                                        // ���� ����� ���������� ���� � weaponHolder (�� ����� ����� firePoint)          
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;                                           // ������� ���� � ��������             
        Quaternion qua1 = Quaternion.Euler(0, 0, aimAngle);                                                                     // ������� ���� ���� � Quaternion
        weaponHolderGO.transform.rotation = Quaternion.Lerp(weaponHolderGO.transform.rotation, qua1, Time.fixedDeltaTime * 15); // ������ Lerp ����� weaponHoder � ����� �����

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
        //bullet.transform.Rotate(0.0f, 0.0f, -90.0f, Space.Self);                                             // ������������ ������
    }
}
