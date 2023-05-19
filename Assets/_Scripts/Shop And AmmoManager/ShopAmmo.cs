using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopAmmo : MonoBehaviour
{
    AmmoPackKoala ammoPack;          // ������ �� ������� (��� � ������ � ����� � ������� ����� �����)
    //AmmoPackStore[] ammoWeapons;            // ��� ������ (��������, ������, �������, ���� � ��.)

    public TextMeshPro textMeshName;
    public TextMeshPro textMeshAmmo;
    public TextMeshPro textMeshGold;

    private void Awake()
    {
        ammoPack = GameManager.instance.ammoManager;
        //ammoWeapons = GameManager.instance.ammoPack.ammoWeapons;        // ������
    }


    public void SetTextWeapon()
    {
        int index = GameManager.instance.GetCurrentWeaponIndex();

        textMeshName.text = ammoPack.ammoWeapons[index].name;
        textMeshAmmo.text = ammoPack.ammoWeapons[index].ammoInReload.ToString();
        textMeshGold.text = ammoPack.ammoWeapons[index].goldPriseAmmo.ToString();
    }
    public void BuyAmmoCurrentWeapon()
    {
        int index = GameManager.instance.GetCurrentWeaponIndex();       // ������� ������
        ammoPack.BuyAmmo(index);                                        // �������� ������ �� �������
    }



    public void SetTextBomb()
    {
        int index = GameManager.instance.GetCurrentBombIndex();

        textMeshName.text = ammoPack.ammoBombs[index].name;
        textMeshAmmo.text = ammoPack.ammoBombs[index].ammoInReload.ToString();
        textMeshGold.text = ammoPack.ammoBombs[index].goldPriseAmmo.ToString();
    }

    public void BuyAmmoCurrentBomb()
    {
        int index = GameManager.instance.GetCurrentBombIndex();         // ������� ������
        ammoPack.BuyAmmoBomb(index);                                        // �������� ������ �� �������
    }
}
