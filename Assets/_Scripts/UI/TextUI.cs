using UnityEngine;
using UnityEngine.UI;

public class TextUI : MonoBehaviour
{
    Player player;                  // ������ �� ������
    public Text hp;                 // ���-�� ��
    public Text shield;             // ���
    public Text gold;               // ���-�� ������
    public Text key;                // ���-�� ������
    public Text battery;            // ���-�� �������
    public Text weaponName;         // ��� ������
    public Text ammoRangeWeapon;    // ���-�� ��������
    public Text ammoBomb;           // ���-�� ����
    public Text bombName;           // ��� �����


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

        // ������
        gold.text = GameManager.instance.gold.ToString("0");

        // �����
        key.text = GameManager.instance.keys.ToString("0");

        // �������
        battery.text = GameManager.instance.battery.ToString("0");

        // �������� ������
        if (GameManager.instance.player.weaponHolder.currentWeapon)
        {
            weaponName.text = GameManager.instance.player.weaponHolder.currentWeaponName;       // �������� �������� ������
            if (GameManager.instance.player.weaponHolder.meleeWeapon)                           // ���� ������ ����
                ammoRangeWeapon.text = "-";                                                     // ������� "-" � ���� ��������
            else
                ammoRangeWeapon.text = GameManager.instance.ammoPack.ammoWeapons[GameManager.instance.
                    player.weaponHolder.currentWeapon.weaponIndexForAmmo].allAmmo.ToString("0");       // ������� ���-�� �������� �������� ������
        }

        // �������� �����
        if (GameManager.instance.player.bombWeaponHolder.currentWeapon)
        {
            bombName.text = GameManager.instance.player.bombWeaponHolder.currentWeaponName;
            ammoBomb.text = GameManager.instance.player.bombWeaponHolder.currentWeapon.ammo.ToString("0");
        }
    }
}
