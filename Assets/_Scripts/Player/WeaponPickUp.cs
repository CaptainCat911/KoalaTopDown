using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickUp : ItemPickUp
{
    [Header("Тип оружия")]
    public bool isRange;
    public bool isBomb;
    public int weaponIndex;

    public void TakeWeapon()
    {
        if (isBomb)
        {
            GameManager.instance.ammoManager.TakeBomb(weaponIndex);
        }        
        else if (isRange)
        {
            GameManager.instance.ammoManager.TakeRangeWeapon(weaponIndex);
        }
        else
        {
            GameManager.instance.ammoManager.TakeMeleeWeapon(weaponIndex);
        }
        Destroy(gameObject);
    }

    public void TakeShield()
    {
        GameManager.instance.player.withShield = true;
        GameManager.instance.CreateFloatingMessage("Щит получен!", Color.white, GameManager.instance.player.transform.position);
        Destroy(gameObject);
    }

    public void TakeMagnet()
    {
        GameManager.instance.player.withGoldMagnet = true;
        GameManager.instance.CreateFloatingMessage("Магнит для золота получен!", Color.white, GameManager.instance.player.transform.position);
        Destroy(gameObject);
    }

    public void TakeBlink()
    {
        GameManager.instance.player.blink = true;
        GameManager.instance.CreateFloatingMessage("Блинк получен!", Color.white, GameManager.instance.player.transform.position);
        Destroy(gameObject);
    }
}
