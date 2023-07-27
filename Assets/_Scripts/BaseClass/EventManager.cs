using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;                // инстанс

    [Header("Ќастройки арены")]    
    public EnemySpawner[] enemySpawners;                // спавнеры
    public EnemySpawner bossSpawner;                    // босс спавнер
    public WaveSpawner[] waveSpawners;                  // волновые спавнеры
    public int arenaMaxEnemys;                          // макс кол-во врагов
    public int arenaMaxBosses = 1;                      // макс кол-во боссов
    [HideInInspector] public int arenaEnemyCount;       // врагов на арене
    [HideInInspector] public int arenaBossCount;        // боссов на арене
    [HideInInspector] public bool arenaStart;           // арена началась
    [HideInInspector] public bool arenaSpawnStarted;    // спавнеры арены запущены
    bool arenaBossStart;                                // дл€ запуска спавнера боссов
    float time;                                         // врем€ на арене
    public float[] timer;                               // сколько до следующего усилени€ арены
    public bool[] timerDone;                            // усиление сделано
    int i;                                              // счетчик
    [HideInInspector] public int arenaEnemyKilled;      // сколько врагов убито
    [HideInInspector] public int arenaBossKilled;       // сколько боссов убито

    [Header("ќружи€")]
    public GameObject[] weapons;
    public int[] countForWeapon;
    public bool[] weaponSpawned;                        // усиление сделано
    int j;                                              // счетчик



    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            waveSpawners[0].MakeWave();
            waveSpawners[1].MakeWave();
            waveSpawners[2].MakeWave();
            waveSpawners[3].MakeWave();
        }

        ArenaUpdate();              // апдейт арены, остановка спауна если слишком много врагов   
    }

    private void FixedUpdate()
    {
        ArenaTimer();               // счетчик таймера арены
    }

    public void ArenaStartStop(bool status)
    {
        arenaStart = status;        // запуск арены
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

        if (arenaEnemyCount >= arenaMaxEnemys)      // если врагов больше, чем положено
        {
            arenaSpawnStarted = false;              // останавливаем спаун
        }
        else if (!arenaSpawnStarted)
        {
            arenaSpawnStarted = true;
        }

        if (arenaBossStart)                         // если спаун боссов запущен
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


        // ”становка сложности по таймеру
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
                ArenaSpawnersSetCooldown(6);
            }
            if (i == 2)
            {
                ArenaAddNewEnemy(2);
                arenaMaxEnemys = 20;
                waveSpawners[1].MakeWave();
            }
            if (i == 3)
            {
                arenaMaxEnemys = 25;            // максимум врагов на арене
                waveSpawners[2].MakeWave();     // вызываем волну скелетов (5 шт)                
                ArenaSpawnersSetCooldown(4);    // устанавливаем кд спаунеров                
            }
            if (i == 4)
            {
                arenaMaxEnemys = 0;
            }
            if (i == 5)
            {
                arenaBossStart = true;          // запускаем спаун боссов
                arenaMaxEnemys = 30;
                waveSpawners[0].MakeWave();
                waveSpawners[3].MakeWave();
            }
            if (i == 6)
            {
                arenaMaxBosses = 3;             // максимум боссов на арене
                bossSpawner.cooldown = 30;      // кд боссов на арене
                waveSpawners[0].MakeWave();
                waveSpawners[3].MakeWave();
                ArenaSpawnersSetCooldown(2);
            }

            timerDone[i] = true;                // событие выполнено
            if (i >= 6)
                return;
            i++;
        }

        // —паун оружи€ от кол-ва убитых врагов
        if (arenaEnemyKilled >= countForWeapon[j] && !weaponSpawned[j])
        {
            Debug.Log(weapons.Length);
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
        //Debug.Log(time);
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

    public void MakeLvlArena()
    {
        GameManager.instance.arenaLvl = true;
    }












    // —вет дл€ 3-го лвл (пока не использую)
    public void LightOff(bool status)
    {
        GameManager.instance.LightsOff(status);
    }

    public void PlayerMiniLightOn(bool status)
    {
        GameManager.instance.player.MiniLightOn(status);
    }
}
