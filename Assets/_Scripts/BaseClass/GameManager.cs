using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;         // инстанс
    public bool startScreen;                    // для стартскрина
    public bool showDamage;                     // показывать урон
    
    public string[] sceneNames;                 // все сцены

    [Header("Ссылки")]
    public Player player;                       // ссылка на игрока    
    public AmmoPackKoala ammoManager;           // аммо менеджер   
    //public GameObject gui;                      // гуи
    //public Dialog dialog;                       // диалог менеджер    

    [Header("Управление игрой")]    
    [HideInInspector] public bool isPlayerEnactive;             // активен игрок или нет
    [HideInInspector] public bool cameraOnPlayer;               // управление камерой
    [HideInInspector] public bool dialogeStart;                 // диалог начался
    [HideInInspector] public bool helpOn;                       // подсказка активна
    [HideInInspector] public bool playerInResroom;              // игрок в комнате воскрешения
    [HideInInspector] public bool playerAtTarget;               // игрок дошёл до места старта диалога
    [HideInInspector] public bool musicOff;                     // музыка

    [Header("Клавиша взаимодействия")]
    public KeyCode keyToUse;                    // клавиша для действия

    [Header("Предметы")]
    public int gold;                            // золото
    public int[] keys;                          // ключи
    public int battery;                         // батареи
    public int pozorCount;                      // счетчик позора

    [Header("События между сценами")]
    [HideInInspector] public bool resroomed;    // побывал в комнате воскрешения
    [HideInInspector] public bool pozored;      // опозорен
    [HideInInspector] public bool weaponHelped; // показали подсказку для смены оружия
    [HideInInspector] public bool bombHelped;   // показали подсказку для бомбы
    [HideInInspector] public bool bombHelped_2; // показали подсказку 2 для бомбы

    [Header("Управление светом")]
    public bool lightDark;
    public bool lightOff;

    [Header("Чат для игрока")]    
    int chatRandom;
    int prevNumber;

    // Для паузы
    bool paused;
    bool slowed;

    // Арена уровень
    //[HideInInspector] public bool arenaLvl;    


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
        SceneManager.sceneLoaded += OnSceneLoaded;      // при загрузке сцены выполнится это += функция
    }

    private void Start()
    {
        //TextUI.instance.CursorVisibleOnOff(false);
    }

    public void Update()
    {
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


        // Чат игрока
        if (Input.GetKeyDown(KeyCode.C))
        {
            // Делаем чат
            ChatBubble.Clear(player.gameObject);            
            while (prevNumber == chatRandom)
            {
                chatRandom = Random.Range(0, player.chatTexts.Length);
            }
            prevNumber = chatRandom;            
            ChatBubble.Create(player.transform, new Vector3(0.2f, 0.2f), player.chatTexts[chatRandom], 2f);
            
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

        if (Input.GetKeyDown(KeyCode.N))
        {
            //NextScene(4);
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
        if (sceneNumber == 0)                           // если сцена 0 (главное меню) - уничтожаем 
        {
            Destroy(gameObject);
            Destroy(ammoManager.gameObject);
            Destroy(player.gameObject);
        }

        if (paused)
            ContinueGame();

        SceneManager.LoadScene(sceneName);              // загружаем сцену

        /*if (sceneNumber == 4)
        {
            arenaLvl = true;
        }*/
    }

    public void OnSceneLoaded(Scene s, LoadSceneMode mode)                              // выполняем при загрузке сцены
    {
        GameObject spawnPoint = GameObject.Find("SpawnPoint");
        if (spawnPoint && player)
        {
            player.transform.position = spawnPoint.transform.position;                  // перемещаем игрока на точку спауна
        }
        
        player.isImmortal = false;                                                      // убираем бессмертие
        keys[0] = 0;                                                                    // сбрасываем ключи
        keys[1] = 0;

        if (ArenaManager.instance)
        {
            if (!ArenaManager.instance.noStartWhiteScreen)
                ArenaManager.instance.whiteScreenAnimator.SetTrigger("StartScreen");
            else
                ArenaManager.instance.whiteScreenAnimator.SetTrigger("StartScreenNormal");
        }
    }
}
