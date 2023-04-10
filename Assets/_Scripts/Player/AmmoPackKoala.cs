using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoPackKoala : MonoBehaviour
{
    Player player;    
    public AmmoPackStore[] ammoWeapons;     // ������ �� ������ � ��������� (��������, ����, �������)
    public AmmoPackStore[] ammoBombs;       // ������ �� ������ � ��������� (��������, ����, �������)
    public GameObject[] buttonsBuyWeapon;
    public GameObject[] buttonsBuyBomb;
    public GameObject[] buttonsSell;

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
        }
        else
        {
            CreateMessage();
        }
    }


    // ���� ������
    public void BuyRangeWeapon(int index)
    {
        if (GameManager.instance.gold >= ammoWeapons[index].goldPriseWeapon)            // ���� ������ ������ ��� ��������� ������
        {
            //buttonsBuyWeapon[index].SetActive(false);
            //buttonsSell[index].SetActive(true);

            GameManager.instance.gold -= ammoWeapons[index].goldPriseWeapon;            // �������� �� ������ ��������� ������

            player.weaponHolder.weapons.Add(ammoWeapons[index].weapon);                 // ��������� ������ � ������ ������
            player.weaponHolder.BuyWeapon(player.weaponHolder.weapons.Count - 1);       // ������� ��� � ��������� ������                                                                                
            //if (player.weaponHolder.weapons.Count - 1 > 0)                              // (������ ������ - 1 � ����� ����� ���������� ������������ ������)
            if (!player.weaponHolder.meleeWeapon)
            {
                player.weaponHolder.selectedWeapon = player.weaponHolder.weapons.Count - 1;
                player.weaponHolder.SelectWeapon();                                         // ������� ������ 
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

        GameManager.instance.gold += ammoWeapons[index].goldPriseWeapon;            // ���������� ������ 

        //player.weaponHolder.weapons.Remove
    }


    // ���� ������
    public void BuyMeleeWeapon(int index)
    {

        if (GameManager.instance.gold >= ammoWeapons[index].goldPriseWeapon)            // ���� ������ ������ ��� ��������� ������
        {
            buttonsBuyWeapon[index].SetActive(false);                                         // ������� ������ �������
            //buttonsSell[index].SetActive(true);

            GameManager.instance.gold -= ammoWeapons[index].goldPriseWeapon;            // �������� �� ������ ��������� ������

            player.weaponHolderMelee.weapons.Add(ammoWeapons[index].weapon);                    // ��������� ������ � ������ ������
            player.weaponHolderMelee.BuyWeapon(player.weaponHolderMelee.weapons.Count - 1);     // ������� ��� � ��������� ������                                                                                
                                                                                                //if (player.weaponHolder.weapons.Count - 1 > 0)                              // (������ ������ - 1 � ����� ����� ���������� ������������ ������)
            if (player.weaponHolder.meleeWeapon)
            {
                player.weaponHolderMelee.selectedWeapon = player.weaponHolderMelee.weapons.Count - 1;
                player.weaponHolderMelee.SelectWeapon();                                        // ������� ������ 
            }
        }
        else
        {
            CreateMessage();
        }
    }



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
            CreateMessage();
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
            CreateMessage();
        }
    }

    void CreateMessage()
    {
        GameObject textPrefab = Instantiate(GameAssets.instance.floatingText, player.transform.position, Quaternion.identity);
        textPrefab.GetComponentInChildren<TextMeshPro>().text = "������������ ������!";
        textPrefab.GetComponentInChildren<TextMeshPro>().color = Color.yellow;
        textPrefab.GetComponentInChildren<Animator>().SetFloat("FloatType", 1);
    }
}
