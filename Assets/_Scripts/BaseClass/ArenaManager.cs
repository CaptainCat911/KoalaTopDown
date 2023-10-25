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
    public GameObject hardcoreStuffs;

    [Header("��������� �����")]    
    public EnemySpawner[] enemySpawners;                // ��������
    public EnemySpawner bossSpawner;                    // ���� �������
    public EnemySpawner megaBossSpawner;                // ���� �������
    public WaveSpawner[] waveSpawners;                  // �������� ��������
    public GameObject shadowBossPrefab;
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
    public int arenaEnemyKilled;                        // ������� ������ �����
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
    //public GameObject loadingImage;

    [Header("���������")]
    public int dialogeNumberForResroom;
    public PauseHelp pozorHelp;             // ��������� ������
    public PauseHelp spacePozorHelp;        // ��������� ������������ ������
    bool spacePozored;



    private void Awake()
    {        
        instance = this;

        if (LanguageManager.instance.hardCoreMode)      // ���� ������� �������
        {
            HardCoreStuffsOn();                         // �������� ����� ��� ��������
        }
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
        if (LanguageManager.instance.hardCoreMode && GameManager.instance.pozorCount >= 10 && !spacePozored)      // ��� ��������
        {
            spacePozorHelp.StartHelpPause();
            spacePozored = true;
        }

        if (!arenaLevel)
            return;

/*        if (Input.GetKeyDown(KeyCode.Y))
        {
            StartResEnemy(2);
            //StartMegaBoss();
        }*/

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


        /*        // ��������� ��������� �� �������
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
                        //arenaMaxEnemys = 30;
                        waveSpawners[0].MakeWave();
                        waveSpawners[3].MakeWave();
                    }
                    if (i == 6)
                    {
                        //arenaMaxBosses = 2;             // �������� ������ �� �����
                        bossSpawner.cooldown = 45;      // �� ������ �� �����
                        waveSpawners[0].MakeWave();
                        waveSpawners[3].MakeWave();
                        ArenaSpawnersSetCooldown(4);
                    }
                    if (i == 7)
                    {
                        //arenaMaxBosses = 3;             // �������� ������ �� �����
                        bossSpawner.cooldown = 30;      // �� ������ �� �����
                        waveSpawners[0].MakeWave();
                        waveSpawners[3].MakeWave();                
                    }

                    timerDone[i] = true;                // ������� ���������
                    if (i >= 7)
                        return;
                    i++;
                }*/

        // ��������� ��������� �� �������� ��������
        if (arenaEnemyKilled >= timer[i] && !timerDone[i])
        {
            if (i == 0)
            {
                ArenaAddNewEnemy(0);            // �������
                ArenaAddNewEnemy(0);
                ArenaAddNewEnemy(0);
                ArenaAddNewEnemy(0);
                arenaMaxEnemys = 15;
                waveSpawners[0].MakeWave();
            }
            if (i == 1)
            {
                ArenaAddNewEnemy(1);            // ����
                ArenaAddNewEnemy(1);
                arenaMaxEnemys = 20;
                waveSpawners[3].MakeWave();
                ArenaSpawnersSetCooldown(12);
            }
            if (i == 2)
            {
                ArenaAddNewEnemy(2);            // ������� �������
                arenaMaxEnemys = 25;
                waveSpawners[1].MakeWave();
                waveSpawners[2].MakeWave();
                ArenaSpawnersSetCooldown(8);    // ������������� �� ���������                
            }
            if (i == 3)
            {
                ArenaAddNewEnemy(3);            // ������� ����
                arenaMaxEnemys = 25;            // �������� ������ �� �����
                waveSpawners[0].MakeWave();     // �������� ����� �������� (5 ��)                
                waveSpawners[3].MakeWave();     // �������� ����� �������� (5 ��)                
            }
            if (i == 4)
            {
                arenaBossStart = true;          // ��������� ����� ������
                //bossSpawner.cooldown = 120;     // �� ������ �� ����� (��������� �� ����� ��������)
                arenaMaxEnemys = 25;
                arenaMaxBosses = 1;
                waveSpawners[1].MakeWave();
                waveSpawners[2].MakeWave();
                //Debug.Log("i == 4!");
            }
            if (i == 5)
            {
                waveSpawners[0].SetNewEnemy(0); // �������� � ���� ���������
                waveSpawners[1].SetNewEnemy(0);
                waveSpawners[2].SetNewEnemy(0);
                waveSpawners[3].SetNewEnemy(0);

                arenaMaxEnemys = 25;
                arenaMaxBosses = 2;             // �������� ������ �� �����
                bossSpawner.cooldown = 90;      // �� ������ �� �����
                waveSpawners[0].MakeWave();
                waveSpawners[3].MakeWave();
                ArenaSpawnersSetCooldown(4);
                //Debug.Log("i == 5!");
            }
            if (i == 6)
            {
                arenaMaxEnemys = 25;
                arenaMaxBosses = 3;             // �������� ������ �� �����
                bossSpawner.cooldown = 60;      // �� ������ �� �����
                waveSpawners[1].MakeWave();
                waveSpawners[2].MakeWave();
                //Debug.Log("i == 6!");
            }
            if (i == 7)
            {
                ArenaAddNewEnemy(2);
                arenaMaxEnemys = 25;
                arenaMaxBosses = 3;             // �������� ������ �� �����
                bossSpawner.cooldown = 60;      // �� ������ �� �����
                waveSpawners[0].MakeWave();
                waveSpawners[3].MakeWave();
            }
            if (i == 8)
            {
                waveSpawners[0].SetNewEnemy(1); // ��������� �������� � ���� ���������
                waveSpawners[1].SetNewEnemy(1);
                waveSpawners[2].SetNewEnemy(1);
                waveSpawners[3].SetNewEnemy(1);

                ArenaAddNewEnemy(3);
                arenaMaxEnemys = 25;
                arenaMaxBosses = 3;             // �������� ������ �� �����
                bossSpawner.cooldown = 60;      // �� ������ �� �����
                waveSpawners[1].MakeWave();
                waveSpawners[2].MakeWave();
            }
            if (i == 9)
            {                
                arenaMaxEnemys = 25;
                arenaMaxBosses = 3;             // �������� ������ �� �����
                bossSpawner.cooldown = 30;      // �� ������ �� �����
                waveSpawners[0].MakeWave();
                waveSpawners[3].MakeWave();
            }

            if (i == 10)
            {
                Debug.Log("SBoss!");
                megaBossSpawner.WaveSpawnEnemy();   // ������� ����� �����
            }

            if (i == 11)
            {
                Debug.Log("ResSkeletons!");
                //StartResEnemy(1);               // ������������ 1 ���
                ArenaAddNewEnemy(2);            // ������� �������
                ArenaAddNewEnemy(3);            // ������� ����
            }
            if (i == 12)
            {
                ArenaAddNewEnemy(2);            // ������� �������
                ArenaAddNewEnemy(3);            // ������� ����
            }
            if (i == 13)
            {
                ArenaAddNewEnemy(2);            // ������� �������
                ArenaAddNewEnemy(3);            // ������� ����
            }
            if (i == 14)
            {
                megaBossSpawner.WaveSpawnEnemy();
                megaBossSpawner.WaveSpawnEnemy();
            }
            if (i == 15)
            {
                //StartResEnemy(2);
                ArenaAddNewEnemy(2);            // ������� �������
                ArenaAddNewEnemy(3);            // ������� ����
            }
            if (i == 16)
            {
                ArenaAddNewEnemy(2);            // ������� �������
                ArenaAddNewEnemy(3);            // ������� ����
            }
            if (i == 17)
            {
                ArenaAddNewEnemy(2);            // ������� �������
                ArenaAddNewEnemy(3);            // ������� ����
            }
            if (i == 18)
            {
                megaBossSpawner.WaveSpawnEnemy();
                megaBossSpawner.WaveSpawnEnemy();
                megaBossSpawner.WaveSpawnEnemy();
            }
            if (i == 19)
            {
                ArenaAddNewEnemy(2);            // ������� �������
                ArenaAddNewEnemy(2);            // ������� �������
                ArenaAddNewEnemy(2);            // ������� �������
                ArenaAddNewEnemy(2);            // ������� �������
                ArenaAddNewEnemy(2);            // ������� �������
                ArenaAddNewEnemy(2);            // ������� �������
                ArenaAddNewEnemy(3);            // ������� ����
                ArenaAddNewEnemy(3);            // ������� ����
                ArenaAddNewEnemy(3);            // ������� ����
                ArenaAddNewEnemy(3);            // ������� ����
                ArenaAddNewEnemy(3);            // ������� ����
                ArenaAddNewEnemy(3);            // ������� ����
            }
            if (i == 20)
            {
                //StartResEnemy(0);
                arenaMaxBosses = 0;             
                arenaMaxEnemys = 20;
                StartMegaBoss();

                GameManager.instance.player.MakeSuperHero();
                GameManager.instance.pozorCount = 0;
                if (LanguageManager.instance.eng)
                {
                    GameManager.instance.CreateFloatingMessage("SHAME WASHED AWAY!", Color.white, GameManager.instance.player.transform.position);
                }
                else
                {
                    GameManager.instance.CreateFloatingMessage("����� ����!", Color.white, GameManager.instance.player.transform.position);
                }
            }

            // ��� ��� ������� ����� ��������

            timerDone[i] = true;                // ������� ���������
            if (i >= timerDone.Length - 1)
            {                
                return;
            }
            i++;
            //Debug.Log(i);
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

    void StartResEnemy(int i)
    {
        foreach (EnemySpawner enemySpawner in enemySpawners)
        {
            enemySpawner.resTimes = i;
        }
    }


    void StartMegaBoss()
    {
        foreach (EnemySpawner enemySpawner in enemySpawners)
        {
            enemySpawner.noItem = false;
            enemySpawner.withItem = true;
            enemySpawner.itemToSpawn = shadowBossPrefab;
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

    /*    public void MakeLvlArena()
        {
            GameManager.instance.arenaLvl = true;
        }*/


    // ��� ������ ������ �����
    public void WhiteScreenEnd()
    {
        whiteScreenAnimator.SetTrigger("EndScreen");
    }



    // ����� �������
    // Ru ������
    public void SayTextPlayer(string text)
    {
        if (LanguageManager.instance.eng)
            return;
        GameManager.instance.player.SayText(text);
    }

    // Eng ������
    public void SayTextPlayerEng(string text)
    {
        if (!LanguageManager.instance.eng)
            return;
        GameManager.instance.player.SayText(text);
    }

    public void PlayerLookAt(Transform transform)
    {
        GameManager.instance.player.LookAt(transform);
    }


    // ��� ��������
    public void HardCoreStuffsOn()
    {
        if (hardcoreStuffs)
            hardcoreStuffs.SetActive(true);
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

    public void SetDarkPlaceLight(bool status)
    {
        GameManager.instance.player.darkPlace = status;
    }

    // ��� ����� ��� 3-�� ���
    public void ShieldKeyOff()
    {
        ghostKilled++;
        if (ghostKilled >= 3)
        {
            forceShield.SetActive(false);
        }
    }

    // ������ ��� �������
    public void ResroomOn()
    {
        if (!GameManager.instance.resroomed)
        {
            GameManager.instance.resroomed = true;              
            DialogManager.instance.StartEvent(dialogeNumberForResroom);         // ��������� ������
        }
    }

    // ��������� ��� ������
    public void PozorOn()
    {
        if (!GameManager.instance.pozored)
        {
            GameManager.instance.pozored = true;        // ��������
            pozorHelp.StartHelpPause();                 // ��������� ��������� ��� ������
        }
    }

    public void ResetHardcoreLevels()
    {
        spacePozored = false;
        GameManager.instance.pozorCount = 0;
        GameManager.instance.player.currentHealth = GameManager.instance.player.maxHealth;
        GameManager.instance.NextScene(1);              // ���������� � 1-� �����
    }
}
