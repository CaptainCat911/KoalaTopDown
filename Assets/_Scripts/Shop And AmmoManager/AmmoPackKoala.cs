using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoPackKoala : MonoBehaviour
{
    Player player;

    [Header("������")]
    public AmmoPackStore[] ammoWeapons;         // ������ �� ������ � ��������� (��������, ����, �������)
    public AmmoPackStore[] ammoMeleeWeapons;    // ������ �� ���� ������ � ��������� (��������, ����, �������)
    public AmmoPackStore[] ammoBombs;           // ������ �� ����� � ��������� (��������, ����, �������)

/*    [Header("������ ��� ������")]
    public GameObject[] buttonsBuyRangeWeapon;
    public GameObject[] buttonsBuyMeleeWeapon;
    public GameObject[] buttonsBuyBomb;

    public GameObject[] buttonsSellRangeWeapon;
    public GameObject[] buttonsSellMeleeWeapon;
    public GameObject[] buttonsSellBomb;*/


    private void Awake()
    {
        player = GameManager.instance.player;        
    }

    // �������
    public bool BuyAmmo(int index)                  
    {
        if (GameManager.instance.gold >= ammoWeapons[index].goldPriseAmmo)          // ���� ������ ������ ��� ��������� ������
        {
            GameManager.instance.gold -= ammoWeapons[index].goldPriseAmmo;          // �������� �� ������ ��������� ������
            ammoWeapons[index].allAmmo += ammoWeapons[index].ammoInReload;          // ��������� �� ��� ������� ���-�� �������� � ������
            CreateMessage("+ " + ammoWeapons[index].ammoInReload + " �����������");
            return true;
        }
        else
        {
            CreateMessage("������������ ������!");
            return false;
        }
    }


    // ���� ������
    public bool BuyRangeWeapon(int index)
    {
        if (GameManager.instance.gold >= ammoWeapons[index].goldPriseWeapon)            // ���� ������ ������ ��� ��������� ������
        {
            if (GameManager.instance.player.weaponHolder.meleeWeapon)
                GameManager.instance.player.weaponHolder.SwapWeapon();                  // ������������� �� ��� ������

            GameManager.instance.gold -= ammoWeapons[index].goldPriseWeapon;            // �������� �� ������ ��������� ������

            player.weaponHolder.weapons.Add(ammoWeapons[index].weapon);                 // ��������� ������ � ������ ������
            player.weaponHolder.BuyWeapon(player.weaponHolder.weapons.Count - 1);       // ������� ��� � ��������� ������                                                                                
            //if (player.weaponHolder.weapons.Count - 1 > 0)                              // (������ ������ - 1 � ����� ����� ���������� ������������ ������)
            if (!player.weaponHolder.meleeWeapon)
            {
                player.weaponHolder.selectedWeapon = player.weaponHolder.weapons.Count - 1;
                player.weaponHolder.SelectWeapon();                                     // ������� ������ 
            }
            CreateMessage(ammoWeapons[index].name + " �������!");
            return true;
        }
        else
        {
            CreateMessage("������������ ������!");
            return false;
        } 
    }


/*    // ������� ���� ������ (���� �� ������)
    public void SellRangeWeapon(int index)
    {
        //buttonsBuyRangeWeapon[index].SetActive(true);
        //buttonsSellRangeWeapon[index].SetActive(false);

        GameManager.instance.gold += ammoWeapons[index].goldPriseWeapon;            // ���������� ������ 

        //player.weaponHolder.weapons.Remove
    }*/


    // ���� ������
    public bool BuyMeleeWeapon(int index)
    {
        if (GameManager.instance.gold >= ammoMeleeWeapons[index].goldPriseWeapon)            // ���� ������ ������ ��� ��������� ������
        {
            //buttonsBuyMeleeWeapon[index].SetActive(false);                                         // ������� ������ �������
            //buttonsSellMeleeWeapon[index].SetActive(true);

            if (!GameManager.instance.player.weaponHolder.meleeWeapon)
                GameManager.instance.player.weaponHolder.SwapWeapon();

            GameManager.instance.gold -= ammoMeleeWeapons[index].goldPriseWeapon;            // �������� �� ������ ��������� ������

            player.weaponHolderMelee.weapons.Add(ammoMeleeWeapons[index].weapon);                    // ��������� ������ � ������ ������
            player.weaponHolderMelee.BuyWeapon(player.weaponHolderMelee.weapons.Count - 1);     // ������� ��� � ��������� ������                                                                                
                                                                                                //if (player.weaponHolder.weapons.Count - 1 > 0)                              // (������ ������ - 1 � ����� ����� ���������� ������������ ������)
            if (player.weaponHolder.meleeWeapon)
            {
                player.weaponHolderMelee.selectedWeapon = player.weaponHolderMelee.weapons.Count - 1;
                player.weaponHolderMelee.SelectWeapon();                                        // ������� ������ 
            }
            CreateMessage(ammoMeleeWeapons[index].name + " �������!");

            return true;
        }
        else
        {
            CreateMessage("������������ ������!");
            return false;
        }
    }


    // �����
    public bool BuyBomb(int index)
    {
        if (GameManager.instance.gold >= ammoBombs[index].goldPriseWeapon)              // ���� ������ ������ ��� ��������� ������
        {
            //buttonsBuyBomb[index].SetActive(false);
            //buttonsSellBomb[index].SetActive(true);

            GameManager.instance.gold -= ammoBombs[index].goldPriseWeapon;              // �������� �� ������ ��������� ������

            player.bombWeaponHolder.weapons.Add(ammoBombs[index].weapon);                 // ��������� ������ � ������ ������ 
            player.bombWeaponHolder.BuyWeapon(player.bombWeaponHolder.weapons.Count - 1);   // ������� ��� � ��������� ������                                                                                
            //if (player.weaponHolder.weapons.Count - 1 > 0)                              // (������ ������ - 1 � ����� ����� ���������� ������������ ������)

            player.bombWeaponHolder.selectedWeapon = player.bombWeaponHolder.weapons.Count - 1;
            player.bombWeaponHolder.SelectWeapon();                                         // ������� ������ 
            CreateMessage(ammoBombs[index].name + " �������!");
            return true;
        }
        else
        {
            CreateMessage("������������ ������!");
            return false;
        }
    }

    public bool BuyAmmoBomb(int index)
    {
        if (GameManager.instance.gold >= ammoBombs[index].goldPriseAmmo)            // ���� ������ ������ ��� ��������� ������
        {
            GameManager.instance.gold -= ammoBombs[index].goldPriseAmmo;            // �������� �� ������ ��������� ������             
            ammoBombs[index].allAmmo += ammoBombs[index].ammoInReload; ;
            CreateMessage("+ " + ammoBombs[index].ammoInReload + " �����");
            return true;
        }
        else
        {
            CreateMessage("������������ ������!");
            return false;
        }
    }


    // ��� �������� ������

    // ������� ���� ������
    public void TakeRangeWeapon(int index)
    {
        if (GameManager.instance.player.weaponHolder.meleeWeapon)
            GameManager.instance.player.weaponHolder.SwapWeapon();

        player.weaponHolder.weapons.Add(ammoWeapons[index].weapon);                 // ��������� ������ � ������ ������
        player.weaponHolder.BuyWeapon(player.weaponHolder.weapons.Count - 1);       // ������� ��� � ��������� ������                                                                                
        //if (player.weaponHolder.weapons.Count - 1 > 0)                              // (������ ������ - 1 � ����� ����� ���������� ������������ ������)
        if (!player.weaponHolder.meleeWeapon)
        {
            player.weaponHolder.selectedWeapon = player.weaponHolder.weapons.Count - 1;
            player.weaponHolder.SelectWeapon();                                     // ������� ������ 
        }
        CreateMessage(ammoWeapons[index].name + " !");

    }
    // ������� ���� ������
    public void TakeMeleeWeapon(int index)
    {
        if (!GameManager.instance.player.weaponHolder.meleeWeapon)
            GameManager.instance.player.weaponHolder.SwapWeapon();

        player.weaponHolderMelee.weapons.Add(ammoMeleeWeapons[index].weapon);                    // ��������� ������ � ������ ������
        player.weaponHolderMelee.BuyWeapon(player.weaponHolderMelee.weapons.Count - 1);     // ������� ��� � ��������� ������                                                                                
                                                                                            //if (player.weaponHolder.weapons.Count - 1 > 0)                              // (������ ������ - 1 � ����� ����� ���������� ������������ ������)
        if (player.weaponHolder.meleeWeapon)
        {
            player.weaponHolderMelee.selectedWeapon = player.weaponHolderMelee.weapons.Count - 1;
            player.weaponHolderMelee.SelectWeapon();                                        // ������� ������ 
        }
        CreateMessage(ammoMeleeWeapons[index].name + " !");  
    }
    // ������� �����
    public void TakeBomb(int index)
    {
        player.bombWeaponHolder.weapons.Add(ammoBombs[index].weapon);                       // ��������� ������ � ������ ������
        player.bombWeaponHolder.BuyWeapon(player.bombWeaponHolder.weapons.Count - 1);       // ������� ��� � ��������� ������                                                                                               

        CreateMessage(ammoBombs[index].name + " !");
    }


    void CreateMessage(string text)
    {
        GameManager.instance.CreateFloatingMessage(text, Color.white, player.transform.position);
    }
}
