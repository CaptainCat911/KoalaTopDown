using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] prefabEnemies;      // ������ �������� � �����
    NavMeshAgent agent;

    public bool active;
    public float cooldown = 1f;             // ����������� ������
    private float lastSpawn;


    void Update()
    {
        if (active && Time.time - lastSpawn > cooldown)
        {
            lastSpawn = Time.time;
            // ��� �������� ������ ���������
            Invoke("SpawnEnemy", 1);
            

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

        go.transform.SetParent(transform, false);                   // ��������� ���� ������� ���������
        agent = go.GetComponent<NavMeshAgent>();                    // ������� �����������
        agent.Warp(transform.position);                             // ���������� ������ � ��������
    }
}
