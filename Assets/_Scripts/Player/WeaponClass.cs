using UnityEngine;

[CreateAssetMenu]
public class WeaponClass : ScriptableObject
{
    public enum WeaponType
    {
        projectileWeapon,
        rayCastWeapon,
        boxCastWeapon,
        splitProjectileWeapon,
        splitRaycastWeapon,
        allRaycastWeapon,
        allBoxcastWeapon,
    }

    public string weaponName;

    public WeaponType weaponType;    
    
    [Space(2)]

    [Header("Параметры оружия")]
    public int damage;
    public float fireRate;
    public float bulletSpeed;
    public float pushForce;
    public float forceBackFire;
    public float recoil;
    //public GameObject flashEffect;

    [Header("Снарядовое оружие")]
    public GameObject bulletPrefab;

    [Header("Если рейкаст оружие")]
    public float boxSize;
    public float range;
    public LayerMask layerRayCast;

    [Header("Если оружие Сплит")]
    //public bool splitProjectileWeapon;
    public int splitTimes;
    public float splitRecoil;

    [Header("Параметры поджога")]
    public bool ignite;
    public int damageBurn;
    public float cooldownBurn;
    public float durationBurn;

}
