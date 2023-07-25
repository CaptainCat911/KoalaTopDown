using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;                // �������

    [Header("��������� �����")]    
    public EnemySpawner[] enemySpawners;                // ��������
    public EnemySpawner bossSpawner;                    // ���� �������
    public int arenaMaxEnemys;                          // ���� ���-�� ������
    [HideInInspector] public int arenaEnemyCount;       // ������ �� �����
    [HideInInspector] public int arenaBossCount;        // ������ �� �����
    [HideInInspector] public bool arenaStart;           // ����� ��������
    [HideInInspector] public bool arenaSpawnStarted;    // �������� ����� ��������
    bool arenaBossStart;                                // ��� ������� �������� ������
    float time;                                         // ����� �� �����
    public float[] timer;                               // ������� �� ���������� �������� �����
    public bool[] timerDone;                            // �������� �������
    int i;                                              // �������

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        ArenaUpdate();              // ������ �����, ��������� ������ ���� ������� ����� ������   
    }

    private void FixedUpdate()
    {
        ArenaTimer();               // ������� ������� �����
    }

    public void ArenaStartStop(bool status)
    {
        arenaStart = status;
    }

    void ArenaUpdate()
    {
        if (!arenaStart)
            return;

        if (arenaEnemyCount >= arenaMaxEnemys)
        {
            arenaSpawnStarted = false;
        }
        else if (!arenaSpawnStarted)
        {
            arenaSpawnStarted = true;
        }

        if (arenaBossStart)
        {
            if (arenaBossCount > 2)
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
                timerDone[i] = true;
            }
            if (i == 1)
            {
                ArenaAddNewEnemy(1);
                ArenaAddNewEnemy(1);
                arenaMaxEnemys = 20;
                timerDone[i] = true;
            }
            if (i == 2)
            {
                arenaMaxEnemys = 0;
                timerDone[i] = true;
            }
            if (i == 3)
            {
                ArenaAddNewEnemy(2);
                arenaMaxEnemys = 25;
                timerDone[i] = true;
            }
            if (i == 4)
            {
                arenaBossStart = true;
                timerDone[i] = true;
            }
            if (i == 5)
            {
                bossSpawner.cooldown = 1;
                timerDone[i] = true;
            }
            if (i >= 5)
                return;

            i++;
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

    public void MakeLvlArena()
    {
        GameManager.instance.arenaLvl = true;
    }












    // ���� ��� 3-�� ��� (���� �� ���������)
    public void LightOff(bool status)
    {
        GameManager.instance.LightsOff(status);
    }

    public void PlayerMiniLightOn(bool status)
    {
        GameManager.instance.player.MiniLightOn(status);
    }
}
