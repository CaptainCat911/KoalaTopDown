using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.AI;

public class EnemyThinker : MonoBehaviour
{
    [HideInInspector] public BotAI botAI;               // ������ �� ����
    //public Brain[] brains;

    //[HideInInspector] public GameObject target;     // ����
    bool isFindTarget;                                  // ����� ����
    float distanceToTarget;                             // ��������� �� ����
    // ����� ����
    //public float targetFindRadius = 5f;                 // ������ ������ ����                                                               
    float lastTargetFind;                               // ����� ���������� ������ ����
    float cooldownFind = 0.5f;                          // ����������� ������ ����
    public float cooldownChangeTarget = 2f;             // ����������� ����� ����

    bool type_1;        // ��� ������ ����
    bool type_2;        // ��� ������ ����

    
    [Header("���������")]
    //public bool patrolingRandomPosition;
    public float cooldownChange;                        // ����������� ����� �������
    public float distancePatrol;                        // ��������� ��� ��������������
    public float maxDistancePatrol;                     // ������������ ��������� �� ��������� �������
    [HideInInspector] public float lastChange;          // ����� ��������� ����� �������

    // ��� ���
    [HideInInspector] public Transform[] positionsPoints;
    [HideInInspector] public int i = 0;
    [HideInInspector] public bool nextPosition;
    [HideInInspector] public bool letsGo;               // ���� �� �������
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
    /// brains[0].Think(this); - ��������� ����� ��� ���� (������ �� �����, ������� � �.�.)
    /// brains[1].Think(this); - ���������� � �������   
    /// </summary>



    private void FixedUpdate()
    {
        // ���� ��� ���������
        if (botAI.isNeutral || !botAI.isAlive)                        
            return;

        // ���������� ������ �����, ���� ���� ���
        if (isFindTarget && !botAI.target)
        {
            isFindTarget = false;
            botAI.ResetTarget();
        }
        

        // ������ ��� ������ ���� � �������������� 
        if (!isFindTarget)                                  // ���� ��� ���� 
        {
            if (Time.time - lastTargetFind > cooldownFind)  // ���� �� ������
            {
                lastTargetFind = Time.time;
                botAI.FindTarget();                         // ����� ����
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
                Patrol();                               // ��������������  
                //patrolingRandomPosition = true;                           
            }
        }
        else            // ������ ������ ����
        {
            if (Time.time - lastTargetFind > Random.Range(cooldownChangeTarget, cooldownChangeTarget + 2f))  // ������� ����, ���� ������ ����� ����
            {
                lastTargetFind = Time.time;
                botAI.FindTarget();                               // ����� ����
            }
        }

        // ���� ����� ���� ������ �������� � ������ ���������
        if (botAI.target)                                     // ���� ����� ����
        {
            botAI.NavMeshRayCast(botAI.target);               // ������ �������
            distanceToTarget = Vector3.Distance(botAI.target.transform.position, botAI.transform.position);   // ������� ��������� 
        }

        // ������ ���� ����� ���� � ��� ������        
        if (botAI.targetVisible)       // ���� ��������� �� ������ < ������ ���������       (distanceToTarget < botAI.triggerLenght &&)
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
                //patrolingRandomPosition = false;        // �������������� 
                botAI.chasing = true;                   // ������������� ��������
                isFindTarget = true;
            }
        }   

        if (botAI.chasing && botAI.target)
        {
            botAI.Chase();                      // ���������� � ��������           
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
        float distance = Vector3.Distance(botAI.transform.position, botAI.target.transform.position);       // ������� ��������� �� ����        
        if (botAI.targetVisible && distance < botAI.distanceToAttack)
        {
            if (!botAI.readyToAttack)
            {
                botAI.agent.ResetPath();                                                              // ���������� ����            
                botAI.readyToAttack = true;                                                           // ����� ��������
            }
        }
        else
        {
            botAI.agent.SetDestination(botAI.target.transform.position);                              // ������������ � ����
            if (botAI.readyToAttack)
                botAI.readyToAttack = false;                                                            // �� ����� ��������                
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
            botAI.SayText("� �� �����");
        }
        if (letsGo)
        {
            botAI.stayOnGround = true;
            botAI.followPlayer = false;
            botAI.SayText("������, � ������� �����");
        }
    }


    public void TriggerSayText(string text)
    {
        textTrigger = text;
        sayTriggerText = true;
    }

    void Patrol()
    {
        if (Time.time - lastChange > Random.Range(cooldownChange, cooldownChange + 2f))        // ���� �� ������
        {
            lastChange = Time.time;

            Vector3 destination = new(botAI.transform.position.x + Random.Range(-distancePatrol, distancePatrol),
                botAI.transform.position.y + Random.Range(-distancePatrol, distancePatrol), botAI.transform.position.z);    // �������� ��������� �������
            botAI.SetDestination(destination);                      // ��� � ��������� �������
        }

        float distanceFromStart = Vector3.Distance(botAI.startPosition, botAI.transform.position);  // ������� ��������� �� ��������� ������� �� ��������� �������

        if (distanceFromStart > maxDistancePatrol)
            botAI.SetDestination(botAI.startPosition);      // ������������ � ��������� �������        
    }

    void MakeFriendly()
    {        
        botAI.gameObject.layer = LayerMask.NameToLayer("NPC");  // ���� ������ ����
        botAI.layerHit = LayerMask.GetMask("Enemy", "ObjectsDestroyble", "Default");    // ���� ��� ������
        
/*        foreach (GameObject weaponGO in botAI.botAIWeaponHolder.weapons)                // ���� ��� ������� ������ � ����
        {
            var weapon = weaponGO.GetComponent<BotAIWeapon>();
            weapon.layerHit = botAI.layerHit;
        }  */      
       
    }
}
