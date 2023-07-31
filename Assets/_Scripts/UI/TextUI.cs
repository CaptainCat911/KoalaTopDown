using UnityEngine;
using UnityEngine.UI;

public class TextUI : MonoBehaviour
{
    Player player;                  // ������ �� ������
    public Text hp;                 // ���-�� ��
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

    public Text arenaEnemyKilledCount;      // ������ �����
    public Text arenaBossKilledCount;       // ����� �����


    void Start()
    {
        player = GameManager.instance.player;
    }
   
    void Update()                                                   // (����� �������� ����� ���������)
    {
        // HP
        hp.text = player.currentHealth.ToString("0");

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

        // �������� ������
        if (GameManager.instance.player.weaponHolder.currentWeapon)
        {
            weaponName.text = GameManager.instance.player.weaponHolder.currentWeaponName;           // �������� �������� ������
            if (GameManager.instance.player.weaponHolder.meleeWeapon)                               // ���� ������ ����
                ammoRangeWeapon.text = "-";                                                         // ������� "-" � ���� ��������
            else
                ammoRangeWeapon.text = GameManager.instance.ammoManager.ammoWeapons[GameManager.instance.
                    player.weaponHolder.currentWeapon.weaponIndexForAmmo].allAmmo.ToString("0");    // ������� ���-�� �������� �������� ������
        }

        // �������� �����
        if (GameManager.instance.player.bombWeaponHolder.currentWeapon)
        {
            bombName.text = GameManager.instance.player.bombWeaponHolder.currentWeaponName;
            ammoBomb.text = GameManager.instance.ammoManager.ammoBombs[GameManager.instance.
                    player.bombWeaponHolder.currentWeapon.weaponIndexForAmmo].allAmmo.ToString("0");       // ������� ���-�� �������� �������� ������
        }

        // �����
        if (ArenaManager.instance.arenaLevel)
        {
            arenaEnemyKilledCount.text = ArenaManager.instance.arenaEnemyKilled.ToString("0");
            arenaBossKilledCount.text = ArenaManager.instance.arenaBossKilled.ToString("0");
        }        
    }
}
