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
    AudioSource audioSource;

    private void Awake()
    {
        ammoPack = GameManager.instance.ammoManager;
        audioSource = GetComponent<AudioSource>();
        //ammoWeapons = GameManager.instance.ammoPack.ammoWeapons;        // оружия
    }

    // Устанавливаем текст для автомата патронов
    public void SetTextWeapon()
    {
        if (GameManager.instance.player.weaponHolder.currentWeapon)
        {
            int index = GameManager.instance.GetCurrentWeaponIndex();
            textMeshName.text = ammoPack.ammoWeapons[index].name;
            textMeshAmmo.text = ammoPack.ammoWeapons[index].ammoInReload.ToString();
            textMeshGold.text = ammoPack.ammoWeapons[index].goldPriseAmmo.ToString();
        }
        else
        {
            textMeshName.text = "-";
            textMeshAmmo.text = "-";
            textMeshGold.text = "-";
        }

    }
    // Покупка патронов
    public void BuyAmmoCurrentWeapon()
    {
        if (GameManager.instance.player.weaponHolder.currentWeapon)
        {
            int index = GameManager.instance.GetCurrentWeaponIndex();       // находим индекс
            ammoPack.BuyAmmo(index);                                        // покупаем оружие по индексу
            audioSource.Play();
        }
    }


    // Устанавливаем текст для автомата бомб
    public void SetTextBomb()
    {
        if (GameManager.instance.player.bombWeaponHolder.currentWeapon)
        {
            int index = GameManager.instance.GetCurrentBombIndex();
            textMeshName.text = ammoPack.ammoBombs[index].name;
            textMeshAmmo.text = ammoPack.ammoBombs[index].ammoInReload.ToString();
            textMeshGold.text = ammoPack.ammoBombs[index].goldPriseAmmo.ToString();
        }
        else
        {
            textMeshName.text = "-";
            textMeshAmmo.text = "-";
            textMeshGold.text = "-";
        }
    }
    // Покупка бомб
    public void BuyAmmoCurrentBomb()
    {
        if (GameManager.instance.player.bombWeaponHolder.currentWeapon)
        {
            int index = GameManager.instance.GetCurrentBombIndex();         // находим индекс
            ammoPack.BuyAmmoBomb(index);                                    // покупаем оружие по индексу
            audioSource.Play();
        }                                  
    }
}
