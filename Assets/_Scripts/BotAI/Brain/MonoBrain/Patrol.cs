using UnityEngine;

public class Patrol : MonoBehaviour
{
    EnemyThinker enemyThinker;

    public float lastChange;                            // время последнего удара (для перезарядки удара)
    public float cooldownChange;                        // перезардяка поиска цели
    public float distancePatrol;                        // дистанция для патрулирования
    public float maxDistancePatrol;                     // максимальная дистанция от стартовой позиции


    private void Awake()
    {
        enemyThinker = GetComponent<EnemyThinker>();
    }

    void FixedUpdate()
    {
        if (!enemyThinker.botAI.isAlive)                // (потом сделать по нормальному)
            return;

/*        if (!enemyThinker.patrolingRandomPosition)
            return;*/

        if (Time.time - lastChange > Random.Range(cooldownChange, cooldownChange + 2f))        // если кд готово
        {
            lastChange = Time.time;

            Vector3 destination = new(enemyThinker.botAI.transform.position.x + Random.Range(-distancePatrol, distancePatrol),
                enemyThinker.botAI.transform.position.y + Random.Range(-distancePatrol, distancePatrol), enemyThinker.botAI.transform.position.z);    // выбираем случайную позицию

            enemyThinker.botAI.SetDestination(destination);                      // идём в случайную позицию            
        }

        float distanceFromStart = Vector3.Distance(enemyThinker.botAI.startPosition, enemyThinker.botAI.transform.position);  // считаем дистанцию от стартовой позиции до следующей позиции

        if (distanceFromStart > maxDistancePatrol)
            enemyThinker.botAI.SetDestination(enemyThinker.botAI.startPosition);      // возвращаемся в стартовую позицию        
    }
}

