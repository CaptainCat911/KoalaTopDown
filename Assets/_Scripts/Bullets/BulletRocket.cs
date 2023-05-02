using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletRocket : Bullet
{
    public float expRadius = 3;             // ������ ������

    public enum FireBallPreFab
    {
        smallFireBall,
        bigFireBall
    }


    [Header("����� ��� ������")]
    public bool withSplit;                  // �� �������
    public FireBallPreFab prefabFireBall;   // ������ �������
    public int splitTimes;                  // ���-�� ��������
    public float splitRecoil;               // ������� ����������
    public int damageSplitProjectile;       // ����
    public float pushForceSplitProjectile;  // ������
    public float splitProjectileSpeed;      // �� ��������
    GameObject prefab;

    [Header("� ��������")]
    public bool withTimer;                  // � ��������
    public float timeToExpl;                // ����� �� ������

    [Header("������ ������ ��� ������")]
    public float cameraAmplitudeShake = 3f;     // ���������
    public float cameraTimedeShake = 0.1f;      // ������������

    public LayerMask layerRayCastTarget;

    private void Awake()
    {
        if (((int)prefabFireBall) == 0)
            prefab = GameAssets.instance.fireBallSmall;
        if (((int)prefabFireBall) == 1)
            prefab = GameAssets.instance.fireBallBig;
    }

    private void Start()
    {
        if (withTimer)
            Invoke("Explosion", timeToExpl);                   // ����� ����� �������
    }
    public override void OnTriggerEnter2D(Collider2D collision)
    {
/*        if (collision.gameObject.TryGetComponent<Fighter>(out Fighter fighter))
        {            
            Vector2 vec2 = (collision.transform.position - GameManager.instance.player.transform.position).normalized;
            fighter.rb2D.AddForce(vec2 * pushForce, ForceMode2D.Impulse);
        }*/
        base.OnTriggerEnter2D(collision);           // ��� ����� ���� ���
        Explosion();
    }

    public override void Explosion()
    {
        Collider2D[] collidersHits = Physics2D.OverlapCircleAll(transform.position, expRadius, layerExplousion);     // ������� ���� � ������� ������� � ��������
        foreach (Collider2D coll in collidersHits)
        {
            if (coll == null)
            {
                continue;
            }

            if (coll.gameObject.TryGetComponent<Fighter>(out Fighter fighter))
            {
                float distance = Vector2.Distance(transform.position, coll.transform.position);

                Vector2 vec2 = (coll.transform.position - transform.position).normalized;
                fighter.TakeDamage(0, vec2, pushForce);

                if (RayCastToTarget(transform, coll.transform, distance))
                {
                    fighter.TakeDamage(damage, new Vector2(0, 0), 0);
                }
            }
            collidersHits = null;
        }
        CMCameraShake.Instance.ShakeCamera(cameraAmplitudeShake, cameraTimedeShake);            // ������ ������
        base.Explosion();                                       // ������ ������ � ���������� ��� � ������

        if (withSplit)
        {
            MultiRangeAttack();
        }
    }


    // ��� ���� ������ (�������� ���)
    public bool RayCastToTarget(Transform fromTrans, Transform toTrans, float distance)
    {
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


    public void MultiRangeAttack()
    {
        for (int i = 0; i < splitTimes; i++)                            // ���-�� �������� (����������)
        {           
            // �������
            if (i % 2 == 1)                                         // ���� ������� ��� ������� (?)
                transform.Rotate(0, 0, (splitRecoil * (i + 1)));       // ������� �� �����������                                                           
            else
                transform.Rotate(0, 0, (-splitRecoil * (i)));

            /*            if (!raycast)
                            FireProjectile();
                        if (raycast)
                            FireRayCast();*/

            RangeAttack();

            // ����������
            if (i % 2 == 1)
                transform.Rotate(0, 0, (-splitRecoil * (i + 1)));
            else
                transform.Rotate(0, 0, (splitRecoil * (i)));
        }
    }

    public void RangeAttack()
    {
        //float randomBulletX = Random.Range(-recoilX, recoilX);                                              // ������� ��������
        //firePoint.Rotate(0, 0, randomBulletX);                                                              // ���� �������
        GameObject bullet = Instantiate(prefab, transform.position, transform.rotation);              // ������� ������ ������� � �������� � ��������� �����
        bullet.GetComponent<Bullet>().damage = damageSplitProjectile;                                                      // ����������� ���� �������
        bullet.GetComponent<Bullet>().pushForce = pushForceSplitProjectile;                                                // ����������� ���� ������ �������
        bullet.GetComponent<Rigidbody2D>().AddForce(transform.right * splitProjectileSpeed, ForceMode2D.Impulse);              // ��� �������        
        //botAI.ForceBackFire(firePoint.transform.position, forceBackFire);                                   // ��� ������ ������
        //firePoint.Rotate(0, 0, -randomBulletX);                                                             // � ���� ���������� �������
/*        if (botAI.isFriendly)
        {
            bullet.layer = LayerMask.NameToLayer("BulletPlayer");           // ���� ����
            bullet.GetComponent<Bullet>().layerExplousion = LayerMask.GetMask("Enemy", "ObjectsDestroyble", "Default");
        }*/
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;        
        Gizmos.DrawWireSphere(transform.position, expRadius);
    }
}
