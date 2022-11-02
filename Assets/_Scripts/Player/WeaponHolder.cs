using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    public GameObject[] weapons;            // ������ ������    
    Weapon currentWeapon;                   // ������� ������
    int selectedWeapon = 0;                 // ������ ������ (��������� � �������� WeaponHolder)

    void Start()
    {      
        BuyWeapon(0);
        BuyWeapon(1);
        //BuyWeapon(2);
        SelectWeapon();
    }

    private void Update()
    {
        // ��������
        if (Input.GetMouseButtonDown(0) && currentWeapon)       
        {
            currentWeapon.fireStart = true;                     // �������� ������� �������� � �������� ������
        }
        if (Input.GetMouseButtonUp(0) && currentWeapon)         
        {
            currentWeapon.fireStart = false;                    // �������� ������� �������� � �������� ������
        }



        // ����� ������
        int previousWeapon = selectedWeapon;                                                // ����������� ���������� ������ ������

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)                         // ���������� �������� (��� ������� �������)
        {
            if (selectedWeapon >= transform.childCount - 1)                                 // ���������� � 0 ������, ���� ������ ����� ���-�� ������� � �������� WeaponHolder - 1(?)
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

        if (previousWeapon != selectedWeapon)                   // ���� ������ ������ ��������� - �������� �������
        {
            SelectWeapon();
        }
    }

    void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
            {
                weapon.gameObject.SetActive(true);                                      // ���������� ������ � ��������
                currentWeapon = weapon.gameObject.GetComponentInChildren<Weapon>();     // �������� ��� ������
            }
            else
                weapon.gameObject.SetActive(false);                                     // ��������� ������ �������������
            i++;
        }
    }

    void BuyWeapon(int weaponNumber)
    {
        GameObject weaponGO = Instantiate(weapons[weaponNumber], (transform.position + new Vector3(0.3f,0,0)), transform.rotation);
        weaponGO.transform.SetParent(this.transform, true);       
        weaponGO.SetActive(false);
    }
}
