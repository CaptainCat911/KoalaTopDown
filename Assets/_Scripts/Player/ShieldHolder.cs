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


    private void Update()
    {
        // ¬ременно вращаем щит тут
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);                        // положение мыши                  
        Vector3 aimDirection = mousePosition - transform.position;                                  // угол между положением мыши и pivot оружи€          
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;                     // находим угол в градусах             
        Quaternion qua1 = Quaternion.Euler(0, 0, aimAngle);                                         // создаем этот угол в Quaternion
        transform.rotation = Quaternion.Lerp(transform.rotation, qua1, Time.fixedDeltaTime * 15);   // делаем Lerp между weaponHoder и нашим углом
                                                                                                    //Debug.Log(aimAngle);

        // ўит
        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.V))
        {
            player.weaponHolder.gameObject.SetActive(false);               // отключаем оружи€ 
            player.weaponHolderMelee.gameObject.SetActive(false);          //
            shield.SetActive(true);            
        }
        if (Input.GetMouseButtonUp(1) || Input.GetKeyUp(KeyCode.V))
        {
            shield.SetActive(false);
            player.weaponHolder.gameObject.SetActive(true);               // отключаем оружи€ 
            player.weaponHolderMelee.gameObject.SetActive(true);          //
        }
    }
}
