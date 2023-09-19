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
            if (player.weaponHolder.currentWeapon)
                player.weaponHolder.currentWeapon.gameObject.SetActive(true);
        }
        else
        {            
            if (player.weaponHolderMelee.currentWeapon)
                player.weaponHolderMelee.currentWeapon.gameObject.SetActive(true);
        }

        player.weaponHolder.stopHolder = false;
        player.weaponHolderMelee.stopHolder = false;
    }


    private void Update()
    {
        if (GameManager.instance.isPlayerEnactive)
        {
            return;
        }

        if (!GameManager.instance.player.withShield)
        {
            return;
        }

        // Временно вращаем щит тут
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);                        // положение мыши                  
        Vector3 aimDirection = mousePosition - transform.position;                                  // угол между положением мыши и pivot оружия          
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;                     // находим угол в градусах             
        Quaternion qua1 = Quaternion.Euler(0, 0, aimAngle);                                         // создаем этот угол в Quaternion
        transform.rotation = Quaternion.Lerp(transform.rotation, qua1, Time.fixedDeltaTime * 15);   // делаем Lerp между weaponHoder и нашим углом
                                                                                                    //Debug.Log(aimAngle);

        // Щит
        if (Input.GetMouseButtonDown(1))
        {
            /*            player.weaponHolder.gameObject.SetActive(false);               // отключаем оружия 
                        player.weaponHolderMelee.gameObject.SetActive(false);          //*/
            
            if (!player.weaponHolder.meleeWeapon)                                   // если оружие ближнего боя
            {
                player.weaponHolder.currentWeapon.gameObject.SetActive(false);      //                 
            }
            else
            {
                player.weaponHolderMelee.currentWeapon.gameObject.SetActive(false);                
            }
            player.weaponHolder.stopHolder = true;
            player.weaponHolderMelee.stopHolder = true;
            shield.SetActive(true);            
        }
        if (Input.GetMouseButtonUp(1))
        {
            shield.SetActive(false);
            if (!player.weaponHolder.meleeWeapon)
            {                
                player.weaponHolder.currentWeapon.gameObject.SetActive(true);
            }
            else
            {                
                player.weaponHolderMelee.currentWeapon.gameObject.SetActive(true);
            }
            player.weaponHolder.stopHolder = false;
            player.weaponHolderMelee.stopHolder = false;
            /*            player.weaponHolder.gameObject.SetActive(true);               // отключаем оружия 
                        player.weaponHolderMelee.gameObject.SetActive(true);          //*/
        }
    }
}
