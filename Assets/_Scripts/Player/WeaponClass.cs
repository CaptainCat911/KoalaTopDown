using UnityEngine;

[CreateAssetMenu]
public class WeaponClass : ScriptableObject
{
    public string weaponName;

    [Header("��� ������")]
    public bool projectileWeapon;
    public bool splitProjectileWeapon;
    public GameObject bulletPrefab;

    public bool rayCastWeapon;
    public LayerMask layerRayCast;

    [Header("��������� ������")]
    public int damage;
    public float fireRate;
    public float bulletSpeed;
    public float pushForce;
    public float forceBackFire;
    public float recoil;
    //public GameObject flashEffect;
}
