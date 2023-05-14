using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResRoomManager : MonoBehaviour
{
    public static ResRoomManager instance;     // �������

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
        GameObject monster = Instantiate(monsterPref, monsetPos.position, Quaternion.identity);             // ������ �������        
        GameObject chest = Instantiate(chestPref, chestPos.position, Quaternion.identity);                // ������ ������        
        //agent = enemyPref.GetComponentInChildren<NavMeshAgent>();                   // ������� �����������        
        //agent.Warp(transform.position);                                             // ���������� ������ � ��������        
    }

}
