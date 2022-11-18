using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// ��������� ������, ����� �������������� ��� �������� ������
/// </summary>

public class WeaponHolder : MonoBehaviour
{
    Player player;
    public List<GameObject> weapons;                    // ������ ������    
    //Weapon currentWeapon;                               // ������� ������
    [HideInInspector] public int selectedWeapon = 0;    // ������ ������ (��������� � �������� WeaponHolder)
    [HideInInspector] public bool fireStart;            // ������ ��������
    [HideInInspector] public bool attackHitBoxStart;    // ������ ����� �����
    [HideInInspector] public float aimAngle;            // ���� �������� ��� �������� ������� � ������� � �������������
    Vector3 mousePosition;                              // ��������� ����

    void Start()
    {
        player = GameManager.instance.player;
        //BuyWeapon(0);
        //BuyWeapon(1);
        //BuyWeapon(2);
        int i = 0;
        foreach (GameObject weapon in weapons)
        {            
            BuyWeapon(i);
            i++;
        }
        SelectWeapon();
    }

    private void Update()
    {
        //Debug.Log(weapons.Count - 1);

        // ��������
        if (Input.GetMouseButtonDown(0))
        {
            fireStart = true;                     // �������� ������� �������� � �������� ������
        }
        if (Input.GetMouseButtonUp(0))
        {
            fireStart = false;                    // �������� ������� �������� � �������� ������
        }

        // ���� �����
        if (Input.GetMouseButtonDown(1))
        {
            attackHitBoxStart = true;           // �������� ������� �������� � �������� ������
        }
        if (Input.GetMouseButtonUp(1))
        {
            attackHitBoxStart = false;           // �������� ������� �������� � �������� ������
        }


        // ������� ������
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);                        // ��������� ����                  
        Vector3 aimDirection = mousePosition - transform.position;                                  // ���� ����� ���������� ���� � pivot ������          
        aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;                     // ������� ���� � ��������             
        Quaternion qua1 = Quaternion.Euler(0, 0, aimAngle);                                         // ������� ���� ���� � Quaternion
        transform.rotation = Quaternion.Lerp(transform.rotation, qua1, Time.fixedDeltaTime * 15);   // ������ Lerp ����� weaponHoder � ����� �����
        //Debug.Log(aimAngle);



        // ����� ������
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

        /*        if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    selectedWeapon = 0;
                }
                if (Input.GetKeyDown(KeyCode.Alpha2) && transform.childCount >= 2)
                {
                    selectedWeapon = 1;
                }
                if (Input.GetKeyDown(KeyCode.Alpha3) && transform.childCount >= 3)
                {
                    selectedWeapon = 2;
                }
                if (Input.GetKeyDown(KeyCode.Alpha3) && transform.childCount >= 4)
                {
                    selectedWeapon = 3;
                }*/

        if (previousWeapon != selectedWeapon)               // ���� ������ ������ ��������� - �������� �������
        {
            SelectWeapon();
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
                weapon.gameObject.SetActive(true);                                      // ���������� ������ � ��������
                //currentWeapon = weapon.gameObject.GetComponentInChildren<Weapon>();     // �������� ��� ������
            }
            else
                weapon.gameObject.SetActive(false);                                     // ��������� ������ �������������
            i++;
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