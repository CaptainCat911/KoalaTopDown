using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ArenaManager : MonoBehaviour
{
    public static ArenaManager instance;                // инстанс

    public bool darkLevel;
    public bool arenaLevel;
    public UnityEvent[] events;                         // ивенты

    [Header("Настройки арены")]    
    public EnemySpawner[] enemySpawners;                // спавнеры
    public EnemySpawner bossSpawner;                    // босс спавнер
    public WaveSpawner[] waveSpawners;                  // волновые спавнеры
    public int arenaMaxEnemys;                          // макс кол-во врагов
    public int arenaMaxBosses = 1;                      // макс кол-во боссов
    [HideInInspector] public int arenaEnemyCount;       // врагов на арене
    [HideInInspector] public int arenaBossCount;        // боссов на арене
    [HideInInspector] public bool arenaStart;           // арена началась
    [HideInInspector] public bool arenaSpawnStarted;    // спавнеры арены запущены
    bool arenaBossStart;                                // для запуска спавнера боссов
    float time;                                         // время на арене
    public float[] timer;                               // сколько до следующего усиления арены
    public bool[] timerDone;                            // усиление сделано
    int i;                                              // счетчик
    [HideInInspector] public int arenaEnemyKilled;      // сколько врагов убито
    [HideInInspector] public int arenaBossKilled;       // сколько боссов убито

    [Header("Оружия")]
    public GameObject[] weapons;                        // оружия для спауна
    public int[] countForWeapon;                        // счетчик убитых врагов для спауна оружия
    public bool[] weaponSpawned;                        // усиление сделано
    int j;                                              // счетчик

    [Header("Темный уровень")]
    public GameObject forceShield;
    int ghostKilled;

    [Header("Белый экран")]
    public bool noStartWhiteScreen;
    public Animator whiteScreenAnimator;
    //public GameObject loadingImage;

    [Header("Подсказки")]
    public int dialogeNumberForResroom;
    public PauseHelp pozorHelp;



    private void Awake()
    {        
        instance = this;
    }

    public void StartEvent(int number)
    {
        events[number].Invoke();
    }

    public void NextScene(int number)
    {
        GameManager.instance.NextScene(number);
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

        ArenaUpdate();              // апдейт арены, остановка спауна если слишком много врагов   
    }

    private void FixedUpdate()
    {
        if (!arenaLevel)
            return;

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


        /*        // Установка сложности по таймеру
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
                        ArenaAddNewEnemy(3);
                        arenaMaxEnemys = 25;            // максимум врагов на арене
                        waveSpawners[2].MakeWave();     // вызываем волну скелетов (5 шт)                
                        ArenaSpawnersSetCooldown(8);    // устанавливаем кд спаунеров                
                    }
                    if (i == 4)
                    {
                        arenaMaxEnemys = 0;
                    }
                    if (i == 5)
                    {
                        arenaBossStart = true;          // запускаем спаун боссов
                        //arenaMaxEnemys = 30;
                        waveSpawners[0].MakeWave();
                        waveSpawners[3].MakeWave();
                    }
                    if (i == 6)
                    {
                        //arenaMaxBosses = 2;             // максимум боссов на арене
                        bossSpawner.cooldown = 45;      // кд боссов на арене
                        waveSpawners[0].MakeWave();
                        waveSpawners[3].MakeWave();
                        ArenaSpawnersSetCooldown(4);
                    }
                    if (i == 7)
                    {
                        //arenaMaxBosses = 3;             // максимум боссов на арене
                        bossSpawner.cooldown = 30;      // кд боссов на арене
                        waveSpawners[0].MakeWave();
                        waveSpawners[3].MakeWave();                
                    }

                    timerDone[i] = true;                // событие выполнено
                    if (i >= 7)
                        return;
                    i++;
                }*/

        // Установка сложности по убийству скелетов
        if (arenaEnemyKilled >= timer[i] && !timerDone[i])
        {
            if (i == 0)
            {
                ArenaAddNewEnemy(0);            // вариоры
                ArenaAddNewEnemy(0);
                ArenaAddNewEnemy(0);
                ArenaAddNewEnemy(0);
                arenaMaxEnemys = 15;
                waveSpawners[0].MakeWave();
            }
            if (i == 1)
            {
                ArenaAddNewEnemy(1);            // маги
                ArenaAddNewEnemy(1);
                arenaMaxEnemys = 20;
                waveSpawners[3].MakeWave();
                ArenaSpawnersSetCooldown(12);
            }
            if (i == 2)
            {
                ArenaAddNewEnemy(2);            // большие вариоры
                arenaMaxEnemys = 25;
                waveSpawners[1].MakeWave();
                waveSpawners[2].MakeWave();
            }
            if (i == 3)
            {
                ArenaAddNewEnemy(3);            // большие маги
                arenaMaxEnemys = 25;            // максимум врагов на арене
                waveSpawners[0].MakeWave();     // вызываем волну скелетов (5 шт)                
                waveSpawners[3].MakeWave();     // вызываем волну скелетов (5 шт)                
                ArenaSpawnersSetCooldown(8);    // устанавливаем кд спаунеров                
            }
            if (i == 4)
            {
                arenaBossStart = true;          // запускаем спаун боссов
                //bossSpawner.cooldown = 120;     // кд боссов на арене (установил на самом спавнере)
                arenaMaxEnemys = 25;
                arenaMaxBosses = 1;
                waveSpawners[1].MakeWave();
                waveSpawners[2].MakeWave();
                //Debug.Log("i == 4!");
            }
            if (i == 5)
            {
                waveSpawners[0].SetNewEnemy(0); // варриоры в вейв спавнерах
                waveSpawners[1].SetNewEnemy(0);
                waveSpawners[2].SetNewEnemy(0);
                waveSpawners[3].SetNewEnemy(0);

                arenaMaxEnemys = 25;
                arenaMaxBosses = 2;             // максимум боссов на арене
                bossSpawner.cooldown = 90;      // кд боссов на арене
                waveSpawners[0].MakeWave();
                waveSpawners[3].MakeWave();
                ArenaSpawnersSetCooldown(4);
                //Debug.Log("i == 5!");
            }
            if (i == 6)
            {
                arenaMaxEnemys = 25;
                arenaMaxBosses = 3;             // максимум боссов на арене
                bossSpawner.cooldown = 60;      // кд боссов на арене
                waveSpawners[1].MakeWave();
                waveSpawners[2].MakeWave();
                //Debug.Log("i == 6!");
            }
            if (i == 7)
            {
                ArenaAddNewEnemy(2);
                arenaMaxEnemys = 25;
                arenaMaxBosses = 3;             // максимум боссов на арене
                bossSpawner.cooldown = 60;      // кд боссов на арене
                waveSpawners[0].MakeWave();
                waveSpawners[3].MakeWave();
            }
            if (i == 8)
            {
                waveSpawners[0].SetNewEnemy(1); // минибоссы варриоры в вейв спавнерах
                waveSpawners[1].SetNewEnemy(1);
                waveSpawners[2].SetNewEnemy(1);
                waveSpawners[3].SetNewEnemy(1);

                ArenaAddNewEnemy(3);
                arenaMaxEnemys = 25;
                arenaMaxBosses = 3;             // максимум боссов на арене
                bossSpawner.cooldown = 60;      // кд боссов на арене
                waveSpawners[1].MakeWave();
                waveSpawners[2].MakeWave();
            }
            if (i == 9)
            {                
                arenaMaxEnemys = 25;
                arenaMaxBosses = 3;             // максимум боссов на арене
                bossSpawner.cooldown = 30;      // кд боссов на арене
                waveSpawners[0].MakeWave();
                waveSpawners[3].MakeWave();
            }
/*            if (i == 10)
            {
                arenaMaxEnemys = 0;
                arenaMaxBosses = 0;
                StartMegaBoss();
            }*/

            // тут ещё темного босса добавить

            timerDone[i] = true;                // событие выполнено
            if (i >= 9)
            {                
                return;
            }
            i++;
            //Debug.Log(i);
        }



        // Спаун оружия от кол-ва убитых врагов
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


    void StartMegaBoss()
    {

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

    /*    public void MakeLvlArena()
        {
            GameManager.instance.arenaLvl = true;
        }*/


    // Для белого экрана босса
    public void WhiteScreenEnd()
    {
        whiteScreenAnimator.SetTrigger("EndScreen");
    }

    public void SayTextPlayer(string text)
    {
        GameManager.instance.player.SayText(text);
    }

    // Свет для 3-го лвл
    public void LightOff(bool status)
    {
        GameManager.instance.LightsOff(status);
    }

    public void PlayerMiniLightOn(bool status)
    {
        GameManager.instance.player.MiniLightOn(status);
    }

    // Щит ключа для 3-го лвл
    public void ShieldKeyOff()
    {
        ghostKilled++;
        if (ghostKilled >= 3)
        {
            forceShield.SetActive(false);
        }
    }

    // Диалог для ресрума
    public void ResroomOn()
    {
        if (!GameManager.instance.resroomed)
        {
            GameManager.instance.resroomed = true;              
            DialogManager.instance.StartEvent(dialogeNumberForResroom);         // запускаем диалог
        }
    }

    // Подсказка для позора
    public void PozorOn()
    {
        if (!GameManager.instance.pozored)
        {
            GameManager.instance.pozored = true;                // опозорен
            pozorHelp.StartHelpPause();                         // запускаем подсказку для позора
        }
    }
}
