using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.AI;

public class EnemyThinker : MonoBehaviour
{
    [HideInInspector] public BotAI botAI;               // ссылка на бота
    //public Brain[] brains;

    //[HideInInspector] public GameObject target;     // цель
    bool isFindTarget;                                  // нашли цель
    float distanceToTarget;                             // дистанция до цели
    // Поиск цели
    //public float targetFindRadius = 5f;                 // радиус поиска цели                                                               
    float lastTargetFind;                               // время последнего поиска цели
    float cooldownFind = 0.5f;                          // перезардяка поиска цели
    public float cooldownChangeTarget = 2f;             // перезарядка смена цели

    bool type_1;        // тип оружия мили
    bool type_2;        // тип оружия ренж

    
    [Header("Поведение")]
    //public bool patrolingRandomPosition;
    public float cooldownChange;                        // перезардяка смены позиции
    public float distancePatrol;                        // дистанция для патрулирования
    public float maxDistancePatrol;                     // максимальная дистанция от стартовой позиции
    [HideInInspector] public float lastChange;          // время последней смены позиции

    // Для НПС
    [HideInInspector] public Transform[] positionsPoints;
    [HideInInspector] public int i = 0;
    [HideInInspector] public bool nextPosition;
    [HideInInspector] public bool letsGo;               // идти за игроком
    public string textTrigger;
    bool sayTriggerText;

    //public bool isFriendly;
    public bool debug;
    

    private void Awake()
    {        
        botAI = GetComponent<BotAI>();        
    }

    private void Start()
    {
        
    }

    /// <summary>
    /// brains[0].Think(this); - поведение когда нет цели (стоять на месте, патруль и т.д.)
    /// brains[1].Think(this); - преследуем и атакуем   
    /// </summary>



    private void FixedUpdate()
    {
        // Если бот нейтрален
        if (botAI.isNeutral || !botAI.isAlive)                        
            return;

        // Сбрасываем всякие штуки, если цели нет
        if (isFindTarget && !botAI.target)
        {
            isFindTarget = false;
            botAI.ResetTarget();
        }
        

        // Логика для поиска цели и патрулирования 
        if (!isFindTarget)                                  // если нет цели 
        {
            if (Time.time - lastTargetFind > cooldownFind)  // если кд готово
            {
                lastTargetFind = Time.time;
                botAI.FindTarget();                         // поиск цели
            }

            if (botAI.followPlayer)
            {
                float distanceToPlayer = Vector3.Distance(GameManager.instance.player.transform.position, botAI.transform.position);
                if (distanceToPlayer > 2)
                {
                    botAI.SetDestination(GameManager.instance.player.transform.position);
                    botAI.startPosition = GameManager.instance.player.transform.position;
                }
                else
                {
                    botAI.agent.ResetPath();
                }
            }
            if (botAI.stayOnGround)                                
            {
                Patrol();                               // патрулирование  
                //patrolingRandomPosition = true;                           
            }
        }
        else            // иногда меняем цель
        {
            if (Time.time - lastTargetFind > Random.Range(cooldownChangeTarget, cooldownChangeTarget + 2f))  // сменяем цель, если больше одной цели
            {
                lastTargetFind = Time.time;
                botAI.FindTarget();                               // поиск цели
            }
        }

        // Если нашли цель делаем рейкасты и меряем дистанцию
        if (botAI.target)                                     // если нашли цель
        {
            botAI.NavMeshRayCast(botAI.target);               // делаем рейкаст
            distanceToTarget = Vector3.Distance(botAI.target.transform.position, botAI.transform.position);   // считаем дистанцию 
        }

        // Логика если нашли цель и она видима        
        if (botAI.targetVisible)       // если дистанция до игрока < тригер дистанции       (distanceToTarget < botAI.triggerLenght &&)
        {
            if (botAI.twoWeapons)
            {
                if (distanceToTarget < 2 && !type_1)
                {
                    botAI.SwitchAttackType(1);
                    type_1 = true;
                    type_2 = false;
                    //Debug.Log("Melee!");
                }
                if (distanceToTarget > botAI.triggerLenght - 1 && !type_2)
                {
                    botAI.SwitchAttackType(2);
                    type_1 = false;
                    type_2 = true;
                    //Debug.Log("Range!");
                }
            }              

            if (!botAI.chasing)
            {
                //patrolingRandomPosition = false;        // патрулирование 
                botAI.chasing = true;                   // преследование включено
                isFindTarget = true;
            }
        }   

        if (botAI.chasing && botAI.target)
        {
            botAI.Chase();                      // преследуем и аттакуем           
        }



        if (sayTriggerText && !isFindTarget)
        {
            botAI.SayText(textTrigger);
            sayTriggerText = false;
        }



        if(debug)
        {
            Debug.Log(botAI.target);
            Debug.Log(isFindTarget);
            //Debug.Log(botAI.chasing);
            //Debug.Log(botAI.readyToAttack);
        }


/*        if (isFriendly)
        {
            MakeFriendly();
            isFriendly = false;
        }*/
    }






    public void GoToPosition(int positionNumber)
    {
        /*        if (nextPosition && positionsPoints.Length > 0)
                {

                }*/
        i = positionNumber;
    }

/*    void ChaseAndAttack()
    {
        float distance = Vector3.Distance(botAI.transform.position, botAI.target.transform.position);       // считаем дистанцию до цели        
        if (botAI.targetVisible && distance < botAI.distanceToAttack)
        {
            if (!botAI.readyToAttack)
            {
                botAI.agent.ResetPath();                                                              // сбрасываем путь            
                botAI.readyToAttack = true;                                                           // готов стрелять
            }
        }
        else
        {
            botAI.agent.SetDestination(botAI.target.transform.position);                              // перемещаемся к цели
            if (botAI.readyToAttack)
                botAI.readyToAttack = false;                                                            // не готов стрелять                
        }
        //Debug.Log(range); 
    }*/



    public void StayGo()
    {
        letsGo = !letsGo;
        if (!letsGo)
        {
            botAI.stayOnGround = false;
            botAI.followPlayer = true;
            botAI.SayText("Я за тобой");
        }
        if (letsGo)
        {
            botAI.stayOnGround = true;
            botAI.followPlayer = false;
            botAI.SayText("Хорошо, я подожду здесь");
        }
    }


    public void TriggerSayText(string text)
    {
        textTrigger = text;
        sayTriggerText = true;
    }

    void Patrol()
    {
        if (Time.time - lastChange > Random.Range(cooldownChange, cooldownChange + 2f))        // если кд готово
        {
            lastChange = Time.time;

            Vector3 destination = new(botAI.transform.position.x + Random.Range(-distancePatrol, distancePatrol),
                botAI.transform.position.y + Random.Range(-distancePatrol, distancePatrol), botAI.transform.position.z);    // выбираем случайную позицию
            botAI.SetDestination(destination);                      // идём в случайную позицию
        }

        float distanceFromStart = Vector3.Distance(botAI.startPosition, botAI.transform.position);  // считаем дистанцию от стартовой позиции до следующей позиции

        if (distanceFromStart > maxDistancePatrol)
            botAI.SetDestination(botAI.startPosition);      // возвращаемся в стартовую позицию        
    }

    void MakeFriendly()
    {        
        botAI.gameObject.layer = LayerMask.NameToLayer("NPC");  // слой самого бота
        botAI.layerHit = LayerMask.GetMask("Enemy", "ObjectsDestroyble", "Default");    // слой для оружия
        
/*        foreach (GameObject weaponGO in botAI.botAIWeaponHolder.weapons)                // слой для каждого оружия у бота
        {
            var weapon = weaponGO.GetComponent<BotAIWeapon>();
            weapon.layerHit = botAI.layerHit;
        }  */      
       
    }
}
