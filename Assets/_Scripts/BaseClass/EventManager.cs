using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;                // инстанс

    [Header("Настройки арены")]    
    public EnemySpawner[] enemySpawners;                // спавнеры
    public EnemySpawner bossSpawner;                    // босс спавнер
    public int arenaMaxEnemys;                          // макс кол-во врагов
    [HideInInspector] public int arenaEnemyCount;       // врагов на арене
    [HideInInspector] public int arenaBossCount;        // боссов на арене
    [HideInInspector] public bool arenaStart;           // арена началась
    [HideInInspector] public bool arenaSpawnStarted;    // спавнеры арены запущены
    bool arenaBossStart;                                // для запуска спавнера боссов
    float time;                                         // время на арене
    public float[] timer;                               // сколько до следующего усиления арены
    public bool[] timerDone;                            // усиление сделано
    int i;                                              // счетчик

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        ArenaUpdate();              // апдейт арены, остановка спауна если слишком много врагов   
    }

    private void FixedUpdate()
    {
        ArenaTimer();               // счетчик таймера арены
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


        // Установка сложности по таймеру
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












    // Свет для 3-го лвл (пока не использую)
    public void LightOff(bool status)
    {
        GameManager.instance.LightsOff(status);
    }

    public void PlayerMiniLightOn(bool status)
    {
        GameManager.instance.player.MiniLightOn(status);
    }
}
