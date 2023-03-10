using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;         // �������

    [Header("������")]
    public Player player;                       // ������ �� ������    
    public GameObject gui;                      // ���
    public Dialog dialog;                       // ������ ��������
    public AmmoPackKoala ammoPack;              // ������ �� �������
    public GameObject magazine;
    bool openMagazine;

    [Header("������� ��������������")]
    public KeyCode keyToUse;                    // ������� ��� ��������
    public KeyCode keyOpenMagazine;

    [Header("��������")]
    public int gold;                            // ������
    public int keys;                            // �����
    public int battery;                         // �������

    public bool isPlayerEnactive;

    [HideInInspector] public int enemyCount;
    


    private void Awake()
    {
        if (GameManager.instance != null)
        {
            Destroy(gameObject);
            Destroy(player.gameObject);
            Destroy(gui);
            //Destroy(floatingTextManager.gameObject);
            //Destroy(hud);
            //Destroy(menu);
            //Destroy(eventSys);


            return;
        }
        // �������� instance (?) ����� ������� � �� ������ �������� ��������� ������� ��������
        instance = this;       
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            ChatBubble.Clear(gameObject);
            ChatBubble.Create(player.transform, new Vector3(0.2f, 0.2f), "Hi");
        }
        
        if (Input.GetKeyDown(keyOpenMagazine))
        {
            OpenCloseMagazine();
        }
    }

    public void OpenCloseMagazine()
    {
        isPlayerEnactive = !isPlayerEnactive;
        openMagazine = !openMagazine;
        magazine.SetActive(openMagazine);
    }

    public void StartDialog(int number)
    {
        dialog.StartDialog(number);
    }

    public void OnSceneLoaded(Scene s, LoadSceneMode mode)                      // ��������� ��� �������� �����
    {
        player.transform.position = GameObject.Find("SpawnPoint").transform.position;
    }
}
