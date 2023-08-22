using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopWeapons : MonoBehaviour
{
    AmmoPackKoala ammoPack;             // ссылка на аммопак (тут и оружие и бомбы и покупка этого всего)
    AudioSource audioSource;

    public ShopWeaponControl[] rangeWeapons;
    public ShopWeaponControl[] meleeWeapons;
    public ShopWeaponControl[] bombsWeapons;

    public PauseHelp helpPauseBuyWeapon;        // подсказка при покупке оружия
    public PauseHelp helpPauseBuyBomb;          // подсказка при покупке бомбы
    public PauseHelp helpPauseBuyBomb_2;        // подсказка при покупке бомбы (для смены типа бомбы)

    /*    public TextMeshPro[] m_textsRange;
        public TextMeshPro[] m_textsMelee;
        public TextMeshPro[] m_textsBomb;*/

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

    // Покупка оружия
    public void ShopBuyRangeWeapon(int index)
    {
        if (ammoPack.BuyRangeWeapon(index))
        {
            rangeWeapons[index].WeaponBuyed();
            MakeSound();

            if (!GameManager.instance.weaponHelped)
            {
                helpPauseBuyWeapon.StartHelpPause();              // подсказка для смены оружия
                GameManager.instance.weaponHelped = true;
            }            
        }
    }

    public void ShopBuyMeleeWeapon(int index)
    {
        if (ammoPack.BuyMeleeWeapon(index))
        {
            meleeWeapons[index].WeaponBuyed();
            MakeSound();

            if (!GameManager.instance.weaponHelped)
            {
                helpPauseBuyWeapon.StartHelpPause();              // подсказка для смены оружия
                GameManager.instance.weaponHelped = true;
            }
        }
    }

    public void ShopBuyBombWeapon(int index)
    {
        if (ammoPack.BuyBomb(index))                // если смогли купитьт бомбу
        {
            bombsWeapons[index].WeaponBuyed();      // для текста ("Куплено!")
            MakeSound();                            // звук

            if (!GameManager.instance.bombHelped)
            {
                helpPauseBuyBomb.StartHelpPause();              // подсказка для броска бомбы
                GameManager.instance.bombHelped = true;
            }
            else if (!GameManager.instance.bombHelped_2)
            {
                helpPauseBuyBomb_2.StartHelpPause();            // подсказка для смены бомбы
                GameManager.instance.bombHelped_2 = true;
            }
        }
    }

    void MakeSound()
    {
        audioSource.Play();
    }
}
