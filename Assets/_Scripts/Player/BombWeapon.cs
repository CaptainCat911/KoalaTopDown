using UnityEngine;

public class BombWeapon : MonoBehaviour
{
    [Header("Ссылки")]                      // почему-то не отображается
    Player player;
    AmmoPackStore[] ammoBombs;
    public WeaponClass weaponClass;         // ссылка на класс оружия
    public Transform firePoint;             // якорь для снарядов
    public Transform pivot;                 // якорь weaponHolder (используется для прицеливания)
    BombWeaponHolder bombWeaponHolder;      // ссылка на скрипт bombweaponHolder (для броска бомбы)

    //GameObject weaponHolderGO;              // ссылка на объект weaponHolder (для поворота)
    //Vector3 mousePosition;                  // положение мыши

    [Header("Параметры оружия")]
    public int weaponIndexForAmmo;                  // индекс оружия (для патронов)
    [HideInInspector] public string weaponName;     // название оружия
    [HideInInspector] public float fireRate;        // скорострельность оружия (10 - 0,1 выстрелов в секунду)
    [HideInInspector] public float nextTimeToFire;  // для стрельбы (когда стрелять в след раз)
    //public int ammo;                                // кол-во бомб

    /*    GameObject bulletPrefab;                // префаб снаряда
        float bulletSpeed;                      // скорость снаряда
        int damage;                             // урон (возможно нужно сделать на снаряде)
        float pushForce;                        // сила толчка (возможно нужно сделать на снаряде)
        float forceBackFire;                    // отдача оружия
        float recoil;                          // разброс стрельбы
        LayerMask layerRayCast;                 // слои для рейкастов*/

    // Для флипа оружия
    bool needFlip;                          // нужен флип (для правильного отображения оружия)    
    bool leftFlip;                          // оружие слева
    bool rightFlip = true;                  // оружие справа

    void Awake()
    {
        player = GameManager.instance.player;
        ammoBombs = GameManager.instance.ammoPack.ammoBombs;
        weaponName = weaponClass.weaponName;                                    // имя оружия        

/*        layerRayCast = weaponClass.layerRayCast;                                       // слои к рейкастам
        if (weaponClass.bulletPrefab)
            bulletPrefab = weaponClass.bulletPrefab;                                  // тип снаряда (если не рейкаст оружие)
        bulletSpeed = weaponClass.bulletSpeed;                                  // скорость
        damage = weaponClass.damage;                                            // урон
        pushForce = weaponClass.pushForce;                                      // сила толчка
        fireRate = weaponClass.fireRate;                                        // скорострельность
        forceBackFire = weaponClass.forceBackFire;                              // сила отдачи
        recoil = weaponClass.recoil;                                           // разброс стрельбы*/
        //flashEffect = weaponClass.flashEffect;                                  // эффект вспышки при выстреле (флэш) 
    }

    private void Start()
    {
        bombWeaponHolder = GetComponentInParent<BombWeaponHolder>();                    // находим скрипт weaponHolder      
    }

    private void Update()
    {
        // Флип оружия
        if (Mathf.Abs(bombWeaponHolder.aimAngle) > 90 && rightFlip)
        {
            needFlip = true;
            leftFlip = true;
            rightFlip = false;
        }
        if (Mathf.Abs(bombWeaponHolder.aimAngle) <= 90 && leftFlip)
        {
            needFlip = true;
            rightFlip = true;
            leftFlip = false;
        }
        if (needFlip)
        {
            Flip();
        }
    }


    private void FixedUpdate()
    {
        // Стрельба
        if (!bombWeaponHolder.fireStart)                        // если не готовы стрелять
        {
            return;                                         // выходим
        }

        if (Time.time >= nextTimeToFire && ammoBombs[weaponIndexForAmmo].allAmmo > 0)                    // если начинаем стрелять и кд готово
        {
            ammoBombs[weaponIndexForAmmo].allAmmo--;                        // - патроны
            nextTimeToFire = Time.time + 1f / weaponClass.fireRate;         // вычисляем кд
            //if (!rayCastWeapon)
            GameManager.instance.player.animator.SetTrigger("ThrowBomb");   // даём тригер аниматору игрока
            Invoke("FireProjectile", 0.2f);                                 // пока что так, потом привязать бросок к анимации
            /*            if (rayCastWeapon)
                            FireRayCast();*/                                                              // выстрел рейкастом
        }
    }

    // Флип
    void Flip()                                                                                         
    {
        if (leftFlip)                                                                                   // разворот налево
        {
            transform.localScale = new Vector3(transform.localScale.x, -1, transform.localScale.z);     // поворачиваем оружие через scale
        }
        if (rightFlip)
        {
            transform.localScale = new Vector3(transform.localScale.x, 1, transform.localScale.z);             
        }
        needFlip = false;
    }


    public void FireProjectile()
    {
        float randomBulletX = Random.Range(-weaponClass.recoil, weaponClass.recoil);                            // разброс стрельбы
        firePoint.Rotate(0, 0, randomBulletX);                                                                  // тупо вращаем
        GameObject bullet = Instantiate(weaponClass.bulletPrefab, firePoint.position, Quaternion.identity);     // создаем префаб снаряда с позицией и поворотом якоря
        bullet.GetComponent<Bullet>().damage = weaponClass.damage;                                              // присваиваем урон снаряду
        bullet.GetComponent<Bullet>().pushForce = weaponClass.pushForce;                                        // присваиваем силу толчка снаряду

        float dist = Vector3.Distance(transform.position, bombWeaponHolder.mousePosition) - 10f;
        if (dist < 0.3f)
            dist = 0.3f;
        if (dist > 1f)
            dist = 1f;
        //Debug.Log(dist);
        bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.right * (weaponClass.bulletSpeed * (dist + 0.3f)), ForceMode2D.Impulse);    // даём импульс
        player.ForceBackFire(firePoint.transform.position, weaponClass.forceBackFire);                          // даём отдачу оружия
        firePoint.Rotate(0, 0, -randomBulletX);                                                                 // и тупо возвращаем поворот
    }
}
