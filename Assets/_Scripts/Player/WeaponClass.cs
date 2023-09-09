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
    public string weaponNameEng;

    public WeaponType weaponType;    
    
    [Space(2)]

    [Header("Параметры оружия")]
    public int damage;
    public float fireRate;
    public float bulletSpeed;
    public float pushForce;
    public int enemyToDamageCount;
    public float recoil;                // разброс
    public float delayFire;             // задердка перед выстрелом
    public float forceBackFire;         // отталкивает назад
    public float effectBackFire;        // эффект отдачи для оружия
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

    [Header("Параметры взрыва")]
    public bool withExplousion;
    public LayerMask layerExplousion;
    public int damageExpl;
    public float radiusExpl;
    public float pushForceExpl;    
    public GameObject expEffect;
    public GameObject sparksEffect;
    public float cameraAmplitudeShake = 3f;     // амплитуда
    public float cameraTimedeShake = 0.1f;      // длительность


    [Header("Параметры поджога")]
    public bool ignite;
    public int damageBurn;
    public float cooldownBurn;
    public float durationBurn;
}
