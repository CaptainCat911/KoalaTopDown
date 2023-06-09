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
    //public Dialog dialog;                       // ������ ��������    
    //public GameObject gui;                      // ���

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

    //[HideInInspector] public int enemyCount;


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
        if (Input.GetKeyDown(KeyCode.H))
        {
            ChatBubble.Clear(gameObject);
            ChatBubble.Create(player.transform, new Vector3(0.2f, 0.2f), "Hi", 2f);
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            NextScene(1);
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


    // ������
    public void StartEvent(int number)
    {
        events[number].Invoke();
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
                    botAI.isNeutral = true;                                     // ������ �����������
                }
                collidersHits = null;
            }

        }
        if (!status)
        {
            Collider2D[] collidersHits = Physics2D.OverlapCircleAll(player.transform.position, 25);         // ������� ���� � ������� ������ � �������� (�������� ����� �������� ���� (������ �������� ����� ����))
            foreach (Collider2D coll in collidersHits)
            {
                if (coll == null)
                {
                    continue;
                }

                if (coll.gameObject.TryGetComponent<BotAI>(out BotAI botAI))
                {
                    botAI.isNeutral = false;                                     // ������ �����������
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
