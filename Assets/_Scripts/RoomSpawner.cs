using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public GameObject walls;
    public GameObject[] spawners;
    bool roomStarted;
    

    public void Start()
    {
        walls.SetActive(false);
        foreach (GameObject spawner in spawners)
        {
            spawner.SetActive(false);
        }
    }

    void Update()
    {
/*        if (!GameManager.instance.roomStart && roomStarted )
        {
            // тут эффекты всякие
            Destroy(gameObject, 1);
        }*/
    }

    public void RoomStart()
    {

        StartCoroutine(RoomStartCoroutine());
        roomStarted = true;
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
        GameManager.instance.roomStart = true;
    }
}
