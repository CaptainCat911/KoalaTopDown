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
    private float lastSpawn;
    public int enemysHowMuch;               // сколько врагов нужено
    int enemyCount;
    public GameObject spawnEffect;          // эффект спавна
    public bool bossSpawner;                // спавнер для босса
    public bool arenaSpawner;               // спавнер для арены
    public bool arenaBossSpawner;           // спавнер боссов для арены
    public bool noItem;                     // спаун без предмета  

    //public int enemyTriggerDistance;        // установить дистанцию тригера врагов

    void Update()
    {
        if (!arenaSpawner && !arenaBossSpawner && active && Time.time - lastSpawn > cooldown && enemyCount < enemysHowMuch)
        {
            lastSpawn = Time.time;

            GameObject effect = Instantiate(spawnEffect, transform.position, Quaternion.identity);      // создаем эффект
            Destroy(effect, 0.5f);                                                                      // уничтожаем эффект через .. сек
            
            Invoke("SpawnEnemy", 0.1f);


/*            float dist = Vector3.Distance(transform.position, GameManager.instance.player.transform.position);
            if (dist > radius)
            {
                SpawnEnemy();
            }*/
        }

        // Для арены
        if (arenaSpawner && active && Time.time - lastSpawn > cooldown && EventManager.instance.arenaSpawnStarted)
        {
            lastSpawn = Time.time;

            GameObject effect = Instantiate(spawnEffect, transform.position, Quaternion.identity);      // создаем эффект
            Destroy(effect, 0.5f);                                                                      // уничтожаем эффект через .. сек            

            Invoke("SpawnEnemy", 0.1f);
        }
        if (arenaBossSpawner && active && Time.time - lastSpawn > cooldown)
        {
            lastSpawn = Time.time;

            GameObject effect = Instantiate(spawnEffect, transform.position, Quaternion.identity);      // создаем эффект
            Destroy(effect, 0.5f);                                                                      // уничтожаем эффект через .. сек            

            Invoke("SpawnEnemy", 0.1f);
        }
    }

    public void SpawnEnemy()
    {
        int ndx = Random.Range(0, prefabEnemies.Count);                // выбираем рандом из массива врагов
        GameObject enemyPref = Instantiate(prefabEnemies[ndx], transform.position, Quaternion.identity);        // создаём префаб
        //go.transform.SetParent(transform, false);                     // назначаем этот спавнер родителем
        agent = enemyPref.GetComponentInChildren<NavMeshAgent>();       // находим НавМешАгент
        //Debug.Log(agent);
        agent.Warp(transform.position);                                 // перемещаем префаб к спавнеру

        BotAI bot = enemyPref.GetComponentInChildren<BotAI>();

        bot.triggerLenght = chaseDistance;                              // устанавливаем дистанцию триггера
        if (arenaSpawner)
        {
            bot.isArenaEnemy = true;
            EventManager.instance.arenaEnemyCount++;
        }
        if (arenaBossSpawner)
        {
            bot.isArenaBoss = true;
            EventManager.instance.arenaBossCount++;
        }
        if (noItem)
        {
            bot.itemToSpawn = null;
        }

/*        if (chasePlayer)
        {
            enemyPref.GetComponentInChildren<BotAI>().target = GameManager.instance.player.gameObject;
            enemyPref.GetComponentInChildren<BotAI>().chasing = true;
        }*/
        if (!arenaSpawner)
            enemyCount++;

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


    public void ActivateSpawner()
    {
        active = !active;
    }
}
