using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Держатель мили оружия для бота
/// </summary>

public class BotAIMeleeWeaponHolder : MonoBehaviour
{
    BotAI botAI;
    public List<GameObject> weapons;                            // список оружий
    [HideInInspector] public BotAIWeaponMelee currentWeapon;    // текущее оружие 
    [HideInInspector] public int selectedWeapon = 0;            // индекс оружия (положение в иерархии WeaponHolder)
    [HideInInspector] public bool fireStart;            // начать стрельбу
    bool weaponChanged;                                 // мили или ренж оружие
    bool attacking;

    private void Awake()
    {
        botAI = GetComponentInParent<BotAI>();
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
            SelectWeapon();                                 // выбираем оружие
            weaponChanged = true;
        }
        else
        {
            HideWeapons();                      // прячем оружие
        }
    }

    private void Update()
    {
        // Стрельба
        if (!botAI.boss)            // если не босс
        {
            if (botAI.closeToTarget && botAI.meleeAttackType && !attacking)
            {
                //fireStart = true;                       // стреляем
                attacking = true;
                botAI.animator.SetBool("Attacking", true);
                //botAI.animator.SetTrigger("Attack");
            }
            else if (!botAI.closeToTarget && attacking)
            {
                //fireStart = false;                      // не стреляем
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
            HideWeapons();                      // прячем оружие
            weaponChanged = false;
        }


        // Выбор оружия
        int previousWeapon = selectedWeapon;                    // присваиваем переменной индекс оружия

/*        if (botAI.switchMelee)                                  // сменить оружие
        {
            if (selectedWeapon >= transform.childCount - 1)     // сбрасываем в 0 индекс, если индекс равен кол-ву объекто в иерархии WeaponHolder - 1(?)
                selectedWeapon = 0;
            else
                selectedWeapon++;
            botAI.switchMelee = false;
        }*/


        if (previousWeapon != selectedWeapon)               // если индекс оружия изменился - вызываем функцию
        {
            SelectWeapon();
        }        
    }


    public void SelectCurrentWeapon(int i)
    {
        selectedWeapon = i;
        SelectWeapon();
    }

    // Смена оружия
    public void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
            {
                weapon.gameObject.SetActive(true);                                              // активируем оружие в иерархии
                currentWeapon = weapon.gameObject.GetComponentInChildren<BotAIWeaponMelee>();   // получаем его скрипт                
            }
            else
                weapon.gameObject.SetActive(false);                                             // остальные оружия дезактивируем
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