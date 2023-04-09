using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("������")]                      // ������-�� �� ������������
    Player player;
    AmmoPackStore[] ammoWeapons;
    public WeaponClass weaponClass;         // ������ �� ����� ������
    public Transform firePoint;             // ����� ��� ��������
    public Transform pivot;                 // ����� weaponHolder (������������ ��� ������������)
    WeaponHolder weaponHolder;              // ������ �� ������ weaponHolder (��� ��������)

    //GameObject weaponHolderGO;              // ������ �� ������ weaponHolder (��� ��������)
    //Vector3 mousePosition;                  // ��������� ����

    [Header("��������� ������")]
    public int weaponIndexForAmmo;                  // ������ ������ (��� ��������)
    bool projectileWeapon;                          // ������ �� ���������
    bool splitProjectileWeapon;                     // ���������
    bool rayCastWeapon;                             // ������� ������
    bool splitRaycastWeapon;                        // ������������� ������

    [HideInInspector] public string weaponName;     // �������� ������
    [HideInInspector] public float fireRate;        // ���������������� ������ (10 - 0,1 ��������� � �������)
    [HideInInspector] public float nextTimeToFire;  // ��� �������� (����� �������� � ���� ���)
    //public int ammo;                                       // �������

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

    [Header("�������")]
    public Animator flashEffectAnimator;    // ���� ��� ��������
    public bool singleFlash;                // ��������� ����
    bool flashActive;                       // ���� ������� (��� �����������)
    //public LineRenderer lineRenderer;       // ����� ��� ������ (������)
    //LineRenderer lineRaycast;               // ����� ��� ������ (������)
    public TrailRenderer tracerEffect;      // ������
    public ParticleSystem flameParticles;   // ������ ������� ������ �������

    [Header("������ ������ ��� ��������")]
    public float cameraAmplitudeShake = 1f; // ���������
    public float cameraTimedeShake = 0.1f;  // ������������

    [Header("����")]
    public bool singleShot;                 // ��������� ����
    public bool multiShot;                  // �������� ����
    bool soundStarted;                      // ���� �������
    public AudioSource audioSource;         // �������� ����
    public AudioSource audioSourceTail;     // "�����"

    public bool debug;

    void Awake()
    {
        player = GameManager.instance.player;
        ammoWeapons = GameManager.instance.ammoPack.ammoWeapons;

        weaponName = weaponClass.weaponName;                                    // ��� ������
        projectileWeapon = weaponClass.projectileWeapon;                        // ������ ���������
        splitProjectileWeapon = weaponClass.splitProjectileWeapon;              // ������ ���������
        rayCastWeapon = weaponClass.rayCastWeapon;                              // ������� ������
        splitRaycastWeapon = weaponClass.splitRaycastWeapon;                    // ������������� ������

        audioSource = GetComponent<AudioSource>();

        if (singleShot)
        {
            audioSource.loop = false;
        }
        if (multiShot)
        {
            audioSource.loop = true;
        }

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
        weaponHolder = GetComponentInParent<WeaponHolder>();                    // ������� ������ weaponHolder
        //ammo = GameManager.instance.ammoPack.ammoTompson;
    }

    private void Update()
    {
        // ���� ������
        if (GameManager.instance.player.leftFlip && rightFlip)
        {
            needFlip = true;
            leftFlip = true;
            rightFlip = false;
        }
        if (GameManager.instance.player.rightFlip && leftFlip)
        {
            needFlip = true;
            rightFlip = true;
            leftFlip = false;
        }
        if (needFlip)
        {
            Flip();
        }
        
        if (!flashEffectAnimator)
        {
            return;
        }

        // ������� ������ ������ � ���������� (�������� ����� ����������)
        if (!singleFlash && Time.time >= nextTimeToFire + 0.1f)
        {
            flashEffectAnimator.SetBool("Fire", false);
            flashActive = false;
            //if (lineRaycast)
                //lineRaycast.enabled = false;
        }

        // ����
        if (multiShot && ammoWeapons[weaponIndexForAmmo].allAmmo > 0)
        {
            if (weaponHolder.fireStart && !soundStarted)                        // ���� �� ������ ��������
            {                
                audioSource.Play();
                soundStarted = true;
            }
            if (!weaponHolder.fireStart && soundStarted)                        // ���� �� ������ ��������
            {                 
                audioSource.Stop();
                if (audioSourceTail)
                    audioSourceTail.Play();
                soundStarted = false;
            }
        }
    }


    private void FixedUpdate()
    {
        // ��������
        if (!weaponHolder.fireStart)        // ���� �� ������ ��������
        {
            if (flameParticles)
                flameParticles.Stop();
            return;                         // �������
        }

        if (Time.time >= nextTimeToFire && ammoWeapons[weaponIndexForAmmo].allAmmo > 0)     // ���� �������� �������� � �� ������
        {
            ammoWeapons[weaponIndexForAmmo].allAmmo--;                      // - �������
            nextTimeToFire = Time.time + 1f / weaponClass.fireRate;         // ��������� ��           
            
            if (projectileWeapon)
                FireProjectile();       // ������� �����
            if (splitProjectileWeapon)
                FireSplit(false);       // ������� "������"
            if (rayCastWeapon)
                FireRayCast();          // ������� ���������
            if (splitRaycastWeapon)
                FireSplit(true);            // ������� "������"

            CMCameraShake.Instance.ShakeCamera(cameraAmplitudeShake, cameraTimedeShake);    // ������ ������

            // ����
            if (flashEffectAnimator != null)        // ���� ���������� ����
                Flash();

            if (flameParticles)
                flameParticles.Play();

            // �����
            if (singleShot)
            {
                audioSource.Play();
            }
        }

        // ������� ���� ��� ����� ������� � ������� ������
        //mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);                // ��������� ����                  
        //Vector3 aimDirection = mousePosition - transform.position;                          // ���� ����� ���������� ���� � pivot ������          
        //float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;       // ������� ���� � ��������             
        //Quaternion qua1 = Quaternion.Euler(0, 0, aimAngle);                                 // ������� ���� ���� � Quaternion
        //weaponHolderGO.transform.rotation = Quaternion.Lerp(weaponHolderGO.transform.rotation, qua1, Time.fixedDeltaTime * 15); // ������ Lerp ����� weaponHoder � ����� �����



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

    // ����
    void Flash()
    {
        if (!singleFlash)
        {
            if (!flashActive)
            {
                flashEffectAnimator.SetBool("Fire", true);
                flashActive = true;
            }
        }
        else
        {
            flashEffectAnimator.SetTrigger("Fire");
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

    void FireProjectile()
    {
        float randomBulletX = Random.Range(-weaponClass.recoil, weaponClass.recoil);                            // ������� ��������
        firePoint.Rotate(0, 0, randomBulletX);                                                                  // ���� �������
        GameObject bullet = Instantiate(weaponClass.bulletPrefab, firePoint.position, firePoint.rotation);      // ������� ������ ������� � �������� � ��������� �����
        bullet.GetComponent<Bullet>().damage = weaponClass.damage;                                              // ����������� ���� �������
        bullet.GetComponent<Bullet>().pushForce = weaponClass.pushForce;                                        // ����������� ���� ������ �������
        bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.right * weaponClass.bulletSpeed, ForceMode2D.Impulse);    // ��� �������
        player.ForceBackFire(firePoint.transform.position, weaponClass.forceBackFire);                          // ��� ������ ������
        firePoint.Rotate(0, 0, -randomBulletX);                                                                 // � ���� ���������� �������
    }

    void FireSplit(bool raycast)
    {
        for (int i = 0; i < weaponClass.splitTimes; i++)                            // ���-�� �������� (����������)
        {
            //Debug.Log("Fire");

            // �������
            if (i % 2 == 1)                                                         // ���� ������� ��� ������� (?)
                firePoint.Rotate(0, 0, (weaponClass.splitRecoil * (i + 1)));        // ������� �� �����������                                                           
            else
                firePoint.Rotate(0, 0, (-weaponClass.splitRecoil * (i )));

            if (!raycast)
                FireProjectile();
            if (raycast)
                FireRayCast();

            // ����������
            if (i % 2 == 1)
                firePoint.Rotate(0, 0, (-weaponClass.splitRecoil * (i + 1)));
            else
                firePoint.Rotate(0, 0, (weaponClass.splitRecoil * (i)));                       
        }
    }    


    void FireRayCast()
    {


        // �������
        float randomBulletX = Random.Range(-weaponClass.recoil, weaponClass.recoil);
        
        // �������2�
        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, firePoint.right + new Vector3(randomBulletX, 0, 0), weaponClass.range, weaponClass.layerRayCast);        
        if (hit.collider != null)
        {
            //Debug.Log("Hit!");
            if (hit.collider.TryGetComponent<Fighter>(out Fighter fighter))
            {
                Vector2 vec2 = (fighter.transform.position - player.transform.position).normalized;
                fighter.TakeDamage(weaponClass.damage, vec2, weaponClass.pushForce);                
            }

            // ��������� ��� ��������
            if (tracerEffect)
            {
                TrailRenderer tracer = Instantiate(tracerEffect, firePoint.position, Quaternion.identity);          // ������� ������
                tracer.AddPosition(firePoint.position);                                                             // ��������� �������
                //tracer.transform.SetParent(transform, true); 
                tracer.transform.position = hit.point;                      // �������� ������� ������� ��������
            }

            
            if (weaponClass.ignite)
            {
                if (hit.collider.TryGetComponent<Ignitable>(out Ignitable ignitable))
                {
                    //Vector2 vec2 = (fighter.transform.position - player.transform.position).normalized;
                    ignitable.Ignite(weaponClass.damageBurn, weaponClass.cooldownBurn, weaponClass.durationBurn);
                }
            }


            /*            if (!lineRaycast)
                        {
                            lineRaycast = Instantiate(lineRenderer, firePoint.position, Quaternion.identity);
                        }
                        lineRaycast.enabled = true;
                        lineRaycast.SetPosition(0, firePoint.position);
                        lineRaycast.SetPosition(1, hit.point);*/



            if (debug)
                Debug.DrawRay(firePoint.position, firePoint.right * weaponClass.range, Color.yellow);
        }
        


        //Debug.Log("Hit");



        /*        if (Physics2D.Raycast(ray, out hit, Mathf.Infinity, layerRayCast))
                {
                    Debug.DrawLine(ray.origin, hit.point, Color.red, 1.0f);                                     // �����, ������� �����

                    //tracer.transform.position = hit.point;                                                    // �������� ������� ������� ���� 

                    if (hit.collider.TryGetComponent<Fighter>(out Fighter fighter))
                    {
                        fighter.TakeDamage(damage);
                        Vector2 vec2 = (player.transform.position - GameManager.instance.player.transform.position).normalized;
                        fighter.rb2D.AddForce(vec2 * pushForce, ForceMode2D.Impulse);
                    }
                }*/
    }


}
