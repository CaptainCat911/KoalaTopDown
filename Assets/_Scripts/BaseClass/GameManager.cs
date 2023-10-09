using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using YG;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;         // �������

    public bool testBuild;
    public bool demoLevel;                               
    public bool forAndroid;                     // ��� ��������  
    public bool forYG;                          // ��� ������ ���    
    public bool withReklamaYG;                  // � �������� ��� YG  
    public bool startScreen;                    // ��� �����������
    public bool showDamage;                     // ���������� ����    
    public string[] sceneNames;                 // ��� �����

    [Header("������")]
    public Player player;                       // ������ �� ������    
    public AmmoPackKoala ammoManager;           // ���� ��������   
    //public GameObject gui;                      // ���
    //public Dialog dialog;                       // ������ ��������    

    [Header("���������� �����")]
    [HideInInspector] public bool isPlayerEnactive;     // ������� ����� ��� ���
    [HideInInspector] public bool cameraOnPlayer;       // ���������� �������
    [HideInInspector] public bool dialogeStart;         // ������ �������
    [HideInInspector] public bool helpOn;               // ��������� �������
    [HideInInspector] public bool playerInResroom;      // ����� � ������� �����������
    [HideInInspector] public bool playerAtTarget;       // ����� ����� �� ����� ������ �������
    [HideInInspector] public bool musicOff;             // ������
    [HideInInspector] public bool screenShakeOff;       // ������ ������
    [HideInInspector] public string currentSceneName;   // ����������� �����
    [HideInInspector] public bool firstLevel;           // ������ �������


    [Header("������� ��������������")]
    public KeyCode keyToUse;            // ������� ��� ��������

    [Header("��������")]
    public int gold;                    // ������
    public int[] keys;                  // �����
    public int battery;                 // �������
    public int pozorCount;              // ������� ������

    [Header("������� ����� �������")]
    [HideInInspector] public bool resroomed;            // ������� � ������� �����������
    [HideInInspector] public bool pozored;              // ��������
    [HideInInspector] public bool weaponHelped;         // �������� ��������� ��� ����� ������
    [HideInInspector] public bool bombHelped;           // �������� ��������� ��� �����
    [HideInInspector] public bool bombHelped_2;         // �������� ��������� 2 ��� �����

    [Header("���������� ������")]
    public bool lightDark;
    public bool lightOff;

    [Header("��� ��� ������")]    
    int chatRandom;
    int prevNumber;

    // ��� �����
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
        
        instance = this;                                // �������� instance (?) ����� ������� � �� ������ �������� ��������� ������� ��������
        SceneManager.sceneLoaded += OnSceneLoaded;      // ��� �������� ����� ���������� ��� += �������

        if (PlayerPrefs.GetInt("GameContinue") == 0)        // ���������� ����
        {
            firstLevel = true;
            //Debug.Log("FirstLevel!");
        }
    }


    // ������������� �� ������� GetDataEvent � OnEnable
    private void OnEnable()
    {
        //YandexGame.GetDataEvent += StartYG;
    }
    // ������������ �� ������� GetDataEvent � OnDisable
    private void OnDisable()
    {
        //YandexGame.GetDataEvent -= StartYG;
    }

    private void Start()
    {
        //Debug.Log(PlayerPrefs.GetInt("GameContinue"));

        // ���������� ���� ��� 1-� ���
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

        // ��������
        if (YandexGame.savesData.loadPlayerData)              // ���� ���� ��������� ���-��
        {
            LoadDataYG();
        }
        // ����������
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
                    for (int i = 0; i < player.rangeWeaponsIndex.Count; i++)                      // ��������� ���� ������
                    {
                        PlayerPrefs.SetInt("PlayerRangeWeapon" + i, player.rangeWeaponsIndex[i]);   // ��������� ������ ��� ������� ������
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

        // ���������� �������
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


        // ��� ������
        if (Input.GetKeyDown(KeyCode.C))
        {
            // ������ ���
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

            
            // ������� ������
            Collider2D[] collidersHits = Physics2D.OverlapCircleAll(player.transform.position, 20);         // ������� ���� � ������� ������ � �������� (�������� ����� �������� ���� (������ �������� ����� ����))
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





    // ������

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
        int floatType = 0;
        if (color == Color.green || color == Color.yellow)                       // ���� ��� �� ��� ������, ������ ��������� ������� (����� ������� �� �����������)
            floatType = Random.Range(0, 3);
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
            Collider2D[] collidersHits = Physics2D.OverlapCircleAll(player.transform.position, 30);         // ������� ���� � ������� ������ � �������� (�������� ����� �������� ���� (������ �������� ����� ����))
            foreach (Collider2D coll in collidersHits)
            {
                if (coll == null)
                {
                    continue;
                }

                if (coll.gameObject.TryGetComponent<BotAI>(out BotAI botAI))
                {
                    botAI.ResetTarget();                                        // ���������� ����
                    botAI.noAgro = true;                                        // ������ �����������
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
                    botAI.noAgro = false;                                     // ����������� �������������
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
        if (sceneNumber == 0)                           // ���� ����� 0 (������� ����) - ���������� 
        {
            Destroy(gameObject, 0.1f);
            Destroy(ammoManager.gameObject);
            Destroy(player.gameObject);
        }
/*        if (sceneNumber > 1)                            // ���� �� ��������� ����� � �� ������ - ��������� ��������
        {
            weaponHelped = true;
            bombHelped = true;
        }*/

        if (paused)
            ContinueGame();

        SceneManager.LoadScene(sceneName);              // ��������� �����
    }

    public void OnSceneLoaded(Scene s, LoadSceneMode mode)                  // ��������� ��� �������� �����
    {
        GameObject spawnPoint = GameObject.Find("SpawnPoint");
        if (spawnPoint && player)
        {
            player.transform.position = spawnPoint.transform.position;      // ���������� ������ �� ����� ������
        }
        
        player.isImmortal = false;              // ������� ����������
        playerInResroom = false;                // ����� �� ������� (��-�� ���������� ����)
        keys[0] = 0;                            // ���������� �����
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
            // ��������
            if (PlayerPrefs.GetInt("LoadPlayerData") == 1)              // ���� ���� ��������� ���-��
            {
                LoadData();
            }
            // ����������
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
        player.currentHealth = PlayerPrefs.GetInt("PlayerCurrentHp");       // ��
        gold = PlayerPrefs.GetInt("PlayerGold");                            // ������
        pozorCount = PlayerPrefs.GetInt("PlayerPozorCount");                // ����� ������

        // ���� ������
        for (int i = 0; i < PlayerPrefs.GetInt("PlayerRangeWeaponCount"); i++)              // ��� ���-�� ���� ������
        {
            ammoManager.TakeRangeWeapon(PlayerPrefs.GetInt("PlayerRangeWeapon" + i));       // ��� ������ �� �������
            ammoManager.ammoWeapons[PlayerPrefs.GetInt("PlayerRangeWeapon" + i)].allAmmo = PlayerPrefs.GetInt("PlayerRangeWeaponAmmo" + i);
        }

        // ���� ������
        for (int i = 0; i < PlayerPrefs.GetInt("PlayerMeleeWeaponCount"); i++)              // ��� ���-�� ���� ������
        {
            ammoManager.TakeMeleeWeapon(PlayerPrefs.GetInt("PlayerMeleeWeapon" + i));       // ��� ������ �� �������                
        }

        // �����
        for (int i = 0; i < PlayerPrefs.GetInt("PlayerBombCount"); i++)              // ��� ���-�� ���� ������
        {            
            ammoManager.TakeBomb(PlayerPrefs.GetInt("PlayerBomb" + i));       // ��� ������ �� �������
            ammoManager.ammoBombs[PlayerPrefs.GetInt("PlayerBomb" + i)].allAmmo = PlayerPrefs.GetInt("PlayerBombAmmo" + i);
        }

        // ����������
        if (PlayerPrefs.GetInt("PlayerShield") == 1)
            player.withShield = true;                           // ���

        if (PlayerPrefs.GetInt("PlayerMagnet") == 1)
            player.withGoldMagnet = true;                       // ������ ��� �������

        PlayerPrefs.SetInt("LoadPlayerData", 0);                            // ��� ���������� �������� ���-�� ������     
    }

    void LoadDataYG()
    {
/*        player.currentHealth = YandexGame.savesData.playerCurrentHp;        // ��
        gold = YandexGame.savesData.playerGold;                             // ������
        pozorCount = YandexGame.savesData.playerPozorCount;                 // ����� ������

        // ���� ������
        for (int i = 0; i < YandexGame.savesData.rangeWeaponsCount; i++)            // ��� ���-�� ���� ������
        {
            ammoManager.TakeRangeWeapon(YandexGame.savesData.rangeWeapons[i]);      // ��� ������ �� �������
            ammoManager.ammoWeapons[YandexGame.savesData.rangeWeapons[i]].allAmmo = YandexGame.savesData.rangeWeaponsAmmo[i];
        }

        *//*      

                // ���� ������
                for (int i = 0; i < PlayerPrefs.GetInt("PlayerMeleeWeaponCount"); i++)              // ��� ���-�� ���� ������
                {
                    ammoManager.TakeMeleeWeapon(PlayerPrefs.GetInt("PlayerMeleeWeapon" + i));       // ��� ������ �� �������                
                }

                // �����
                for (int i = 0; i < PlayerPrefs.GetInt("PlayerBombCount"); i++)              // ��� ���-�� ���� ������
                {
                    ammoManager.TakeBomb(PlayerPrefs.GetInt("PlayerBomb" + i));       // ��� ������ �� �������
                    ammoManager.ammoBombs[PlayerPrefs.GetInt("PlayerBomb" + i)].allAmmo = PlayerPrefs.GetInt("PlayerBombAmmo" + i);
                }

                // ����������
                if (PlayerPrefs.GetInt("PlayerShield") == 1)
                    player.withShield = true;                           // ���

                if (PlayerPrefs.GetInt("PlayerMagnet") == 1)
                    player.withGoldMagnet = true;                       // ������ ��� �������

                   *//*

        YandexGame.savesData.loadPlayerData = false;*/
    }

    void SaveData()
    {
        currentSceneName = SceneManager.GetActiveScene().name;          // ������� �������� ������� �����
        PlayerPrefs.SetString("SceneName", currentSceneName);           // ��������� ���
        PlayerPrefs.SetInt("PlayerCurrentHp", player.currentHealth);    // ��������� �� ������
        PlayerPrefs.SetInt("PlayerGold", gold);                         // ��������� ������
        PlayerPrefs.SetInt("PlayerPozorCount", pozorCount);             // ��������� ����� ������

        // ���� ������
        PlayerPrefs.SetInt("PlayerRangeWeaponCount", player.rangeWeaponsIndex.Count);   // ����� ���� ������
        for (int i = 0; i < player.rangeWeaponsIndex.Count; i++)                      // ��������� ���� ������
        {
            PlayerPrefs.SetInt("PlayerRangeWeapon" + i, player.rangeWeaponsIndex[i]);   // ��������� ������ ��� ������� ������
            PlayerPrefs.SetInt("PlayerRangeWeaponAmmo" + i, ammoManager.ammoWeapons[player.rangeWeaponsIndex[i]].allAmmo);   // ��������� ������ ��� ������� ������
        }

        // ���� ������
        PlayerPrefs.SetInt("PlayerMeleeWeaponCount", player.meleeWeaponsIndex.Count);   // ����� ���� ������
        for (int i = 0; i < player.meleeWeaponsIndex.Count; i++)                        // ��������� ���� ������
        {
            PlayerPrefs.SetInt("PlayerMeleeWeapon" + i, player.meleeWeaponsIndex[i]);   // ��������� ������ ��� ������� ������                
        }

        // �����
        PlayerPrefs.SetInt("PlayerBombCount", player.bombsIndex.Count);             // ����� ����� ����
        for (int i = 0; i < player.bombsIndex.Count; i++)                           // ��������� �����
        {            
            PlayerPrefs.SetInt("PlayerBomb" + i, player.bombsIndex[i]);   // ��������� ������ ��� ������� ������
            PlayerPrefs.SetInt("PlayerBombAmmo" + i, ammoManager.ammoBombs[player.bombsIndex[i]].allAmmo);   // ��������� ������ ��� ������� ������
        }

        // ����������
        if (player.withShield)
            PlayerPrefs.SetInt("PlayerShield", 1);          // ���

        if (player.withGoldMagnet)
            PlayerPrefs.SetInt("PlayerMagnet", 1);          // ������ ��� �������

        PlayerPrefs.SetInt("GameContinue", 1);              // ������� ����������

        TextUI.instance.Saving();                           // ������� ����������
    }

    void SaveDataYG()
    {
/*        currentSceneName = SceneManager.GetActiveScene().name;          // ������� �������� ������� �����
        YandexGame.savesData.sceneNameToLoad = currentSceneName;

        YandexGame.savesData.playerCurrentHp = player.currentHealth;    // ��������� �� ������
        YandexGame.savesData.playerGold = gold;                         // ��������� ������
        YandexGame.savesData.playerPozorCount = pozorCount;             // ��������� ����� ������

        // ���� ������
        YandexGame.savesData.rangeWeaponsCount = player.rangeWeaponsIndex.Count;        // ����� ���� ������
        for (int i = 0; i < player.rangeWeaponsIndex.Count; i++)                        // ��������� ���� ������
        {
            YandexGame.savesData.rangeWeapons.Add(player.rangeWeaponsIndex[i]);         // ��������� ������ ��� ������� ������
            YandexGame.savesData.rangeWeaponsAmmo.Add(ammoManager.ammoWeapons[player.rangeWeaponsIndex[i]].allAmmo);   // ��������� ������ ��� ������� ������
        }
        Debug.Log(YandexGame.savesData.rangeWeapons[1]);

        *//*      



                // ���� ������
                PlayerPrefs.SetInt("PlayerMeleeWeaponCount", player.meleeWeaponsIndex.Count);   // ����� ���� ������
                for (int i = 0; i < player.meleeWeaponsIndex.Count; i++)                        // ��������� ���� ������
                {
                    PlayerPrefs.SetInt("PlayerMeleeWeapon" + i, player.meleeWeaponsIndex[i]);   // ��������� ������ ��� ������� ������                
                }

                // �����
                PlayerPrefs.SetInt("PlayerBombCount", player.bombsIndex.Count);             // ����� ����� ����
                for (int i = 0; i < player.bombsIndex.Count; i++)                           // ��������� �����
                {
                    PlayerPrefs.SetInt("PlayerBomb" + i, player.bombsIndex[i]);   // ��������� ������ ��� ������� ������
                    PlayerPrefs.SetInt("PlayerBombAmmo" + i, ammoManager.ammoBombs[player.bombsIndex[i]].allAmmo);   // ��������� ������ ��� ������� ������
                }

                // ����������
                if (player.withShield)
                    PlayerPrefs.SetInt("PlayerShield", 1);          // ���

                if (player.withGoldMagnet)
                    PlayerPrefs.SetInt("PlayerMagnet", 1);          // ������ ��� �������                

                TextUI.instance.Saving();                           // ������� ����������*//*

        YandexGame.savesData.gameContinue = true;

        YandexGame.SaveProgress();*/
    }


    void ClearPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}
