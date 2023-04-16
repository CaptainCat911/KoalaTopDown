using UnityEngine;

[CreateAssetMenu]
public class WeaponClass : ScriptableObject
{
    public string weaponName;

    [Header("���������� ������")]
    public bool projectileWeapon;
    public bool splitProjectileWeapon;
    public GameObject bulletPrefab;  

    [Header("������ ������")]
    public bool rayCastWeapon;
    public bool boxCastWeapon;
    public bool splitRaycastWeapon;
    public bool allRaycastWeapon;
    public bool allBoxcastWeapon;
    public float boxSize;

    public float range;
    public LayerMask layerRayCast;

    [Header("���� ������ �����")]
    public int splitTimes;
    public float splitRecoil;

    [Header("��������� �������")]
    public bool ignite;
    public int damageBurn;
    public float cooldownBurn;
    public float durationBurn;    

    [Header("��������� ������")]
    public int damage;
    public float fireRate;
    public float bulletSpeed;
    public float pushForce;
    public float forceBackFire;
    public float recoil;
    //public GameObject flashEffect;
}
