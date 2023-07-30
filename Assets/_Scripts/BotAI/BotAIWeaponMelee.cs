using UnityEngine;
using UnityEngine.AI;
public class BotAIWeaponMelee : MonoBehaviour
{
    BotAI botAI;
    //BotAIMeleeWeaponHolder weaponHolderMelee;   // ������ �� ������ weaponHolder (��� ��������)
    Animator animator;    
    [HideInInspector] public LayerMask layerHit;    // ���� ��� ����� (����� �� �����)    

    public string weaponName;
    public Transform hitBox;
    //public string attackClass;                      // ��� ����� ������ (1 - ����, 2 - ����, 3 - ������)
    public float cooldown = 1f;                     // ����������� �����
    float lastAttack;                               // ����� ���������� ����� (��� ����������� �����)

    [Header("��������� ���� �����")]
    public int damage = 10;                         // ����
    public float pushForce = 1;                     // ���� ������
    public float radius = 1;                        // ������
    public TrailRenderer trail;
    public bool hitShield;                          // ��������� ���

    [Header("��������� ���� ����� (������)")]
    public GameObject bulletPrefab;                 // ������ �������
    public int damageRange;                         // ���� ����
    public float pushForceRange;                    // ���� ������ ����
    public float projctleSpeed;                     // �������� �������    

    [Header("��������� ���� ����� (�������)")]
    public GameObject bulletPrefabBig;              // ������ �������
    public int damageRangeBig;                      // ���� ����
    public float pushForceRangeBig;                 // ���� ������ ����
    public float projctleSpeedBig;                  // �������� �������
    public bool withSplit;                          // � ������� �� ������ �������

    [Header("��������� ������")]
    public int explousionDamage;                    // ���� ������
    public float explousionForce;                   // ���� ������ ������
    public float explousionRadius;                  // ������ ������
    //public GameObject explousionEffect;             // ������ ������


    [Header("��������� �������")]
    public EnemySpawner[] enemySpawners;            // ������ �������� �� ����������
    public int spawnTimes;                          // ������� ��������� �� ���
    //public GameObject[] prefabEnemies;            // ������ �������� �� ���������
    //NavMeshAgent agent;                           // �� �����

    [Header("��������� �����������")]
    public int splitTimes;                          // ���� ������
    public float splitRecoil;                       // ���� ������ ������    

    [Header("��������� ������")]
    public int laserDamage;                         // ���� ������
    public float laserForce;                        // ���� ������ ������ 
    [HideInInspector] public bool laserStart;       // ������ �������� �����
    public int damageBurn;
    public float cooldownBurn;
    public float durationBurn;
    public ParticleSystem laserEffectParticles;     // ������ ������� ������ ��� ������    

    [Header("��������� �������������� �����")]
    public int gravityDamage;                       // ���� 
    public float gravityForce;                      // ���� ������������ 
    public float gravityRadius;                     // ������ ����������
    public float gravityDistanceToDamage;           // ���������, � ������� ����� ���������� ����
    public float gravityCooldown;                   // ����������� ����������
    float gravityLastAttack;                        // ����� ���������� �����
    [HideInInspector] public bool gravityStart;     // ������ ����������
    public ParticleSystem gravityEffectParticles;   // ������ ������� ������ ��� ������
    public GameObject effectExplGravity;            // ������ ������� ������ ����������

    [Header("��������� ���������")]
    public Transform[] teleports;
    public float teleportForceRadius;
    public float teleportInForce;
    public float teleportOutForce;

    [Header("��������� ����������� �� �������")]
    public int sceneNumberStuff;

    [Header("��� ����")]
    public LayerMask layerRayCastTarget;

    [Header("�����")]
    [HideInInspector] public AudioSource audioSource;
    public AudioWeaponMelee audioWeapon;
    

    //public bool demon;


    void Start()
    {
        botAI = GetComponentInParent<BotAI>();
        animator = GetComponentInParent<BotAIAnimator>().GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        //weaponHolderMelee = GetComponentInParent<BotAIMeleeWeaponHolder>();
        layerHit = botAI.layerHit;
    }


    void Update()
    {
        // �����
/*        if (!weaponHolderMelee.fireStart)                       // ���� �� ������ ��������
        {
            return;                                             // �������
        }*/


    }

    private void FixedUpdate()
    {
        if (laserStart)
        {
            LaserAttack();
        }
        if (gravityStart)
        {
            if (Time.time - gravityLastAttack > gravityCooldown)            // ���� ������ ��������� � �� ������
            {
                gravityLastAttack = Time.time;                             // ����������� ����� �����
                GravityAttack();
            }
        }
    }   

    public void Attack(int type)
    {
        if (Time.time - lastAttack > cooldown)      // ���� ������ ��������� � �� ������
        {
            lastAttack = Time.time;                 // ����������� ����� �����

            animator.SetFloat("HitType", type);     // ��� �����
            animator.SetTrigger("Hit");             // ������ ��� ������ ��������
            if (audioWeapon && !botAI.newNpcSystem)
            {
                audioSource.clip = audioWeapon.hitStart;            // ���� ������
                audioSource.Play();
            }
        }  
    }

    public void LaserOn(int number)
    {
        if (number == 1)
            laserStart = true;
        if (number == 0)
            laserStart = false;

        if (laserEffectParticles)
        {
            if (number == 1)
                laserEffectParticles.Play();
            if (number == 0)
                laserEffectParticles.Stop();
        }
    }    
    
    public void GravityOn(int number)
    {
        if (number == 1)
            gravityStart = true;
        if (number == 0)
            gravityStart = false;

        if (gravityEffectParticles)
        {
            if (number == 1)
                gravityEffectParticles.Play();
            if (number == 0)
                gravityEffectParticles.Stop();
        }
    }


    // ��� ���� ������
    public bool RayCastToTarget(Transform fromTrans, Transform toTrans, float distance)
    {
        if (hitShield)
        {
            return true;
        }
        Vector2 vec2 = (toTrans.transform.position - fromTrans.transform.position).normalized;

        RaycastHit2D hit = Physics2D.Raycast(fromTrans.position, vec2, distance, layerRayCastTarget);

        if (hit.collider != null)
        {
            if (hit.collider.TryGetComponent<Shield>(out Shield shield))
            {
                shield.TakeDamage();
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            return true;
        }
        //Debug.DrawRay(fromTrans.position, vec2, Color.yellow);
    }

    public void MeleeAttack()
    {
        Collider2D[] collidersHits = Physics2D.OverlapCircleAll(hitBox.position, radius, layerHit);     // ������� ���� � ������� ������� � ��������
        foreach (Collider2D coll in collidersHits)
        {
            if (coll == null)
            {
                continue;
            }

            if (coll.gameObject.TryGetComponent<Fighter>(out Fighter fighter))
            {
                float distance = Vector2.Distance(botAI.transform.position, coll.transform.position);

                Vector2 vec2 = (coll.transform.position - botAI.transform.position).normalized;
                fighter.TakeDamage(0, vec2, pushForce);

                if (RayCastToTarget(botAI.transform, coll.transform, distance))     // �������� �� ���
                {
                    fighter.TakeDamage(damage, new Vector2(0, 0), 0);
                }               

                if (audioWeapon)                    
                {
                    audioSource.clip = audioWeapon.hitDone;                 // ���� ���������
                    audioSource.Play();                        
                }
            }
            collidersHits = null;
        }        
    }

    public void RangeAttack()
    {
        //float randomBulletX = Random.Range(-recoilX, recoilX);                                              // ������� ��������
        //firePoint.Rotate(0, 0, randomBulletX);                                                              // ���� �������
        GameObject bullet = Instantiate(bulletPrefab, hitBox.transform.position, hitBox.transform.rotation);              // ������� ������ ������� � �������� � ��������� �����
        bullet.GetComponent<Bullet>().damage = damageRange;                                                      // ����������� ���� �������
        bullet.GetComponent<Bullet>().pushForce = pushForceRange;                                                // ����������� ���� ������ �������
        bullet.GetComponent<Rigidbody2D>().AddForce(hitBox.transform.right * projctleSpeed, ForceMode2D.Impulse);              // ��� �������        
        //botAI.ForceBackFire(firePoint.transform.position, forceBackFire);                                   // ��� ������ ������
        //firePoint.Rotate(0, 0, -randomBulletX);                                                             // � ���� ���������� �������
        if (botAI.isFriendly)
        {
            bullet.layer = LayerMask.NameToLayer("BulletPlayer");           // ���� ����
            bullet.GetComponent<Bullet>().layerExplousion = LayerMask.GetMask("Enemy", "ObjectsDestroyble", "Default");
        }
        if (audioWeapon && !botAI.newNpcSystem)
        {
            audioSource.clip = audioWeapon.hitRange;                 // ���� ���������
            audioSource.Play();
        }
    }

    public void RangeAttackBig()
    {
        //float randomBulletX = Random.Range(-recoilX, recoilX);                                              // ������� ��������
        //firePoint.Rotate(0, 0, randomBulletX);                                                              // ���� �������
        GameObject bullet = Instantiate(bulletPrefabBig, hitBox.transform.position, hitBox.transform.rotation);              // ������� ������ ������� � �������� � ��������� �����
        bullet.GetComponent<Bullet>().damage = damageRangeBig;                                                      // ����������� ���� �������
        bullet.GetComponent<Bullet>().pushForce = pushForceRangeBig;                                                // ����������� ���� ������ �������
        bullet.GetComponent<BulletRocket>().withSplit = withSplit;
        bullet.GetComponent<Rigidbody2D>().AddForce(hitBox.transform.right * projctleSpeedBig, ForceMode2D.Impulse);              // ��� �������        
        //botAI.ForceBackFire(firePoint.transform.position, forceBackFire);                                   // ��� ������ ������
        //firePoint.Rotate(0, 0, -randomBulletX);                                                             // � ���� ���������� �������
        if (botAI.isFriendly)
        {
            bullet.layer = LayerMask.NameToLayer("BulletPlayer");           // ���� ����
            bullet.GetComponent<Bullet>().layerExplousion = LayerMask.GetMask("Enemy", "ObjectsDestroyble", "Default");
        }
        if (audioWeapon)
        {
            audioSource.clip = audioWeapon.hitRangeBig;                 // ���� ���������
            audioSource.Play();
        }
    }

    public void MultiRangeAttack()
    {
        for (int i = 0; i < splitTimes; i++)                            // ���-�� �������� (����������)
        {
            //Debug.Log("Fire");

            // �������
            if (i % 2 == 1)                                         // ���� ������� ��� ������� (?)
                hitBox.Rotate(0, 0, (splitRecoil * (i + 1)));       // ������� �� �����������                                                           
            else
                hitBox.Rotate(0, 0, (-splitRecoil * (i)));

            /*            if (!raycast)
                            FireProjectile();
                        if (raycast)
                            FireRayCast();*/

            RangeAttack();

            // ����������
            if (i % 2 == 1)
                hitBox.Rotate(0, 0, (-splitRecoil * (i + 1)));
            else
                hitBox.Rotate(0, 0, (splitRecoil * (i)));
        }

        if (audioWeapon)
        {
            audioSource.clip = audioWeapon.hitMultiRange;                 // ���� ���������
            audioSource.Play();
        }
    }


    public void SpawnAttack()
    {
        if (enemySpawners.Length < 1)
            return;

        foreach(EnemySpawner enemySpawner in enemySpawners)
        {
            enemySpawner.enemysHowMuch += spawnTimes;
        }

        if (audioWeapon)
        {
            audioSource.clip = audioWeapon.hitSpawn;                 // ���� ���������
            audioSource.Play();
        }

        /*        int ndx = Random.Range(0, prefabEnemies.Length);            // �������� ������ �� ������� ������
                GameObject go = Instantiate(prefabEnemies[ndx]);            // ������ ������
                //go.transform.SetParent(transform, false);                   // ��������� ���� ������� ���������
                agent = go.GetComponentInChildren<NavMeshAgent>();                    // ������� �����������
                agent.Warp(transform.position);                             // ���������� ������ � ��������
                go.GetComponentInChildren<BotAI>().target = GameManager.instance.player.gameObject;*/
    }

    public void ExplousionAttack()
    {
        Collider2D[] collidersHits = Physics2D.OverlapCircleAll(hitBox.position, explousionRadius, layerHit);     // ������� ���� � ������� ������� � ��������
        foreach (Collider2D coll in collidersHits)
        {
            if (coll == null)
            {
                continue;
            }

            if (coll.gameObject.TryGetComponent<Fighter>(out Fighter fighter))
            {
                float distance = Vector2.Distance(botAI.transform.position, coll.transform.position);

                Vector2 vec2 = (coll.transform.position - botAI.transform.position).normalized;
                fighter.TakeDamage(0, vec2, explousionForce);

                if (RayCastToTarget(botAI.transform, coll.transform, distance))
                {
                    fighter.TakeDamage(explousionDamage, new Vector2(0, 0), 0);
                }
            }
            collidersHits = null;
        }
        GameObject effect = Instantiate(GameAssets.instance.explousionRedEffect, hitBox.position, Quaternion.identity);     // ������� ������ ��������
        Destroy(effect, 1);                                                             // ���������� ������ ����� .. ���
    }


    public void LaserAttack()
    {
        // �������2�        
        RaycastHit2D[] hits = Physics2D.BoxCastAll(hitBox.position, new Vector2(0.8f, 0.8f), 0f, hitBox.right, 14, layerHit);
        if (hits != null)
        {
            foreach (RaycastHit2D hit in hits)
            {
                //Debug.Log("Hit!");
                if (hit.collider.TryGetComponent<Fighter>(out Fighter fighter))
                {
                    float distance = Vector2.Distance(hitBox.transform.position, fighter.transform.position);

                    Vector2 vec2 = (fighter.transform.position - hitBox.transform.position).normalized;
                    fighter.TakeDamage(0, vec2, laserForce);

                    if (RayCastToTarget(hitBox.transform, fighter.transform, distance))
                    {
                        fighter.TakeDamage(laserDamage, new Vector2(0, 0), 0);

                        if (hit.collider.TryGetComponent<Ignitable>(out Ignitable ignitable))
                        {
                            //Vector2 vec2 = (fighter.transform.position - player.transform.position).normalized;
                            ignitable.Ignite(damageBurn, cooldownBurn, durationBurn);
                        }
                    }
                }               
            }
        }
    }

    public void GravityAttack()
    {
        Collider2D[] collidersHits = Physics2D.OverlapCircleAll(hitBox.position, gravityRadius, layerHit);     // ������� ���� � ������� ������� � ��������
        foreach (Collider2D coll in collidersHits)
        {
            if (coll == null)
            {
                continue;
            }

            if (coll.gameObject.TryGetComponent<Fighter>(out Fighter fighter))
            {
                float distance = Vector2.Distance(hitBox.transform.position, fighter.transform.position);
                Vector2 vec2 = (coll.transform.position - hitBox.transform.position).normalized;
                if (distance > gravityDistanceToDamage)
                    fighter.TakeDamage(0, -vec2, gravityForce);
                if (distance <= gravityDistanceToDamage)
                    fighter.TakeDamage(gravityDamage, -vec2, gravityForce);
            }
            collidersHits = null;
        }
        if (effectExplGravity)
        {
            GameObject effect = Instantiate(effectExplGravity, hitBox.position, Quaternion.identity);       // ������� ������ 
            Destroy(effect, 0.5f);          // ���������� ������ ����� .. ���
        }
        if (audioWeapon)
        {
            audioSource.clip = audioWeapon.hitGravity;                 // ���� ���������
            audioSource.Play();
        }
    }

    public void Teleport()
    {
        //Debug.Log(teleports.Length);
        if (teleports.Length < 1)
            return;

        Transform point = teleports[Random.Range(0, teleports.Length)];
        float distance = Vector2.Distance(point.position, botAI.transform.position);
        if (distance > 5)
        {
            GameObject effect = Instantiate(GameAssets.instance.explousionTeleportIn, botAI.transform.position, Quaternion.identity);    // ������� ������
            Destroy(effect, 0.5f);                          // ���������� ������ ����� .. ���
            // ������ ����������
            Collider2D[] collidersHitsIn = Physics2D.OverlapCircleAll(botAI.transform.position, teleportForceRadius, layerHit);     // ������� ���� � ������� ������� � ��������
            foreach (Collider2D coll in collidersHitsIn)
            {
                if (coll == null)
                {
                    continue;
                }

                if (coll.gameObject.TryGetComponent<Fighter>(out Fighter fighter))
                {
                    Vector2 vec2 = (coll.transform.position - botAI.transform.position).normalized;
                    fighter.TakeDamage(0, -vec2, teleportInForce);
                }
                collidersHitsIn = null;
            }

            botAI.agent.Warp(point.position);               // ��� ������������� �����

            GameObject effectExit = Instantiate(GameAssets.instance.explousionTeleportOut, botAI.transform.position, Quaternion.identity);    // ������� ������
            Destroy(effectExit, 0.5f);                      // ���������� ������ ����� .. ���     
            // ������ �����
            Collider2D[] collidersHitsOut = Physics2D.OverlapCircleAll(botAI.transform.position, teleportForceRadius, layerHit);     // ������� ���� � ������� ������� � ��������
            foreach (Collider2D coll in collidersHitsOut)
            {
                if (coll == null)
                {
                    continue;
                }

                if (coll.gameObject.TryGetComponent<Fighter>(out Fighter fighter))
                {
                    Vector2 vec2 = (coll.transform.position - botAI.transform.position).normalized;
                    fighter.TakeDamage(0, vec2, teleportOutForce);
                }
                collidersHitsOut = null;
            }
        }
        else
        {
            Teleport();
        }

        if (audioWeapon)
        {
            audioSource.clip = audioWeapon.hitTeleport;                 // ���� ���������
            audioSource.Play();
        }
    }

    public void TimeReverceAttack()
    {
        GameManager.instance.NextScene(sceneNumberStuff);
    }


    public void TrailOn(int number)
    {
        if (trail)
        {
            if (number == 1)
                trail.emitting = true;
            if (number == 0)
                trail.emitting = false;
        }
    }



    

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(hitBox.position, radius);
    }
}
