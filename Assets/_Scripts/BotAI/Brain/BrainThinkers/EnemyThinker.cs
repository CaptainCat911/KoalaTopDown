using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.AI;

public class EnemyThinker : MonoBehaviour
{
    [HideInInspector] public BotAI botAI;               // ссылка на бота
    //public Brain[] brains;

    //[HideInInspector] public GameObject target;     // цель
    bool isFindTarget;                              // нашли цель
    float distanceToTarget;                         // дистанци€ до цели
    // ѕоиск цели
    //public float targetFindRadius = 5f;             // радиус поиска цели                                                               
    float lastTargetFind;                           // врем€ последнего поиска цели
    float cooldownFind = 0.1f;                      // перезард€ка поиска цели
    public float cooldownChangeTarget = 2f;         // перезар€дка смена цели

    bool type_1;        // тип оружи€ мили
    bool type_2;        // тип оружи€ ренж


    [Header("ѕоведение")]
    //public bool patrolingRandomPosition;
    public float cooldownChange;                    // перезард€ка смены позиции
    public float distancePatrol;                    // дистанци€ дл€ патрулировани€
    public float maxDistancePatrol;                 // максимальна€ дистанци€ от стартовой позиции
    [HideInInspector] public float lastChange;      // врем€ последней смены позиции

    [Header("ƒл€ корол€ скелетов")] 
    public bool withSpell;                          // со спелом
    public float cooldownSpell;                     // кд спелов
    float lastSpell;    
    int iSpell = 1;                                     // счетчик

    // ƒл€ Ќѕ—
    public Transform goToPosition;
    [HideInInspector] public Transform[] positionsPoints;
    [HideInInspector] public int i = 0;
    [HideInInspector] public bool nextPosition;
    bool letsGo;               // идти за игроком
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
    /// brains[0].Think(this); - поведение когда нет цели (сто€ть на месте, патруль и т.д.)
    /// brains[1].Think(this); - преследуем и атакуем   
    /// </summary>



    private void FixedUpdate()
    {
        // ≈сли бот нейтрален
        if (botAI.isNeutral || !botAI.isAlive)                        
            return;

        // —брасываем вс€кие штуки, если цели нет
        if (isFindTarget && !botAI.target)
        {
            isFindTarget = false;
            botAI.ResetTarget();
        }
        

        // Ћогика дл€ поиска цели и патрулировани€ 
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
            if (botAI.stayOnGround && !botAI.noPatrol)                                
            {
                Patrol();                               // патрулирование  
                //patrolingRandomPosition = true;                           
            }
            if (botAI.goTo)
            {
/*                float distanceToPlayer = Vector3.Distance(GameManager.instance.player.transform.position, botAI.transform.position);
                if (distanceToPlayer > 10)
                    return;*/
                botAI.SetDestination(goToPosition.position);
            }
        }
        else            // иногда мен€ем цель
        {
            if (Time.time - lastTargetFind > Random.Range(cooldownChangeTarget, cooldownChangeTarget + 2f))  // смен€ем цель, если больше одной цели
            {
                lastTargetFind = Time.time;
                botAI.FindTarget();                               // поиск цели
            }
        }

        // ≈сли нашли цель делаем рейкасты и мер€ем дистанцию
        if (botAI.target)                                     // если нашли цель
        {
            botAI.NavMeshRayCast(botAI.target);               // делаем рейкаст
            distanceToTarget = Vector3.Distance(botAI.target.transform.position, botAI.transform.position);   // считаем дистанцию 
        }

        // Ћогика если нашли цель и она видима        
        if (botAI.targetVisible)       // если дистанци€ до игрока < тригер дистанции       (distanceToTarget < botAI.triggerLenght &&)
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
            if (withSpell)
            {
                if (Time.time - lastSpell > cooldownSpell && !botAI.nowAttacking)
                {
                    lastSpell = Time.time;              // присваиваем врем€ атаки
                    // Spell                    
                    int randomSpell = Random.Range(0, 101);

                    if (randomSpell < 40)
                        iSpell = 1;
                    if (randomSpell >= 40 && randomSpell < 80)
                        iSpell = 2;
                    if ( randomSpell >= 80)
                        iSpell = 3;

                    botAI.botAIMeleeWeaponHolder.currentWeapon.AttackNoCd(iSpell);
                }
                else
                {
                    botAI.Chase();                      // преследуем и аттакуем
                }
            }
            else
            {
                botAI.Chase();                          // преследуем и аттакуем
            }            
        }



        if (sayTriggerText && !isFindTarget)
        {
            botAI.SayText(textTrigger);
            sayTriggerText = false;
        }


/*
        if(debug)
        {
            Debug.Log(botAI.target);
            Debug.Log(isFindTarget);
            //Debug.Log(botAI.chasing);
            //Debug.Log(botAI.readyToAttack);
        }*/


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
                botAI.readyToAttack = true;                                                           // готов стрел€ть
            }
        }
        else
        {
            botAI.agent.SetDestination(botAI.target.transform.position);                              // перемещаемс€ к цели
            if (botAI.readyToAttack)
                botAI.readyToAttack = false;                                                            // не готов стрел€ть                
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
            botAI.SayText("я за тобой");
        }
        if (letsGo)
        {
            botAI.stayOnGround = true;
            botAI.followPlayer = false;
            botAI.SayText("’орошо, € подожду здесь");
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
            botAI.SetDestination(destination);                      // идЄм в случайную позицию
        }

        float distanceFromStart = Vector3.Distance(botAI.startPosition, botAI.transform.position);  // считаем дистанцию от стартовой позиции до следующей позиции

        if (distanceFromStart > maxDistancePatrol)
            botAI.SetDestination(botAI.startPosition);      // возвращаемс€ в стартовую позицию        
    }

    void MakeFriendly()
    {        
        botAI.gameObject.layer = LayerMask.NameToLayer("NPC");  // слой самого бота
        botAI.layerHit = LayerMask.GetMask("Enemy", "ObjectsDestroyble", "Default");    // слой дл€ оружи€
        
/*        foreach (GameObject weaponGO in botAI.botAIWeaponHolder.weapons)                // слой дл€ каждого оружи€ у бота
        {
            var weapon = weaponGO.GetComponent<BotAIWeapon>();
            weapon.layerHit = botAI.layerHit;
        }  */      
       
    }
}
