using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    NavMeshAgent agent;

    [Header("Префабы врагов")]
    public List<GameObject> prefabEnemies;      // массив префабов с врагами
    public List<GameObject> prefabEnemiesToAdd; // массив префабов с врагами, которох потом добавим

    public bool active;                     // активен или нет
    //public bool chasePlayer;                // триггер врагов срабатывает сразу
    public float chaseDistance;             // дистанция триггера врагов
    public float cooldown = 1f;             // перезарядка спауна
    float randomCd;                         // рандом для кд
    private float lastSpawn;
    public int enemysHowMuch;               // сколько врагов нужно
    int enemyCount;
    public GameObject spawnEffect;          // эффект спавна
    public bool arenaMegaBossSpawner;       // спавнер для мегабосса
    public bool bossSpawner;                // спавнер для босса
    public bool arenaSpawner;               // спавнер для арены
    public bool arenaBossSpawner;           // спавнер боссов для арены
    public bool noItem;                     // спаун без предмета  
    public bool withItem;
    public GameObject itemToSpawn;
    public int resTimes;

    [Header("Портал")]
    public Animator portalAnimator;

    //public int enemyTriggerDistance;        // установить дистанцию тригера врагов

    private void Start()
    {
        randomCd = Random.Range(1, 4);
        lastSpawn = Time.time;
    }

    void Update()
    {
        if (!arenaSpawner && !arenaBossSpawner && active && Time.time - (lastSpawn) > cooldown && enemyCount < enemysHowMuch)
        {
            lastSpawn = Time.time;
            portalAnimator.SetTrigger("Open");
            Invoke("SpawnEnemy", 2f);
            enemyCount++;


            /*            float dist = Vector3.Distance(transform.position, GameManager.instance.player.transform.position);
                        if (dist > radius)
                        {
                            SpawnEnemy();
                        }*/
        }

        // Для арены
        if (arenaSpawner && active && Time.time - (lastSpawn + randomCd) > cooldown && ArenaManager.instance.arenaSpawnStarted)
        {
            lastSpawn = Time.time;
            portalAnimator.SetTrigger("Open");
            Invoke("SpawnEnemy", 2f);
        }
        if (arenaBossSpawner && active && Time.time - lastSpawn > cooldown)
        {
            lastSpawn = Time.time;
            portalAnimator.SetTrigger("Open");
            Invoke("SpawnEnemy", 2f);
        }
    }

    public void WaveSpawnEnemy()
    {
        portalAnimator.SetTrigger("Open");
        Invoke("SpawnEnemy", 2f);
    }

    public void SpawnEnemy()
    {
        GameObject effect = Instantiate(spawnEffect, transform.position, Quaternion.identity);      // создаем эффект
        Destroy(effect, 0.5f);                                                                      // уничтожаем эффект через .. сек 

        int ndx = Random.Range(0, prefabEnemies.Count);                // выбираем рандом из массива врагов
        GameObject enemyPref = Instantiate(prefabEnemies[ndx], transform.position, Quaternion.identity);        // создаём префаб
        //go.transform.SetParent(transform, false);                     // назначаем этот спавнер родителем
        agent = enemyPref.GetComponentInChildren<NavMeshAgent>();       // находим НавМешАгент
        //Debug.Log(agent);
        agent.Warp(transform.position);                                 // перемещаем префаб к спавнеру

        BotAI bot = enemyPref.GetComponentInChildren<BotAI>();

        bot.triggerLenght = chaseDistance;                              // устанавливаем дистанцию триггера

        if (bossSpawner)
        {
            bot.chaseLeght = 0;                         // убираем дистанцию преследования
        }

        if (arenaSpawner)
        {
            bot.isArenaEnemy = true;                    // бот для арены
            bot.chaseLeght = 0;                         // убираем дистанцию преследования
            ArenaManager.instance.arenaEnemyCount++;    // + счетчик врагов на арене
        }

        if (arenaBossSpawner)
        {
            bot.isArenaBoss = true;
            bot.isArenaEnemy = true;                    // бот для арены
            bot.chaseLeght = 0;                         // убираем дистанцию преследования
            ArenaManager.instance.arenaBossCount++;
            ArenaManager.instance.arenaEnemyCount++;    // + счетчик врагов на арене
        }
        if (noItem)
        {
            bot.itemToSpawn = null;
        }

        if (withItem)
        {
            bot.itemToSpawn = itemToSpawn;
        }

        if (resTimes > 0)
        {
            bot.skeletonResble = true;
            bot.resTimes = resTimes;
        }

/*        if (chasePlayer)
        {
            enemyPref.GetComponentInChildren<BotAI>().target = GameManager.instance.player.gameObject;
            enemyPref.GetComponentInChildren<BotAI>().chasing = true;
        }*/
/*        if (!arenaSpawner)
            enemyCount++;*/

        if (portalAnimator)
            portalAnimator.SetTrigger("Close");

        //GameManager.instance.enemyCount++;
        if (enemyCount >= enemysHowMuch && !bossSpawner && !arenaSpawner)
        {
            //Invoke("NextSpawnersOn", 5f);
            Destroy(gameObject, 0.1f);
        }
    }

    public void AddNewEnemy(int number)
    {
        prefabEnemies.Add(prefabEnemiesToAdd[number]);
    }

    public void RemoveNewEnemy(int number)
    {
        prefabEnemies.Remove(prefabEnemies[number]);
    }

    public void ActivateSpawner()
    {
        active = !active;
    }
}
