using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    Player player;
    //WeaponHolder weaponHolder;                          // ������ �� ������ weaponHolder (��� �����)
    WeaponHolderMelee weaponHolderMelee;
    Animator animator;
    public LayerMask layer;                             // ���� ��� �����

    public string weaponName;
    public bool swoard;
    public bool spear;
    public bool hummer;

    [Header("��������� ������")]
    public Transform hitBox;                            // ��������� ��������
    public int damage = 10;                             // ����
    public float pushForce = 1;                         // ���� ������
    public float radius = 1;                            // ������
    public float cooldown = 1f;                         // ����������� �����
    float lastAttack;                                   // ����� ���������� ����� (��� ����������� �����)

    [Header("��������� �������")]
    public bool ignite;
    public int damageBurn;
    public float cooldownBurn;
    public float durationBurn;

    [Header("��������� ������")]
    public bool electro;
    public int damageElectro;
    public float pushForceElectro;
    public float radiusElectro;
    public float cameraAmplitudeShakeElectro;      // ���������
    public float cameraTimedeShakeElectro;         // ������������

    // ����� 
    public TrailRenderer trail;

    void Start()
    {
        player = GameManager.instance.player;
        //weaponHolder = GameManager.instance.player.weaponHolder;            // ������� ������ weaponHolder
        weaponHolderMelee = GameManager.instance.player.weaponHolderMelee;      // ������� ������ weaponHolder
        animator = GetComponentInParent<Animator>();        
    }

    
    void Update()
    {
        if (weaponHolderMelee.attackHitBoxStart && Time.time - lastAttack > cooldown)          // ���� ������ ��������� � �� ������
        {
            //Debug.Log("Attack!");
            lastAttack = Time.time;                             // ����������� ����� �����
            if (swoard)
                animator.SetTrigger("HitSwoard");
            if (spear)
                animator.SetTrigger("HitSpear");
            if (hummer)
                animator.SetTrigger("HitHummer");
        }
    }

    public void MeleeAttack()
    {
        Collider2D[] collidersHits = Physics2D.OverlapCircleAll(hitBox.position, radius, layer);     // ������� ���� � ������� ������� � ��������
                                                                                                     // 
        // ��� ������� �����, ����� (�������� �����)
        if (electro && collidersHits.Length > 0)        
        {
            Collider2D[] collidersHitsElectro = Physics2D.OverlapCircleAll(hitBox.position, radiusElectro, layer);     // ������� ���� � ������� ������� � ��������
            foreach (Collider2D collEl in collidersHitsElectro)
            {
                if (collEl == null)
                {
                    continue;
                }

                if (collEl.gameObject.TryGetComponent<Fighter>(out Fighter fighterEl))
                {
                    Vector2 vec2 = (collEl.transform.position - player.transform.position).normalized;
                    fighterEl.TakeDamage(damageElectro, vec2, pushForceElectro);
                }
            }

            GameObject effect = Instantiate(GameAssets.instance.explousionBlue,
                    hitBox.position, Quaternion.identity);                                      // ������� ������ ��������
            Destroy(effect, 1);                                                                 // ���������� ������ ����� .. ���
            CMCameraShake.Instance.ShakeCamera(cameraAmplitudeShakeElectro, cameraTimedeShakeElectro);        // ������ ������
        }

        // ������� �����
        foreach (Collider2D coll in collidersHits)
        {
            if (coll == null)
            {
                continue;
            }

            if (coll.gameObject.TryGetComponent<Fighter>(out Fighter fighter))
            {
                Vector2 vec2 = (coll.transform.position - player.transform.position).normalized;
                fighter.TakeDamage(damage, vec2, pushForce);                
            }
            if (ignite)
            {
                if (coll.gameObject.TryGetComponent<Ignitable>(out Ignitable ignitable))
                {
                    //Vector2 vec2 = (fighter.transform.position - player.transform.position).normalized;
                    ignitable.Ignite(damageBurn, cooldownBurn, durationBurn);
                }                
            }

            collidersHits = null;
        }
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
