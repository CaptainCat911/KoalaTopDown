using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("������")]                      // ������-�� �� ������������
    Player player;
    SpriteRenderer spriteRenderer;
    public WeaponClass weaponClass;         // ������ �� ����� ������
    public Transform firePoint;             // ����� ��� ��������
    public Transform pivot;                 // ����� weaponHolder (������������ ��� ������������)
    WeaponHolder weaponHolder;              // ������ �� ������ weaponHolder (��� ��������)
    //GameObject weaponHolderGO;              // ������ �� ������ weaponHolder (��� ��������)
    //Vector3 mousePosition;                  // ��������� ����

    // ��������� ������ (�� ������ ������)
    string weaponName;                      // �������� ������
    public bool rayCastWeapon;
    GameObject bulletPrefab;                // ������ �������
    float bulletSpeed;                      // �������� �������
    int damage;                             // ���� (�������� ����� ������� �� �������)
    float pushForce;                        // ���� ������ (�������� ����� ������� �� �������)
    [HideInInspector] public float fireRate;                // ���������������� ������ (10 - 0,1 ��������� � �������)
    [HideInInspector] public float nextTimeToFire;          // ��� �������� (����� �������� � ���� ���)
    float forceBackFire;                    // ������ ������
    public float recoilX;                   // ������� ��������
    public LayerMask layerRayCast;          //

    // ��� ����� ������
    bool needFlip;                          // ����� ���� (��� ����������� ����������� ������)    
    bool leftFlip;                          // ������ �����
    bool rightFlip = true;                  // ������ ������

    [Header("�������")]
    public Animator flashEffectAnimator;
    public bool singleFlash;
    bool flashEffectActive;
    public TrailRenderer tracerEffect;
    public LineRenderer lineRenderer;

    [Header("������ ������ ��� ��������")]
    public float cameraAmplitudeShake = 1f;     // ���������
    public float cameraTimedeShake = 0.1f;      // ������������


    private void Start()
    {
        player = GameManager.instance.player;
        spriteRenderer = GetComponent<SpriteRenderer>();
        //weaponHolderGO = GetComponentInParent<WeaponHolder>().gameObject;       // ������� ������ weaponHolder
        weaponHolder = GetComponentInParent<WeaponHolder>();                    // ������� ������ weaponHolder
        weaponName = weaponClass.name;                                          // ���
        bulletPrefab = weaponClass.bullet;                                      // ��� �������
        bulletSpeed = weaponClass.bulletSpeed;                                  // ��������
        damage = weaponClass.damage;                                            // ����
        pushForce = weaponClass.pushForce;                                      // ���� ������
        fireRate = weaponClass.fireRate;                                        // ����������������
        forceBackFire = weaponClass.forceBackFire;                              // ���� ������
        //flashEffect = weaponClass.flashEffect;                                  // ������ ������� ��� �������� (����)

        if (lineRenderer)
        {
            lineRenderer = Instantiate(lineRenderer, firePoint.position, Quaternion.identity);
        }
       
    }

    private void Update()
    {
        // ���� ������
        if (Mathf.Abs(weaponHolder.aimAngle) > 90 && rightFlip)
        {
            needFlip = true;
            leftFlip = true;
            rightFlip = false;
        }
        if (Mathf.Abs(weaponHolder.aimAngle) <= 90 && leftFlip)
        {
            needFlip = true;
            rightFlip = true;
            leftFlip = false;
        }
        if (needFlip)
        {
            Flip();
        }

        // ������� ������ ������ (�������� ����� ����������)
        if (lineRenderer && Time.time >= nextTimeToFire + 0.1f)
        {
            lineRenderer.enabled = false;
        }
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
        // ��������
        if (weaponHolder.fireStart && Time.time >= nextTimeToFire)                          // ���� �������� �������� � �� ������
        {
            nextTimeToFire = Time.time + 1f / fireRate;                                     // ��������� ��
            if (!rayCastWeapon)
                FireProjectile();                                                           // ������� �����
            if (rayCastWeapon)
                FireRayCast();                                                              // ������� ���������


            CMCameraShake.Instance.ShakeCamera(cameraAmplitudeShake, cameraTimedeShake);    // ������ ������

            // ������ ���� ��� ���������� ������� (����� ��� ��� ������� �������� ����)
            if (singleFlash)
                FlashSingle();
        }

        // ������ ���� ��� �������������
        if (flashEffectAnimator != null && !singleFlash)        // ���� ���������� ����
            Flash();

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
        float randomBulletX = Random.Range(-recoilX, recoilX);
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);              // ������� ������ ������� � �������� � ��������� �����
        bullet.GetComponent<Bullet>().damage = damage;                                                      // ����������� ���� �������
        bullet.GetComponent<Bullet>().pushForce = pushForce;                                                // ����������� ���� ������ �������
        bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.right * bulletSpeed, ForceMode2D.Impulse);              // ��� �������
        //bullet.transform.Rotate(0.0f, 0.0f, -90.0f, Space.Self);                                             // ������������ ������
        player.ForceBackFire(firePoint.transform.position, forceBackFire);                                  // ��� ������ ������
    }

    public void FireRayCast()
    {
        // ��������� ��� ��������
        TrailRenderer tracer = Instantiate(tracerEffect, firePoint.position, Quaternion.identity);          // ������� ������
        tracer.AddPosition(firePoint.position);                                                             // ��������� ������� 

        // �������
        float randomBulletX = Random.Range(-recoilX, recoilX);
        
        // �������2�
        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, firePoint.right + new Vector3(randomBulletX, 0, 0), Mathf.Infinity, layerRayCast);        
        if (hit.collider != null)
        {
            //Debug.Log("Hit!");
            if (hit.collider.TryGetComponent<Fighter>(out Fighter fighter))
            {
                fighter.TakeDamage(damage);
                Vector2 vec2 = (fighter.transform.position - player.transform.position).normalized;
                fighter.rb2D.AddForce(vec2 * pushForce, ForceMode2D.Impulse);
            }
            //tracer.transform.position = hit.point;                                                      // �������� ������� ������� �������� 
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, hit.point);
            //Debug.DrawRay(firePoint.position, firePoint.right * 100f, Color.yellow);
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
