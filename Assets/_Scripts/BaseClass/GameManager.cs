using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;         // �������

    [Header("������")]
    public Player player;                       // ������ �� ������    
    public GameObject gui;
    

    [Header("��������")]
    public int keys;                            // �����

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

    private void Start()
    {
        
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            ChatBubble.Create(player.transform, new Vector3(0f, 0f), "hi");
        }    
    }

    public void TakeKey(bool findKey)
    {
        if (findKey)
            keys++;
        else if (!findKey && keys > 0)
            keys--;
    }

    public void OnSceneLoaded(Scene s, LoadSceneMode mode)
    {
        player.transform.position = GameObject.Find("SpawnPoint").transform.position;
    }

}
