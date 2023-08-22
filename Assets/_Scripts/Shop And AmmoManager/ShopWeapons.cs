using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopWeapons : MonoBehaviour
{
    AmmoPackKoala ammoPack;             // ������ �� ������� (��� � ������ � ����� � ������� ����� �����)
    AudioSource audioSource;

    public ShopWeaponControl[] rangeWeapons;
    public ShopWeaponControl[] meleeWeapons;
    public ShopWeaponControl[] bombsWeapons;

    public PauseHelp helpPauseBuyWeapon;        // ��������� ��� ������� ������
    public PauseHelp helpPauseBuyBomb;          // ��������� ��� ������� �����
    public PauseHelp helpPauseBuyBomb_2;        // ��������� ��� ������� ����� (��� ����� ���� �����)

    /*    public TextMeshPro[] m_textsRange;
        public TextMeshPro[] m_textsMelee;
        public TextMeshPro[] m_textsBomb;*/

    private void Awake()
    {
        ammoPack = GameManager.instance.ammoManager;
        audioSource = GetComponent<AudioSource>();
    }


    void Start()
    {
        // ��� ���� ������
        int i;
        for (i = 1; i < rangeWeapons.Length; i++)
        {
            rangeWeapons[i].textGold.text = ammoPack.ammoWeapons[i].goldPriseWeapon.ToString();
        }

        // ��� ���� ������
        int j;
        for (j = 1; j < meleeWeapons.Length; j++)
        {
            meleeWeapons[j].textGold.text = ammoPack.ammoMeleeWeapons[j].goldPriseWeapon.ToString();
        }

        // ��� ����
        int k;
        for (k = 1; k < bombsWeapons.Length; k++)
        {
            bombsWeapons[k].textGold.text = ammoPack.ammoBombs[k].goldPriseWeapon.ToString();
        }
    }

    // ������� ������
    public void ShopBuyRangeWeapon(int index)
    {
        if (ammoPack.BuyRangeWeapon(index))
        {
            rangeWeapons[index].WeaponBuyed();
            MakeSound();

            if (!GameManager.instance.weaponHelped)
            {
                helpPauseBuyWeapon.StartHelpPause();              // ��������� ��� ����� ������
                GameManager.instance.weaponHelped = true;
            }            
        }
    }

    public void ShopBuyMeleeWeapon(int index)
    {
        if (ammoPack.BuyMeleeWeapon(index))
        {
            meleeWeapons[index].WeaponBuyed();
            MakeSound();

            if (!GameManager.instance.weaponHelped)
            {
                helpPauseBuyWeapon.StartHelpPause();              // ��������� ��� ����� ������
                GameManager.instance.weaponHelped = true;
            }
        }
    }

    public void ShopBuyBombWeapon(int index)
    {
        if (ammoPack.BuyBomb(index))                // ���� ������ ������� �����
        {
            bombsWeapons[index].WeaponBuyed();      // ��� ������ ("�������!")
            MakeSound();                            // ����

            if (!GameManager.instance.bombHelped)
            {
                helpPauseBuyBomb.StartHelpPause();              // ��������� ��� ������ �����
                GameManager.instance.bombHelped = true;
            }
            else if (!GameManager.instance.bombHelped_2)
            {
                helpPauseBuyBomb_2.StartHelpPause();            // ��������� ��� ����� �����
                GameManager.instance.bombHelped_2 = true;
            }
        }
    }

    void MakeSound()
    {
        audioSource.Play();
    }
}
