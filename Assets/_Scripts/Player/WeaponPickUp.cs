using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickUp : ItemPickUp
{   
    public GameObject weaponToPickUp;
    Player player;

    public override void Start()
    {
        player = GameManager.instance.player;
    }

    public void TakeWeapon()
    {
        player.weaponHolder.weapons.Add(weaponToPickUp);                        // добавляем оружие в список оружий
        player.weaponHolder.BuyWeapon(player.weaponHolder.weapons.Count - 1);   // создаем его в инвентаре игрока                                                                                
        if (player.weaponHolder.weapons.Count - 1 > 0)                          // (длинна списка - 1 и будет номер последнего добавленного оружия)
            player.weaponHolder.selectedWeapon++;
        player.weaponHolder.SelectWeapon();                                     // выбрать оружие        
    }
}
