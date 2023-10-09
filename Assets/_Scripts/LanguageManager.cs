using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager instance;          // �������

    [HideInInspector] public bool eng;          // ����
    public bool hardCoreMode;                   // ������� ���

    public FixedJoystick joystickMove;
    public FixedJoystick joystickFire;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;                                // �������� instance (?) ����� ������� � �� ������ �������� ��������� ������� ��������   
    }

    public void MakeEng(bool status)
    {
        eng = status;
        if (status)
            PlayerPrefs.SetInt("Language", 1);      // ���� ����
        else
            PlayerPrefs.SetInt("Language", 2);      // ���� ��
    }

    public void MakeHardCore(bool status)
    {
        hardCoreMode = status;
    }

    public void SwapWeapons()
    {
        GameManager.instance.player.weaponHolder.SwapWeapon();
    }
}
