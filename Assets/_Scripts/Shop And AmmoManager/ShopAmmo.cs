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
    AudioSource audioSource;

    private void Awake()
    {
        ammoPack = GameManager.instance.ammoManager;
        audioSource = GetComponent<AudioSource>();
        //ammoWeapons = GameManager.instance.ammoPack.ammoWeapons;        // ������
    }

    // ������������� ����� ��� �������� ��������
    public void SetTextWeapon()
    {
        if (GameManager.instance.player.weaponHolder.currentWeapon)
        {
            int index = GameManager.instance.GetCurrentWeaponIndex();
            textMeshName.text = ammoPack.ammoWeapons[index].name;
            textMeshAmmo.text = ammoPack.ammoWeapons[index].ammoInReload.ToString();
            textMeshGold.text = ammoPack.ammoWeapons[index].goldPriseAmmo.ToString();
        }
        else
        {
            textMeshName.text = "-";
            textMeshAmmo.text = "-";
            textMeshGold.text = "-";
        }

    }
    // ������� ��������
    public void BuyAmmoCurrentWeapon()
    {
        if (GameManager.instance.player.weaponHolder.currentWeapon)
        {
            int index = GameManager.instance.GetCurrentWeaponIndex();       // ������� ������
            ammoPack.BuyAmmo(index);                                        // �������� ������ �� �������
            audioSource.Play();
        }
    }


    // ������������� ����� ��� �������� ����
    public void SetTextBomb()
    {
        if (GameManager.instance.player.bombWeaponHolder.currentWeapon)
        {
            int index = GameManager.instance.GetCurrentBombIndex();
            textMeshName.text = ammoPack.ammoBombs[index].name;
            textMeshAmmo.text = ammoPack.ammoBombs[index].ammoInReload.ToString();
            textMeshGold.text = ammoPack.ammoBombs[index].goldPriseAmmo.ToString();
        }
        else
        {
            textMeshName.text = "-";
            textMeshAmmo.text = "-";
            textMeshGold.text = "-";
        }
    }
    // ������� ����
    public void BuyAmmoCurrentBomb()
    {
        if (GameManager.instance.player.bombWeaponHolder.currentWeapon)
        {
            int index = GameManager.instance.GetCurrentBombIndex();         // ������� ������
            ammoPack.BuyAmmoBomb(index);                                    // �������� ������ �� �������
            audioSource.Play();
        }                                  
    }
}
