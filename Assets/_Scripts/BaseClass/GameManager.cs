using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;         // �������

    public UnityEvent[] events;                 // ������
    public string[] sceneNames;                 // ��� �����

    [Header("������")]
    public Player player;                       // ������ �� ������    
    public AmmoPackKoala ammoManager;           // ���� ��������   
    //public GameObject gui;                      // ���
    //public Dialog dialog;                       // ������ ��������    

    [Header("���������� �����")]
    public bool isPlayerEnactive;               // ������� ����� ��� ���
    public bool cameraOnPlayer;                 // ���������� �������
    public bool dialogeStart;                   // ������ �������
    public bool playerInResroom;                // ����� � ������� �����������
    [HideInInspector] public bool playerAtTarget;   // ����� ����� �� ����� ������ �������

    [Header("������� ��������������")]
    public KeyCode keyToUse;                    // ������� ��� ��������

    [Header("��������")]
    public int gold;                            // ������
    public int[] keys;                          // �����
    public int battery;                         // �������     

    [Header("���������� ������")]
    public bool lightDark;
    public bool lightOff;

    // ��� �����
    bool paused;
    bool slowed;

    [Header("��������� �����")]
    public EnemySpawner[] enemySpawners;
    public bool arenaStart;
    public int arenaMaxEnemys;
    public bool arenaSpawnStarted;
    public int arenaEnemyCount;
    // ������
    float time;
    public float timer_1; 
    public float timer_2; 
    public float timer_3;
    public float timer_4;
    bool timerDone_1;
    bool timerDone_2;
    bool timerDone_3;
    bool timerDone_4;


    private void Awake()
    {
        if (GameManager.instance != null)
        {
            Destroy(gameObject);
            Destroy(ammoManager.gameObject);
            Destroy(player.gameObject);
            //Destroy(gui);
            //Destroy(floatingTextManager.gameObject);
            //Destroy(hud);
            //Destroy(menu);
            //Destroy(eventSys);

            return;
        }
        
        instance = this;                                // �������� instance (?) ����� ������� � �� ������ �������� ��������� ������� ��������
        SceneManager.sceneLoaded += OnSceneLoaded;      // ��� �������� ����� ���������� ��� += �������
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!paused)
            {
                Time.timeScale = 0f;
                paused = true;
                Debug.Log("Pause!");
            }
            else
            {
                Time.timeScale = 1f;
                paused = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (!slowed)
            {
                Time.timeScale = 0.5f;
                slowed = true;
            }
            else
            {
                Time.timeScale = 1f;
                slowed = false;
            }
        }

        


        if (Input.GetKeyDown(KeyCode.H))
        {
            ChatBubble.Clear(gameObject);
            ChatBubble.Create(player.transform, new Vector3(0.2f, 0.2f), "�� ���, ��� ����!", 2f);
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            NextScene(3);
        }

        ArenaUpdate();              // ������ �����, ��������� ������ ���� ������� ����� ������       


        /*        if (Input.GetKeyDown(keyOpenMagazine))
                {
                    OpenCloseMagazine();
                }*/
    }

    /*    public void OpenCloseMagazine()
        {
            isPlayerEnactive = !isPlayerEnactive;
            openMagazine = !openMagazine;
            magazine.SetActive(openMagazine);
        }*/


    private void FixedUpdate()
    {
        if (arenaStart)
        {
            ArenaTimer();
        }            
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

        // ��������� ��������� �� �������
        if (time >= timer_1 && !timerDone_1)
        {
            ArenaAddNewEnemy(0);
            ArenaAddNewEnemy(0);
            arenaMaxEnemys = 15;
            timerDone_1 = true;
        }
        if (time >= timer_2 && !timerDone_2)
        {
            ArenaAddNewEnemy(1);
            ArenaAddNewEnemy(1);
            arenaMaxEnemys = 20;
            timerDone_2 = true;
        }
        if (time >= timer_3 && !timerDone_3)
        {
            ArenaAddNewEnemy(2);
            arenaMaxEnemys = 25;
            timerDone_3 = true;
        }
        if (time >= timer_4 && !timerDone_4)
        {
            ArenaAddNewEnemy(3);
            arenaMaxEnemys = 30;
            timerDone_4 = true;
        }
    }
    
    void ArenaTimer()
    {
        time += 0.02f;
        Debug.Log(Time.time);
    }

    void ArenaAddNewEnemy(int number)
    {
        foreach (EnemySpawner enemySpawner in enemySpawners)
        {
            enemySpawner.AddNewEnemy(number);
        }
    }




    // ������
    public void StartEvent(int number)
    {
        events[number].Invoke();
    }


    // 3-� ���
    public void LightsOff(bool status)
    {
        lightOff = status;
    }

    public void LightsDark(bool status)
    {
        lightDark = status;
    }  











    /*    // ������ ����� �������
        public void StartDialog(int number)
        {
            dialog.StartEvent(number);
        }*/





    public void CreateFloatingMessage(string message, Color color, Vector2 position)
    {
        int floatType = Random.Range(0, 3);
        GameObject textPrefab = Instantiate(GameAssets.instance.floatingMessage, position, Quaternion.identity);
        textPrefab.GetComponentInChildren<TextMeshPro>().text = message;
        textPrefab.GetComponentInChildren<TextMeshPro>().color = color;
        textPrefab.GetComponentInChildren<Animator>().SetFloat("FloatType", floatType);
        //textPrefab.transform.SetParent(player.transform);
    }


    // ����� ������ ������ (��� �������� � ���������)
    public int GetCurrentWeaponIndex()
    {
        int weaponAmmo = player.weaponHolder.currentWeapon.weaponIndexForAmmo;
        return (weaponAmmo);
    }

    // ����� ������ ����� (��� �������� � �������)
    public int GetCurrentBombIndex()
    {
        int bombAmmo = player.bombWeaponHolder.currentWeapon.weaponIndexForAmmo;
        return (bombAmmo);
    }


    // ����������� ������ (��� �������)
    public void MovePlayer(Vector2 targetPosition)
    {
        float distance = Vector2.Distance(player.transform.position, targetPosition);
        if (distance > 0.1f)
        {
            player.Move(targetPosition, true);            
        }
        else
        {
            player.Move(targetPosition, false);
            playerAtTarget = true;
        }
    }



    // ���������� ���� ����� � ������ �� ������������ (������ ������)
    public void EnemyResetAndNeutral(bool status)
    {
        if (status)
        {
            player.noAgro = true;
            Collider2D[] collidersHits = Physics2D.OverlapCircleAll(player.transform.position, 20);         // ������� ���� � ������� ������ � �������� (�������� ����� �������� ���� (������ �������� ����� ����))
            foreach (Collider2D coll in collidersHits)
            {
                if (coll == null)
                {
                    continue;
                }

                if (coll.gameObject.TryGetComponent<BotAI>(out BotAI botAI))
                {
                    botAI.ResetTarget();                                        // ���������� ����
                    botAI.noAgro = true;                                     // ������ �����������
                }
                collidersHits = null;
            }

        }
        if (!status)
        {
            player.noAgro = false;
            Collider2D[] collidersHits = Physics2D.OverlapCircleAll(player.transform.position, 25);         // ������� ���� � ������� ������ � �������� (�������� ����� �������� ���� (������ �������� ����� ����))
            foreach (Collider2D coll in collidersHits)
            {
                if (coll == null)
                {
                    continue;
                }

                if (coll.gameObject.TryGetComponent<BotAI>(out BotAI botAI))
                {
                    botAI.noAgro = false;                                     // ������ �����������
                }
                collidersHits = null;
            }
        }
    }


  













    public void NextScene(int sceneNumber)
    {
        //GameManager.instance.SaveState();
        //string sceneName = sceneNames[Random.Range(0, sceneNames.Length)];
        string sceneName = sceneNames[sceneNumber];     // �������� �����
        SceneManager.LoadScene(sceneName);              // ��������� �����
    }

    public void OnSceneLoaded(Scene s, LoadSceneMode mode)                      // ��������� ��� �������� �����
    {
        player.transform.position = GameObject.Find("SpawnPoint").transform.position;
        keys[0] = 0;
        keys[1] = 0;
    }
}
