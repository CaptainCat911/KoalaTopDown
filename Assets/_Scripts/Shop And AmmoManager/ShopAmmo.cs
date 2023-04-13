using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopAmmo : MonoBehaviour
{
    AmmoPackKoala ammoPack;          // ссылка на аммопак (тут и оружие и бомбы и покупка этого всего)
    //AmmoPackStore[] ammoWeapons;            // все оружия (название, префаб, патроны, цены и др.)

    public TextMeshPro textMeshName;
    public TextMeshPro textMeshAmmo;
    public TextMeshPro textMeshGold;

    private void Awake()
    {
        ammoPack = GameManager.instance.ammoPack;
        //ammoWeapons = GameManager.instance.ammoPack.ammoWeapons;        // оружия
    }


    public void SetTextWeapon()
    {
        int index = GameManager.instance.GetCurrentWeaponIndex();

        textMeshName.text = ammoPack.ammoWeapons[index].name;
        textMeshAmmo.text = ammoPack.ammoWeapons[index].ammoInReload.ToString();
        textMeshGold.text = ammoPack.ammoWeapons[index].goldPriseAmmo.ToString();
    }



    public void BuyAmmoCurrentWeapon()
    {
        int index = GameManager.instance.GetCurrentWeaponIndex();       // находим индекс
        ammoPack.BuyAmmo(index);                                        // покупаем оружие по индексу
    }
}
