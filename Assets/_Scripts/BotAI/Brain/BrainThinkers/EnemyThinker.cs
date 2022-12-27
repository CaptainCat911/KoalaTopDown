using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.AI;

public class EnemyThinker : MonoBehaviour
{
    [HideInInspector] public BotAI botAI;
    public Brain[] brains;

    [HideInInspector] public GameObject target;     // цель
    bool isFindTarget;                              // нашли цель
    float distanceToTarget;                         // дистанци€ до цели

    public Transform[] positionsPoints;
    [HideInInspector] public int i = 0;    
    public bool nextPosition;
    public bool letsGo;

    // ѕоиск цели
    //public float targetFindRadius = 5f;                 // радиус поиска цели                                                               
    float lastTargetFind;                               // врем€ последнего удара (дл€ перезар€дки удара)
    float cooldownFind = 0.5f;                          // перезард€ка поиска цели
    float cooldownChangeTarget = 2f;                    // перезар€дка смена цели

    bool type_1;        // тип оружи€ мили
    bool type_2;        // тип оружи€ ренж

    public string textTrigger;
    bool sayTriggerText;
    
    [Header("ѕоведение")]
    public bool patrolingRandomPosition;


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
        if (botAI.isNeutral)                        
            return;

        // —брасываем вс€кие штуки, если цели нет
        if (isFindTarget && !target)
        {
            isFindTarget = false;
            botAI.chasing = false;                      // преследование отключено            
            botAI.targetVisible = false;
            botAI.readyToAttack = false;
            botAI.agent.ResetPath();
        }
        

        // Ћогика дл€ поиска цели и патрулировани€ 
        if (!isFindTarget)                                  // если нет цели 
        {
            if (Time.time - lastTargetFind > cooldownFind)  // если кд готово
            {
                FindTarget();                               // поиск цели
            }

            if (letsGo)
            {
                brains[0].Think(this);                          // патрулирование 
            }
            if (!letsGo)
            {
                patrolingRandomPosition = true;                 // патрулирование            
            }
        }
        else
        {
            if (Time.time - lastTargetFind > Random.Range(cooldownChangeTarget, cooldownChangeTarget + 2f))  // смен€ем цель, если больше одной цели
            {
                FindTarget();                               // поиск цели
            }
        }

        // ≈сли нашли цель делаем рейкасты и мер€ем дистанцию
        if (target)                                     // если нашли цель
        {
            botAI.NavMeshRayCast(target);               // делаем рейкаст
            distanceToTarget = Vector3.Distance(target.transform.position, botAI.transform.position);   // считаем дистанцию 
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
                    Debug.Log("Melee!");
                }
                if (distanceToTarget > botAI.triggerLenght - 1 && !type_2)
                {
                    botAI.SwitchAttackType(2);
                    type_1 = false;
                    type_2 = true;
                    Debug.Log("Range!");
                }
            }              

            if (!botAI.chasing)
            {
                patrolingRandomPosition = false;        // патрулирование 
                botAI.chasing = true;                   // преследование включено
                isFindTarget = true;
            }
        }   

        if (botAI.chasing && target)
        {
            brains[1].Think(this);                      // преследуем и аттакуем           
        }



        if (sayTriggerText && !isFindTarget)
        {
            botAI.SayText(textTrigger);
            sayTriggerText = false;
        }



        if(debug)
        {
            Debug.Log(target);
            //Debug.Log(isFindTarget);
            //Debug.Log(botAI.chasing);
            //Debug.Log(botAI.readyToAttack);
        }


/*        if (isFriendly)
        {
            MakeFriendly();
            isFriendly = false;
        }*/
    }


    void FindTarget()
    {
        lastTargetFind = Time.time;                                                 // присваиваем врем€ атаки
        Collider2D[] collidersHitbox = Physics2D.OverlapCircleAll(transform.position, botAI.triggerLenght, botAI.layerTarget);    // создаем круг в позиции объекта с радиусом 
        List<GameObject> targets = new List<GameObject>(); 
        foreach (Collider2D enObjectBox in collidersHitbox)
        {
            if (enObjectBox == null)
            {
                continue;
            }

            if (enObjectBox.gameObject.TryGetComponent(out Fighter fighter))        // ищем скрипт файтер
            {
                botAI.NavMeshRayCast(fighter.gameObject);
                float distance = Vector3.Distance(fighter.transform.position, botAI.transform.position);   // считаем дистанцию 
                if (!target)
                {
                    if (botAI.targetVisible)
                    {
                        targets.Add(fighter.gameObject);
                    }
                }
                else
                {
                    if (botAI.targetVisible && distance < 3)
                    {
                        targets.Add(fighter.gameObject);                            
                    }
                }       
            }
            collidersHitbox = null;                         // сбрасываем все найденные объекты (на самом деле непон€тно как это работает)
        }
        if (targets.Count > 0)
            target = targets[Random.Range(0, targets.Count)];            
        
    }

    public void GoToPosition(int positionNumber)
    {
        /*        if (nextPosition && positionsPoints.Length > 0)
                {

                }*/
        i = positionNumber;
    }

    public void LetsGo(int go)
    {
        if (go == 0)
            letsGo = false;
        if (go == 1)
            letsGo = true;
    }

    public void StayGo()
    {
        letsGo = !letsGo;
        if (letsGo)
        {
            botAI.SayText("я за тобой");
        }
        if (!letsGo)
        {
            botAI.SayText("∆ду здесь");
        }
    }


    public void TriggerSayText(string text)
    {
        textTrigger = text;
        sayTriggerText = true;
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
