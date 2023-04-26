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

    [Header("��������� ���� ����� (������)")]
    //public GameObject bulletPrefab;               // ������ ������� ��� ������
    public int damageRange;                         // ���� ����
    public float pushForceRange;                    // ���� ������ ����
    public float projctleSpeed;                     // �������� �������

    [Header("��������� ���� ����� (�������)")]    
    public int damageRangeBig;                      // ���� ����
    public float pushForceRangeBig;                 // ���� ������ ����
    public float projctleSpeedBig;                  // �������� �������

    [Header("��������� ������")]
    public int explousionDamage;                    // ���� ������
    public float explousionForce;                   // ���� ������ ������
    public float explousionRadius;                  // ������ ������

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

    [Header("��������� ���������")]
    public Transform[] teleports;
    public float teleportForceRadius;
    public float teleportInForce;
    public float teleportOutForce;

    //public bool demon;


    void Start()
    {
        botAI = GetComponentInParent<BotAI>();
        animator = GetComponentInParent<BotAIAnimator>().GetComponent<Animator>();
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
        if (Time.time - lastAttack > cooldown)                  // ���� ������ ��������� � �� ������
        {
            lastAttack = Time.time;                             // ����������� ����� �����

            animator.SetFloat("HitType", type);
            animator.SetTrigger("Hit");
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
                Vector2 vec2 = (coll.transform.position - botAI.transform.position).normalized;
                fighter.TakeDamage(damage, vec2, pushForce);                
            }
            collidersHits = null;
        }
    }

    public void RangeAttack()
    {
        //float randomBulletX = Random.Range(-recoilX, recoilX);                                              // ������� ��������
        //firePoint.Rotate(0, 0, randomBulletX);                                                              // ���� �������
        GameObject bullet = Instantiate(GameAssets.instance.fireBallSmall, hitBox.transform.position, hitBox.transform.rotation);              // ������� ������ ������� � �������� � ��������� �����
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
    }

    public void RangeAttackBig()
    {
        //float randomBulletX = Random.Range(-recoilX, recoilX);                                              // ������� ��������
        //firePoint.Rotate(0, 0, randomBulletX);                                                              // ���� �������
        GameObject bullet = Instantiate(GameAssets.instance.fireBallBig, hitBox.transform.position, hitBox.transform.rotation);              // ������� ������ ������� � �������� � ��������� �����
        bullet.GetComponent<Bullet>().damage = damageRangeBig;                                                      // ����������� ���� �������
        bullet.GetComponent<Bullet>().pushForce = pushForceRangeBig;                                                // ����������� ���� ������ �������
        bullet.GetComponent<Rigidbody2D>().AddForce(hitBox.transform.right * projctleSpeedBig, ForceMode2D.Impulse);              // ��� �������        
        //botAI.ForceBackFire(firePoint.transform.position, forceBackFire);                                   // ��� ������ ������
        //firePoint.Rotate(0, 0, -randomBulletX);                                                             // � ���� ���������� �������
        if (botAI.isFriendly)
        {
            bullet.layer = LayerMask.NameToLayer("BulletPlayer");           // ���� ����
            bullet.GetComponent<Bullet>().layerExplousion = LayerMask.GetMask("Enemy", "ObjectsDestroyble", "Default");
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
    }


    public void SpawnAttack()
    {
        if (enemySpawners.Length < 1)
            return;

        foreach(EnemySpawner enemySpawner in enemySpawners)
        {
            enemySpawner.enemysHowMuch += spawnTimes;
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
                Vector2 vec2 = (coll.transform.position - botAI.transform.position).normalized;
                fighter.TakeDamage(explousionDamage, vec2, explousionForce);
            }
            collidersHits = null;
        }
        GameObject effect = Instantiate(GameAssets.instance.explousionRedEffect,
            hitBox.position, Quaternion.identity);                                      // ������� ������ ��������
        Destroy(effect, 1);                                                             // ���������� ������ ����� .. ���
    }


    public void LaserAttack()
    {
        // �������2�        
        RaycastHit2D[] hits = Physics2D.BoxCastAll(hitBox.position, new Vector2(0.8f, 0.8f), 0f, hitBox.right, 15, layerHit);
        if (hits != null)
        {
            foreach (RaycastHit2D hit in hits)
            {
                //Debug.Log("Hit!");
                if (hit.collider.TryGetComponent<Fighter>(out Fighter fighter))
                {
                    Vector2 vec2 = (fighter.transform.position - transform.position).normalized;
                    fighter.TakeDamage(laserDamage, vec2, laserForce);
                }

                if (hit.collider.TryGetComponent<Ignitable>(out Ignitable ignitable))
                {
                    //Vector2 vec2 = (fighter.transform.position - player.transform.position).normalized;
                    ignitable.Ignite(damageBurn, cooldownBurn, durationBurn);
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
        GameObject effect = Instantiate(GameAssets.instance.explousionGravity,
            hitBox.position, Quaternion.identity);                                      // ������� ������ ��������
        Destroy(effect, 0.5f);                                                             // ���������� ������ ����� .. ���
    }

    public void Teleport()
    {
        Debug.Log(teleports.Length);
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
    }

    public void TimeReverceAttack()
    {
        GameManager.instance.NextScene(0);
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
