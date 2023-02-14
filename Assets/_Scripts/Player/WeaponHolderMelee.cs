using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Держатель оружия, также поворачивается для поворота оружия
/// </summary>

public class WeaponHolderMelee : MonoBehaviour
{
    public WeaponHolder weaponHolder;
    public List<GameObject> weapons;                        // список оружий
    [HideInInspector] public MeleeWeapon currentWeapon;     // текущее оружие (пока что толька для текста ui)
    [HideInInspector] public int selectedWeapon = 0;        // индекс оружия (положение в иерархии WeaponHolder)   
    [HideInInspector] public bool rangeWeapon = true;       // мили или ренж оружие
    [HideInInspector] public bool attackHitBoxStart;    // начать атаку мечом

    void Start()
    {
        int i = 0;
        foreach (GameObject weapon in weapons)
        {
            BuyWeapon(i);
            i++;
        }
        SelectWeapon();
        HideWeapons();                      // прячем оружие
    }

    private void Update()
    {
        if (GameManager.instance.isPlayerEnactive)
        {
            attackHitBoxStart = false;
            return;
        }

        // Атака
        if (Input.GetMouseButton(0))
        {
            //if (meleeWeapon)                        // если оружие ближнего боя
                attackHitBoxStart = true;           // начинаем атаку хитбоксом
            //else
                //fireStart = true;                   // стреляем
        }
        else
        {
            //if (meleeWeapon)
                attackHitBoxStart = false;
            //else
                //fireStart = false;
        }

        // Выбор оружия
        if (!rangeWeapon)
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
    }


    // Смена оружия
    public void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
            {
                weapon.gameObject.SetActive(true);                                          // активируем оружие в иерархии
                currentWeapon = weapon.gameObject.GetComponentInChildren<MeleeWeapon>();    // получаем его скрипт
                weaponHolder.currentWeaponName = currentWeapon.weaponName;                  // получаем имя оружия для ui
            }
            else
                weapon.gameObject.SetActive(false);                                     // остальные оружия дезактивируем
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