using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;         // инстанс

    [Header("Ссылки")]
    public Player player;                       // ссылка на игрока    
    public GameObject gui;   
    
    [Header("Клавиша взаимодействия")]
    public KeyCode keyToUse;                    // клавиша для действия

    [Header("Предметы")]
    public int keys;                            // ключи
    public int battery;                         // батареи

    [HideInInspector] public int enemyCount;
    [HideInInspector] public bool roomStart;


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
        // присваем instance (?) этому обьекту и по ивенту загрузки запускаем функцию загрузки
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

        if (roomStart && enemyCount <= 0)
        {
            roomStart = false;
        }
    }



    public void OnSceneLoaded(Scene s, LoadSceneMode mode)                      // выполняем при загрузке сцены
    {
        player.transform.position = GameObject.Find("SpawnPoint").transform.position;
    }

}
