using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    GameObject hooker;
    Rigidbody2D rb2;


    public float fireRate;        // скорострельность оружия (10 - 0,1 выстрелов в секунду)
    public float nextTimeToFire;  // для стрельбы (когда стрелять в след раз)

    private void Awake()
    {
        rb2 = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Time.time >= nextTimeToFire)     // если начинаем стрелять и кд готово
        {            
            nextTimeToFire = Time.time + 1f / fireRate;         // вычисляем кд

            //FireHook();
        }
    }

/*    void FireHook()
    {
        float randomBulletX = Random.Range(-weaponClass.recoil, weaponClass.recoil);                            // разброс стрельбы
        firePoint.Rotate(0, 0, randomBulletX);                                                                  // тупо вращаем
        GameObject bullet = Instantiate(weaponClass.bulletPrefab, firePoint.position, firePoint.rotation);      // создаем префаб снаряда с позицией и поворотом якоря
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.damage = weaponClass.damage;                                              // присваиваем урон снаряду
        bulletScript.pushForce = weaponClass.pushForce;                                        // присваиваем силу толчка снаряду
        bulletScript.enemyToDamageCount = weaponClass.enemyToDamageCount;                      // сколько врагов пробьёт снаряд
        bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.right * weaponClass.bulletSpeed, ForceMode2D.Impulse);    // даём импульс
        player.ForceBackFire(firePoint.transform.position, weaponClass.forceBackFire);                          // даём отдачу оружия
        firePoint.Rotate(0, 0, -randomBulletX);                                                                 // и тупо возвращаем поворот
    }*/

    void HookForce()
    {
        rb2.AddForce(transform.right * 3, ForceMode2D.Force);
    }
}
