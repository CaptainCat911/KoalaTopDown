using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    NavMeshAgent agent;

    [Header("Префабы врагов")]
    public GameObject[] prefabEnemies;      // массив префабов с зомби

    public bool active;                     // активен или нет
    //public bool chasePlayer;                // триггер врагов срабатывает сразу
    public float chaseDistance;             // дистанция триггера врагов
    public float cooldown = 1f;             // перезарядка спауна    
    private float lastSpawn;
    public int enemysHowMuch;               // сколько врагов нужено
    int enemyCount;
    public GameObject spawnEffect;          // эффект спавна
    public bool bossSpawner;                // спавнер для босса
    public bool noItem;                     // спаун без предмета       

    //public int enemyTriggerDistance;        // установить дистанцию тригера врагов

    void Update()
    {
        if (active && Time.time - lastSpawn > cooldown && enemyCount < enemysHowMuch)
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
    }

    public void SpawnEnemy()
    {
        int ndx = Random.Range(0, prefabEnemies.Length);                            // выбираем рандом из массива врагов
        GameObject enemyPref = Instantiate(prefabEnemies[ndx], transform.position, Quaternion.identity);                     // создаём префаб
        //go.transform.SetParent(transform, false);                   // назначаем этот спавнер родителем
        agent = enemyPref.GetComponentInChildren<NavMeshAgent>();                   // находим НавМешАгент
        Debug.Log(agent);
        agent.Warp(transform.position);                                             // перемещаем префаб к спавнеру
        enemyPref.GetComponentInChildren<BotAI>().triggerLenght = chaseDistance;    // устанавливаем дистанцию триггера
        if(noItem)
            enemyPref.GetComponentInChildren<BotAI>().itemToSpawn = null;

/*        if (chasePlayer)
        {
            enemyPref.GetComponentInChildren<BotAI>().target = GameManager.instance.player.gameObject;
            enemyPref.GetComponentInChildren<BotAI>().chasing = true;
        }*/

        enemyCount++;

        //GameManager.instance.enemyCount++;
        if (enemyCount >= enemysHowMuch && !bossSpawner)
        {
            //Invoke("NextSpawnersOn", 5f);
            Destroy(gameObject, 0.1f);
        }
    }

/*    void NextSpawnersOn()
    {
        foreach (GameObject spawner in nextSpawners)
        {
            spawner.SetActive(true);
        }
    }*/

    public void ActivateSpawner()
    {
        active = !active;
    }
}
