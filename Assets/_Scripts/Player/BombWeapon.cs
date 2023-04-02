using UnityEngine;

public class BombWeapon : MonoBehaviour
{
    [Header("������")]                      // ������-�� �� ������������
    Player player;
    AmmoPackStore[] ammoBombs;
    public WeaponClass weaponClass;         // ������ �� ����� ������
    public Transform firePoint;             // ����� ��� ��������
    public Transform pivot;                 // ����� weaponHolder (������������ ��� ������������)
    BombWeaponHolder bombWeaponHolder;      // ������ �� ������ bombweaponHolder (��� ������ �����)

    //GameObject weaponHolderGO;              // ������ �� ������ weaponHolder (��� ��������)
    //Vector3 mousePosition;                  // ��������� ����

    [Header("��������� ������")]
    public int weaponIndexForAmmo;                  // ������ ������ (��� ��������)
    [HideInInspector] public string weaponName;     // �������� ������
    [HideInInspector] public float fireRate;        // ���������������� ������ (10 - 0,1 ��������� � �������)
    [HideInInspector] public float nextTimeToFire;  // ��� �������� (����� �������� � ���� ���)
    //public int ammo;                                // ���-�� ����

    /*    GameObject bulletPrefab;                // ������ �������
        float bulletSpeed;                      // �������� �������
        int damage;                             // ���� (�������� ����� ������� �� �������)
        float pushForce;                        // ���� ������ (�������� ����� ������� �� �������)
        float forceBackFire;                    // ������ ������
        float recoil;                          // ������� ��������
        LayerMask layerRayCast;                 // ���� ��� ���������*/

    // ��� ����� ������
    bool needFlip;                          // ����� ���� (��� ����������� ����������� ������)    
    bool leftFlip;                          // ������ �����
    bool rightFlip = true;                  // ������ ������

    void Awake()
    {
        player = GameManager.instance.player;
        ammoBombs = GameManager.instance.ammoPack.ammoBombs;
        weaponName = weaponClass.weaponName;                                    // ��� ������        

/*        layerRayCast = weaponClass.layerRayCast;                                       // ���� � ���������
        if (weaponClass.bulletPrefab)
            bulletPrefab = weaponClass.bulletPrefab;                                  // ��� ������� (���� �� ������� ������)
        bulletSpeed = weaponClass.bulletSpeed;                                  // ��������
        damage = weaponClass.damage;                                            // ����
        pushForce = weaponClass.pushForce;                                      // ���� ������
        fireRate = weaponClass.fireRate;                                        // ����������������
        forceBackFire = weaponClass.forceBackFire;                              // ���� ������
        recoil = weaponClass.recoil;                                           // ������� ��������*/
        //flashEffect = weaponClass.flashEffect;                                  // ������ ������� ��� �������� (����) 
    }

    private void Start()
    {
        bombWeaponHolder = GetComponentInParent<BombWeaponHolder>();                    // ������� ������ weaponHolder      
    }

    private void Update()
    {
        // ���� ������
        if (Mathf.Abs(bombWeaponHolder.aimAngle) > 90 && rightFlip)
        {
            needFlip = true;
            leftFlip = true;
            rightFlip = false;
        }
        if (Mathf.Abs(bombWeaponHolder.aimAngle) <= 90 && leftFlip)
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


    private void FixedUpdate()
    {
        // ��������
        if (!bombWeaponHolder.fireStart)                        // ���� �� ������ ��������
        {
            return;                                         // �������
        }

        if (Time.time >= nextTimeToFire && ammoBombs[weaponIndexForAmmo].allAmmo > 0)                    // ���� �������� �������� � �� ������
        {
            ammoBombs[weaponIndexForAmmo].allAmmo--;                        // - �������
            nextTimeToFire = Time.time + 1f / weaponClass.fireRate;         // ��������� ��
            //if (!rayCastWeapon)
            GameManager.instance.player.animator.SetTrigger("ThrowBomb");   // ��� ������ ��������� ������
            Invoke("FireProjectile", 0.2f);                                 // ���� ��� ���, ����� ��������� ������ � ��������
            /*            if (rayCastWeapon)
                            FireRayCast();*/                                                              // ������� ���������
        }
    }

    // ����
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


    public void FireProjectile()
    {
        float randomBulletX = Random.Range(-weaponClass.recoil, weaponClass.recoil);                            // ������� ��������
        firePoint.Rotate(0, 0, randomBulletX);                                                                  // ���� �������
        GameObject bullet = Instantiate(weaponClass.bulletPrefab, firePoint.position, Quaternion.identity);     // ������� ������ ������� � �������� � ��������� �����
        bullet.GetComponent<Bullet>().damage = weaponClass.damage;                                              // ����������� ���� �������
        bullet.GetComponent<Bullet>().pushForce = weaponClass.pushForce;                                        // ����������� ���� ������ �������

        float dist = Vector3.Distance(transform.position, bombWeaponHolder.mousePosition) - 10f;
        if (dist < 0.3f)
            dist = 0.3f;
        if (dist > 1f)
            dist = 1f;
        //Debug.Log(dist);
        bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.right * (weaponClass.bulletSpeed * (dist + 0.3f)), ForceMode2D.Impulse);    // ��� �������
        player.ForceBackFire(firePoint.transform.position, weaponClass.forceBackFire);                          // ��� ������ ������
        firePoint.Rotate(0, 0, -randomBulletX);                                                                 // � ���� ���������� �������
    }
}
