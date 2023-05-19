using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickUp : ItemPickUp
{
    [Header("Тип оружия")]
    public bool isRange;
    public int weaponIndex;

    public void TakeWeapon()
    {
        if (isRange)
        {
            GameManager.instance.ammoManager.TakeRangeWeapon(weaponIndex);
        }
        else
        {
            GameManager.instance.ammoManager.TakeMeleeWeapon(weaponIndex);
        }
        Destroy(gameObject);
    }
}
