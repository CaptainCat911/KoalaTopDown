using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyThinker : MonoBehaviour
{
    [HideInInspector] public Enemy botAI;
    public Brain[] brains;

    [HideInInspector] public GameObject target;     // ����
    bool isFindTarget;
    float distanceToTarget;                         // ��������� �� ����


    public bool isFriendly;
    public GameObject[] patrolPoints;
    [HideInInspector] public int i = 0;
    public bool go;

    // ����� ����
    float hitBoxRadius = 5f;                             // ������ �����                                                               
    float lastAttack;                               // ����� ���������� ����� (��� ����������� �����)
    float cooldown = 0.5f;                          // ����������� �����
    LayerMask layerTarget;

    private void Awake()
    {        
        botAI = GetComponent<Enemy>();
        layerTarget = LayerMask.GetMask("Player", "NPC");
    }

    private void Start()
    {
        
    }

    /// <summary>
    /// brains[0].Think(this); - ��������� ����� ��� ���� (������ �� �����, ������� � �.�.)
    /// 
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

        Debug.Log(target);
        //Debug.Log(isFindTarget);
        //Debug.Log(botAI.chasing);


        if (isFriendly)
        {
            MakeFriendly();
            isFriendly = false;
        }
    }


    void FindTarget()
    {
        if (!target && Time.time - lastAttack > cooldown)                           // ���� ������ ��������� � �� ������
        {
            lastAttack = Time.time;                                                 // ����������� ����� �����
            Collider2D[] collidersHitbox = Physics2D.OverlapCircleAll(transform.position, hitBoxRadius, layerTarget);    // ������� ���� � ������� ������� � ��������
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
        botAI.hitBox.layer = LayerMask.GetMask("Enemy", "ObjectsDestroyble", "Default");
        botAI.gameObject.layer = LayerMask.NameToLayer("NPC");
        botAI.spriteRenderer.color = Color.yellow;
        brains[1].Think(this);              // ����� ����
    }
}
