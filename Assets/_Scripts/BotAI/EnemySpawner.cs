using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    NavMeshAgent agent;

    [Header("������� ������")]
    public GameObject[] prefabEnemies;      // ������ �������� � �����

    public bool active;                     // ������� ��� ���
    //public bool chasePlayer;                // ������� ������ ����������� �����
    //public float chaseDistance;             // ��������� �������� ������
    public float cooldown = 1f;             // ����������� ������    
    private float lastSpawn;
    public int enemysHowMuch;
    int enemyCount;
    public GameObject spawnEffect;
    public GameObject[] nextSpawners;

    //public int enemyTriggerDistance;        // ���������� ��������� ������� ������

    void Update()
    {
        if (active && Time.time - lastSpawn > cooldown && enemyCount < enemysHowMuch)
        {
            lastSpawn = Time.time;

            GameObject effect = Instantiate(spawnEffect, transform.position, Quaternion.identity);      // ������� ������
            Destroy(effect, 0.5f);                                                                      // ���������� ������ ����� .. ���
            
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
        int ndx = Random.Range(0, prefabEnemies.Length);            // �������� ������ �� ������� ������
        GameObject go = Instantiate(prefabEnemies[ndx]);            // ������ ������
        //go.transform.SetParent(transform, false);                   // ��������� ���� ������� ���������
        agent = go.GetComponent<NavMeshAgent>();                    // ������� �����������
        agent.Warp(transform.position);                             // ���������� ������ � ��������
        //if(chasePlayer)
            //go.GetComponent<BotAI>().triggerLenght = chaseDistance; // ������������� ������������� �� �������
        enemyCount++;
        GameManager.instance.enemyCount++;
        if (enemyCount >= enemysHowMuch)
        {
            Invoke("NextSpawnersOn", 5f);
            Destroy(gameObject, 6f);
        }
    }

    void NextSpawnersOn()
    {
        foreach (GameObject spawner in nextSpawners)
        {
            spawner.SetActive(true);
        }
    }

    public void ActivateSpawner()
    {
        active = !active;
    }


}
