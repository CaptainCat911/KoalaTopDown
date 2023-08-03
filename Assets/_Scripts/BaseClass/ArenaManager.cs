using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ArenaManager : MonoBehaviour
{
    public static ArenaManager instance;                // �������

    public bool darkLevel;
    public bool arenaLevel;
    public UnityEvent[] events;                         // ������

    [Header("��������� �����")]    
    public EnemySpawner[] enemySpawners;                // ��������
    public EnemySpawner bossSpawner;                    // ���� �������
    public WaveSpawner[] waveSpawners;                  // �������� ��������
    public int arenaMaxEnemys;                          // ���� ���-�� ������
    public int arenaMaxBosses = 1;                      // ���� ���-�� ������
    [HideInInspector] public int arenaEnemyCount;       // ������ �� �����
    [HideInInspector] public int arenaBossCount;        // ������ �� �����
    [HideInInspector] public bool arenaStart;           // ����� ��������
    [HideInInspector] public bool arenaSpawnStarted;    // �������� ����� ��������
    bool arenaBossStart;                                // ��� ������� �������� ������
    float time;                                         // ����� �� �����
    public float[] timer;                               // ������� �� ���������� �������� �����
    public bool[] timerDone;                            // �������� �������
    int i;                                              // �������
    [HideInInspector] public int arenaEnemyKilled;      // ������� ������ �����
    [HideInInspector] public int arenaBossKilled;       // ������� ������ �����

    [Header("������")]
    public GameObject[] weapons;                        // ������ ��� ������
    public int[] countForWeapon;                        // ������� ������ ������ ��� ������ ������
    public bool[] weaponSpawned;                        // �������� �������
    int j;                                              // �������

    [Header("������ �������")]
    public GameObject forceShield;
    int ghostKilled;

    [Header("����� �����")]
    public bool noStartWhiteScreen;
    public Animator whiteScreenAnimator;



    private void Awake()
    {        
        instance = this;
    }

    public void StartEvent(int number)
    {
        events[number].Invoke();
    }



    private void Update()
    {
        if (!arenaLevel)
            return;

        if (Input.GetKeyDown(KeyCode.Y))
        {
            waveSpawners[0].MakeWave();
            waveSpawners[1].MakeWave();
            waveSpawners[2].MakeWave();
            waveSpawners[3].MakeWave();
        }

        ArenaUpdate();              // ������ �����, ��������� ������ ���� ������� ����� ������   
    }

    private void FixedUpdate()
    {
        if (!arenaLevel)
            return;

        ArenaTimer();               // ������� ������� �����
    }

    public void ArenaStartStop(bool status)
    {
        arenaStart = status;        // ������ �����
    }

    void ArenaUpdate()
    {
        if (!arenaStart)
        {
            if (arenaSpawnStarted)
                arenaSpawnStarted = false;
            if (bossSpawner.active)
                bossSpawner.active = false;
            return;
        }

        if (arenaEnemyCount >= arenaMaxEnemys)      // ���� ������ ������, ��� ��������
        {
            arenaSpawnStarted = false;              // ������������� �����
        }
        else if (!arenaSpawnStarted)
        {
            arenaSpawnStarted = true;
        }

        if (arenaBossStart)                         // ���� ����� ������ �������
        {
            if (arenaBossCount >= arenaMaxBosses)
            {
                bossSpawner.active = false;
            }
            else if (!bossSpawner.active)
            {
                bossSpawner.active = true;
            }
        }


        // ��������� ��������� �� �������
        if (time >= timer[i] && !timerDone[i])
        {
            if (i == 0)
            {
                ArenaAddNewEnemy(0);
                ArenaAddNewEnemy(0);
                arenaMaxEnemys = 15;
                waveSpawners[0].MakeWave();
            }
            if (i == 1)
            {
                ArenaAddNewEnemy(1);
                ArenaAddNewEnemy(1);
                arenaMaxEnemys = 20;
                waveSpawners[3].MakeWave();
                ArenaSpawnersSetCooldown(12);
            }
            if (i == 2)
            {
                ArenaAddNewEnemy(2);
                arenaMaxEnemys = 20;
                waveSpawners[1].MakeWave();
            }
            if (i == 3)
            {
                arenaMaxEnemys = 25;            // �������� ������ �� �����
                waveSpawners[2].MakeWave();     // �������� ����� �������� (5 ��)                
                ArenaSpawnersSetCooldown(8);    // ������������� �� ���������                
            }
            if (i == 4)
            {
                arenaMaxEnemys = 0;
            }
            if (i == 5)
            {
                arenaBossStart = true;          // ��������� ����� ������
                arenaMaxEnemys = 30;
                waveSpawners[0].MakeWave();
                waveSpawners[3].MakeWave();
            }
            if (i == 6)
            {
                arenaMaxBosses = 2;             // �������� ������ �� �����
                bossSpawner.cooldown = 45;      // �� ������ �� �����
                waveSpawners[0].MakeWave();
                waveSpawners[3].MakeWave();
                ArenaSpawnersSetCooldown(4);
            }
            if (i == 7)
            {
                arenaMaxBosses = 3;             // �������� ������ �� �����
                bossSpawner.cooldown = 30;      // �� ������ �� �����
                waveSpawners[0].MakeWave();
                waveSpawners[3].MakeWave();                
            }

            timerDone[i] = true;                // ������� ���������
            if (i >= 7)
                return;
            i++;
        }

        // ����� ������ �� ���-�� ������ ������
        if (arenaEnemyKilled >= countForWeapon[j] && !weaponSpawned[j])
        {
            //Debug.Log(weapons.Length);
            weapons[j].SetActive(true);
            weaponSpawned[j] = true;
            if (j >= weapons.Length - 1)
                return;
            j++;
        }
    }

    void ArenaTimer()
    {
        if (!arenaStart)
            return;

        time += 0.02f;
        Debug.Log(time);
    }

    void ArenaAddNewEnemy(int number)
    {
        foreach (EnemySpawner enemySpawner in enemySpawners)
        {
            enemySpawner.AddNewEnemy(number);
        }
    }

    void ArenaSpawnersSetCooldown(int number)
    {
        foreach (EnemySpawner enemySpawner in enemySpawners)
        {
            enemySpawner.cooldown = number;
        }
    }

    /*    public void MakeLvlArena()
        {
            GameManager.instance.arenaLvl = true;
        }*/


    // ��� ������ ������ �����
    public void WhiteScreenEnd()
    {
        whiteScreenAnimator.SetTrigger("EndScreen");
    }


    public void SayTextPlayer(string text)
    {
        GameManager.instance.player.SayText(text);
    }


    // ���� ��� 3-�� ���
    public void LightOff(bool status)
    {
        GameManager.instance.LightsOff(status);
    }

    public void PlayerMiniLightOn(bool status)
    {
        GameManager.instance.player.MiniLightOn(status);
    }

    public void ShieldKeyOff()
    {
        ghostKilled++;
        if (ghostKilled >= 3)
        {
            forceShield.SetActive(false);
        }
    }
}
