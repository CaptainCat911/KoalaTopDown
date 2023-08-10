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
    public Transform chestPosHp;        // позиция для сундука хп
    public GameObject monsterPref;      // префаб монстра
    public GameObject chestPref;        // префаб сундука
    public GameObject chestHpPref;      // префаб сундука хп
    //GameObject monster;
    GameObject chest;
    GameObject chestHp;

    private void Awake()
    {
        instance = this;
    }

    // Спауним сундуки и возрождаем монстра при попадании в ресрум
    public void SpawnMonsterAndChest()
    {
        if (ArenaManager.instance.arenaLevel)
            return;
/*        if (monster == null)
        {
            monster = Instantiate(monsterPref, monsterPos.position, Quaternion.identity);       // создаём монстра
        }*/        
        
        BotAI monsterBotAi = monsterPref.GetComponent<BotAI>();     // находим скрипт
        if (!monsterBotAi.isAlive)                                  // если монстр убит
            monsterBotAi.StartRes();                                // возрождаем
                    
        chest = Instantiate(chestPref, chestPos.position, Quaternion.identity);         // создаём сундук        
        chestHp = Instantiate(chestHpPref, chestPosHp.position, Quaternion.identity);   // создаём сундук хп      

        //agent = enemyPref.GetComponentInChildren<NavMeshAgent>();                   // находим НавМешАгент        
        //agent.Warp(transform.position);                                             // перемещаем префаб к спавнеру        
    }

    // После выхода из ресрума
    public void ResetMonsterAndChest()
    {
        //Destroy(monster, 1);
        BotAI monsterBotAi = monsterPref.GetComponent<BotAI>();     // находим скрипт монстра
        monsterBotAi.currentHealth = monsterBotAi.maxHealth;        // восполняем хп монстра
        monsterBotAi.hpBarGO.SetActive(false);                      // прячем хп монстра
        Destroy(chest, 1);                                          // убираем сундуки
        Destroy(chestHp, 1);
    }
}
