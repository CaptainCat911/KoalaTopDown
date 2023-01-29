using UnityEngine;

[CreateAssetMenu]
public class WeaponClass : ScriptableObject
{
    public string weaponName;
    public GameObject bulletPrefab;
    public int damage;
    public float fireRate;
    public float pushForce;
    public int bulletSpeed;
    public float forceBackFire;
    public float recoil;
    public bool rayCastWeapon;
    public LayerMask layerRayCast;
    //public GameObject flashEffect;
}
