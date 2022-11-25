using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyThinker : MonoBehaviour
{
    [HideInInspector] public Enemy botAI;
    public Brain[] brains;

    [HideInInspector] public GameObject target;     // ����
    float distanceToTarget;                         // ��������� �� ����


    public bool isFriendly;
    public GameObject[] patrolPoints;
    int i = 0;
    public bool go;

    private void Awake()
    {        
        botAI = GetComponent<Enemy>();
    }

    private void FixedUpdate()
    {
        if (botAI.isNeutral)                    // ���� ��� ���������
            return;

        // ��� ������ ��� ������ ���� � ��������������




        

        if (!target && !botAI.agent.hasPath && go)                            // ���� ��� ���� � ��� ����
        {
            //brains[0].Think(this);              // ����� ����

                      
            botAI.SetDestination(patrolPoints[i].transform.position);
            i++;
            Debug.Log("������� !");
            return;
        }

        if (target)                             // ���� ����� ����
        {
            botAI.NavMeshRayCast(target);       // ������ �������
            distanceToTarget = Vector3.Distance(target.transform.position, botAI.transform.position);   // ������� ���������
        }      

        // ��� ������ ���� ����� ���� � ��� ������

        // ������������� 
        if (!botAI.chasing && distanceToTarget < botAI.triggerLenght && botAI.targetVisible)       // ���� ��������� �� ������ < ������ ���������
        {
            botAI.chasing = true;                   // ������������� ��������                                              
        }   
        if (botAI.chasing)
        {
            botAI.Chase(target);                    // ����������
        }

        if (isFriendly)
        {
            MakeFriendly();
            isFriendly = false;
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
