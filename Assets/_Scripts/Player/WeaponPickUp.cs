using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickUp : ItemPickUp
{
    [Header("��� ������")]
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

        if (LanguageManager.instance.eng)
        {
            GameManager.instance.CreateFloatingMessage("Shield!", Color.white, GameManager.instance.player.transform.position);
        }
        else
        {
            GameManager.instance.CreateFloatingMessage("���!", Color.white, GameManager.instance.player.transform.position);
        }
        
        Destroy(gameObject);
    }

    public void TakeMagnet()
    {
        GameManager.instance.player.withGoldMagnet = true;

        if (LanguageManager.instance.eng)
        {
            GameManager.instance.CreateFloatingMessage("Gold magnet!", Color.white, GameManager.instance.player.transform.position);
        }
        else
        {
            GameManager.instance.CreateFloatingMessage("������ ��� ������!", Color.white, GameManager.instance.player.transform.position);
        }

        Destroy(gameObject);
    }

    public void TakeBlink()
    {
        GameManager.instance.player.blink = true;

        if (LanguageManager.instance.eng)
        {
            GameManager.instance.CreateFloatingMessage("Blink!", Color.white, GameManager.instance.player.transform.position);
        }
        else
        {
            GameManager.instance.CreateFloatingMessage("�����!", Color.white, GameManager.instance.player.transform.position);
        }

        Destroy(gameObject);
    }
}
