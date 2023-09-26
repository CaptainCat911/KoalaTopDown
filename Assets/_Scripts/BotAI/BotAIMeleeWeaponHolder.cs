using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// ��������� ���� ������ ��� ����
/// </summary>

public class BotAIMeleeWeaponHolder : MonoBehaviour
{
    BotAI botAI;
    public List<GameObject> weapons;                            // ������ ������
    public List<GameObject> weaponsHardCore;                    // ������ ������ ��� ��������
    [HideInInspector] public BotAIWeaponMelee currentWeapon;    // ������� ������ 
    [HideInInspector] public int selectedWeapon = 0;            // ������ ������ (��������� � �������� WeaponHolder)
    [HideInInspector] public bool fireStart;            // ������ ��������
    bool weaponChanged;                                 // ���� ��� ���� ������
    bool attacking;

    private void Awake()
    {
        botAI = GetComponentInParent<BotAI>();

        if (LanguageManager.instance.hardCoreMode)
        {
            weapons = weaponsHardCore;
        }
    }

    void Start()
    {
        int i = 0;
        foreach (GameObject weapon in weapons)
        {
            BuyWeapon(i);
            i++;
        }

        if (botAI.meleeAttackType)
        {
            SelectWeapon();                                 // �������� ������
            weaponChanged = true;
        }
        else
        {
            HideWeapons();                      // ������ ������
        }
    }

    private void Update()
    {
        // ��������
        if (!botAI.newNpcSystem)            // ���� �� ����
        {
            if (botAI.closeToTarget && botAI.meleeAttackType && !attacking)
            {
                //fireStart = true;                       // ��������
                attacking = true;
                botAI.animator.SetBool("Attacking", true);
                //botAI.animator.SetTrigger("Attack");
            }
            else if (!botAI.closeToTarget && attacking)
            {
                //fireStart = false;                      // �� ��������
                attacking = false;
                botAI.animator.SetBool("Attacking", false);
                //botAI.animator.ResetTrigger("Attack");
            }
        }


        if (botAI.meleeAttackType && !weaponChanged)
        {            
            SelectWeapon();
            weaponChanged = true;
        }
        if (botAI.rangeAttackType && weaponChanged)
        {
            HideWeapons();                      // ������ ������
            weaponChanged = false;
        }


        // ����� ������
        int previousWeapon = selectedWeapon;                    // ����������� ���������� ������ ������

/*        if (botAI.switchMelee)                                  // ������� ������
        {
            if (selectedWeapon >= transform.childCount - 1)     // ���������� � 0 ������, ���� ������ ����� ���-�� ������� � �������� WeaponHolder - 1(?)
                selectedWeapon = 0;
            else
                selectedWeapon++;
            botAI.switchMelee = false;
        }*/


        if (previousWeapon != selectedWeapon)               // ���� ������ ������ ��������� - �������� �������
        {
            SelectWeapon();
        }        
    }


    public void SelectCurrentWeapon(int i)
    {
        selectedWeapon = i;
        SelectWeapon();
    }

    // ����� ������
    public void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
            {
                weapon.gameObject.SetActive(true);                                              // ���������� ������ � ��������
                currentWeapon = weapon.gameObject.GetComponentInChildren<BotAIWeaponMelee>();   // �������� ��� ������                
            }
            else
                weapon.gameObject.SetActive(false);                                             // ��������� ������ �������������
            i++;
        }
    }

    public void HideWeapons()
    {
        foreach (Transform weapon in transform)
        {
            weapon.gameObject.SetActive(false);                                     // ��������� ������ �������������
        }
    }

    // ������� ������ (��������� ������)
    public void BuyWeapon(int weaponNumber)
    {
        GameObject weaponGO = Instantiate(weapons[weaponNumber], (transform.position), transform.rotation);
        weaponGO.transform.SetParent(transform, true);
        weaponGO.SetActive(false);
    }
}