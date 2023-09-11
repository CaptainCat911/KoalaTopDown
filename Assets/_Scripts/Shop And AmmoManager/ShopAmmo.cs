using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopAmmo : MonoBehaviour
{
    AmmoPackKoala ammoPack;                 // ������ �� ������� (��� � ������ � ����� � ������� ����� �����)   
    AudioSource audioSource;

    public TextMeshPro textMeshName;        // �������� ������
    public TextMeshPro textMeshAmmo;        // ���-�� ��������
    public TextMeshPro textMeshGold;        // ��������� ������

    public GameObject automatTextRu;        // �� �����
    public GameObject automatTextEng;


    private void Awake()
    {
        ammoPack = GameManager.instance.ammoManager;
        audioSource = GetComponent<AudioSource>();

        //ammoWeapons = GameManager.instance.ammoPack.ammoWeapons;        // ������
    }

    private void Start()
    {
        if (LanguageManager.instance.eng)
        {
            automatTextEng.SetActive(true);
        }
        else
        {
            automatTextRu.SetActive(true);
        }
    }

    // ������������� ����� ��� �������� ��������
    public void SetTextWeapon()
    {
        if (GameManager.instance.player.weaponHolder.currentWeapon && !GameManager.instance.player.weaponHolder.meleeWeapon)
        {
            int index = GameManager.instance.GetCurrentWeaponIndex();

            if (LanguageManager.instance.eng)
            {
                textMeshName.text = ammoPack.ammoWeapons[index].nameEng;
            }
            else
            {
                textMeshName.text = ammoPack.ammoWeapons[index].name;
            }
            
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
        if (GameManager.instance.player.weaponHolder.currentWeapon && !GameManager.instance.player.weaponHolder.meleeWeapon)
        {
            int index = GameManager.instance.GetCurrentWeaponIndex();       // ������� ������

            if (ammoPack.BuyAmmo(index))                                    // �������� ������ �� �������
                audioSource.Play();
        }
    }

    // ������������� ����� ��� �������� ����
    public void SetTextBomb()
    {
        if (GameManager.instance.player.bombWeaponHolder.currentWeapon)
        {
            int index = GameManager.instance.GetCurrentBombIndex();            

            if (LanguageManager.instance.eng)
            {
                textMeshName.text = ammoPack.ammoBombs[index].nameEng;
            }
            else
            {
                textMeshName.text = ammoPack.ammoBombs[index].name;
            }

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

            if (ammoPack.BuyAmmoBomb(index))                                // �������� ������ �� �������
                audioSource.Play();
        }                                  
    }
}
