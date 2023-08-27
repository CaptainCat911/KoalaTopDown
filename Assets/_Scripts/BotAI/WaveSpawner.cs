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
            enemySpawner.WaveSpawnEnemy();
        }
    }

    public void SetNewEnemy(int number)
    {
        if (number == 0)
        {
            foreach (EnemySpawner enemySpawner in waveSpawner)
            {
                enemySpawner.AddNewEnemy(0);
                enemySpawner.RemoveNewEnemy(0);
            }
        }
        if (number == 1)
        {
            foreach (EnemySpawner enemySpawner in waveSpawner)
            {
                enemySpawner.AddNewEnemy(1);
                enemySpawner.RemoveNewEnemy(0);
            }
        }
    }
}
