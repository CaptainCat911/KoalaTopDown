using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Держатель оружия, также поворачивается для поворота оружия
/// </summary>

public class WeaponHolderMelee : MonoBehaviour
{
    Player player;
    public WeaponHolder weaponHolder;
    public List<GameObject> weapons;                        // список оружий
    [HideInInspector] public MeleeWeapon currentWeapon;     // текущее оружие (пока что толька для текста ui)
    [HideInInspector] public int selectedWeapon = 0;        // индекс оружия (положение в иерархии WeaponHolder)   
    [HideInInspector] public bool rangeWeapon = true;       // мили или ренж оружие
    [HideInInspector] public bool attackHitBoxStart;        // начать атаку мечом
    [HideInInspector] public bool stopHolder;



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
        //SelectWeapon();
        HideWeapons();                      // прячем оружие
    }

    private void Update()
    {
        if (GameManager.instance.isPlayerEnactive)
        {
            attackHitBoxStart = false;
            return;
        }

        if (stopHolder)
            return;

        // Атака
        if (GameManager.instance.forAndroid)
        {
            if (player.joystickFire.Direction.magnitude > 0)            // && player.playerWeaponReady
            {
                attackHitBoxStart = true;           // начинаем атаку хитбоксом
            }
            else
            {
                attackHitBoxStart = false;
            }
        }
        else
        {
            if (Input.GetMouseButton(0))            // && player.playerWeaponReady
            {
                attackHitBoxStart = true;           // начинаем атаку хитбоксом
            }
            else
            {
                attackHitBoxStart = false;
            }
        }
    }


    // Смена оружия
    public void SelectWeapon()
    {
        /*        if (!weaponHolder.meleeWeapon)
                    return;*/

        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
            {
                weapon.gameObject.SetActive(true);                                          // активируем оружие в иерархии
                currentWeapon = weapon.gameObject.GetComponentInChildren<MeleeWeapon>();    // получаем его скрипт
                if (LanguageManager.instance.eng)
                    weaponHolder.currentWeaponName = currentWeapon.weaponNameEng;                  // получаем имя оружия для ui
                else
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
        if (player.leftFlip)            // если при покупке игрок повёрнут влево
        {
            player.hitBoxPivot.transform.localScale = new Vector3(transform.localScale.x, 1, transform.localScale.z);   // поворачиваем хитбокспивот
            GameObject weaponGO = Instantiate(weapons[weaponNumber], (transform.position), transform.rotation);         // создаём оружие в "инвентаре" (в иерархии)
            weaponGO.transform.SetParent(transform, true);                                                              // этот объект делаем родителем
            weaponGO.SetActive(false);                                                                                  // отключаем это оружие (для выбора используется SelectWeapon)
            player.hitBoxPivot.transform.localScale = new Vector3(transform.localScale.x, -1, transform.localScale.z);  // возвращаем хитбокспивот в исходное положение
        }
        else
        {
            GameObject weaponGO = Instantiate(weapons[weaponNumber], (transform.position), transform.rotation);
            weaponGO.transform.SetParent(transform, true);
            weaponGO.SetActive(false);
        }
    }
}