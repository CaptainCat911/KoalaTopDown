using UnityEngine;
using UnityEngine.UI;

public class TextUI : MonoBehaviour
{
    public static TextUI instance;          // �������

    //public bool startScreen;
    [Header("UI")]
    Player player;                  // ������ �� ������
    public Text hp;                 // ���-�� ��
    public Animator animatorHp;
    public string takeDamageAnimation;
    public string takeHealAnimation;

    public Text shield;             // ���
    public Text bootsEnedgy;        // ������� �������
    public Text gold;               // ���-�� ������
    public Text key;                // ���-�� ������
    public Text keyRed;             // ���-�� ������ ������
    public Text battery;            // ���-�� �������
    public Text weaponName;         // ��� ������
    public Text ammoRangeWeapon;    // ���-�� ��������
    public Text bombName;           // ��� �����
    public Text ammoBomb;           // ���-�� ����

    public GameObject pozorGo;      // ������ (����� ��������)
    public Text pozorCount;         // ������� ������

    public Text arenaEnemyKilledCount;      // ������ �����
    public Text arenaBossKilledCount;       // ����� �����

    public GameObject bars;             // ���� (��� ���������� UI)
    bool barsOff;
    public GameObject menu;             // ����
    public GameObject menuEng;          // ���� ����
    public Animator saving;           // ����������...
    public Animator savingEng;        //

    //public GameObject buttonMusicOn;    // ���������� �������
    //public GameObject buttonMusicOff;   // 

    [Header("������")]
    public Texture2D cursorTexture;                 // ������
    public Texture2D cursorTextureCrosshair;        // ������
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
        UpdateHealthText(false, false);             // ��������� �� ��� ������

        if (GameManager.instance.pozorCount > 0)    // ��� ����� ������, ���� ��� ����
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

    void FixedUpdate()                                                   // (����� �������� ����� ���������)
    {
        // HP
        //hp.text = player.currentHealth.ToString("0");

        // ���
        if (player.shield.shieldOn)
            shield.text = player.shield.shieldHp.ToString("0");
        else
            shield.text = "OFF";

        // �������
        if (player.bootsMod)
            bootsEnedgy.text = player.bootsEnergy.ToString("0");
        else
            bootsEnedgy.text = "OFF";

        // ������
        gold.text = GameManager.instance.gold.ToString("0");

        // �����
        key.text = GameManager.instance.keys[0].ToString("0");

        // ����� ������
        keyRed.text = GameManager.instance.keys[1].ToString("0");

        // �������
        battery.text = GameManager.instance.battery.ToString("0");

        // �����
        pozorCount.text = GameManager.instance.pozorCount.ToString("0");

        // �������� ������
        if (GameManager.instance.player.weaponHolder.currentWeapon)
        {
            weaponName.text = GameManager.instance.player.weaponHolder.currentWeaponName;           // �������� �������� ������

            if (GameManager.instance.player.weaponHolder.meleeWeapon)                               // ���� ������ ����
                ammoRangeWeapon.text = "-";                                                         // ������� "-" � ���� ��������
            else
                ammoRangeWeapon.text = GameManager.instance.ammoManager.ammoWeapons[GameManager.instance.player.weaponHolder.currentWeapon.weaponIndexForAmmo].allAmmo.ToString("0");    // ������� ���-�� �������� �������� ������
        }

        // �������� �����
        if (GameManager.instance.player.bombWeaponHolder.currentWeapon)
        {
            bombName.text = GameManager.instance.player.bombWeaponHolder.currentWeaponName;
            ammoBomb.text = GameManager.instance.ammoManager.ammoBombs[GameManager.instance.player.bombWeaponHolder.currentWeapon.weaponIndexForAmmo].allAmmo.ToString("0");       // ������� ���-�� �������� �������� ������
        }

        // �����
        if (ArenaManager.instance.arenaLevel)
        {
            arenaEnemyKilledCount.text = ArenaManager.instance.arenaEnemyKilled.ToString("0");
            arenaBossKilledCount.text = ArenaManager.instance.arenaBossKilled.ToString("0");
        }        
    }


    // ���������� �� ����
    public void UpdateHealthText(bool dmg, bool withAnim)
    {
        hp.text = player.currentHealth.ToString("0");           // ��������� �����

        if (withAnim)
        {
            if (dmg)
            {
                animatorHp.Play(takeDamageAnimation, 0, 0.0f);      // �������� UI ��������� �����
            }
            else
            {
                animatorHp.Play(takeHealAnimation, 0, 0.0f);
            }
        }
        
        // ����� ���� ��
        if (player.currentHealth <= 25)
        {
            animatorHp.SetBool("Low_HP", true);
        }
        else
        {
            animatorHp.SetBool("Low_HP", false);
        }
    }



    // ��� ����
    // ���������� ����
    public void ContinueGameButton()                
    {
        GameManager.instance.ContinueGame();
    }

    // ����� � ������� ����
    public void GoToMainMenuButton()                
    {
        GameManager.instance.NextScene(0);
    }

    // ����� � �������
    public void ExitGameButton()                    
    {
        Application.Quit();
        Debug.Log("Exit!");
    }

    // �������� ��� ������ ����
    public void ShowMenu(bool status)               
    {
        if (LanguageManager.instance.eng)
            menuEng.SetActive(status);
        else
            menu.SetActive(status);
    }

    // ���/���� ������
    public void Music(bool status)                  
    {
        AudioManager.instance.StartStopTrack(status);
        GameManager.instance.musicOff = !status;
    }

    // ���/���� ����������� �����
    public void ShowDamage(bool status)
    {
        GameManager.instance.showDamage = status;
    }

    // ���/���� ������ ������
    public void ScreenShake(bool status)
    {
        GameManager.instance.screenShakeOff = status;
    }

    // ��������� ����
    public void NextTrack()
    {
        AudioManager.instance.SetNextTrack();
    }

    // ����������
    public void Saving()
    {
        if (LanguageManager.instance.eng)
            savingEng.SetTrigger("Saving");
        else
            saving.SetTrigger("Saving");
    }




    // ������
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
