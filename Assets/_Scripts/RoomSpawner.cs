using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public GameObject walls;
    public GameObject[] spawners;
    bool roomStarted;
    public LayerMask layerEnemy;

    public float cooldown = 1f;             // перезардяка сканирование
    float lastScan;                         // время последнего скана

    int i;



    public void Start()
    {
/*        walls.SetActive(false);
        foreach (GameObject spawner in spawners)
        {
            spawner.SetActive(false);
        }*/
    }

    void Update()
    {
        if (roomStarted && Time.time - lastScan > cooldown)
        {
            
            Scan();                             // сканируем на наличие врагов
            lastScan = Time.time;               // присваиваем время скана
        }

        if (i > 2)
        {
            Destroy(this);
        }

        Debug.Log(i);
    }

    public void RoomStart()
    {
        StartCoroutine(RoomStartCoroutine());        
    }

    IEnumerator RoomStartCoroutine()
    {
        //Debug.Log("Start");
        walls.SetActive(true);
        yield return new WaitForSeconds(2);
        foreach (GameObject spawner in spawners)
        {
            spawner.SetActive(true);
        }
        yield return new WaitForSeconds(3);
        roomStarted = true;        
    }

    public void Scan()
    {

        Collider2D[] collidersHits = Physics2D.OverlapCircleAll(transform.position, 20, layerEnemy);        // создаем круг в позиции объекта с радиусом
        foreach (Collider2D coll in collidersHits)
        {
            i++;
            if (coll == null)
            {
                continue;
            }

            if (coll.gameObject.TryGetComponent<BotAI>(out BotAI bot))
            {
                i--;
            }
            collidersHits = null;

            Debug.Log("i");
            
        }
        Debug.Log("Scan");        


        /*        foreach (Collider2D coll in collidersHits)
                {
                    if (coll == null)
                    {

                    }
                    collidersHits = null;
                }*/
    }
}
