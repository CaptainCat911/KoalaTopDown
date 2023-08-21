using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// ��������� ������, ����� �������������� ��� �������� ������
/// </summary>

public class WeaponHolder : MonoBehaviour
{
    Player player;
    public WeaponHolderMelee weaponHolderMelee;         // ������ �� ������ ��� ���� ������
    public List<GameObject> weapons;                    // ������ ������    
    [HideInInspector] public Weapon currentWeapon;      // ������� ������ 
    [HideInInspector] public int selectedWeapon = 0;    // ������ ������ (��������� � �������� WeaponHolder)
    [HideInInspector] public bool fireStart;            // ������ ��������
    
    [HideInInspector] public float aimAngle;            // ���� �������� ��� �������� ������� � ������� � �������������
    Vector3 mousePosition;                              // ��������� ����
    [HideInInspector] public bool meleeWeapon;          // ���� ������ ��� ����
    [HideInInspector] public bool stopHolder;


    [HideInInspector] public string currentWeaponName;  // ��� ������ ui
    bool stopAiming;                                    // ��� ������

    private void Awake()
    {
        player = GameManager.instance.player;
    }

    void Start()
    {
        int i = 0;
        foreach (GameObject weapon in weapons)          // �������� ��� ������ �� ������ ������ ��� ������
        {            
            BuyWeapon(i);
            i++;
        }
        SelectWeapon();                                 // �������� ������
    }

    private void Update()
    {
        // ��� ���� ��������
        if (GameManager.instance.isPlayerEnactive)
        {
            fireStart = false;

            if (GameManager.instance.player.rightFlip)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 0), Time.fixedDeltaTime * 15);   // ������ Lerp ����� weaponHoder � ����� �����
            }
            if (GameManager.instance.player.leftFlip)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 180), Time.fixedDeltaTime * 15);   // ������ Lerp ����� weaponHoder � ����� �����
            }

            return;
        }

        // ������� ������
        if (!stopAiming)
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);                        // ��������� ����                  
            Vector3 aimDirection = mousePosition - transform.position;                                  // ���� ����� ���������� ���� � pivot ������          
            aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;                     // ������� ���� � ��������             
            Quaternion qua1 = Quaternion.Euler(0, 0, aimAngle);                                         // ������� ���� ���� � Quaternion
            transform.rotation = Quaternion.Lerp(transform.rotation, qua1, Time.fixedDeltaTime * 15);   // ������ Lerp ����� weaponHoder � ����� �����
            //Debug.Log(aimAngle);
        }

        if (stopHolder)
            return;

        // ��������
        if (Input.GetMouseButton(0))                    // && player.playerWeaponReady
        {
            //if (meleeWeapon)                        // ���� ������ �������� ���
                //attackHitBoxStart = true;           // �������� ����� ���������
            //else
                fireStart = true;                   // ��������
        }
        else
        {
            //if (meleeWeapon)
                //attackHitBoxStart = false;
            //else
                fireStart = false;                  
        }


        // ����� ������ ����
        if (meleeWeapon)
        {
            int previousWeaponMelee = weaponHolderMelee.selectedWeapon;                                // ����������� ���������� ������ ������

            if (Input.GetAxis("Mouse ScrollWheel") > 0f || Input.GetKeyDown(KeyCode.Alpha1))                        // ���������� �������� (��� ������� �������)
            {
                if (weaponHolderMelee.selectedWeapon >= weaponHolderMelee.transform.childCount - 1)                 // ���������� � 0 ������, ���� ������ ����� ���-�� ������� � �������� WeaponHolder - 1(?)
                    weaponHolderMelee.selectedWeapon = 0;
                else
                    weaponHolderMelee.selectedWeapon++;
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0f)                        // ���������� �������� (��� ������ �������)
            {
                if (weaponHolderMelee.selectedWeapon <= 0)
                    weaponHolderMelee.selectedWeapon = weaponHolderMelee.transform.childCount - 1;
                else
                    weaponHolderMelee.selectedWeapon--;
            }
            if (previousWeaponMelee != weaponHolderMelee.selectedWeapon)               // ���� ������ ������ ��������� - �������� �������
            {
                weaponHolderMelee.SelectWeapon();
            }
        }

        // ����� ������
        if (!meleeWeapon)
        {
            int previousWeapon = selectedWeapon;                                // ����������� ���������� ������ ������

            if (Input.GetAxis("Mouse ScrollWheel") > 0f || Input.GetKeyDown(KeyCode.Alpha2))                        // ���������� �������� (��� ������� �������)
            {
                if (selectedWeapon >= transform.childCount - 1)                 // ���������� � 0 ������, ���� ������ ����� ���-�� ������� � �������� WeaponHolder - 1(?)
                    selectedWeapon = 0;
                else
                    selectedWeapon++;
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0f)                        // ���������� �������� (��� ������ �������)
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



        // ����� ������ ���� ��� ����
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            HideWeapons();                          // ������ ���������
            weaponHolderMelee.SelectWeapon();       // ������ ����
            meleeWeapon = true;                     // ������ ����
            weaponHolderMelee.rangeWeapon = false; 
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            weaponHolderMelee.HideWeapons();        // ������ ����
            SelectWeapon();                         // ������ ���������
            meleeWeapon = false;                
            weaponHolderMelee.rangeWeapon = true;   // ������ ����
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwapWeapon();
        }


/*        if (Input.GetKeyDown(KeyCode.Z))
        {
            stopAiming = !stopAiming;           // ��� ������, ������� ������� ������
        }*/





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
    }

    // ����� ������

    public void SwapWeapon()
    {
        if (meleeWeapon)
        {
            SelectWeapon();                         // ������ ���������
            weaponHolderMelee.HideWeapons();        // ������ ����
            meleeWeapon = false;
            weaponHolderMelee.rangeWeapon = true;   // ������ ����
        }
        else
        {
            HideWeapons();                          // ������ ���������
            weaponHolderMelee.SelectWeapon();       // ������ ����
            meleeWeapon = true;                     // ������ ����
            weaponHolderMelee.rangeWeapon = false;
        }
    }

    public void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
            {
                weapon.gameObject.SetActive(true);                                      // ���������� ������ � ��������
                currentWeapon = weapon.gameObject.GetComponentInChildren<Weapon>();     // �������� ��� ������
                currentWeaponName = currentWeapon.weaponName;                           // �������� ��� ������ ��� ui
                //Debug.Log(currentWeapon.weaponName);
            }
            else
            {
                weapon.gameObject.SetActive(false);                                     // ��������� ������ �������������
            }
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

    public void CurrentWeaponVisible(bool hide)
    {
        currentWeapon.gameObject.SetActive(hide);
    }

    // ������� ������ (��������� ������)
    public void BuyWeapon(int weaponNumber)
    {
        GameObject weaponGO = Instantiate(weapons[weaponNumber], (transform.position), transform.rotation);
        weaponGO.transform.SetParent(transform, true);  
        weaponGO.SetActive(false);
    }
}