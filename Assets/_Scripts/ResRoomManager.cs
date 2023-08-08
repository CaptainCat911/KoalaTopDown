using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResRoomManager : MonoBehaviour
{
    public static ResRoomManager instance;     // инстанс

    public Transform deathPos;          // место несчастного случая
    public Transform resStart;          // позиция для воскрешения в комнате
    public Transform resStartPozor;     // позиция для воскрешения в комнате
    public Transform monsterPos;        // позиция для монстра
    public Transform chestPos;          // позиция для сундука
    public GameObject monsterPref;      // префаб монстра
    public GameObject chestPref;        // префаб сундука
    //GameObject monster;
    GameObject chest;

    private void Awake()
    {
        instance = this;
    }

    public void SpawnMonsterAndChest()
    {
        if (ArenaManager.instance.arenaLevel)
            return;
/*        if (monster == null)
        {
            monster = Instantiate(monsterPref, monsterPos.position, Quaternion.identity);       // создаём монстра
        }*/        
        
        BotAI monsterBotAi = monsterPref.GetComponent<BotAI>();
        if (!monsterBotAi.isAlive)
            monsterBotAi.StartRes();
                    
        chest = Instantiate(chestPref, chestPos.position, Quaternion.identity);         // создаём сундук        
        //agent = enemyPref.GetComponentInChildren<NavMeshAgent>();                   // находим НавМешАгент        
        //agent.Warp(transform.position);                                             // перемещаем префаб к спавнеру        
    }

    public void ResetMonsterAndChest()
    {
        //Destroy(monster, 1);
        BotAI monsterBotAi = monsterPref.GetComponent<BotAI>();
        monsterBotAi.currentHealth = monsterBotAi.maxHealth;
        monsterBotAi.hpBarGO.SetActive(false);
        Destroy(chest, 1);
    }
}
