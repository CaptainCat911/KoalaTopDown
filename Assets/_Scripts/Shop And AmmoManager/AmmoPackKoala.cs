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

    [Header("������ ��� ������")]
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

    // �������
    public void BuyAmmo(int index)                  
    {
        if (GameManager.instance.gold >= ammoWeapons[index].goldPriseAmmo)          // ���� ������ ������ ��� ��������� ������
        {
            GameManager.instance.gold -= ammoWeapons[index].goldPriseAmmo;          // �������� �� ������ ��������� ������
            ammoWeapons[index].allAmmo += ammoWeapons[index].ammoInReload;          // ��������� �� ��� ������� ���-�� �������� � ������
            CreateMessage("+ " + ammoWeapons[index].ammoInReload + " �����������");
        }
        else
        {
            CreateMessage("������������ ������!");            
        }
    }


    // ���� ������
    public void BuyRangeWeapon(int index)
    {
        if (GameManager.instance.gold >= ammoWeapons[index].goldPriseWeapon)            // ���� ������ ������ ��� ��������� ������
        {
            buttonsBuyRangeWeapon[index].SetActive(false);
            buttonsSellRangeWeapon[index].SetActive(true);

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
        }
        else
        {
            CreateMessage("������������ ������!");
        } 
    }


    // ������� ���� ������ (���� �� ������)
    public void SellRangeWeapon(int index)
    {
        buttonsBuyRangeWeapon[index].SetActive(true);
        buttonsSellRangeWeapon[index].SetActive(false);

        GameManager.instance.gold += ammoWeapons[index].goldPriseWeapon;            // ���������� ������ 

        //player.weaponHolder.weapons.Remove
    }


    // ���� ������
    public void BuyMeleeWeapon(int index)
    {

        if (GameManager.instance.gold >= ammoMeleeWeapons[index].goldPriseWeapon)            // ���� ������ ������ ��� ��������� ������
        {
            //buttonsBuyWeapon[index].SetActive(false);                                         // ������� ������ �������
            //buttonsSell[index].SetActive(true);

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
        }
        else
        {
            CreateMessage("������������ ������!");
        }
    }


    // �����
    public void BuyBomb(int index)
    {
        if (GameManager.instance.gold >= ammoBombs[index].goldPriseWeapon)            // ���� ������ ������ ��� ��������� ������
        {
            buttonsBuyBomb[index].SetActive(false);

            GameManager.instance.gold -= ammoBombs[index].goldPriseWeapon;            // �������� �� ������ ��������� ������

            player.bombWeaponHolder.weapons.Add(ammoBombs[index].weapon);                 // ��������� ������ � ������ ������
                                                                                          // 
            player.bombWeaponHolder.BuyWeapon(player.bombWeaponHolder.weapons.Count - 1);       // ������� ��� � ��������� ������                                                                                
            //if (player.weaponHolder.weapons.Count - 1 > 0)                              // (������ ������ - 1 � ����� ����� ���������� ������������ ������)

            player.bombWeaponHolder.selectedWeapon = player.bombWeaponHolder.weapons.Count - 1;
            player.bombWeaponHolder.SelectWeapon();                                         // ������� ������ 
            
        }
        else
        {
            CreateMessage("������������ ������!");
        }
    }

    public void BuyAmmoBomb(int index)
    {
        if (GameManager.instance.gold >= ammoBombs[index].goldPriseAmmo)            // ���� ������ ������ ��� ��������� ������
        {
            GameManager.instance.gold -= ammoBombs[index].goldPriseAmmo;            // �������� �� ������ ��������� ������             
            ammoBombs[index].allAmmo += 1;
        }
        else
        {
            CreateMessage("������������ ������!");
        }
    }

    void CreateMessage(string text)
    {
        GameManager.instance.CreateFloatingMessage(text, Color.white, player.transform.position);
    }
}