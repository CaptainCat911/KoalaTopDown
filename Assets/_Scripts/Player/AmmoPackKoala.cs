using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPackKoala : MonoBehaviour
{
    Player player;    
    public AmmoPackStore[] ammoWeapons;     // ������ �� ������ � ��������� (��������, ����, �������)
    public GameObject[] buttons;

    private void Awake()
    {
        player = GameManager.instance.player;        
    }



    // �������
    public void BuyAmmo(int index)                  
    {
        if (GameManager.instance.gold >= ammoWeapons[index].goldPriseAmmo)            // ���� ������ ������ ��� ��������� ������
        {
            GameManager.instance.gold -= ammoWeapons[index].goldPriseAmmo;            // �������� �� ������ ��������� ������

            if (index == 1)                         // ������
            {
                ammoWeapons[index].allAmmo += 10;
            }
            if (index == 2)                         // ���������
            {
                ammoWeapons[index].allAmmo += 20;
            }
        }
        else
        {
            GameObject textPrefab = Instantiate(GameAssets.instance.floatingText, player.transform.position, Quaternion.identity);
            textPrefab.GetComponentInChildren<TextMesh>().text = "������������ ������!";
            textPrefab.GetComponentInChildren<TextMesh>().color = Color.white;
            textPrefab.GetComponentInChildren<Animator>().SetFloat("FloatType", 0);
        }
    }


    // ���� ������
    public void BuyRangeWeapon(int index)
    {
        if (GameManager.instance.gold >= ammoWeapons[index].goldPriseWeapon)            // ���� ������ ������ ��� ��������� ������
        {
            buttons[index].SetActive(false);

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
            GameObject textPrefab = Instantiate(GameAssets.instance.floatingText, player.transform.position, Quaternion.identity);
            textPrefab.GetComponentInChildren<TextMesh>().text = "������������ ������!";
            textPrefab.GetComponentInChildren<TextMesh>().color = Color.white;
            textPrefab.GetComponentInChildren<Animator>().SetFloat("FloatType", 0);
        } 
    }


    // ���� ������
    public void BuyMeleeWeapon(int index)
    {
        player.weaponHolderMelee.weapons.Add(ammoWeapons[index].weapon);                    // ��������� ������ � ������ ������
        player.weaponHolderMelee.BuyWeapon(player.weaponHolderMelee.weapons.Count - 1);     // ������� ��� � ��������� ������                                                                                
        //if (player.weaponHolder.weapons.Count - 1 > 0)                              // (������ ������ - 1 � ����� ����� ���������� ������������ ������)
        if (player.weaponHolder.meleeWeapon)
        {
            player.weaponHolderMelee.selectedWeapon = player.weaponHolderMelee.weapons.Count - 1;
            player.weaponHolderMelee.SelectWeapon();                                        // ������� ������ 
        }
    }
}
