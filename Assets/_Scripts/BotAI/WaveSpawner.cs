using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public EnemySpawner[] waveSpawner;

    public void MakeWave()
    {
        foreach (EnemySpawner enemySpawner in waveSpawner)
        {
            enemySpawner.SpawnEnemy();
        }
    }
}
