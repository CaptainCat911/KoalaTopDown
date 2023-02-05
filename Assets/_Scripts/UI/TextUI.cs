using UnityEngine;
using UnityEngine.UI;

public class TextUI : MonoBehaviour
{
    Player player;                  // ссылка на игрока
    public Text hp;                 // кол-во хп
    public Text shield;             // щит
    public Text key;                // кол-во ключей
    public Text battery;            // кол-во батарей
    public Text weaponName;         // имя оружия
    public Text ammoRangeWeapon;    // кол-во патронов
    public Text ammoBomb;           // кол-во бомб
    public Text bombName;           // имя бомбы


    void Start()
    {
        player = GameManager.instance.player;
    }
   
    void Update()                                                   // (потом изменить вывод сообщений)
    {
        // HP
        hp.text = player.currentHealth.ToString("0");

        // Щит
        if (player.shield.shieldOn)
            shield.text = player.shield.shieldHp.ToString("0");
        else
            shield.text = "OFF";

        // Ключи
        key.text = GameManager.instance.keys.ToString("0");

        // Батареи
        battery.text = GameManager.instance.battery.ToString("0");

        // Активное оружие
        if (GameManager.instance.player.weaponHolder.currentWeapon)
        {
            weaponName.text = GameManager.instance.player.weaponHolder.currentWeaponName;
            if (GameManager.instance.player.weaponHolder.meleeWeapon)
                ammoRangeWeapon.text = "-";
            else
                ammoRangeWeapon.text = GameManager.instance.player.weaponHolder.currentWeapon.ammo.ToString("0");
        }

        // Активная бомба
        if (GameManager.instance.player.bombWeaponHolder.currentWeapon)
        {
            bombName.text = GameManager.instance.player.bombWeaponHolder.currentWeaponName;
            ammoBomb.text = GameManager.instance.player.bombWeaponHolder.currentWeapon.ammo.ToString("0");
        }
    }
}
