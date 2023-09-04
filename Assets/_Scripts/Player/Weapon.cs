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
    [HideInInspector] public int ammo;
    /*    bool projectileWeapon;                          // ������ �� ���������
        bool splitProjectileWeapon;                     // ���������
        bool rayCastWeapon;                             // ������� ������
        bool splitRaycastWeapon;                        // ������������� ������
        bool allRaycastWeapon;                          // ���������� ������*/

    [HideInInspector] public string weaponName;     // �������� ������
    [HideInInspector] public float fireRate;        // ���������������� ������ (10 - 0,1 ��������� � �������)
    [HideInInspector] public float nextTimeToFire;  // ��� �������� (����� �������� � ���� ���)
    //public int ammo;                                       // �������    
    float currentDelay;

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
    public LineRenderer lineRenderer;       // ����� ��� ������ (������)
    LineRenderer lineRaycast;               // ����� ��� ������ (������)
    public TrailRenderer tracerEffect;      // ������
    public ParticleSystem effectParticles;  // ������ ������� ������ ��������
    public ParticleSystem startParticles;   // ������ ������� ������ ��� ������ ����

    [Header("������ ������ ��� ��������")]
    public float cameraAmplitudeShake = 1f; // ���������
    public float cameraTimedeShake = 0.1f;  // ������������

    [Header("����")]
    public bool singleShot;                 // ��������� ����
    public bool multiShot;                  // �������� ����
    bool soundStarted;                      // ���� �������
    public AudioSource audioSource;         // �������� ����
    public AudioSource audioSourceTail;     // "�����"

    [Header("��� ���������")]
    float cooldownMessage = 1f;             // ����������� �����
    float lastMessage;                      // ����� ���������� ����� (��� ����������� �����)

    Vector3 originPosition;

    public bool debug;

    void Awake()
    {
        player = GameManager.instance.player;                           // �����
        ammoWeapons = GameManager.instance.ammoManager.ammoWeapons;        // ������
        weaponName = weaponClass.weaponName;                            // ��� ������
        ammo = ammoWeapons[weaponIndexForAmmo].allAmmo;


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
        originPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
        //ammo = GameManager.instance.ammoPack.ammoTompson;
        //currentDelay = delayFire + Time.time;
    }

    private void OnEnable()
    {        
        currentDelay = weaponClass.delayFire;
    }
    private void OnDisable()
    {
        soundStarted = false;               
    }

    private void Update()
    {
        //Debug.Log(currentDelay);

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

        // ������� ���������
        if (!flashEffectAnimator)
        {
            return;
        }

        // ������� ������ ������ � ���������� (�������� ����� ����������)
        if (!singleFlash && Time.time >= nextTimeToFire + 0.1f)
        {
            flashEffectAnimator.SetBool("Fire", false);
            flashActive = false;

            if (lineRaycast)
                lineRaycast.enabled = false;
        }

        // ����
        if (multiShot)
        {
            if (ammoWeapons[weaponIndexForAmmo].allAmmo > 0)            // ���� �������� ������ 0
            {
                if (weaponHolder.fireStart && !soundStarted)            // ���� ������ ��������
                {
                    audioSource.Play();
                    soundStarted = true;
                }
                if (!weaponHolder.fireStart && soundStarted)            // ���� �� ������ ��������
                {
                    audioSource.Stop();
                    if (audioSourceTail)
                        audioSourceTail.Play();
                    soundStarted = false;
                }
            }
            else                                    // ���� ������� �����������
            {
                if (soundStarted)                   // � ����� ����
                {
                    audioSource.Stop();             // ������������� ����
                    if (audioSourceTail)
                        audioSourceTail.Play();     // ������������� "������"
                    soundStarted = false;           // ���� ����������
                }
            }
        }        
    }


    private void FixedUpdate()
    {
        // ��������
        //Debug.Log(originPosition.x);

        // ����������� ������
        if (transform.localPosition.x < originPosition.x)       
        {
            //Debug.Log("Recoil!");
            transform.localPosition = new Vector3(transform.localPosition.x + 0.05f, transform.localPosition.y, transform.localPosition.z);
        }

        // �������� ����� ���������
        if (weaponHolder.fireStart && ammoWeapons[weaponIndexForAmmo].allAmmo > 0)          // ���� ������ �������� � ���� �������
        {
            currentDelay -= 0.02f;

            if (startParticles)
            {
                if (currentDelay > 0)
                {
                    startParticles.Play();
                }
                else
                {
                    startParticles.Stop();
                }
            }
        }

        // ���������, ��� ����������� �������
        if (weaponHolder.fireStart && ammoWeapons[weaponIndexForAmmo].allAmmo <= 0 && Time.fixedTime - lastMessage > cooldownMessage)
        {
            GameManager.instance.CreateFloatingMessage("��� ��������", Color.white, GameManager.instance.player.transform.position);
            lastMessage = Time.fixedTime;
        }

        // ���� ����������� �������
        if (!weaponHolder.fireStart || ammoWeapons[weaponIndexForAmmo].allAmmo <= 0)        // ���� �� ������ �������� ��� ����������� �������
        {
            currentDelay = weaponClass.delayFire;

            if (startParticles)
                startParticles.Stop();
            if (effectParticles)
                effectParticles.Stop();
            return;
        }

        // �������� ����� ���������
        if (currentDelay > 0)
            return;                

        if (Time.time >= nextTimeToFire)     // ���� �������� �������� � �� ������
        {
/*            if (ammoWeapons[weaponIndexForAmmo].allAmmo <= 0)
            {                
                if (startParticles)
                    startParticles.Stop();
                if (effectParticles)
                    effectParticles.Stop();
                return;
            }*/

            transform.localPosition = new Vector3(transform.localPosition.x - weaponClass.effectBackFire, transform.localPosition.y, transform.localPosition.z);    // ������ (������)

            ammoWeapons[weaponIndexForAmmo].allAmmo--;                      // - �������
            nextTimeToFire = Time.time + 1f / weaponClass.fireRate;         // ��������� ��           

            if (((int)weaponClass.weaponType) == 0)
                FireProjectile();                       // ������� �����
            if (((int)weaponClass.weaponType) == 1)
                FireRayCast();                          // ������� ���������
            if (((int)weaponClass.weaponType) == 2)
                FireBoxCast();                          // ������� ����������
            if (((int)weaponClass.weaponType) == 3)
                FireSplit(false);                       // ������� "������"
            if (((int)weaponClass.weaponType) == 4)
                FireSplit(true);                        // ������� "������" ���������
            if (((int)weaponClass.weaponType) == 5)
                FireRayCastAll();                       // ������� ��������� �� ���� (���������)
            if (((int)weaponClass.weaponType) == 6)
                FireBoxCastAll();                       // ������� ���������� �� ���� (���������)

            CMCameraShake.Instance.ShakeCamera(cameraAmplitudeShake, cameraTimedeShake);    // ������ ������

            // ����
            if (flashEffectAnimator != null)        // ���� ���������� ����
                Flash();

            if (effectParticles)
            {
                effectParticles.Play();
                //flameParticles.transform.position = firePoint.transform.position;              
            }

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
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.damage = weaponClass.damage;                                              // ����������� ���� �������
        bulletScript.pushForce = weaponClass.pushForce;                                        // ����������� ���� ������ �������
        bulletScript.enemyToDamageCount = weaponClass.enemyToDamageCount;                      // ������� ������ ������� ������
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
                firePoint.Rotate(0, 0, (-weaponClass.splitRecoil * (i)));

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

            // ���� �� �������
            if (weaponClass.withExplousion)
            {
                Explousion(hit.point);
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
*//*            lineRaycast.SetPosition(1, (hit.point - new Vector2(firePoint.position.x, firePoint.position.y))/2);            
            lineRaycast.SetPosition(2, (hit.point + new Vector2(firePoint.position.x, firePoint.position.y)) / 2);
            lineRaycast.SetPosition(3, hit.point - (hit.point - new Vector2(firePoint.position.x, firePoint.position.y)) / 1.5f);*//*
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

    void Explousion(Vector3 explPosition)
    {
        Collider2D[] collidersHits = Physics2D.OverlapCircleAll(explPosition, weaponClass.radiusExpl, weaponClass.layerExplousion);     // ������� ���� � ������� ������� � ��������
        foreach (Collider2D coll in collidersHits)
        {
            if (coll == null)
            {
                continue;
            }

            if (coll.gameObject.TryGetComponent<Fighter>(out Fighter fighter))
            {
                float distance = Vector2.Distance(transform.position, coll.transform.position);

                Vector2 vec2 = (coll.transform.position - explPosition).normalized;
                fighter.TakeDamage(weaponClass.damageExpl, vec2, weaponClass.pushForceExpl);
            }
            collidersHits = null;
        }

        if (weaponClass.expEffect)
        {
            GameObject effect = Instantiate(weaponClass.expEffect, explPosition, Quaternion.identity);          // ������� ������
            Destroy(effect, 0.5f);                                                                              // ���������� ������ ����� .. ���    
        }
        if (weaponClass.sparksEffect)
        {
            GameObject effect = Instantiate(weaponClass.sparksEffect, explPosition, Quaternion.identity);       // ������� ������
            Destroy(effect, 1);
        }

        CMCameraShake.Instance.ShakeCamera(cameraAmplitudeShake, cameraTimedeShake);            // ������ ������
    }


    void FireRayCastAll()
    {
        // �������
        float randomBulletX = Random.Range(-weaponClass.recoil, weaponClass.recoil);

        // �������2�
        RaycastHit2D[] hits = Physics2D.RaycastAll(firePoint.position, firePoint.right + new Vector3(randomBulletX, 0, 0), weaponClass.range, weaponClass.layerRayCast);
        if (hits != null)
        {
            foreach (RaycastHit2D hit in hits)
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
            }

            if (debug)
                Debug.DrawRay(firePoint.position, firePoint.right * weaponClass.range, Color.yellow);
        }

        player.ForceBackFire(firePoint.transform.position, weaponClass.forceBackFire);                          // ��� ������ ������
    }



    void FireBoxCast()      // (���� �� ���������)
    {
        // �������
        float randomBulletX = Random.Range(-weaponClass.recoil, weaponClass.recoil);

        // �������2�
        //RaycastHit2D hit = Physics2D.Raycast(firePoint.position, firePoint.right + new Vector3(randomBulletX, 0, 0), weaponClass.range, weaponClass.layerRayCast);

        RaycastHit2D hit = Physics2D.BoxCast(firePoint.position, new Vector2(1f, 1f), 0f, firePoint.right + new Vector3(randomBulletX, 0, 0), weaponClass.range, weaponClass.layerRayCast);

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

            //BoxCast(Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance = Mathf.Infinity, int layerMask = Physics2D.AllLayers, float minDepth = -Mathf.Infinity, float maxDepth = Mathf.Infinity);
        }
    }

    void FireBoxCastAll()
    {
        // �������
        float randomBulletX = Random.Range(-weaponClass.recoil, weaponClass.recoil);

        // �������2�        
        RaycastHit2D[] hits = Physics2D.BoxCastAll(firePoint.position, new Vector2(weaponClass.boxSize, weaponClass.boxSize), 0f, firePoint.right + new Vector3(randomBulletX, 0, 0), weaponClass.range, weaponClass.layerRayCast);
        if (hits != null)                       // ���� �� ���-�� ������
        {
            foreach (RaycastHit2D hit in hits)
            {
                bool stop = false;
                if (hit.collider.tag == "Wall")
                {
                    if (tracerEffect)
                    {
                        TrailRenderer tracer = Instantiate(tracerEffect, firePoint.position, Quaternion.identity);          // ������� ������
                        tracer.AddPosition(firePoint.position);                                                             // ��������� �������
                        //tracer.transform.SetParent(transform, true); 
                        tracer.transform.position = hit.point;            // �������� ������� ������� ��������
                    }
                    stop = true;
                }
                if (stop)
                    break;

                //Debug.Log("Hit!");
                if (hit.collider.TryGetComponent<Fighter>(out Fighter fighter))
                {
                    Vector2 vec2 = (fighter.transform.position - player.transform.position).normalized;
                    fighter.TakeDamage(weaponClass.damage, vec2, weaponClass.pushForce);
                }

                if (weaponClass.ignite)
                {
                    if (hit.collider.TryGetComponent<Ignitable>(out Ignitable ignitable))
                    {
                        //Vector2 vec2 = (fighter.transform.position - player.transform.position).normalized;
                        ignitable.Ignite(weaponClass.damageBurn, weaponClass.cooldownBurn, weaponClass.durationBurn);
                    }
                }
                hits = null;

                /*                if (tracerEffect)
                                {
                                    TrailRenderer tracer = Instantiate(tracerEffect, firePoint.position, Quaternion.identity);          // ������� ������
                                    tracer.AddPosition(firePoint.position);                                                             // ��������� �������
                                    //tracer.transform.SetParent(transform, true); 
                                    tracer.transform.position = tracer.transform.position + firePoint.right * 20;                      // �������� ������� ������� ��������
                                }*/
            }

            if (debug)
            {
                //Debug.Log("Ok!");
                Debug.DrawRay(firePoint.position, firePoint.right * weaponClass.range, Color.yellow);
            }
        }

        else                // ���� �� �� ��� �� ������
        {
            // ��������� ��� ��������
            if (tracerEffect)
            {
                TrailRenderer tracer = Instantiate(tracerEffect, firePoint.position, Quaternion.identity);          // ������� ������
                tracer.AddPosition(firePoint.position);                                                             // ��������� �������
                //tracer.transform.SetParent(transform, true); 
                tracer.transform.position = tracer.transform.position + firePoint.right * 20;                      // �������� ������� ������� ��������
            }
        }

        player.ForceBackFire(firePoint.transform.position, weaponClass.forceBackFire);                          // ��� ������ ������
    }
}