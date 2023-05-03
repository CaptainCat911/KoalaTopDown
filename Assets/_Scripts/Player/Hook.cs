using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    GameObject hooker;
    Rigidbody2D rb2;


    public float fireRate;        // ���������������� ������ (10 - 0,1 ��������� � �������)
    public float nextTimeToFire;  // ��� �������� (����� �������� � ���� ���)

    private void Awake()
    {
        rb2 = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Time.time >= nextTimeToFire)     // ���� �������� �������� � �� ������
        {            
            nextTimeToFire = Time.time + 1f / fireRate;         // ��������� ��

            //FireHook();
        }
    }

/*    void FireHook()
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
    }*/

    void HookForce()
    {
        rb2.AddForce(transform.right * 3, ForceMode2D.Force);
    }
}
