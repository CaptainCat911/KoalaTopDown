using UnityEngine;
using UnityEngine.AI;
public class BotAIWeaponMelee : MonoBehaviour
{
    BotAI botAI;
    BotAIMeleeWeaponHolder weaponHolderMelee;   // ������ �� ������ weaponHolder (��� ��������)
    Animator animator;    

    public string weaponName;
    public Transform hitBox;
    public int damage = 10;                             // ����
    public float pushForce = 1;                         // ���� ������
    public float radius = 1;                            // ������
    public float cooldown = 1f;                         // ����������� �����
    float lastAttack;                                   // ����� ���������� ����� (��� ����������� �����)
    [HideInInspector] public LayerMask layerHit;        // ���� ��� ����� (����� �� �����)

    public GameObject bulletPrefab;                     // ������ ������� ��� ������
    public string attackClass;                          // ��� ����� ������ (1 - ����, 2 - ����, 3 - ������)
    //public bool demon;
    public GameObject[] prefabEnemies;                  // ������ �������� �� ���������
    NavMeshAgent agent;                                 // �� �����

    [Header("��������� ������")]    
    public int explousionDamage;
    public float explousionForce;
    public float explousionRadius;

    // ����� 
    public TrailRenderer trail;

    void Start()
    {
        botAI = GetComponentInParent<BotAI>();
        animator = GetComponentInParent<BotAIAnimator>().GetComponent<Animator>();
        weaponHolderMelee = GetComponentInParent<BotAIMeleeWeaponHolder>();
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

    public void Attack(string type)
    {
        if (Time.time - lastAttack > cooldown)                  // ���� ������ ��������� � �� ������
        {
            lastAttack = Time.time;                             // ����������� ����� �����

            switch (type)
            {
                case "1":
                    {
                        animator.SetTrigger("HitMelee");
                    }
                    break;

                case "2":
                    {
                        animator.SetTrigger("HitRange");
                    }
                    break;

                case "3":
                    {
                        animator.SetTrigger("HitSpawn");
                    }
                    break;
                case "4":
                    {
                        animator.SetTrigger("HitExplousion");
                    }
                    break;
            }
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
        GameObject bullet = Instantiate(bulletPrefab, hitBox.transform.position, hitBox.transform.rotation);              // ������� ������ ������� � �������� � ��������� �����
        bullet.GetComponent<Bullet>().damage = damage;                                                      // ����������� ���� �������
        bullet.GetComponent<Bullet>().pushForce = pushForce;                                                // ����������� ���� ������ �������
        bullet.GetComponent<Rigidbody2D>().AddForce(hitBox.transform.right * 4, ForceMode2D.Impulse);              // ��� �������        
        //botAI.ForceBackFire(firePoint.transform.position, forceBackFire);                                   // ��� ������ ������
        //firePoint.Rotate(0, 0, -randomBulletX);                                                             // � ���� ���������� �������
        if (botAI.isFriendly)
        {
            bullet.layer = LayerMask.NameToLayer("BulletPlayer");           // ���� ����
            bullet.GetComponent<Bullet>().layerExplousion = LayerMask.GetMask("Enemy", "ObjectsDestroyble", "Default");
        }
    }

    public void SpawnAttack()
    {
        int ndx = Random.Range(0, prefabEnemies.Length);            // �������� ������ �� ������� ������
        GameObject go = Instantiate(prefabEnemies[ndx]);            // ������ ������
        //go.transform.SetParent(transform, false);                   // ��������� ���� ������� ���������
        agent = go.GetComponentInChildren<NavMeshAgent>();                    // ������� �����������
        agent.Warp(transform.position);                             // ���������� ������ � ��������
        go.GetComponentInChildren<BotAI>().target = GameManager.instance.player.gameObject;
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
        GameObject effect = Instantiate(GameAssets.instance.explousionStaffEffect,
            hitBox.position, Quaternion.identity);                                      // ������� ������ ��������
        Destroy(effect, 1);                                                             // ���������� ������ ����� .. ���
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
