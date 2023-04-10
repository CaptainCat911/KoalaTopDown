using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoPackKoala : MonoBehaviour
{
    Player player;    
    public AmmoPackStore[] ammoWeapons;     // ссылка на оружие в инвентаре (название, цена, патроны)
    public AmmoPackStore[] ammoBombs;       // ссылка на оружие в инвентаре (название, цена, патроны)
    public GameObject[] buttonsBuyWeapon;
    public GameObject[] buttonsBuyBomb;
    public GameObject[] buttonsSell;

    private void Awake()
    {
        player = GameManager.instance.player;        
    }



    // Патроны
    public void BuyAmmo(int index)                  
    {
        if (GameManager.instance.gold >= ammoWeapons[index].goldPriseAmmo)          // если золота больше чем стоимость оружия
        {
            GameManager.instance.gold -= ammoWeapons[index].goldPriseAmmo;          // вычитаем из золота стоимость оружия
            ammoWeapons[index].allAmmo += ammoWeapons[index].ammoInReload;          // добавляем во все патроны кол-во патронов в обойме
        }
        else
        {
            CreateMessage();
        }
    }


    // Ренж оружие
    public void BuyRangeWeapon(int index)
    {
        if (GameManager.instance.gold >= ammoWeapons[index].goldPriseWeapon)            // если золота больше чем стоимость оружия
        {
            //buttonsBuyWeapon[index].SetActive(false);
            //buttonsSell[index].SetActive(true);

            GameManager.instance.gold -= ammoWeapons[index].goldPriseWeapon;            // вычитаем из золота стоимость оружия

            player.weaponHolder.weapons.Add(ammoWeapons[index].weapon);                 // добавляем оружие в список оружий
            player.weaponHolder.BuyWeapon(player.weaponHolder.weapons.Count - 1);       // создаем его в инвентаре игрока                                                                                
            //if (player.weaponHolder.weapons.Count - 1 > 0)                              // (длинна списка - 1 и будет номер последнего добавленного оружия)
            if (!player.weaponHolder.meleeWeapon)
            {
                player.weaponHolder.selectedWeapon = player.weaponHolder.weapons.Count - 1;
                player.weaponHolder.SelectWeapon();                                         // выбрать оружие 
            }
        }
        else
        {
            CreateMessage();
        } 
    }

    public void SellRangeWeapon(int index)
    {
        buttonsBuyWeapon[index].SetActive(true);
        buttonsSell[index].SetActive(false);

        GameManager.instance.gold += ammoWeapons[index].goldPriseWeapon;            // возвращаем золото 

        //player.weaponHolder.weapons.Remove
    }


    // Мили оружие
    public void BuyMeleeWeapon(int index)
    {

        if (GameManager.instance.gold >= ammoWeapons[index].goldPriseWeapon)            // если золота больше чем стоимость оружия
        {
            buttonsBuyWeapon[index].SetActive(false);                                         // убираем кнопку покупки
            //buttonsSell[index].SetActive(true);

            GameManager.instance.gold -= ammoWeapons[index].goldPriseWeapon;            // вычитаем из золота стоимость оружия

            player.weaponHolderMelee.weapons.Add(ammoWeapons[index].weapon);                    // добавляем оружие в список оружий
            player.weaponHolderMelee.BuyWeapon(player.weaponHolderMelee.weapons.Count - 1);     // создаем его в инвентаре игрока                                                                                
                                                                                                //if (player.weaponHolder.weapons.Count - 1 > 0)                              // (длинна списка - 1 и будет номер последнего добавленного оружия)
            if (player.weaponHolder.meleeWeapon)
            {
                player.weaponHolderMelee.selectedWeapon = player.weaponHolderMelee.weapons.Count - 1;
                player.weaponHolderMelee.SelectWeapon();                                        // выбрать оружие 
            }
        }
        else
        {
            CreateMessage();
        }
    }



    public void BuyBomb(int index)
    {
        if (GameManager.instance.gold >= ammoBombs[index].goldPriseWeapon)            // если золота больше чем стоимость оружия
        {
            buttonsBuyBomb[index].SetActive(false);

            GameManager.instance.gold -= ammoBombs[index].goldPriseWeapon;            // вычитаем из золота стоимость оружия

            player.bombWeaponHolder.weapons.Add(ammoBombs[index].weapon);                 // добавляем оружие в список оружий
                                                                                          // 
            player.bombWeaponHolder.BuyWeapon(player.bombWeaponHolder.weapons.Count - 1);       // создаем его в инвентаре игрока                                                                                
            //if (player.weaponHolder.weapons.Count - 1 > 0)                              // (длинна списка - 1 и будет номер последнего добавленного оружия)

            player.bombWeaponHolder.selectedWeapon = player.bombWeaponHolder.weapons.Count - 1;
            player.bombWeaponHolder.SelectWeapon();                                         // выбрать оружие 
            
        }
        else
        {
            CreateMessage();
        }
    }

    public void BuyAmmoBomb(int index)
    {
        if (GameManager.instance.gold >= ammoBombs[index].goldPriseAmmo)            // если золота больше чем стоимость оружия
        {
            GameManager.instance.gold -= ammoBombs[index].goldPriseAmmo;            // вычитаем из золота стоимость оружия             
            ammoBombs[index].allAmmo += 1;
        }
        else
        {
            CreateMessage();
        }
    }

    void CreateMessage()
    {
        GameObject textPrefab = Instantiate(GameAssets.instance.floatingText, player.transform.position, Quaternion.identity);
        textPrefab.GetComponentInChildren<TextMeshPro>().text = "Недостаточно золота!";
        textPrefab.GetComponentInChildren<TextMeshPro>().color = Color.yellow;
        textPrefab.GetComponentInChildren<Animator>().SetFloat("FloatType", 1);
    }
}
