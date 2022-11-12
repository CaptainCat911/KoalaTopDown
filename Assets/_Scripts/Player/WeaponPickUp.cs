using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickUp : ItemPickUp
{   
    public GameObject weaponToPickUp;
    Player player;

    private void Start()
    {
        player = GameManager.instance.player;
    }

    public void TakeWeapon()
    {
        player.weaponHolder.weapons.Add(weaponToPickUp);                        // ��������� ������ � ������ ������
        player.weaponHolder.BuyWeapon(player.weaponHolder.weapons.Count - 1);   // ������� ��� � ��������� ������                                                                                
        if (player.weaponHolder.weapons.Count - 1 > 0)                          // (������ ������ - 1 � ����� ����� ���������� ������������ ������)
            player.weaponHolder.selectedWeapon++;
        player.weaponHolder.SelectWeapon();                                     // ������� ������        
    }
}
