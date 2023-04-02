using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// ��������� ������, ����� �������������� ��� �������� ������
/// </summary>

public class WeaponHolderMelee : MonoBehaviour
{
    Player player;
    public WeaponHolder weaponHolder;
    public List<GameObject> weapons;                        // ������ ������
    [HideInInspector] public MeleeWeapon currentWeapon;     // ������� ������ (���� ��� ������ ��� ������ ui)
    [HideInInspector] public int selectedWeapon = 0;        // ������ ������ (��������� � �������� WeaponHolder)   
    [HideInInspector] public bool rangeWeapon = true;       // ���� ��� ���� ������
    [HideInInspector] public bool attackHitBoxStart;        // ������ ����� �����



    private void Awake()
    {
        player = GameManager.instance.player;
    }

    void Start()
    {
        int i = 0;
        foreach (GameObject weapon in weapons)
        {
            BuyWeapon(i);
            i++;
        }
        SelectWeapon();
        HideWeapons();                      // ������ ������
    }

    private void Update()
    {
        if (GameManager.instance.isPlayerEnactive)
        {
            attackHitBoxStart = false;
            return;
        }

        // �����
        if (Input.GetMouseButton(0))
        {
            //if (meleeWeapon)                        // ���� ������ �������� ���
                attackHitBoxStart = true;           // �������� ����� ���������
            //else
                //fireStart = true;                   // ��������
        }
        else
        {
            //if (meleeWeapon)
                attackHitBoxStart = false;
            //else
                //fireStart = false;
        }

        // ����� ������
        if (!rangeWeapon)
        {
            int previousWeapon = selectedWeapon;                                // ����������� ���������� ������ ������

            if (Input.GetAxis("Mouse ScrollWheel") < 0f)                        // ���������� �������� (��� ������� �������)
            {
                if (selectedWeapon >= transform.childCount - 1)                 // ���������� � 0 ������, ���� ������ ����� ���-�� ������� � �������� WeaponHolder - 1(?)
                    selectedWeapon = 0;
                else
                    selectedWeapon++;
            }
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)                        // ���������� �������� (��� ������ �������)
            {
                if (selectedWeapon <= 0)
                    selectedWeapon = transform.childCount - 1;
                else
                    selectedWeapon--;
            }
            if (previousWeapon != selectedWeapon)               // ���� ������ ������ ��������� - �������� �������
            {
                SelectWeapon();
            }
        }
    }


    // ����� ������
    public void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
            {
                weapon.gameObject.SetActive(true);                                          // ���������� ������ � ��������
                currentWeapon = weapon.gameObject.GetComponentInChildren<MeleeWeapon>();    // �������� ��� ������
                weaponHolder.currentWeaponName = currentWeapon.weaponName;                  // �������� ��� ������ ��� ui
            }
            else
                weapon.gameObject.SetActive(false);                                     // ��������� ������ �������������
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
        if (player.leftFlip)            // ���� ��� ������� ����� ������� �����
        {
            player.hitBoxPivot.transform.localScale = new Vector3(transform.localScale.x, 1, transform.localScale.z);   // ������������ ������������
            GameObject weaponGO = Instantiate(weapons[weaponNumber], (transform.position), transform.rotation);         // ������ ������ � "���������" (� ��������)
            weaponGO.transform.SetParent(transform, true);                                                              // ���� ������ ������ ���������
            weaponGO.SetActive(false);                                                                                  // ��������� ��� ������ (��� ������ ������������ SelectWeapon)
            player.hitBoxPivot.transform.localScale = new Vector3(transform.localScale.x, -1, transform.localScale.z);  // ���������� ������������ � �������� ���������
        }
        else
        {
            GameObject weaponGO = Instantiate(weapons[weaponNumber], (transform.position), transform.rotation);
            weaponGO.transform.SetParent(transform, true);
            weaponGO.SetActive(false);
        }
    }
}