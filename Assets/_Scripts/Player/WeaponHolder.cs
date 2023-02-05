using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Держатель оружия, также поворачивается для поворота оружия
/// </summary>

public class WeaponHolder : MonoBehaviour
{    
    public WeaponHolderMelee weaponHolderMelee;         // ссылка на холдер для мили оружия
    public List<GameObject> weapons;                    // Список оружий    
    [HideInInspector] public Weapon currentWeapon;      // текущее оружие 
    [HideInInspector] public int selectedWeapon = 0;    // индекс оружия (положение в иерархии WeaponHolder)
    [HideInInspector] public bool fireStart;            // начать стрельбу
    [HideInInspector] public bool attackHitBoxStart;    // начать атаку мечом
    [HideInInspector] public float aimAngle;            // угол поворота для вращения холдера с оружием и хитбоксПивота
    Vector3 mousePosition;                              // положение мыши
    [HideInInspector] public bool meleeWeapon;                                   // мили оружие или ренж

    [HideInInspector] public string currentWeaponName;  // для текста ui


    bool stopAiming;                                    // для дебага

    void Start()
    {
        int i = 0;
        foreach (GameObject weapon in weapons)          // покупаем все оружия из списка оружий при старте
        {            
            BuyWeapon(i);
            i++;
        }
        SelectWeapon();                                 // выбираем оружие
    }

    private void Update()
    {
        //Debug.Log(weapons.Count - 1);

        // Стрельба
        if (Input.GetMouseButton(0))
        {
            if (meleeWeapon)                        // если оружие ближнего боя
                attackHitBoxStart = true;           // начинаем атаку хитбоксом
            else
                fireStart = true;                   // стреляем
        }
        else
        {
            if (meleeWeapon)
                attackHitBoxStart = false;
            else
                fireStart = false;                  
        }

        // Смена оружия на мили
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectWeapon();                     // достаём огнестрел
            weaponHolderMelee.HideWeapons();    // прячем мили
            meleeWeapon = false;
            weaponHolderMelee.rangeWeapon = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            HideWeapons();                      // прячем огнестрел
            weaponHolderMelee.SelectWeapon();   // достаём мили
            meleeWeapon = true;                 // оружие мили
            weaponHolderMelee.rangeWeapon = false;
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            stopAiming = !stopAiming;           // для дебага, убираем поворот оружия
        }

        // Поворот оружия
        if (!stopAiming)
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);                        // положение мыши                  
            Vector3 aimDirection = mousePosition - transform.position;                                  // угол между положением мыши и pivot оружия          
            aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;                     // находим угол в градусах             
            Quaternion qua1 = Quaternion.Euler(0, 0, aimAngle);                                         // создаем этот угол в Quaternion
            transform.rotation = Quaternion.Lerp(transform.rotation, qua1, Time.fixedDeltaTime * 15);   // делаем Lerp между weaponHoder и нашим углом
            //Debug.Log(aimAngle);
        }

        // Выбор оружия
        if (!meleeWeapon)
        {
            int previousWeapon = selectedWeapon;                                // присваиваем переменной индекс оружия

            if (Input.GetAxis("Mouse ScrollWheel") < 0f)                        // управление колёсиком (для правого холдера)
            {
                if (selectedWeapon >= transform.childCount - 1)                 // сбрасываем в 0 индекс, если индекс равен кол-ву объекто в иерархии WeaponHolder - 1(?)
                    selectedWeapon = 0;
                else
                    selectedWeapon++;
            }
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)                        // управление колёсиком (для левого холдера)
            {
                if (selectedWeapon <= 0)
                    selectedWeapon = transform.childCount - 1;
                else
                    selectedWeapon--;
            }

            if (previousWeapon != selectedWeapon)               // если индекс оружия изменился - вызываем функцию
            {
                SelectWeapon();
            }
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
    }

    // Смена оружия
    public void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
            {
                weapon.gameObject.SetActive(true);                                      // активируем оружие в иерархии
                currentWeapon = weapon.gameObject.GetComponentInChildren<Weapon>();     // получаем его скрипт
                currentWeaponName = currentWeapon.weaponName;                           // получаем имя оружия для ui
                //Debug.Log(currentWeapon.weaponName);
            }
            else
            {
                weapon.gameObject.SetActive(false);                                     // остальные оружия дезактивируем
            }
            i++;
        }
    }

    public void HideWeapons()
    {        
        foreach (Transform weapon in transform)
        {
            weapon.gameObject.SetActive(false);                                     // остальные оружия дезактивируем
        }
    }

    // Покупка оружия (подбираем оружие)
    public void BuyWeapon(int weaponNumber)
    {
        GameObject weaponGO = Instantiate(weapons[weaponNumber], (transform.position), transform.rotation);
        weaponGO.transform.SetParent(transform, true);  
        weaponGO.SetActive(false);
    }
}