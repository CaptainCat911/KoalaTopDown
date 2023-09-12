using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoPackKoala : MonoBehaviour
{
    Player player;

    [Header("Оружие")]
    public AmmoPackStore[] ammoWeapons;         // ссылка на оружие в инвентаре (название, цена, патроны)
    public AmmoPackStore[] ammoMeleeWeapons;    // ссылка на мили оружие в инвентаре (название, цена, патроны)
    public AmmoPackStore[] ammoBombs;           // ссылка на бомбы в инвентаре (название, цена, патроны)

    [Header("Для перевода")]
    public string ammoRu;
    public string ammoEng;
    string ammo;  
    
    public string bombRu;
    public string bombEng;
    string bomb;  

    public string buyedRu;
    public string buyedEng;
    string buyed;


    /*    [Header("Кнопки для замены")]
        public GameObject[] buttonsBuyRangeWeapon;
        public GameObject[] buttonsBuyMeleeWeapon;
        public GameObject[] buttonsBuyBomb;

        public GameObject[] buttonsSellRangeWeapon;
        public GameObject[] buttonsSellMeleeWeapon;
        public GameObject[] buttonsSellBomb;*/


    private void Awake()
    {
        player = GameManager.instance.player;        
    }

    private void Start()
    {
        if (LanguageManager.instance.eng)
        {
            foreach (AmmoPackStore pack in ammoWeapons)
            {
                pack.MakeNameEng();
            }

            foreach (AmmoPackStore pack in ammoMeleeWeapons)
            {
                pack.MakeNameEng();
            }

            foreach (AmmoPackStore pack in ammoBombs)
            {
                pack.MakeNameEng();
            }

            ammo = ammoEng;
            bomb = bombEng;
            buyed = buyedEng;
        }
        else
        {
            ammo = ammoRu;
            bomb = bombRu;
            buyed = buyedRu;
        }
    }

    // Патроны
    public bool BuyAmmo(int index)                  
    {
        if (GameManager.instance.gold >= ammoWeapons[index].goldPriseAmmo)          // если золота больше чем стоимость оружия
        {
            GameManager.instance.gold -= ammoWeapons[index].goldPriseAmmo;          // вычитаем из золота стоимость оружия
            ammoWeapons[index].allAmmo += ammoWeapons[index].ammoInReload;          // добавляем во все патроны кол-во патронов в обойме
            CreateMessage("+ " + ammoWeapons[index].ammoInReload + " " + ammo);
            return true;
        }
        else
        {
            CreateMessageNoGold();
            return false;
        }
    }


    // Ренж оружие
    public bool BuyRangeWeapon(int index)
    {
        if (GameManager.instance.gold >= ammoWeapons[index].goldPriseWeapon)            // если золота больше чем стоимость оружия
        {
            if (player.weaponHolder.meleeWeapon)
                player.weaponHolder.SwapWeapon();                  // переключаемся на это оружие

            GameManager.instance.gold -= ammoWeapons[index].goldPriseWeapon;            // вычитаем из золота стоимость оружия

            player.weaponHolder.weapons.Add(ammoWeapons[index].weapon);                 // добавляем оружие в список оружий
            player.weaponHolder.BuyWeapon(player.weaponHolder.weapons.Count - 1);       // создаем его в инвентаре игрока                                                                                
            //if (player.weaponHolder.weapons.Count - 1 > 0)                              // (длинна списка - 1 и будет номер последнего добавленного оружия)
            if (!player.weaponHolder.meleeWeapon)
            {
                player.weaponHolder.selectedWeapon = player.weaponHolder.weapons.Count - 1;
                player.weaponHolder.SelectWeapon();                                     // выбрать оружие 
            }

            player.rangeWeaponsIndex.Add(index);                // добавляем оружие "в индексе"

            CreateMessage(ammoWeapons[index].name + " " + buyed);
            return true;
        }
        else
        {
            CreateMessageNoGold();
            return false;
        } 
    }


/*    // Продать ренж оружие (пока не сделал)
    public void SellRangeWeapon(int index)
    {
        //buttonsBuyRangeWeapon[index].SetActive(true);
        //buttonsSellRangeWeapon[index].SetActive(false);

        GameManager.instance.gold += ammoWeapons[index].goldPriseWeapon;            // возвращаем золото 

        //player.weaponHolder.weapons.Remove
    }*/


    // Мили оружие
    public bool BuyMeleeWeapon(int index)
    {
        if (GameManager.instance.gold >= ammoMeleeWeapons[index].goldPriseWeapon)            // если золота больше чем стоимость оружия
        {
            //buttonsBuyMeleeWeapon[index].SetActive(false);                                         // убираем кнопку покупки
            //buttonsSellMeleeWeapon[index].SetActive(true);

            if (!GameManager.instance.player.weaponHolder.meleeWeapon)
                GameManager.instance.player.weaponHolder.SwapWeapon();

            GameManager.instance.gold -= ammoMeleeWeapons[index].goldPriseWeapon;            // вычитаем из золота стоимость оружия

            player.weaponHolderMelee.weapons.Add(ammoMeleeWeapons[index].weapon);                    // добавляем оружие в список оружий
            player.weaponHolderMelee.BuyWeapon(player.weaponHolderMelee.weapons.Count - 1);     // создаем его в инвентаре игрока                                                                                
                                                                                                //if (player.weaponHolder.weapons.Count - 1 > 0)                              // (длинна списка - 1 и будет номер последнего добавленного оружия)
            if (player.weaponHolder.meleeWeapon)
            {
                player.weaponHolderMelee.selectedWeapon = player.weaponHolderMelee.weapons.Count - 1;
                player.weaponHolderMelee.SelectWeapon();                                        // выбрать оружие 
            }

            player.meleeWeaponsIndex.Add(index);                // добавляем оружие "в индексе"

            CreateMessage(ammoMeleeWeapons[index].name + " " + buyed);

            return true;
        }
        else
        {
            CreateMessageNoGold();
            return false;
        }
    }


    // Бомбы
    public bool BuyBomb(int index)
    {
        if (GameManager.instance.gold >= ammoBombs[index].goldPriseWeapon)              // если золота больше чем стоимость оружия
        {
            //buttonsBuyBomb[index].SetActive(false);
            //buttonsSellBomb[index].SetActive(true);

            GameManager.instance.gold -= ammoBombs[index].goldPriseWeapon;              // вычитаем из золота стоимость оружия

            player.bombWeaponHolder.weapons.Add(ammoBombs[index].weapon);                 // добавляем оружие в список оружий 
            player.bombWeaponHolder.BuyWeapon(player.bombWeaponHolder.weapons.Count - 1);   // создаем его в инвентаре игрока                                                                                
            //if (player.weaponHolder.weapons.Count - 1 > 0)                              // (длинна списка - 1 и будет номер последнего добавленного оружия)

            player.bombWeaponHolder.selectedWeapon = player.bombWeaponHolder.weapons.Count - 1;
            player.bombWeaponHolder.SelectWeapon();                                         // выбрать оружие 

            player.bombsIndex.Add(index);                // добавляем оружие "в индексе"

            CreateMessage(ammoBombs[index].name + " " + buyed);
            return true;
        }
        else
        {
            CreateMessageNoGold();
            return false;
        }
    }

    public bool BuyAmmoBomb(int index)
    {
        if (GameManager.instance.gold >= ammoBombs[index].goldPriseAmmo)            // если золота больше чем стоимость оружия
        {
            GameManager.instance.gold -= ammoBombs[index].goldPriseAmmo;            // вычитаем из золота стоимость оружия             
            ammoBombs[index].allAmmo += ammoBombs[index].ammoInReload; ;
            CreateMessage("+ " + ammoBombs[index].ammoInReload + " " + bomb);
            return true;
        }
        else
        {
            CreateMessageNoGold();
            return false;
        }
    }


    // Для поднятия оружия

    // Поднять ренж оружие
    public void TakeRangeWeapon(int index)
    {
        if (GameManager.instance.player.weaponHolder.meleeWeapon)
            GameManager.instance.player.weaponHolder.SwapWeapon();

        player.weaponHolder.weapons.Add(ammoWeapons[index].weapon);                 // добавляем оружие в список оружий
        player.weaponHolder.BuyWeapon(player.weaponHolder.weapons.Count - 1);       // создаем его в инвентаре игрока                                                                                
        //if (player.weaponHolder.weapons.Count - 1 > 0)                              // (длинна списка - 1 и будет номер последнего добавленного оружия)
        if (!player.weaponHolder.meleeWeapon)
        {
            player.weaponHolder.selectedWeapon = player.weaponHolder.weapons.Count - 1;
            player.weaponHolder.SelectWeapon();                                     // выбрать оружие 
        }

        player.rangeWeaponsIndex.Add(index);                // добавляем оружие "в индексе"

        CreateMessage(ammoWeapons[index].name + " !");

    }
    // Поднять мили оружие
    public void TakeMeleeWeapon(int index)
    {
        if (!GameManager.instance.player.weaponHolder.meleeWeapon)
            GameManager.instance.player.weaponHolder.SwapWeapon();

        player.weaponHolderMelee.weapons.Add(ammoMeleeWeapons[index].weapon);                    // добавляем оружие в список оружий
        player.weaponHolderMelee.BuyWeapon(player.weaponHolderMelee.weapons.Count - 1);     // создаем его в инвентаре игрока                                                                                
                                                                                            //if (player.weaponHolder.weapons.Count - 1 > 0)                              // (длинна списка - 1 и будет номер последнего добавленного оружия)
        if (player.weaponHolder.meleeWeapon)
        {
            player.weaponHolderMelee.selectedWeapon = player.weaponHolderMelee.weapons.Count - 1;
            player.weaponHolderMelee.SelectWeapon();                                        // выбрать оружие 
        }

        player.meleeWeaponsIndex.Add(index);                // добавляем оружие "в индексе"

        CreateMessage(ammoMeleeWeapons[index].name + " !");  
    }
    // Поднять бомбу
    public void TakeBomb(int index)
    {
        player.bombWeaponHolder.weapons.Add(ammoBombs[index].weapon);                       // добавляем оружие в список оружий
        player.bombWeaponHolder.BuyWeapon(player.bombWeaponHolder.weapons.Count - 1);       // создаем его в инвентаре игрока

        player.bombWeaponHolder.selectedWeapon = player.bombWeaponHolder.weapons.Count - 1;
        player.bombWeaponHolder.SelectWeapon();                                        // выбрать оружие 

        player.bombsIndex.Add(index);                // добавляем оружие "в индексе"

        CreateMessage(ammoBombs[index].name + " !");
    }


    void CreateMessage(string text)
    {
        GameManager.instance.CreateFloatingMessage(text, Color.white, player.transform.position);
    }

    void CreateMessageNoGold()
    {
        if (LanguageManager.instance.eng)
        {
            GameManager.instance.CreateFloatingMessage("Not enough gold!", Color.white, player.transform.position);
        }
        else
        {
            GameManager.instance.CreateFloatingMessage("Недостаточно золота!", Color.white, player.transform.position);
        }
    }
}
