using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopWeapons : MonoBehaviour
{
    public ShopWeaponControl[] rangeWeapons;
    public ShopWeaponControl[] meleeWeapons;
    public ShopWeaponControl[] bombsWeapons;
/*    public TextMeshPro[] m_textsRange;
    public TextMeshPro[] m_textsMelee;
    public TextMeshPro[] m_textsBomb;*/
    AmmoPackKoala ammoPack;             // ссылка на аммопак (тут и оружие и бомбы и покупка этого всего)
    AudioSource audioSource;

    private void Awake()
    {
        ammoPack = GameManager.instance.ammoManager;
        audioSource = GetComponent<AudioSource>();
    }


    void Start()
    {
        // для ренж оружия
        int i;
        for (i = 1; i < rangeWeapons.Length; i++)
        {
            rangeWeapons[i].textGold.text = ammoPack.ammoWeapons[i].goldPriseWeapon.ToString();
        }

        // для мили оружия
        int j;
        for (j = 1; j < meleeWeapons.Length; j++)
        {
            meleeWeapons[j].textGold.text = ammoPack.ammoMeleeWeapons[j].goldPriseWeapon.ToString();
        }

        // для бомб
        int k;
        for (k = 1; k < bombsWeapons.Length; k++)
        {
            bombsWeapons[k].textGold.text = ammoPack.ammoBombs[k].goldPriseWeapon.ToString();
        }
    }

    public void ShopBuyRangeWeapon(int index)
    {
        if (ammoPack.BuyRangeWeapon(index))
        {
            rangeWeapons[index].WeaponBuyed();
            MakeSound();
        }
    }

    public void ShopBuyMeleeWeapon(int index)
    {
        if (ammoPack.BuyMeleeWeapon(index))
        {
            meleeWeapons[index].WeaponBuyed();
            MakeSound();
        }
    }

    public void ShopBuyBombWeapon(int index)
    {
        if (ammoPack.BuyBomb(index))
        {
            bombsWeapons[index].WeaponBuyed();
            MakeSound();
        }
    }

    void MakeSound()
    {
        audioSource.Play();
    }
}
