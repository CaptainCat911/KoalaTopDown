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
    public GameObject gui;                      // гуи
    public Dialog dialog;                       // диалог менеджер
    public AmmoPackKoala ammoPack;              // ссылка на аммопак
    //public GameObject magazine;                 // магазин
    bool openMagazine;                          // магазин открыт

    [Header("Клавиша взаимодействия")]
    public KeyCode keyToUse;                    // клавиша для действия
    public KeyCode keyOpenMagazine;

    [Header("Предметы")]
    public int gold;                            // золото
    public int[] keys;                          // ключи
    public int battery;                         // батареи

    public bool isPlayerEnactive;               // активен игрок или нет

    public Animator blackImagesAnim;            // аниматор чёрных полос
    [HideInInspector] public bool playerAtTarget;

    //[HideInInspector] public int enemyCount;


    private void Awake()
    {
        if (GameManager.instance != null)
        {
            Destroy(gameObject);
            Destroy(ammoPack);
            Destroy(player.gameObject);
            //Destroy(gui);
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
            ChatBubble.Create(player.transform, new Vector3(0.2f, 0.2f), "Hi", 2f);
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


    public void StartEvent(int number)
    {
        events[number].Invoke();
    }




    public void StartDialog(int number)
    {
        dialog.StartEvent(number);
    }

    // Чёрные полосы
    public void BlackTapes(bool status)
    {
        if (status)
            blackImagesAnim.SetTrigger("In");                   // запускаем чёрные полосы
        else
            blackImagesAnim.SetTrigger("Out");                  // убираем чёрные полосы
    }


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
                    botAI.isNeutral = true;                                     // делаем нейтральным
                }
                collidersHits = null;
            }

        }
        if (!status)
        {
            Collider2D[] collidersHits = Physics2D.OverlapCircleAll(player.transform.position, 25);         // создаем круг в позиции игрока с радиусом (возможно стоит добавить слой (сейчас задевает ботов тоже))
            foreach (Collider2D coll in collidersHits)
            {
                if (coll == null)
                {
                    continue;
                }

                if (coll.gameObject.TryGetComponent<BotAI>(out BotAI botAI))
                {
                    botAI.isNeutral = false;                                     // делаем нейтральным
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
    }
}
