using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResRoomManager : MonoBehaviour
{
    public static ResRoomManager instance;     // �������

    public Transform deathPos;          // ����� ����������� ������
    public Transform resStart;          // ������� ��� ����������� � �������
    public Transform resStartPozor;     // ������� ��� ����������� � �������
    public Transform monsterPos;        // ������� ��� �������
    public Transform chestPos;          // ������� ��� �������
    public Transform chestPosHp;        // ������� ��� ������� ��
    public GameObject monsterPref;      // ������ �������
    public GameObject chestPref;        // ������ �������
    public GameObject chestHpPref;      // ������ ������� ��
    //GameObject monster;
    GameObject chest;
    GameObject chestHp;

    private void Awake()
    {
        instance = this;
    }

    // ������� ������� � ���������� ������� ��� ��������� � ������
    public void SpawnMonsterAndChest()
    {
        if (ArenaManager.instance.arenaLevel)
            return;
/*        if (monster == null)
        {
            monster = Instantiate(monsterPref, monsterPos.position, Quaternion.identity);       // ������ �������
        }*/        
        
        BotAI monsterBotAi = monsterPref.GetComponent<BotAI>();     // ������� ������
        if (!monsterBotAi.isAlive)                                  // ���� ������ ����
            monsterBotAi.StartRes();                                // ����������
                    
        chest = Instantiate(chestPref, chestPos.position, Quaternion.identity);         // ������ ������        
        chestHp = Instantiate(chestHpPref, chestPosHp.position, Quaternion.identity);   // ������ ������ ��      

        //agent = enemyPref.GetComponentInChildren<NavMeshAgent>();                   // ������� �����������        
        //agent.Warp(transform.position);                                             // ���������� ������ � ��������        
    }

    // ����� ������ �� �������
    public void ResetMonsterAndChest()
    {
        //Destroy(monster, 1);
        BotAI monsterBotAi = monsterPref.GetComponent<BotAI>();     // ������� ������ �������
        monsterBotAi.currentHealth = monsterBotAi.maxHealth;        // ���������� �� �������
        monsterBotAi.hpBarGO.SetActive(false);                      // ������ �� �������
        Destroy(chest, 1);                                          // ������� �������
        Destroy(chestHp, 1);
    }
}
