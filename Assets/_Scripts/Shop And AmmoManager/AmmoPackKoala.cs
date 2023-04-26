using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoPackKoala : MonoBehaviour
{
    Player player;

    [Header("ќружие")]
    public AmmoPackStore[] ammoWeapons;         // ссылка на оружие в инвентаре (название, цена, патроны)
    public AmmoPackStore[] ammoMeleeWeapons;    // ссылка на мили оружие в инвентаре (название, цена, патроны)
    public AmmoPackStore[] ammoBombs;           // ссылка на бомбы в инвентаре (название, цена, патроны)

    [Header(" нопки дл€ замены")]
    public GameObject[] buttonsBuyRangeWeapon;
    public GameObject[] buttonsBuyMeleeWeapon;
    public GameObject[] buttonsBuyBomb;

    public GameObject[] buttonsSellRangeWeapon;
    public GameObject[] buttonsSellMeleeWeapon;
    public GameObject[] buttonsSellBomb;


    private void Awake()
    {
        player = GameManager.instance.player;        
    }

    // ѕатроны
    public void BuyAmmo(int index)                  
    {
        if (GameManager.instance.gold >= ammoWeapons[index].goldPriseAmmo)          // если золота больше чем стоимость оружи€
        {
            GameManager.instance.gold -= ammoWeapons[index].goldPriseAmmo;          // вычитаем из золота стоимость оружи€
            ammoWeapons[index].allAmmo += ammoWeapons[index].ammoInReload;          // добавл€ем во все патроны кол-во патронов в обойме
            CreateMessage("+ " + ammoWeapons[index].ammoInReload + " боеприпасов");
        }
        else
        {
            CreateMessage("Ќедостаточно золота!");            
        }
    }


    // –енж оружие
    public void BuyRangeWeapon(int index)
    {
        if (GameManager.instance.gold >= ammoWeapons[index].goldPriseWeapon)            // если золота больше чем стоимость оружи€
        {
            buttonsBuyRangeWeapon[index].SetActive(false);                              // кнопка покупки
            buttonsSellRangeWeapon[index].SetActive(true);                              // кнопка продажи

            GameManager.instance.gold -= ammoWeapons[index].goldPriseWeapon;            // вычитаем из золота стоимость оружи€

            player.weaponHolder.weapons.Add(ammoWeapons[index].weapon);                 // добавл€ем оружие в список оружий
            player.weaponHolder.BuyWeapon(player.weaponHolder.weapons.Count - 1);       // создаем его в инвентаре игрока                                                                                
            //if (player.weaponHolder.weapons.Count - 1 > 0)                              // (длинна списка - 1 и будет номер последнего добавленного оружи€)
            if (!player.weaponHolder.meleeWeapon)
            {
                player.weaponHolder.selectedWeapon = player.weaponHolder.weapons.Count - 1;
                player.weaponHolder.SelectWeapon();                                     // выбрать оружие 
            }
            CreateMessage(ammoWeapons[index].name + "  уплено!");
        }
        else
        {
            CreateMessage("Ќедостаточно золота!");
        } 
    }


    // ѕродать ренж оружие (пока не сделал)
    public void SellRangeWeapon(int index)
    {
        buttonsBuyRangeWeapon[index].SetActive(true);
        buttonsSellRangeWeapon[index].SetActive(false);

        GameManager.instance.gold += ammoWeapons[index].goldPriseWeapon;            // возвращаем золото 

        //player.weaponHolder.weapons.Remove
    }


    // ћили оружие
    public void BuyMeleeWeapon(int index)
    {

        if (GameManager.instance.gold >= ammoMeleeWeapons[index].goldPriseWeapon)            // если золота больше чем стоимость оружи€
        {
            buttonsBuyMeleeWeapon[index].SetActive(false);                                         // убираем кнопку покупки
            buttonsSellMeleeWeapon[index].SetActive(true);

            GameManager.instance.gold -= ammoMeleeWeapons[index].goldPriseWeapon;            // вычитаем из золота стоимость оружи€

            player.weaponHolderMelee.weapons.Add(ammoMeleeWeapons[index].weapon);                    // добавл€ем оружие в список оружий
            player.weaponHolderMelee.BuyWeapon(player.weaponHolderMelee.weapons.Count - 1);     // создаем его в инвентаре игрока                                                                                
                                                                                                //if (player.weaponHolder.weapons.Count - 1 > 0)                              // (длинна списка - 1 и будет номер последнего добавленного оружи€)
            if (player.weaponHolder.meleeWeapon)
            {
                player.weaponHolderMelee.selectedWeapon = player.weaponHolderMelee.weapons.Count - 1;
                player.weaponHolderMelee.SelectWeapon();                                        // выбрать оружие 
            }
            CreateMessage(ammoMeleeWeapons[index].name + "  уплено!");
        }
        else
        {
            CreateMessage("Ќедостаточно золота!");
        }
    }


    // Ѕомбы
    public void BuyBomb(int index)
    {
        if (GameManager.instance.gold >= ammoBombs[index].goldPriseWeapon)              // если золота больше чем стоимость оружи€
        {
            buttonsBuyBomb[index].SetActive(false);
            buttonsSellBomb[index].SetActive(true);

            GameManager.instance.gold -= ammoBombs[index].goldPriseWeapon;              // вычитаем из золота стоимость оружи€

            player.bombWeaponHolder.weapons.Add(ammoBombs[index].weapon);                 // добавл€ем оружие в список оружий 
            player.bombWeaponHolder.BuyWeapon(player.bombWeaponHolder.weapons.Count - 1);   // создаем его в инвентаре игрока                                                                                
            //if (player.weaponHolder.weapons.Count - 1 > 0)                              // (длинна списка - 1 и будет номер последнего добавленного оружи€)

            player.bombWeaponHolder.selectedWeapon = player.bombWeaponHolder.weapons.Count - 1;
            player.bombWeaponHolder.SelectWeapon();                                         // выбрать оружие 
            CreateMessage(ammoBombs[index].name + "  уплено!");
        }
        else
        {
            CreateMessage("Ќедостаточно золота!");
        }
    }

    public void BuyAmmoBomb(int index)
    {
        if (GameManager.instance.gold >= ammoBombs[index].goldPriseAmmo)            // если золота больше чем стоимость оружи€
        {
            GameManager.instance.gold -= ammoBombs[index].goldPriseAmmo;            // вычитаем из золота стоимость оружи€             
            ammoBombs[index].allAmmo += ammoBombs[index].ammoInReload; ;
            CreateMessage("+ " + ammoBombs[index].ammoInReload + " бомб");
        }
        else
        {
            CreateMessage("Ќедостаточно золота!");
        }
    }

    void CreateMessage(string text)
    {
        GameManager.instance.CreateFloatingMessage(text, Color.white, player.transform.position);
    }
}
