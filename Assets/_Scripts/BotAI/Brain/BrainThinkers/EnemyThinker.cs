using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyThinker : MonoBehaviour
{
    [HideInInspector] public BotAI botAI;
    public Brain[] brains;

    [HideInInspector] public GameObject target;     // ����
    bool isFindTarget;
    float distanceToTarget;                         // ��������� �� ����


    public bool isFriendly;
    public GameObject[] patrolPoints;
    [HideInInspector] public int i = 0;    

    // ����� ����
    //public float targetFindRadius = 5f;                 // ������ ������ ����                                                               
    float lastTargetFind;                               // ����� ���������� ����� (��� ����������� �����)
    float cooldownFind = 0.5f;                          // ����������� �����
    LayerMask layerTarget;                              // ���� ��� ������ ����

    private void Awake()
    {        
        botAI = GetComponent<BotAI>();
        layerTarget = LayerMask.GetMask("Player", "NPC");
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
        if (botAI.isNeutral)                        // ���� ��� ���������
            return;

        // ���������� ������ �����, ���� ���� ���
        if (isFindTarget && !target)
        {
            isFindTarget = false;
            botAI.chasing = false;                  // ������������� ���������
            //botAI.target = null;
            botAI.targetVisible = false;
            botAI.readyToAttack = false;
            botAI.agent.ResetPath();
        }


        // ������ ��� ������ ���� � �������������� 
        if (!isFindTarget)                              // ���� ��� ���� 
        {            
            brains[0].Think(this);                      // ��������������                     
            FindTarget();                               // ����� ����  
        }

        // ���� ����� ���� ������ �������� � ������ ���������
        if (target)                                     // ���� ����� ����
        {
            botAI.NavMeshRayCast(target);               // ������ �������
            distanceToTarget = Vector3.Distance(target.transform.position, botAI.transform.position);   // ������� ��������� 
        }


        // ������ ���� ����� ���� � ��� ������        
        if (distanceToTarget < botAI.triggerLenght && botAI.targetVisible)       // ���� ��������� �� ������ < ������ ���������
        {
            if (!botAI.chasing)
            {
                botAI.chasing = true;                   // ������������� ��������
                isFindTarget = true;
            }
        }   

        if (botAI.chasing && target)
        {
            brains[1].Think(this);                      // ���������� � ��������           
        }

        //Debug.Log(target);
        //Debug.Log(isFindTarget);
        //Debug.Log(botAI.chasing);
        //Debug.Log(botAI.readyToAttack);


        if (isFriendly)
        {
            MakeFriendly();
            isFriendly = false;
        }
    }


    void FindTarget()
    {
        if (!target && Time.time - lastTargetFind > cooldownFind)                       // ���� ������ ��������� � �� ������
        {
            lastTargetFind = Time.time;                                                 // ����������� ����� �����
            Collider2D[] collidersHitbox = Physics2D.OverlapCircleAll(transform.position, botAI.triggerLenght, layerTarget);    // ������� ���� � ������� ������� � �������� 
            foreach (Collider2D enObjectBox in collidersHitbox)
            {
                if (enObjectBox == null)
                {
                    continue;
                }

                if (enObjectBox.gameObject.TryGetComponent(out Fighter fighter))        // ���� ������ ������
                {
                    NavMeshHit hit;
                    if (!botAI.agent.Raycast(fighter.transform.position, out hit))      // ������ �������
                    {
                        target = fighter.gameObject;
                        //botAI.target = target;
                    }
                }
                collidersHitbox = null;                                                 // ���������� ��� ��������� ������� (�� ����� ���� ��������� ��� ��� ��������)
            }
        }
    }

    void MakeFriendly()
    {
        layerTarget = LayerMask.GetMask("Enemy");               // ���� ������ ����
        botAI.gameObject.layer = LayerMask.NameToLayer("NPC");  // ���� ������ ����
        botAI.layerHit = LayerMask.GetMask("Enemy", "ObjectsDestroyble", "Default");    // ���� ��� ������
        foreach (GameObject weaponGO in botAI.botAIWeaponHolder.weapons)                // ���� ��� ������� ������ � ����
        {
            var weapon = weaponGO.GetComponent<BotAIWeapon>();
            weapon.layerHit = botAI.layerHit;
        }        
        //botAI.hitBox.layer = LayerMask.GetMask("Enemy", "ObjectsDestroyble", "Default");
        //botAI.spriteRenderer.color = Color.yellow;        
    }
}
