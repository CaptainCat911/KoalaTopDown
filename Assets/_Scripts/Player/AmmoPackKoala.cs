using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPackKoala : MonoBehaviour
{
    Player player;    
    public AmmoPackStore[] ammoWeapons;     // ссылка на оружие в инвентаре (название, цена, патроны)
    public GameObject[] buttons;

    private void Awake()
    {
        player = GameManager.instance.player;        
    }



    // Патроны
    public void BuyAmmo(int index)                  
    {
        if (GameManager.instance.gold >= ammoWeapons[index].goldPriseAmmo)            // если золота больше чем стоимость оружия
        {
            GameManager.instance.gold -= ammoWeapons[index].goldPriseAmmo;            // вычитаем из золота стоимость оружия

            if (index == 1)                         // мушкет
            {
                ammoWeapons[index].allAmmo += 10;
            }
            if (index == 2)                         // револьвер
            {
                ammoWeapons[index].allAmmo += 20;
            }
        }
        else
        {
            GameObject textPrefab = Instantiate(GameAssets.instance.floatingText, player.transform.position, Quaternion.identity);
            textPrefab.GetComponentInChildren<TextMesh>().text = "Недостаточно золота!";
            textPrefab.GetComponentInChildren<TextMesh>().color = Color.white;
            textPrefab.GetComponentInChildren<Animator>().SetFloat("FloatType", 0);
        }
    }


    // Ренж оружие
    public void BuyRangeWeapon(int index)
    {
        if (GameManager.instance.gold >= ammoWeapons[index].goldPriseWeapon)            // если золота больше чем стоимость оружия
        {
            buttons[index].SetActive(false);

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
            GameObject textPrefab = Instantiate(GameAssets.instance.floatingText, player.transform.position, Quaternion.identity);
            textPrefab.GetComponentInChildren<TextMesh>().text = "Недостаточно золота!";
            textPrefab.GetComponentInChildren<TextMesh>().color = Color.white;
            textPrefab.GetComponentInChildren<Animator>().SetFloat("FloatType", 0);
        } 
    }


    // Мили оружие
    public void BuyMeleeWeapon(int index)
    {
        player.weaponHolderMelee.weapons.Add(ammoWeapons[index].weapon);                    // добавляем оружие в список оружий
        player.weaponHolderMelee.BuyWeapon(player.weaponHolderMelee.weapons.Count - 1);     // создаем его в инвентаре игрока                                                                                
        //if (player.weaponHolder.weapons.Count - 1 > 0)                              // (длинна списка - 1 и будет номер последнего добавленного оружия)
        if (player.weaponHolder.meleeWeapon)
        {
            player.weaponHolderMelee.selectedWeapon = player.weaponHolderMelee.weapons.Count - 1;
            player.weaponHolderMelee.SelectWeapon();                                        // выбрать оружие 
        }
    }
}
