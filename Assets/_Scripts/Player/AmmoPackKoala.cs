using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPackKoala : MonoBehaviour
{
    public AmmoPackStore[] ammoWeapons;     // ссылка на оружие в инвентаре (название и патроны)
    Player player;

    private void Awake()
    {
        player = GameManager.instance.player;
    }

    public void BuyAmmo(int index)
    {
        if (index == 1)
        {
            ammoWeapons[index].allAmmo += 10;
        }
        if (index == 2)
        {
            ammoWeapons[index].allAmmo += 20;
        }
    }

    public void BuyRangeWeapon(int index)
    {
        player.weaponHolder.weapons.Add(ammoWeapons[index].weapon);                 // добавляем оружие в список оружий
        player.weaponHolder.BuyWeapon(player.weaponHolder.weapons.Count - 1);       // создаем его в инвентаре игрока                                                                                
        //if (player.weaponHolder.weapons.Count - 1 > 0)                              // (длинна списка - 1 и будет номер последнего добавленного оружия)
        if (!player.weaponHolder.meleeWeapon)
        {
            player.weaponHolder.selectedWeapon = player.weaponHolder.weapons.Count - 1;
            player.weaponHolder.SelectWeapon();                                         // выбрать оружие 
        }
    }
}
