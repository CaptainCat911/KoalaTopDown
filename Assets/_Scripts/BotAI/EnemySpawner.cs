using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    NavMeshAgent agent;

    [Header("������� ������")]
    public List<GameObject> prefabEnemies;      // ������ �������� � �������
    public List<GameObject> prefabEnemiesToAdd; // ������ �������� � �������, ������� ����� �������

    public bool active;                     // ������� ��� ���
    //public bool chasePlayer;                // ������� ������ ����������� �����
    public float chaseDistance;             // ��������� �������� ������
    public float cooldown = 1f;             // ����������� ������
    float randomCd;                         // ������ ��� ��
    private float lastSpawn;
    public int enemysHowMuch;               // ������� ������ �����
    int enemyCount;
    public GameObject spawnEffect;          // ������ ������
    public bool arenaMegaBossSpawner;       // ������� ��� ���������
    public bool bossSpawner;                // ������� ��� �����
    public bool arenaSpawner;               // ������� ��� �����
    public bool arenaBossSpawner;           // ������� ������ ��� �����
    public bool noItem;                     // ����� ��� ��������  
    public bool withItem;
    public GameObject itemToSpawn;
    public int resTimes;

    [Header("������")]
    public Animator portalAnimator;

    //public int enemyTriggerDistance;        // ���������� ��������� ������� ������

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

        // ��� �����
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
        GameObject effect = Instantiate(spawnEffect, transform.position, Quaternion.identity);      // ������� ������
        Destroy(effect, 0.5f);                                                                      // ���������� ������ ����� .. ��� 

        int ndx = Random.Range(0, prefabEnemies.Count);                // �������� ������ �� ������� ������
        GameObject enemyPref = Instantiate(prefabEnemies[ndx], transform.position, Quaternion.identity);        // ������ ������
        //go.transform.SetParent(transform, false);                     // ��������� ���� ������� ���������
        agent = enemyPref.GetComponentInChildren<NavMeshAgent>();       // ������� �����������
        //Debug.Log(agent);
        agent.Warp(transform.position);                                 // ���������� ������ � ��������

        BotAI bot = enemyPref.GetComponentInChildren<BotAI>();

        bot.triggerLenght = chaseDistance;                              // ������������� ��������� ��������

        if (bossSpawner)
        {
            bot.chaseLeght = 0;                         // ������� ��������� �������������
        }

        if (arenaSpawner)
        {
            bot.isArenaEnemy = true;                    // ��� ��� �����
            bot.chaseLeght = 0;                         // ������� ��������� �������������
            ArenaManager.instance.arenaEnemyCount++;    // + ������� ������ �� �����
        }

        if (arenaBossSpawner)
        {
            bot.isArenaBoss = true;
            bot.isArenaEnemy = true;                    // ��� ��� �����
            bot.chaseLeght = 0;                         // ������� ��������� �������������
            ArenaManager.instance.arenaBossCount++;
            ArenaManager.instance.arenaEnemyCount++;    // + ������� ������ �� �����
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
