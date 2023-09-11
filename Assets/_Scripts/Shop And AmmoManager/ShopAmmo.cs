using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopAmmo : MonoBehaviour
{
    AmmoPackKoala ammoPack;                 // ссылка на аммопак (тут и оружие и бомбы и покупка этого всего)   
    AudioSource audioSource;

    public TextMeshPro textMeshName;        // название оружия
    public TextMeshPro textMeshAmmo;        // кол-во патронов
    public TextMeshPro textMeshGold;        // стоимость золота

    public GameObject automatTextRu;        // ру текст
    public GameObject automatTextEng;


    private void Awake()
    {
        ammoPack = GameManager.instance.ammoManager;
        audioSource = GetComponent<AudioSource>();

        //ammoWeapons = GameManager.instance.ammoPack.ammoWeapons;        // оружия
    }

    private void Start()
    {
        if (LanguageManager.instance.eng)
        {
            automatTextEng.SetActive(true);
        }
        else
        {
            automatTextRu.SetActive(true);
        }
    }

    // Устанавливаем текст для автомата патронов
    public void SetTextWeapon()
    {
        if (GameManager.instance.player.weaponHolder.currentWeapon && !GameManager.instance.player.weaponHolder.meleeWeapon)
        {
            int index = GameManager.instance.GetCurrentWeaponIndex();

            if (LanguageManager.instance.eng)
            {
                textMeshName.text = ammoPack.ammoWeapons[index].nameEng;
            }
            else
            {
                textMeshName.text = ammoPack.ammoWeapons[index].name;
            }
            
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
        if (GameManager.instance.player.weaponHolder.currentWeapon && !GameManager.instance.player.weaponHolder.meleeWeapon)
        {
            int index = GameManager.instance.GetCurrentWeaponIndex();       // находим индекс

            if (ammoPack.BuyAmmo(index))                                    // покупаем оружие по индексу
                audioSource.Play();
        }
    }

    // Устанавливаем текст для автомата бомб
    public void SetTextBomb()
    {
        if (GameManager.instance.player.bombWeaponHolder.currentWeapon)
        {
            int index = GameManager.instance.GetCurrentBombIndex();            

            if (LanguageManager.instance.eng)
            {
                textMeshName.text = ammoPack.ammoBombs[index].nameEng;
            }
            else
            {
                textMeshName.text = ammoPack.ammoBombs[index].name;
            }

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

            if (ammoPack.BuyAmmoBomb(index))                                // покупаем оружие по индексу
                audioSource.Play();
        }                                  
    }
}
