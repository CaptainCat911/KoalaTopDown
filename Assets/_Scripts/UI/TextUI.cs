using UnityEngine;
using UnityEngine.UI;

public class TextUI : MonoBehaviour
{
    public static TextUI instance;          // инстанс

    //public bool startScreen;
    [Header("UI")]
    Player player;                  // ссылка на игрока
    public Text hp;                 // кол-во хп
    public Animator animatorHp;
    public string takeDamageAnimation;
    public string takeHealAnimation;

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

    public GameObject bars;             // бары (для отключения UI)
    bool barsOff;
    public GameObject menu;             // меню
    public GameObject menuEng;          // меню англ
    public Animator saving;           // сохранение...
    public Animator savingEng;        //

    //public GameObject buttonMusicOn;    // управление музыкой
    //public GameObject buttonMusicOff;   // 

    [Header("Курсор")]
    public Texture2D cursorTexture;                 // курсор
    public Texture2D cursorTextureCrosshair;        // прицел
    public CursorMode cursorMode = CursorMode.Auto;

    //public GameObject cursor;
    //bool cursorVisible;
    //public Vector2 hotSpot = Vector2.zero;

    void OnMouseEnter()
    {
        
    }

    private void Awake()
    {
        instance = this;
        player = GameManager.instance.player;
    }

    void Start()
    {
        UpdateHealthText(false, false);             // одновляем хп при старте

        if (GameManager.instance.pozorCount > 0)    // вкл метки позора, если они есть
        {
            pozorGo.SetActive(true);
        }

/*        if (GameManager.instance.musicOff)
        {
            buttonMusicOn.SetActive(false);
            buttonMusicOff.SetActive(true);
        }*/
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            barsOff = !barsOff;
            if (barsOff)
                bars.SetActive(false);
            else
                bars.SetActive(true);
        }

        /*        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                cursor.transform.position = cursorPos;*/

        /*        if (!cursorVisible)
                    Cursor.visible = false;*/
    }

    void FixedUpdate()                                                   // (потом изменить вывод сообщений)
    {
        // HP
        //hp.text = player.currentHealth.ToString("0");

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
                ammoRangeWeapon.text = GameManager.instance.ammoManager.ammoWeapons[GameManager.instance.player.weaponHolder.currentWeapon.weaponIndexForAmmo].allAmmo.ToString("0");    // находим кол-во патронов текущего оружия
        }

        // Активная бомба
        if (GameManager.instance.player.bombWeaponHolder.currentWeapon)
        {
            bombName.text = GameManager.instance.player.bombWeaponHolder.currentWeaponName;
            ammoBomb.text = GameManager.instance.ammoManager.ammoBombs[GameManager.instance.player.bombWeaponHolder.currentWeapon.weaponIndexForAmmo].allAmmo.ToString("0");       // находим кол-во патронов текущего оружия
        }

        // Арена
        if (ArenaManager.instance.arenaLevel)
        {
            arenaEnemyKilledCount.text = ArenaManager.instance.arenaEnemyKilled.ToString("0");
            arenaBossKilledCount.text = ArenaManager.instance.arenaBossKilled.ToString("0");
        }        
    }


    // Обновление хп бара
    public void UpdateHealthText(bool dmg, bool withAnim)
    {
        hp.text = player.currentHealth.ToString("0");           // обновляем текст

        if (withAnim)
        {
            if (dmg)
            {
                animatorHp.Play(takeDamageAnimation, 0, 0.0f);      // анимация UI получения урона
            }
            else
            {
                animatorHp.Play(takeHealAnimation, 0, 0.0f);
            }
        }
        
        // когда мало хп
        if (player.currentHealth <= 25)
        {
            animatorHp.SetBool("Low_HP", true);
        }
        else
        {
            animatorHp.SetBool("Low_HP", false);
        }
    }



    // Для меню
    // продолжить игру
    public void ContinueGameButton()                
    {
        GameManager.instance.ContinueGame();
    }

    // выйти в главное меню
    public void GoToMainMenuButton()                
    {
        GameManager.instance.NextScene(0);
    }

    // выйти в виндоус
    public void ExitGameButton()                    
    {
        Application.Quit();
        Debug.Log("Exit!");
    }

    // показать или скрыть меню
    public void ShowMenu(bool status)               
    {
        if (LanguageManager.instance.eng)
            menuEng.SetActive(status);
        else
            menu.SetActive(status);
    }

    // вкл/выкл музыки
    public void Music(bool status)                  
    {
        AudioManager.instance.StartStopTrack(status);
        GameManager.instance.musicOff = !status;
    }

    // вкл/выкл отображения урона
    public void ShowDamage(bool status)
    {
        GameManager.instance.showDamage = status;
    }

    // вкл/выкл тряску экрана
    public void ScreenShake(bool status)
    {
        GameManager.instance.screenShakeOff = status;
    }

    // Следующий трек
    public void NextTrack()
    {
        AudioManager.instance.SetNextTrack();
    }

    // Сохранение
    public void Saving()
    {
        if (LanguageManager.instance.eng)
            savingEng.SetTrigger("Saving");
        else
            saving.SetTrigger("Saving");
    }




    // Курсор
    public void CursorVisibleOnOff(bool status)
    {
        /*        if (status)
                {
                    cursorVisible = true;
                    Cursor.visible = true;
                    cursor.SetActive(false);
                }
                if (!status)
                {
                    cursorVisible = false;
                    //Cursor.visible = false;
                    cursor.SetActive(true);
                }*/

        if (status)
        {
            Texture2D texture2D = cursorTexture;
            Vector2 hotspot = Vector2.zero;
            ChangeCursor(texture2D, hotspot);

        }
        if (!status)
        {
            Texture2D texture2D = cursorTextureCrosshair;
            Vector2 hotspot = new Vector2(texture2D.width / 2, texture2D.height / 2);
            ChangeCursor(texture2D, hotspot);
        }
    }

    void ChangeCursor(Texture2D cursorType, Vector2 hotspot)
    {        
        Cursor.SetCursor(cursorType, hotspot, cursorMode);
    }
}
