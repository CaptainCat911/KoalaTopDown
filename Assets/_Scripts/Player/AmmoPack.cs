﻿/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPack : MonoBehaviour
{
    Player player;                          // сслыка на игрока
    public RaycastWeapon[] weapons;         // массив оружий
    int weaponNumber;                       // переменная для выбора оружия

    public int souls = 0;

    public int allAmmo_9 = 0;
    public int allAmmo_0_357 = 0;
    public int allAmmo_5_56 = 0;
    public int allAmmo_0_12 = 0;
    public int allAmmo_7_62 = 0;
    public int allAmmo_0_50 = 0;
    public int granate = 0;
    public int HPBox = 0;

    public string message;                  // сообщение при подборе патронов
    public Color colorText = Color.yellow;  // цвет текста
    public bool messageReady;               // сообщение готово
    
    [Header("Стоимость патронов")]
    public int ammoSoulsPistol;
    public int ammoSoulsAR;
    public int ammoSoulsRevolver;
    public int ammoSoulsShotgun;
    public int ammoSoulsSvd;
    public int ammoSoulsPulemet;
    public int ammoSoulsHp;
    public int ammoSoulsGranate;
    
    [Header("Стоимость оружия")]
    public int WeaponSoulsPistol;
    public int WeaponSoulsAR;
    public int WeaponSoulsRevolver;
    public int WeaponSoulsShotgun;
    public int WeaponSoulsSVD;
    public int WeaponSoulsPulemet;
    public int WeaponSoulsAxe;
    public int soulsArmor;
    public int soulsArmorBlack;
    
    [Header("Стоимость апгрейда оружия")]
    public int WeaponSoulsAxeUpgreade;
    public int WeaponSoulsPistolUpgreade;
    public int WeaponSoulsARUpgreade;
    public int WeaponSoulsRevolverUpgreade;
    public int WeaponSoulsShotgunUpgreade;
    public int WeaponSoulsSVDUpgreade;
    public int WeaponSoulsPulemetUpgreade;

    // Оружие экипировано
    bool pistol;
    bool aR;
    bool shotgun;
    bool revolver;
    bool svd;
    bool pulemet;

    // Кнопки
    [Header("Кнопки пистолет")]
    public GameObject buttonBuyPistol;
    public GameObject buttonUp_2_Pistol;
    public GameObject buttonUp_3_Pistol;
    public GameObject buttonUpgradedPistol;
    int nPistol;

    [Header("Кнопки АР")]
    public GameObject buttonBuyAr;
    public GameObject buttonUp_2_Ar;
    public GameObject buttonUp_3_Ar;
    public GameObject buttonUpgradedAr;
    int nAr;

    [Header("Кнопки револьвер")]
    public GameObject buttonBuyRevolver;
    public GameObject buttonUp_2_Revolver;
    public GameObject buttonUp_3_Revolver;
    public GameObject buttonUpgradedRevolver;
    int nRevolver;

    [Header("Кнопки дробовик")]
    public GameObject buttonBuyShotgun;
    public GameObject buttonUp_2_Shotgun;
    public GameObject buttonUp_3_Shotgun;
    public GameObject buttonUpgradedShotgun;
    int nShotgun;

    [Header("Кнопки СВД")]
    public GameObject buttonBuySVD;
    public GameObject buttonUp_2_SVD;
    public GameObject buttonUp_3_SVD;
    public GameObject buttonUpgradedSVD;
    int nSVD;

    [Header("Кнопки пулемет")]
    public GameObject buttonBuyPulemet;
    public GameObject buttonUp_2_Pulemet;
    public GameObject buttonUp_3_Pulemet;
    public GameObject buttonUpgradedPulemet;
    int nPulemet;

    [Header("Кнопки топор")]
    public GameObject buttonAxe;
    public GameObject buttonAxeUp_2;
    public GameObject buttonAxeUpgraded;


    public void Start()
    {
        player = GameManager.instance.player;
    }

    public void GiveAmmo (string ammoType)
    {
        RaycastWeapon weapon = player.activeWeapon.GetActiveWeapon();
        switch (ammoType)
        {
            case "9":
                if (souls >= ammoSoulsPistol)
                {
                    allAmmo_9 += 100;
                    souls -= ammoSoulsPistol;
                }
                else
                {
                    SendToMessage("Недостаточно убийств");
                }
                break;

            case "5.56":
                if (souls >= ammoSoulsAR)
                {
                    allAmmo_5_56 += 100;
                    souls -= ammoSoulsAR;
                }
                else
                {
                    SendToMessage("Недостаточно убийств");
                }
                break;


            case "0.12":
                if (souls >= ammoSoulsShotgun)
                {
                    allAmmo_0_12 += 36;
                    souls -= ammoSoulsShotgun;
                }
                else
                {
                    SendToMessage("Недостаточно убийств");
                }
                break;

            case "0.357":
                if (souls >= ammoSoulsRevolver)
                {
                    allAmmo_0_357 += 36;
                    souls -= ammoSoulsRevolver;
                }
                else
                {
                    SendToMessage("Недостаточно убийств");
                }
                break;

            case "0.50":
                if (souls >= ammoSoulsSvd)
                {
                    allAmmo_0_50 += 30;
                    souls -= ammoSoulsSvd;
                }
                else
                {
                    SendToMessage("Недостаточно убийств");
                }
                break;

            case "7.62":
                if (souls >= ammoSoulsPulemet)
                {
                    allAmmo_7_62 += 100;
                    souls -= ammoSoulsPulemet;
                }
                else
                {
                    SendToMessage("Недостаточно убийств");
                }
                break;

            case "hp":
                if (souls >= ammoSoulsHp)
                {
                    HPBox += 1;
                    souls -= ammoSoulsHp;
                }
                else
                {
                    SendToMessage("Недостаточно убийств");
                }
                break;

            case "Granate":
                if (souls >= ammoSoulsGranate)
                {
                    granate += 1;
                    souls -= ammoSoulsGranate;
                }
                else
                {
                    SendToMessage("Недостаточно убийств");
                }
                break;



                *//*            case "0.12":
                                playerAmmo.allAmmo_0_12 += ammoSize;
                                playerAmmo.message = "+ патроны дробовик";
                                break;
                            case "7.62":
                                playerAmmo.allAmmo_7_62 += ammoSize;
                                playerAmmo.message = "+ патроны пулемет";
                                break;
                            case "0.50":
                                playerAmmo.allAmmo_0_50 += ammoSize;
                                playerAmmo.message = "+ патроны СВД";
                                break;
                            case "granate":
                                playerAmmo.message = "+ гранаты";
                                playerAmmo.granate += ammoSize;
                                break;*//*
        }
        weapon.TakeAmmo();
    }

    public void GiveWeapon (string weapon)
    {        
        switch (weapon)
        {
            case "Pistol":
                if (pistol)
                {
                    SendToMessage("Уже куплено !");
                    return;
                }
                if (souls >= WeaponSoulsPistol && !pistol)
                {
                    weaponNumber = 5;
                    pistol = true;
                    souls -= WeaponSoulsPistol;
                    buttonBuyPistol.SetActive(false);
                    buttonUp_2_Pistol.SetActive(true);
                }
                else
                {
                    SendToMessage("Недостаточно убийств");
                    return;
                }
                break;

            case "AR":
                if (aR)                                 // если винтовка уже есть
                {
                    SendToMessage("Уже куплено !");
                    return;
                }
                if (souls >= WeaponSoulsAR && !aR)      // если душ больше чем цена оружия и его нет
                {
                    weaponNumber = 0;                   // номер префаба
                    aR = true;                          // оружие купили
                    souls -= WeaponSoulsAR;             // забираем цену оружия из общих душ
                    buttonBuyAr.SetActive(false);
                    buttonUp_2_Ar.SetActive(true);
                }
                else
                {
                    SendToMessage("Недостаточно убийств");
                    return;
                }
                break;



            case "Revolver":
                if (revolver)
                {
                    SendToMessage("Уже куплено !");
                    return;
                }
                if (souls >= WeaponSoulsRevolver && !revolver)
                {
                    weaponNumber = 2;
                    revolver = true;
                    souls -= WeaponSoulsRevolver;
                    buttonBuyRevolver.SetActive(false);
                    buttonUp_2_Revolver.SetActive(true);
                }
                else
                {
                    SendToMessage("Недостаточно убийств");
                    return;
                }
                break;

            case "Shotgun":
                if (shotgun)
                {
                    SendToMessage("Уже куплено !");
                    return;
                }
                if (souls >= WeaponSoulsShotgun && !shotgun)
                {
                    weaponNumber = 1;
                    shotgun = true;
                    souls -= WeaponSoulsShotgun;
                    buttonBuyShotgun.SetActive(false);
                    buttonUp_2_Shotgun.SetActive(true);
                }
                else
                {
                    SendToMessage("Недостаточно убийств");
                    return;
                }
                break;


            case "SVD":
                if (svd)
                {
                    SendToMessage("Уже куплено !");
                    return;
                }
                if (souls >= WeaponSoulsSVD && !svd)
                {
                    weaponNumber = 3;
                    svd = true;
                    souls -= WeaponSoulsSVD;
                    buttonBuySVD.SetActive(false);
                    buttonUp_2_SVD.SetActive(true);
                }
                else
                {
                    SendToMessage("Недостаточно убийств");
                    return;
                }
                break;

            case "Pulemet":
                if (pulemet)
                {
                    SendToMessage("Уже куплено !");
                    return;
                }
                if (souls >= WeaponSoulsPulemet && !pulemet)
                {
                    weaponNumber = 4;
                    pulemet = true;
                    souls -= WeaponSoulsPulemet;
                    buttonBuyPulemet.SetActive(false);
                    buttonUp_2_Pulemet.SetActive(true);
                }
                else
                {
                    SendToMessage("Недостаточно убийств");
                    return;
                }
                break;
        }

        RaycastWeapon newWeapon = Instantiate(weapons[weaponNumber]);
        player.activeWeapon.GetWeaponUp(newWeapon);
        
    }

    public void UpgradeWeapon(string weapon)
    {
        //Debug.Log(player.activeWeapon.listWeapons);
        foreach (RaycastWeapon w in player.activeWeapon.listWeapons)
        {
            // Пистолет
            if (w.weaponName == weapon && weapon == "pistol")
            {
                if (souls >= WeaponSoulsPistolUpgreade)
                {
                    w.rayDamage += 8;
                    w.clipSize += 10;
                    souls -= WeaponSoulsPistolUpgreade;

                    nPistol++;
                    if (nPistol == 1)
                    {
                        buttonUp_2_Pistol.SetActive(false);
                        buttonUp_3_Pistol.SetActive(true);
                    }
                    if (nPistol == 2)
                    {
                        buttonUp_3_Pistol.SetActive(false);
                        buttonUpgradedPistol.SetActive(true);
                    }

                    //Debug.Log("Pistol Upgraded");
                }
                else
                {
                    SendToMessage("Недостаточно убийств");
                }
            }

            // AR
            if (w.weaponName == weapon && weapon == "rifle")
            {
                if (souls >= WeaponSoulsARUpgreade)
                {
                    w.rayDamage += 12;
                    w.clipSize += 15;
                    souls -= WeaponSoulsARUpgreade;

                    nAr++;
                    if (nAr == 1) 
                    {
                        buttonUp_2_Ar.SetActive(false);
                        buttonUp_3_Ar.SetActive(true);
                    }
                    if (nAr == 2)
                    {
                        buttonUp_3_Ar.SetActive(false);
                        buttonUpgradedAr.SetActive(true);
                    }


                    //Debug.Log("AR Upgraded");
                }
                else
                {
                    SendToMessage("Недостаточно убийств");
                }
            }

            // Револьвер
            if (w.weaponName == weapon && weapon == "revolver")
            {
                if (souls >= WeaponSoulsRevolverUpgreade)
                {
                    w.rayDamage += 50;
                    w.clipSize += 3;
                    souls -= WeaponSoulsRevolverUpgreade;

                    nRevolver++;
                    if (nRevolver == 1)
                    {
                        buttonUp_2_Revolver.SetActive(false);
                        buttonUp_3_Revolver.SetActive(true);
                    }
                    if (nRevolver == 2)
                    {
                        buttonUp_3_Revolver.SetActive(false);
                        buttonUpgradedRevolver.SetActive(true);
                    }

                    //Debug.Log("Revolver Upgraded");
                }
                else
                {
                    SendToMessage("Недостаточно убийств");
                }
            }

            // Дробовик
            if (w.weaponName == weapon && weapon == "shotgun")
            {
                if (souls >= WeaponSoulsShotgunUpgreade)
                {
                    w.rayDamage += 12;
                    w.clipSize += 4;
                    souls -= WeaponSoulsShotgunUpgreade;

                    nShotgun++;
                    if (nShotgun == 1)
                    {
                        buttonUp_2_Shotgun.SetActive(false);
                        buttonUp_3_Shotgun.SetActive(true);
                    }
                    if (nShotgun == 2)
                    {
                        buttonUp_3_Shotgun.SetActive(false);
                        buttonUpgradedShotgun.SetActive(true);
                    }

                    //Debug.Log("Shotgun Upgraded");
                }
                else
                {
                    SendToMessage("Недостаточно убийств");
                }
            }

            // СВД
            if (w.weaponName == weapon && weapon == "sniper")
            {
                if (souls >= WeaponSoulsSVDUpgreade)
                {
                    w.rayDamage += 300;
                    w.clipSize += 5;
                    souls -= WeaponSoulsSVDUpgreade;

                    nSVD++;
                    if (nSVD == 1)
                    {
                        buttonUp_2_SVD.SetActive(false);
                        buttonUp_3_SVD.SetActive(true);
                    }
                    if (nSVD == 2)
                    {
                        buttonUp_3_SVD.SetActive(false);
                        buttonUpgradedSVD.SetActive(true);
                    }


                    //Debug.Log("SVD Upgraded");
                }
                else
                {
                    SendToMessage("Недостаточно убийств");
                }
            }

            // Пулемет
            if (w.weaponName == weapon && weapon == "heavy")
            {
                if (souls >= WeaponSoulsPulemetUpgreade)
                {
                    w.rayDamage += 26;
                    w.clipSize += 50;
                    souls -= WeaponSoulsPulemetUpgreade;

                    nPulemet++;
                    if (nPulemet == 1)
                    {
                        buttonUp_2_Pulemet.SetActive(false);
                        buttonUp_3_Pulemet.SetActive(true);
                    }
                    if (nPulemet == 2)
                    {
                        buttonUp_3_Pulemet.SetActive(false);
                        buttonUpgradedPulemet.SetActive(true);
                    }

                    //Debug.Log("Pulemet Upgraded");
                }
                else
                {
                    SendToMessage("Недостаточно убийств");
                }
            }
        }
    }

    public void GetAxe()
    {
        if (souls >= WeaponSoulsAxe)
        {
            souls -= WeaponSoulsAxe;
            player.activeWeapon.getAxe = true;                                  // выдаем топор
            player.activeWeapon.axeBack.SetActive(true);                        // топор активен
            buttonAxe.SetActive(false);
            buttonAxeUp_2.SetActive(true);
            

        }
    }

    public void UpgradeAxe()
    {
        if (souls >= WeaponSoulsAxeUpgreade)
        {
            player.activeWeapon.damage += 300;
            player.activeWeapon.attackRadiusHitBox += 0.6f;
            player.activeWeapon.axeEffectSmoke.SetActive(true);
            souls -= WeaponSoulsAxeUpgreade;

            buttonAxeUp_2.SetActive(false);
            buttonAxeUpgraded.SetActive(true);
        }
        else
        {
            SendToMessage("Недостаточно убийств");
        }
    }

    public void EffectSmokeActivate(string weapon)
    {
        foreach (RaycastWeapon w in player.activeWeapon.listWeapons)
        {
            if (w.weaponName == weapon)
            {
                w.effectSmoke.SetActive(true);
            }
        }
    }

    public void GiveArmor(string armor)
    {
        switch (armor)
        {
            case "1":
                if (souls >= soulsArmor && player.armor < 100)
                {
                    player.armor = 100;
                    player.armorProtection = 1.5f;
                    player.armored = true;
                    player.armorGameObject.SetActive(true);
                    souls -= soulsArmor;
                }
                else if (player.armor >= 100)
                {
                    SendToMessage("У вас целый бронежилет");
                }
                else
                {
                    SendToMessage("Недостаточно убийств");
                }
                break;

            case "2":
                if (souls >= soulsArmorBlack && player.armor < 200)
                {
                    player.armor = 200;
                    player.armorProtection = 3f;
                    player.armored = true;
                    player.armorGameObject.SetActive(false);
                    player.armorBlackGameObject.SetActive(true);
                    souls -= soulsArmorBlack;
                }
                else if (player.armor >= 200)
                {
                    SendToMessage("У вас целый бронежилет");
                }               
                else
                {
                    SendToMessage("Недостаточно убийств");
                }
                break;
        }
    }

    public void SendToMessage(string messageToSend)
    {
        message = messageToSend;
        messageReady = true;
        colorText = Color.yellow;
    }
}

*/