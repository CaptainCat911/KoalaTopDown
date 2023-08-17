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
    public int enemysHowMuch;               // ������� ������ ������
    int enemyCount;
    public GameObject spawnEffect;          // ������ ������
    public bool bossSpawner;                // ������� ��� �����
    public bool arenaSpawner;               // ������� ��� �����
    public bool arenaBossSpawner;           // ������� ������ ��� �����
    public bool noItem;                     // ����� ��� ��������  

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
        if (!arenaSpawner && !arenaBossSpawner && active && Time.time - (lastSpawn + randomCd) > cooldown && enemyCount < enemysHowMuch)
        {
            lastSpawn = Time.time;
            portalAnimator.SetTrigger("Open");
            Invoke("SpawnEnemy", 2f);


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
            ArenaManager.instance.arenaBossCount++;
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


    public void ActivateSpawner()
    {
        active = !active;
    }
}
