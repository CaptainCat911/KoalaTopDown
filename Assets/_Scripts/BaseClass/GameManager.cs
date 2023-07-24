using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;         // инстанс

    public UnityEvent[] events;                 // ивенты
    public string[] sceneNames;                 // все сцены

    [Header("Ссылки")]
    public Player player;                       // ссылка на игрока    
    public AmmoPackKoala ammoManager;           // аммо менеджер   
    //public GameObject gui;                      // гуи
    //public Dialog dialog;                       // диалог менеджер    

    [Header("Управление игрой")]
    public bool isPlayerEnactive;               // активен игрок или нет
    public bool cameraOnPlayer;                 // управление камерой
    public bool dialogeStart;                   // диалог начался
    public bool playerInResroom;                // игрок в комнате воскрешения
    [HideInInspector] public bool playerAtTarget;   // игрок дошёл до места старта диалога

    [Header("Клавиша взаимодействия")]
    public KeyCode keyToUse;                    // клавиша для действия

    [Header("Предметы")]
    public int gold;                            // золото
    public int[] keys;                          // ключи
    public int battery;                         // батареи     

    [Header("Управление светом")]
    public bool lightDark;
    public bool lightOff;

    // Для паузы
    bool paused;
    bool slowed;

    [Header("Настройки арены")]
    public EnemySpawner[] enemySpawners;
    public bool arenaStart;
    public int arenaMaxEnemys;
    public bool arenaSpawnStarted;
    public int arenaEnemyCount;
    // Таймер
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
        
        instance = this;                                // присваем instance (?) этому обьекту и по ивенту загрузки запускаем функцию загрузки
        SceneManager.sceneLoaded += OnSceneLoaded;      // при загрузке сцены выполнится это += функция
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
            ChatBubble.Create(player.transform, new Vector3(0.2f, 0.2f), "Ну все, вам жопа!", 2f);
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            NextScene(3);
        }

        ArenaUpdate();              // апдейт арены, остановка спауна если слишком много врагов       


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

        // Установка сложности по таймеру
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




    // Ивенты
    public void StartEvent(int number)
    {
        events[number].Invoke();
    }


    // 3-й лвл
    public void LightsOff(bool status)
    {
        lightOff = status;
    }

    public void LightsDark(bool status)
    {
        lightDark = status;
    }  











    /*    // Начать ивент диалога
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


    // Найти индекс оружия (для автомата с патронами)
    public int GetCurrentWeaponIndex()
    {
        int weaponAmmo = player.weaponHolder.currentWeapon.weaponIndexForAmmo;
        return (weaponAmmo);
    }

    // Найти индекс бомбы (для автомата с бомбами)
    public int GetCurrentBombIndex()
    {
        int bombAmmo = player.bombWeaponHolder.currentWeapon.weaponIndexForAmmo;
        return (bombAmmo);
    }


    // Перемещение игрока (для диалога)
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



    // Сбрасывать цель ботам и делать их нейтральными (вокруг игрока)
    public void EnemyResetAndNeutral(bool status)
    {
        if (status)
        {
            player.noAgro = true;
            Collider2D[] collidersHits = Physics2D.OverlapCircleAll(player.transform.position, 20);         // создаем круг в позиции игрока с радиусом (возможно стоит добавить слой (сейчас задевает ботов тоже))
            foreach (Collider2D coll in collidersHits)
            {
                if (coll == null)
                {
                    continue;
                }

                if (coll.gameObject.TryGetComponent<BotAI>(out BotAI botAI))
                {
                    botAI.ResetTarget();                                        // сбрасываем цель
                    botAI.noAgro = true;                                     // делаем нейтральным
                }
                collidersHits = null;
            }

        }
        if (!status)
        {
            player.noAgro = false;
            Collider2D[] collidersHits = Physics2D.OverlapCircleAll(player.transform.position, 25);         // создаем круг в позиции игрока с радиусом (возможно стоит добавить слой (сейчас задевает ботов тоже))
            foreach (Collider2D coll in collidersHits)
            {
                if (coll == null)
                {
                    continue;
                }

                if (coll.gameObject.TryGetComponent<BotAI>(out BotAI botAI))
                {
                    botAI.noAgro = false;                                     // делаем нейтральным
                }
                collidersHits = null;
            }
        }
    }


  













    public void NextScene(int sceneNumber)
    {
        //GameManager.instance.SaveState();
        //string sceneName = sceneNames[Random.Range(0, sceneNames.Length)];
        string sceneName = sceneNames[sceneNumber];     // выбираем сцену
        SceneManager.LoadScene(sceneName);              // загружаем сцену
    }

    public void OnSceneLoaded(Scene s, LoadSceneMode mode)                      // выполняем при загрузке сцены
    {
        player.transform.position = GameObject.Find("SpawnPoint").transform.position;
        keys[0] = 0;
        keys[1] = 0;
    }
}
