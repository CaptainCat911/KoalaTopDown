using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResRoomManager : MonoBehaviour
{
    public static ResRoomManager instance;     // инстанс

    public Transform deathPos;
    public Transform monsetPos;
    public Transform chestPos;
    public GameObject monsterPref;
    public GameObject chestPref;

    private void Awake()
    {
        instance = this;
    }

    public void SpawnMonsterAndChest()
    {       
        GameObject monster = Instantiate(monsterPref, monsetPos.position, Quaternion.identity);             // создаём монстра        
        GameObject chest = Instantiate(chestPref, chestPos.position, Quaternion.identity);                // создаём сундук        
        //agent = enemyPref.GetComponentInChildren<NavMeshAgent>();                   // находим НавМешАгент        
        //agent.Warp(transform.position);                                             // перемещаем префаб к спавнеру        
    }

}
