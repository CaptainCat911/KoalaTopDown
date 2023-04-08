using UnityEngine;

[CreateAssetMenu]
public class WeaponClass : ScriptableObject
{
    public string weaponName;

    [Header("Снарядовое оружие")]
    public bool projectileWeapon;
    public bool splitProjectileWeapon;
    public GameObject bulletPrefab;
    public int splitTimes;
    public float splitRecoil;

    [Header("Рейкас оружие")]
    public bool rayCastWeapon;
    public bool splitRaycastWeapon;
    public LayerMask layerRayCast;

    [Header("Параметры оружия")]
    public int damage;
    public float fireRate;
    public float bulletSpeed;
    public float pushForce;
    public float forceBackFire;
    public float recoil;
    //public GameObject flashEffect;
}
