using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Держатель оружия, также поворачивается для поворота оружия
/// </summary>

public class WeaponHolder : MonoBehaviour
{
    Player player;
    public WeaponHolderMelee weaponHolderMelee;         // ссылка на холдер для мили оружия
    public List<GameObject> weapons;                    // Список оружий    
    [HideInInspector] public Weapon currentWeapon;      // текущее оружие 
    [HideInInspector] public int selectedWeapon = 0;    // индекс оружия (положение в иерархии WeaponHolder)
    [HideInInspector] public bool fireStart;            // начать стрельбу
    
    [HideInInspector] public float aimAngle;            // угол поворота для вращения холдера с оружием и хитбоксПивота
    Vector3 mousePosition;                              // положение мыши
    [HideInInspector] public bool meleeWeapon;          // мили оружие или ренж
    [HideInInspector] public bool stopHolder;


    [HideInInspector] public string currentWeaponName;  // для текста ui
    bool stopAiming;                                    // для дебага

    private void Awake()
    {
        player = GameManager.instance.player;
    }

    void Start()
    {
/*        int i = 0;
        foreach (GameObject weapon in weapons)          // покупаем все оружия из списка оружий при старте
        {
            BuyWeapon(i);
            i++;
        }
        SelectWeapon();                                 // выбираем оружие*/
/*        if (GameManager.instance.firstLevel)
            Invoke(nameof(SwapWeapon), 0.1f);               // чтобы при первой загрузке в руках было мили оружие*/
    }

    private void Update()
    {
        // Для сцен диалогов
        if (GameManager.instance.isPlayerEnactive)
        {
            fireStart = false;

            if (GameManager.instance.player.rightFlip)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 0), Time.fixedDeltaTime * 15);   // делаем Lerp между weaponHoder и нашим углом
            }
            if (GameManager.instance.player.leftFlip)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 180), Time.fixedDeltaTime * 15);   // делаем Lerp между weaponHoder и нашим углом
            }

            return;
        }

        // Поворот оружия
        if (GameManager.instance.forAndroid)
        {
            if (player.joystickFire.Direction.magnitude > 0)
            {
                aimAngle = Mathf.Atan2(player.joystickFire.Direction.y, player.joystickFire.Direction.x) * Mathf.Rad2Deg;       // находим угол в градусах             
                Quaternion qua1 = Quaternion.Euler(0, 0, aimAngle);                                                             // создаем этот угол в Quaternion
                transform.rotation = Quaternion.Lerp(transform.rotation, qua1, Time.fixedDeltaTime * 100);                      // делаем Lerp между weaponHoder и нашим углом
                //Debug.Log(aimAngle);
            }
        }
        else
        {
            if (stopAiming)
            {
                return;
            }
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);                        // положение мыши                  
            Vector3 aimDirection = mousePosition - transform.position;                                  // угол между положением мыши и pivot оружия          
            aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;                     // находим угол в градусах             
            Quaternion qua1 = Quaternion.Euler(0, 0, aimAngle);                                         // создаем этот угол в Quaternion
            transform.rotation = Quaternion.Lerp(transform.rotation, qua1, Time.fixedDeltaTime * 15);   // делаем Lerp между weaponHoder и нашим углом
            //Debug.Log(aimAngle);
        }

        if (stopHolder)
            return;

        // Стрельба
        if (GameManager.instance.forAndroid)
        {
            if (player.joystickFire.Direction.magnitude > 0)            // && player.playerWeaponReady
            {
                fireStart = true;                   // стреляем
            }
            else
            {
                fireStart = false;
            }
        }
        else
        {
            if (Input.GetMouseButton(0))            // && player.playerWeaponReady
            {
                fireStart = true;                   // стреляем
            }
            else
            {
                fireStart = false;
            }
        }




        // Выбор оружия мили
        if (meleeWeapon)
        {
            int previousWeaponMelee = weaponHolderMelee.selectedWeapon;                                // присваиваем переменной индекс оружия

            if (Input.GetAxis("Mouse ScrollWheel") > 0f || Input.GetKeyDown(KeyCode.Alpha1))                        // управление колёсиком (для правого холдера)
            {
                if (weaponHolderMelee.selectedWeapon >= weaponHolderMelee.transform.childCount - 1)                 // сбрасываем в 0 индекс, если индекс равен кол-ву объекто в иерархии WeaponHolder - 1(?)
                    weaponHolderMelee.selectedWeapon = 0;
                else
                    weaponHolderMelee.selectedWeapon++;
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0f)                        // управление колёсиком (для левого холдера)
            {
                if (weaponHolderMelee.selectedWeapon <= 0)
                    weaponHolderMelee.selectedWeapon = weaponHolderMelee.transform.childCount - 1;
                else
                    weaponHolderMelee.selectedWeapon--;
            }
            if (previousWeaponMelee != weaponHolderMelee.selectedWeapon)               // если индекс оружия изменился - вызываем функцию
            {
                weaponHolderMelee.SelectWeapon();
            }
        }

        // Выбор оружия
        if (!meleeWeapon)
        {
            int previousWeapon = selectedWeapon;                                // присваиваем переменной индекс оружия

            if (Input.GetAxis("Mouse ScrollWheel") > 0f || Input.GetKeyDown(KeyCode.Alpha2))                        // управление колёсиком (для правого холдера)
            {
                if (selectedWeapon >= transform.childCount - 1)                 // сбрасываем в 0 индекс, если индекс равен кол-ву объекто в иерархии WeaponHolder - 1(?)
                    selectedWeapon = 0;
                else
                    selectedWeapon++;
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0f)                        // управление колёсиком (для левого холдера)
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



        // Смена оружия ренж или мили
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            HideWeapons();                          // прячем огнестрел
            weaponHolderMelee.SelectWeapon();       // достаём мили
            meleeWeapon = true;                     // оружие мили
            weaponHolderMelee.rangeWeapon = false; 
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            weaponHolderMelee.HideWeapons();        // прячем мили
            SelectWeapon();                         // достаём огнестрел
            meleeWeapon = false;                
            weaponHolderMelee.rangeWeapon = true;   // оружие ренж
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwapWeapon();
        }


/*        if (Input.GetKeyDown(KeyCode.Z))
        {
            stopAiming = !stopAiming;           // для дебага, убираем поворот оружия
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

    // Смена оружия
    public void SwapWeapon()
    {
        if (meleeWeapon)
        {
            SelectWeapon();                         // достаём огнестрел
            weaponHolderMelee.HideWeapons();        // прячем мили
            meleeWeapon = false;
            weaponHolderMelee.rangeWeapon = true;   // оружие ренж
        }
        else
        {
            HideWeapons();                          // прячем огнестрел
            weaponHolderMelee.SelectWeapon();       // достаём мили
            meleeWeapon = true;                     // оружие мили
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
                weapon.gameObject.SetActive(true);                                      // активируем оружие в иерархии
                currentWeapon = weapon.gameObject.GetComponentInChildren<Weapon>();     // получаем его скрипт
                if (LanguageManager.instance.eng)
                    currentWeaponName = currentWeapon.weaponNameEng;                           // получаем имя оружия для ui
                else
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

    public void CurrentWeaponVisible(bool hide)
    {
        currentWeapon.gameObject.SetActive(hide);
    }

    // Покупка оружия (подбираем оружие)
    public void BuyWeapon(int weaponNumber)
    {
        GameObject weaponGO = Instantiate(weapons[weaponNumber], (transform.position), transform.rotation);
        weaponGO.transform.SetParent(transform, true);  
        weaponGO.SetActive(false);
    }
}