using UnityEngine;
using UnityEngine.UI;

public class TextUI : MonoBehaviour
{
    public static TextUI instance;          // инстанс

    //public bool startScreen;

    Player player;                  // ссылка на игрока
    public Text hp;                 // кол-во хп
    public Text shield;             // щит
    public Text bootsEnedgy;        // энергия ботинок
    public Text gold;               // кол-во золота
    public Text key;                // кол-во ключей
    public Text keyRed;             // кол-во особых ключей
    public Text battery;            // кол-во батарей
    public Text weaponName;         // имя оружия
    public Text ammoRangeWeapon;    // кол-во патронов
    public Text bombName;           // имя бомбы
    public Text ammoBomb;           // кол-во бомб

    public GameObject pozorGo;      // объект (чтобы включать)
    public Text pozorCount;         // счетчик позора

    public Text arenaEnemyKilledCount;      // врагов убито
    public Text arenaBossKilledCount;       // босов убито

    public GameObject menu;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        player = GameManager.instance.player;
    }
   
    void FixedUpdate()                                                   // (потом изменить вывод сообщений)
    {
        // HP
        hp.text = player.currentHealth.ToString("0");

        // Щит
        if (player.shield.shieldOn)
            shield.text = player.shield.shieldHp.ToString("0");
        else
            shield.text = "OFF";

        // Ботинки
        if (player.bootsMod)
            bootsEnedgy.text = player.bootsEnergy.ToString("0");
        else
            bootsEnedgy.text = "OFF";

        // Золото
        gold.text = GameManager.instance.gold.ToString("0");

        // Ключи
        key.text = GameManager.instance.keys[0].ToString("0");

        // Ключи особые
        keyRed.text = GameManager.instance.keys[1].ToString("0");

        // Батареи
        battery.text = GameManager.instance.battery.ToString("0");

        // Позор
        pozorCount.text = GameManager.instance.pozorCount.ToString("0");

        // Активное оружие
        if (GameManager.instance.player.weaponHolder.currentWeapon)
        {
            weaponName.text = GameManager.instance.player.weaponHolder.currentWeaponName;           // название текущего оружия
            if (GameManager.instance.player.weaponHolder.meleeWeapon)                               // если оружие мили
                ammoRangeWeapon.text = "-";                                                         // выводим "-" в баре патронов
            else
                ammoRangeWeapon.text = GameManager.instance.ammoManager.ammoWeapons[GameManager.instance.
                    player.weaponHolder.currentWeapon.weaponIndexForAmmo].allAmmo.ToString("0");    // находим кол-во патронов текущего оружия
        }

        // Активная бомба
        if (GameManager.instance.player.bombWeaponHolder.currentWeapon)
        {
            bombName.text = GameManager.instance.player.bombWeaponHolder.currentWeaponName;
            ammoBomb.text = GameManager.instance.ammoManager.ammoBombs[GameManager.instance.
                    player.bombWeaponHolder.currentWeapon.weaponIndexForAmmo].allAmmo.ToString("0");       // находим кол-во патронов текущего оружия
        }

        // Арена
        if (ArenaManager.instance.arenaLevel)
        {
            arenaEnemyKilledCount.text = ArenaManager.instance.arenaEnemyKilled.ToString("0");
            arenaBossKilledCount.text = ArenaManager.instance.arenaBossKilled.ToString("0");
        }        
    }

    // Для меню
    
    public void ContinueGameButton()                // продолжить игру
    {
        GameManager.instance.ContinueGame();
    }

    public void GoToMainMenuButton()                // выйти в главное меню
    {
        GameManager.instance.NextScene(0);
    }

    public void ExitGameButton()                    // выйти в виндоус
    {
        Application.Quit();
        Debug.Log("Exit!");
    }

    public void ShowMenu(bool status)               // показать или скрыть меню
    {
        menu.SetActive(status);
    }

    public void Music(bool status)                  // вкл/выкл музыки
    {
        AudioManager.instance.StartStopTrack(status);
    }
}
