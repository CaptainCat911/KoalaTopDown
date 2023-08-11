using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldHolder : MonoBehaviour
{
    public GameObject shield;
    Player player;

    private void Awake()
    {
        player = GameManager.instance.player;
    }

    private void OnDisable()
    {
        shield.SetActive(false);
        if (!player.weaponHolder.meleeWeapon)
        {
            player.weaponHolder.stopHolder = false;
            player.weaponHolder.currentWeapon.gameObject.SetActive(true);
        }
        else
        {
            player.weaponHolderMelee.stopHolder = false;
            player.weaponHolderMelee.currentWeapon.gameObject.SetActive(true);
        }
    }


    private void Update()
    {
        if (GameManager.instance.isPlayerEnactive)
        {
            return;
        }

        // �������� ������� ��� ���
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);                        // ��������� ����                  
        Vector3 aimDirection = mousePosition - transform.position;                                  // ���� ����� ���������� ���� � pivot ������          
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;                     // ������� ���� � ��������             
        Quaternion qua1 = Quaternion.Euler(0, 0, aimAngle);                                         // ������� ���� ���� � Quaternion
        transform.rotation = Quaternion.Lerp(transform.rotation, qua1, Time.fixedDeltaTime * 15);   // ������ Lerp ����� weaponHoder � ����� �����
                                                                                                    //Debug.Log(aimAngle);

        // ���
        if (Input.GetMouseButtonDown(1) && GameManager.instance.player.withShield)
        {
            /*            player.weaponHolder.gameObject.SetActive(false);               // ��������� ������ 
                        player.weaponHolderMelee.gameObject.SetActive(false);          //*/
            
            if (!player.weaponHolder.meleeWeapon)
            {
                player.weaponHolder.currentWeapon.gameObject.SetActive(false);
                player.weaponHolder.stopHolder = true;
            }
            else
            {
                player.weaponHolderMelee.currentWeapon.gameObject.SetActive(false);
                player.weaponHolderMelee.stopHolder = true;
            }
            shield.SetActive(true);            
        }
        if (Input.GetMouseButtonUp(1) && GameManager.instance.player.withShield)
        {
            shield.SetActive(false);
            if (!player.weaponHolder.meleeWeapon)
            {
                player.weaponHolder.stopHolder = false;
                player.weaponHolder.currentWeapon.gameObject.SetActive(true);
            }
            else
            {
                player.weaponHolderMelee.stopHolder = false;
                player.weaponHolderMelee.currentWeapon.gameObject.SetActive(true);
            }
            /*            player.weaponHolder.gameObject.SetActive(true);               // ��������� ������ 
                        player.weaponHolderMelee.gameObject.SetActive(true);          //*/
        }
    }
}
