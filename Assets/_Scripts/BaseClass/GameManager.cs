using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using YG;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;         // инстанс

    public bool testBuild;
    public bool demoLevel;                               
    public bool forAndroid;                     // для андроида  
    public bool forYG;                          // для яндекс игр    
    public bool withReklamaYG;                  // с рекламой для YG  
    public bool startScreen;                    // для стартскрина
    public bool showDamage;                     // показывать урон    
    public string[] sceneNames;                 // все сцены

    [Header("Ссылки")]
    public Player player;                       // ссылка на игрока    
    public AmmoPackKoala ammoManager;           // аммо менеджер   
    //public GameObject gui;                      // гуи
    //public Dialog dialog;                       // диалог менеджер    

    [Header("Управление игрой")]
    [HideInInspector] public bool isPlayerEnactive;     // активен игрок или нет
    [HideInInspector] public bool cameraOnPlayer;       // управление камерой
    [HideInInspector] public bool dialogeStart;         // диалог начался
    [HideInInspector] public bool helpOn;               // подсказка активна
    [HideInInspector] public bool playerInResroom;      // игрок в комнате воскрешения
    [HideInInspector] public bool playerAtTarget;       // игрок дошёл до места старта диалога
    [HideInInspector] public bool musicOff;             // музыка
    [HideInInspector] public bool screenShakeOff;       // тряска экрана
    [HideInInspector] public string currentSceneName;   // действуящая сцена
    [HideInInspector] public bool firstLevel;           // первый уровень


    [Header("Клавиша взаимодействия")]
    public KeyCode keyToUse;            // клавиша для действия

    [Header("Предметы")]
    public int gold;                    // золото
    public int[] keys;                  // ключи
    public int battery;                 // батареи
    public int pozorCount;              // счетчик позора

    [Header("События между сценами")]
    [HideInInspector] public bool resroomed;            // побывал в комнате воскрешения
    [HideInInspector] public bool pozored;              // опозорен
    [HideInInspector] public bool weaponHelped;         // показали подсказку для смены оружия
    [HideInInspector] public bool bombHelped;           // показали подсказку для бомбы
    [HideInInspector] public bool bombHelped_2;         // показали подсказку 2 для бомбы

    [Header("Управление светом")]
    public bool lightDark;
    public bool lightOff;

    [Header("Чат для игрока")]    
    int chatRandom;
    int prevNumber;

    // Для паузы
    bool paused;
    bool slowed;

    public bool infiniteAmmo;


    private void Awake()
    {        
        if (startScreen)
        {
            instance = null;
            Destroy(gameObject);
            return;
        }

        if (instance != null)
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
        SceneManager.sceneLoaded += OnSceneLoaded;      // при загрузке сцены выполнится эта += функция

        if (PlayerPrefs.GetInt("GameContinue") == 0)        // продолжить игру
        {
            firstLevel = true;
            //Debug.Log("FirstLevel!");
        }
    }


    // Подписываемся на событие GetDataEvent в OnEnable
    private void OnEnable()
    {
        //YandexGame.GetDataEvent += StartYG;
    }
    // Отписываемся от события GetDataEvent в OnDisable
    private void OnDisable()
    {
        //YandexGame.GetDataEvent -= StartYG;
    }

    private void Start()
    {
        //Debug.Log(PlayerPrefs.GetInt("GameContinue"));

        // Продолжаем игру или 1-й раз
        if (forYG)
        {
            //StartYG();
        }
        else
        {
            //Debug.Log("No for YG");

            if (firstLevel || demoLevel)
            {
                ammoManager.TakeMeleeWeapon(0);
                ammoManager.TakeRangeWeapon(0);
                Invoke(nameof(SwapWeaponPlayer), 0.1f);
                if (!demoLevel)
                    SaveData();               
                firstLevel = false;
            }
        }
    }

    public void StartYG()
    {
 /*       //Debug.Log("StartYG!");
        if (!YandexGame.SDKEnabled)
        {
            return;
        }

        if (!YandexGame.savesData.gameContinue)
        {
            firstLevel = true;
        }
        if (firstLevel)
        {
            ammoManager.TakeMeleeWeapon(0);
            ammoManager.TakeRangeWeapon(0);
            Invoke(nameof(SwapWeaponPlayer), 0.1f);
            Invoke(nameof(SaveDataYG), 0.1f);            
            firstLevel = false;
            //Debug.Log("Weapons!");
        }

        // Загрузка
        if (YandexGame.savesData.loadPlayerData)              // если надо загрузить пар-ры
        {
            LoadDataYG();
        }
        // Сохранение
        else
        {
            if (firstLevel)
                return;
            SaveDataYG();
        }*/
    }

    void SwapWeaponPlayer()
    {
        player.weaponHolder.SwapWeapon();
    }

    public void Update()
    {
        if (testBuild)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                ammoManager.TakeMeleeWeapon(1);
                ammoManager.TakeMeleeWeapon(2);
                ammoManager.TakeMeleeWeapon(3);
                ammoManager.TakeMeleeWeapon(4);

                ammoManager.TakeRangeWeapon(1);
                ammoManager.TakeRangeWeapon(2);
                ammoManager.TakeRangeWeapon(3);
                ammoManager.TakeRangeWeapon(4);
                ammoManager.TakeRangeWeapon(5);
                ammoManager.TakeRangeWeapon(6);
                ammoManager.TakeRangeWeapon(7);
                ammoManager.TakeRangeWeapon(8);
                ammoManager.TakeRangeWeapon(9);

                ammoManager.TakeBomb(1);
                ammoManager.TakeBomb(2);

                player.withShield = true;
                player.withGoldMagnet = true;
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                infiniteAmmo = !infiniteAmmo;
            }

            if (Input.GetKeyDown(KeyCode.N))
            {
                NextScene(2);
            }
        }



        /*        if (Input.GetKeyDown(KeyCode.I))
                {
                    for (int i = 0; i < player.rangeWeaponsIndex.Count; i++)                      // сохраняем ренж оружия
                    {
                        PlayerPrefs.SetInt("PlayerRangeWeapon" + i, player.rangeWeaponsIndex[i]);   // сохраняем индекс для каждого оружия
                        Debug.Log(i);
                    }           

                    Debug.Log(PlayerPrefs.GetInt("PlayerRangeWeapon" + 0));
                    Debug.Log(PlayerPrefs.GetInt("PlayerRangeWeapon" + 1));
                    Debug.Log(PlayerPrefs.GetInt("PlayerRangeWeapon" + 2));
                    //Debug.Log(PlayerPrefs.GetInt("PlayerRangeWeapon" + 3));
                }*/



        if (Input.GetKeyDown(KeyCode.Escape) && !dialogeStart && !helpOn)
        {
            if (!paused)
            {
                StopGame();
                //Debug.Log("Pause!");
            }
            else
            {
                ContinueGame();
            }
        }

        // Замедление времени
/*        if (Input.GetKeyDown(KeyCode.T))
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
        } */       


        // Чат игрока
        if (Input.GetKeyDown(KeyCode.C))
        {
            // Делаем чат
            ChatBubble.Clear(player.gameObject);

            if (LanguageManager.instance.eng)
            {
                while (prevNumber == chatRandom)
                {
                    chatRandom = Random.Range(0, player.chatTextsEng.Length);
                }
                prevNumber = chatRandom;
                ChatBubble.Create(player.transform, new Vector3(0.2f, 0.2f), player.chatTextsEng[chatRandom], 2f);
            }
            else
            {
                while (prevNumber == chatRandom)
                {
                    chatRandom = Random.Range(0, player.chatTexts.Length);
                }
                prevNumber = chatRandom;
                ChatBubble.Create(player.transform, new Vector3(0.2f, 0.2f), player.chatTexts[chatRandom], 2f);
            }

            
            // Триггер врагов
            Collider2D[] collidersHits = Physics2D.OverlapCircleAll(player.transform.position, 20);         // создаем круг в позиции игрока с радиусом (возможно стоит добавить слой (сейчас задевает ботов тоже))
            foreach (Collider2D coll in collidersHits)
            {
                if (coll == null)
                {
                    continue;
                }

                if (coll.gameObject.TryGetComponent<BotAI>(out BotAI botAI))
                {
                    botAI.TriggerEnemy();
                }
                collidersHits = null;
            }
        }

   

            


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

    public void StopGame()
    {
        Time.timeScale = 0f;
        isPlayerEnactive = true;
        TextUI.instance.ShowMenu(true);
        TextUI.instance.CursorVisibleOnOff(true);
        paused = true;
    }

    public void ContinueGame()
    {
        Time.timeScale = 1f;
        isPlayerEnactive = false;
        TextUI.instance.ShowMenu(false);
        TextUI.instance.CursorVisibleOnOff(false);
        paused = false;
    }





    // Ивенты

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
        int floatType = 0;
        if (color == Color.green || color == Color.yellow)                       // если это хп или золото, делаем рандомный поворот (потом сделать по нормальному)
            floatType = Random.Range(0, 3);
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
            Collider2D[] collidersHits = Physics2D.OverlapCircleAll(player.transform.position, 30);         // создаем круг в позиции игрока с радиусом (возможно стоит добавить слой (сейчас задевает ботов тоже))
            foreach (Collider2D coll in collidersHits)
            {
                if (coll == null)
                {
                    continue;
                }

                if (coll.gameObject.TryGetComponent<BotAI>(out BotAI botAI))
                {
                    botAI.ResetTarget();                                        // сбрасываем цель
                    botAI.noAgro = true;                                        // делаем нейтральным
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
                    botAI.noAgro = false;                                     // сбрасывааем нейтральность
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
        if (sceneNumber == 0)                           // если сцена 0 (главное меню) - уничтожаем 
        {
            Destroy(gameObject, 0.1f);
            Destroy(ammoManager.gameObject);
            Destroy(player.gameObject);
        }
/*        if (sceneNumber > 1)                            // если не стартовая сцена и не первая - подсказки показаны
        {
            weaponHelped = true;
            bombHelped = true;
        }*/

        if (paused)
            ContinueGame();

        SceneManager.LoadScene(sceneName);              // загружаем сцену
    }

    public void OnSceneLoaded(Scene s, LoadSceneMode mode)                  // выполняем при загрузке сцены
    {
        GameObject spawnPoint = GameObject.Find("SpawnPoint");
        if (spawnPoint && player)
        {
            player.transform.position = spawnPoint.transform.position;      // перемещаем игрока на точку спауна
        }
        
        player.isImmortal = false;              // убираем бессмертие
        playerInResroom = false;                // вышли из ресрума (из-за возможного бага)
        keys[0] = 0;                            // сбрасываем ключи
        keys[1] = 0;

        if (ArenaManager.instance)
        {
            if (!ArenaManager.instance.noStartWhiteScreen)
                ArenaManager.instance.whiteScreenAnimator.SetTrigger("StartScreen");
            else
                ArenaManager.instance.whiteScreenAnimator.SetTrigger("StartScreenNormal");
        }        

        if (forYG)
        {
            StartYG();
        }
        else
        {
            // Загрузка
            if (PlayerPrefs.GetInt("LoadPlayerData") == 1)              // если надо загрузить пар-ры
            {
                LoadData();
            }
            // Сохранение
            else
            {
                if (firstLevel)
                    return;
                SaveData();
            }
        }
    }

    void LoadData()
    {        
        player.currentHealth = PlayerPrefs.GetInt("PlayerCurrentHp");       // хп
        gold = PlayerPrefs.GetInt("PlayerGold");                            // золото
        pozorCount = PlayerPrefs.GetInt("PlayerPozorCount");                // метки позора

        // Ренж оружие
        for (int i = 0; i < PlayerPrefs.GetInt("PlayerRangeWeaponCount"); i++)              // для кол-ва ренж оружия
        {
            ammoManager.TakeRangeWeapon(PlayerPrefs.GetInt("PlayerRangeWeapon" + i));       // даём оружие по индексу
            ammoManager.ammoWeapons[PlayerPrefs.GetInt("PlayerRangeWeapon" + i)].allAmmo = PlayerPrefs.GetInt("PlayerRangeWeaponAmmo" + i);
        }

        // Мили оружие
        for (int i = 0; i < PlayerPrefs.GetInt("PlayerMeleeWeaponCount"); i++)              // для кол-ва ренж оружия
        {
            ammoManager.TakeMeleeWeapon(PlayerPrefs.GetInt("PlayerMeleeWeapon" + i));       // даём оружие по индексу                
        }

        // Бомбы
        for (int i = 0; i < PlayerPrefs.GetInt("PlayerBombCount"); i++)              // для кол-ва ренж оружия
        {            
            ammoManager.TakeBomb(PlayerPrefs.GetInt("PlayerBomb" + i));       // даём оружие по индексу
            ammoManager.ammoBombs[PlayerPrefs.GetInt("PlayerBomb" + i)].allAmmo = PlayerPrefs.GetInt("PlayerBombAmmo" + i);
        }

        // Снаряжение
        if (PlayerPrefs.GetInt("PlayerShield") == 1)
            player.withShield = true;                           // щит

        if (PlayerPrefs.GetInt("PlayerMagnet") == 1)
            player.withGoldMagnet = true;                       // магнит для монеток

        PlayerPrefs.SetInt("LoadPlayerData", 0);                            // для отключения загрузки пар-ов игрока     
    }

    void LoadDataYG()
    {
/*        player.currentHealth = YandexGame.savesData.playerCurrentHp;        // хп
        gold = YandexGame.savesData.playerGold;                             // золото
        pozorCount = YandexGame.savesData.playerPozorCount;                 // метки позора

        // Ренж оружие
        for (int i = 0; i < YandexGame.savesData.rangeWeaponsCount; i++)            // для кол-ва ренж оружия
        {
            ammoManager.TakeRangeWeapon(YandexGame.savesData.rangeWeapons[i]);      // даём оружие по индексу
            ammoManager.ammoWeapons[YandexGame.savesData.rangeWeapons[i]].allAmmo = YandexGame.savesData.rangeWeaponsAmmo[i];
        }

        *//*      

                // Мили оружие
                for (int i = 0; i < PlayerPrefs.GetInt("PlayerMeleeWeaponCount"); i++)              // для кол-ва ренж оружия
                {
                    ammoManager.TakeMeleeWeapon(PlayerPrefs.GetInt("PlayerMeleeWeapon" + i));       // даём оружие по индексу                
                }

                // Бомбы
                for (int i = 0; i < PlayerPrefs.GetInt("PlayerBombCount"); i++)              // для кол-ва ренж оружия
                {
                    ammoManager.TakeBomb(PlayerPrefs.GetInt("PlayerBomb" + i));       // даём оружие по индексу
                    ammoManager.ammoBombs[PlayerPrefs.GetInt("PlayerBomb" + i)].allAmmo = PlayerPrefs.GetInt("PlayerBombAmmo" + i);
                }

                // Снаряжение
                if (PlayerPrefs.GetInt("PlayerShield") == 1)
                    player.withShield = true;                           // щит

                if (PlayerPrefs.GetInt("PlayerMagnet") == 1)
                    player.withGoldMagnet = true;                       // магнит для монеток

                   *//*

        YandexGame.savesData.loadPlayerData = false;*/
    }

    void SaveData()
    {
        currentSceneName = SceneManager.GetActiveScene().name;          // находим название текущей сцены
        PlayerPrefs.SetString("SceneName", currentSceneName);           // сохраняем его
        PlayerPrefs.SetInt("PlayerCurrentHp", player.currentHealth);    // сохраняем хп игрока
        PlayerPrefs.SetInt("PlayerGold", gold);                         // сохраняем золото
        PlayerPrefs.SetInt("PlayerPozorCount", pozorCount);             // сохраняем метки позора

        // Ренж оружие
        PlayerPrefs.SetInt("PlayerRangeWeaponCount", player.rangeWeaponsIndex.Count);   // всего ренж оружия
        for (int i = 0; i < player.rangeWeaponsIndex.Count; i++)                      // сохраняем ренж оружия
        {
            PlayerPrefs.SetInt("PlayerRangeWeapon" + i, player.rangeWeaponsIndex[i]);   // сохраняем индекс для каждого оружия
            PlayerPrefs.SetInt("PlayerRangeWeaponAmmo" + i, ammoManager.ammoWeapons[player.rangeWeaponsIndex[i]].allAmmo);   // сохраняем индекс для каждого оружия
        }

        // Мили оружие
        PlayerPrefs.SetInt("PlayerMeleeWeaponCount", player.meleeWeaponsIndex.Count);   // всего мили оружия
        for (int i = 0; i < player.meleeWeaponsIndex.Count; i++)                        // сохраняем мили оружия
        {
            PlayerPrefs.SetInt("PlayerMeleeWeapon" + i, player.meleeWeaponsIndex[i]);   // сохраняем индекс для каждого оружия                
        }

        // Бомбы
        PlayerPrefs.SetInt("PlayerBombCount", player.bombsIndex.Count);             // всего видов бомб
        for (int i = 0; i < player.bombsIndex.Count; i++)                           // сохраняем бомбы
        {            
            PlayerPrefs.SetInt("PlayerBomb" + i, player.bombsIndex[i]);   // сохраняем индекс для каждого оружия
            PlayerPrefs.SetInt("PlayerBombAmmo" + i, ammoManager.ammoBombs[player.bombsIndex[i]].allAmmo);   // сохраняем индекс для каждого оружия
        }

        // Снаряжение
        if (player.withShield)
            PlayerPrefs.SetInt("PlayerShield", 1);          // щит

        if (player.withGoldMagnet)
            PlayerPrefs.SetInt("PlayerMagnet", 1);          // магнит для монеток

        PlayerPrefs.SetInt("GameContinue", 1);              // сделали сохранение

        TextUI.instance.Saving();                           // полоска сохранения
    }

    void SaveDataYG()
    {
/*        currentSceneName = SceneManager.GetActiveScene().name;          // находим название текущей сцены
        YandexGame.savesData.sceneNameToLoad = currentSceneName;

        YandexGame.savesData.playerCurrentHp = player.currentHealth;    // сохраняем хп игрока
        YandexGame.savesData.playerGold = gold;                         // сохраняем золото
        YandexGame.savesData.playerPozorCount = pozorCount;             // сохраняем метки позора

        // Ренж оружие
        YandexGame.savesData.rangeWeaponsCount = player.rangeWeaponsIndex.Count;        // всего ренж оружия
        for (int i = 0; i < player.rangeWeaponsIndex.Count; i++)                        // сохраняем ренж оружия
        {
            YandexGame.savesData.rangeWeapons.Add(player.rangeWeaponsIndex[i]);         // сохраняем индекс для каждого оружия
            YandexGame.savesData.rangeWeaponsAmmo.Add(ammoManager.ammoWeapons[player.rangeWeaponsIndex[i]].allAmmo);   // сохраняем индекс для каждого оружия
        }
        Debug.Log(YandexGame.savesData.rangeWeapons[1]);

        *//*      



                // Мили оружие
                PlayerPrefs.SetInt("PlayerMeleeWeaponCount", player.meleeWeaponsIndex.Count);   // всего мили оружия
                for (int i = 0; i < player.meleeWeaponsIndex.Count; i++)                        // сохраняем мили оружия
                {
                    PlayerPrefs.SetInt("PlayerMeleeWeapon" + i, player.meleeWeaponsIndex[i]);   // сохраняем индекс для каждого оружия                
                }

                // Бомбы
                PlayerPrefs.SetInt("PlayerBombCount", player.bombsIndex.Count);             // всего видов бомб
                for (int i = 0; i < player.bombsIndex.Count; i++)                           // сохраняем бомбы
                {
                    PlayerPrefs.SetInt("PlayerBomb" + i, player.bombsIndex[i]);   // сохраняем индекс для каждого оружия
                    PlayerPrefs.SetInt("PlayerBombAmmo" + i, ammoManager.ammoBombs[player.bombsIndex[i]].allAmmo);   // сохраняем индекс для каждого оружия
                }

                // Снаряжение
                if (player.withShield)
                    PlayerPrefs.SetInt("PlayerShield", 1);          // щит

                if (player.withGoldMagnet)
                    PlayerPrefs.SetInt("PlayerMagnet", 1);          // магнит для монеток                

                TextUI.instance.Saving();                           // полоска сохранения*//*

        YandexGame.savesData.gameContinue = true;

        YandexGame.SaveProgress();*/
    }


    void ClearPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}
