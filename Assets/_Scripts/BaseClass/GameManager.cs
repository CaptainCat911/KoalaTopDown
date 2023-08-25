using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;         // �������
    public bool startScreen;                    // ��� �����������
    public bool showDamage;                     // ���������� ����
    
    public string[] sceneNames;                 // ��� �����

    [Header("������")]
    public Player player;                       // ������ �� ������    
    public AmmoPackKoala ammoManager;           // ���� ��������   
    //public GameObject gui;                      // ���
    //public Dialog dialog;                       // ������ ��������    

    [Header("���������� �����")]    
    [HideInInspector] public bool isPlayerEnactive;             // ������� ����� ��� ���
    [HideInInspector] public bool cameraOnPlayer;               // ���������� �������
    [HideInInspector] public bool dialogeStart;                 // ������ �������
    [HideInInspector] public bool helpOn;                       // ��������� �������
    [HideInInspector] public bool playerInResroom;              // ����� � ������� �����������
    [HideInInspector] public bool playerAtTarget;               // ����� ����� �� ����� ������ �������
    [HideInInspector] public bool musicOff;                     // ������

    [Header("������� ��������������")]
    public KeyCode keyToUse;                    // ������� ��� ��������

    [Header("��������")]
    public int gold;                            // ������
    public int[] keys;                          // �����
    public int battery;                         // �������
    public int pozorCount;                      // ������� ������

    [Header("������� ����� �������")]
    [HideInInspector] public bool resroomed;    // ������� � ������� �����������
    [HideInInspector] public bool pozored;      // ��������
    [HideInInspector] public bool weaponHelped; // �������� ��������� ��� ����� ������
    [HideInInspector] public bool bombHelped;   // �������� ��������� ��� �����
    [HideInInspector] public bool bombHelped_2; // �������� ��������� 2 ��� �����

    [Header("���������� ������")]
    public bool lightDark;
    public bool lightOff;

    [Header("��� ��� ������")]    
    int chatRandom;
    int prevNumber;

    // ��� �����
    bool paused;
    bool slowed;

    // ����� �������
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
        
        instance = this;                                // �������� instance (?) ����� ������� � �� ������ �������� ��������� ������� ��������
        SceneManager.sceneLoaded += OnSceneLoaded;      // ��� �������� ����� ���������� ��� += �������
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

        // ���������� �������
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


        // ��� ������
        if (Input.GetKeyDown(KeyCode.C))
        {
            // ������ ���
            ChatBubble.Clear(player.gameObject);            
            while (prevNumber == chatRandom)
            {
                chatRandom = Random.Range(0, player.chatTexts.Length);
            }
            prevNumber = chatRandom;            
            ChatBubble.Create(player.transform, new Vector3(0.2f, 0.2f), player.chatTexts[chatRandom], 2f);
            
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
        if (sceneNumber == 0)                           // ���� ����� 0 (������� ����) - ���������� 
        {
            Destroy(gameObject);
            Destroy(ammoManager.gameObject);
            Destroy(player.gameObject);
        }

        if (paused)
            ContinueGame();

        SceneManager.LoadScene(sceneName);              // ��������� �����

        /*if (sceneNumber == 4)
        {
            arenaLvl = true;
        }*/
    }

    public void OnSceneLoaded(Scene s, LoadSceneMode mode)                              // ��������� ��� �������� �����
    {
        GameObject spawnPoint = GameObject.Find("SpawnPoint");
        if (spawnPoint && player)
        {
            player.transform.position = spawnPoint.transform.position;                  // ���������� ������ �� ����� ������
        }
        
        player.isImmortal = false;                                                      // ������� ����������
        keys[0] = 0;                                                                    // ���������� �����
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
