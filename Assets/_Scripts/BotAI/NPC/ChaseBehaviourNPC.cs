using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCChaseBehaviour : StateMachineBehaviour
{
    NPC boss;                              // ������ �� ����
    public float cooldownAttack = 2f;       // ����������� �����   
    float lastAttack;                       // ����� ���������� �������  
    float randomCooldown = 1f;              // ����������� �������   
    float lastRandom;                       // ����� ���������� �������
    int attackNumber;                       // ��� �����

    // ����� ����
    float lastTargetChange;                 // ����� ���������� ������ ����
    float cooldownChange = 4f;              // ����������� ������ ����


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = animator.GetComponent<NPC>();       // ��������
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // ���� ��� ���� - ������������ � ����
        if (!boss.target)
        {
            animator.SetTrigger("Idle");            // �������
            boss.chasing = false;                   // ��������� �������������
            return;
        }

        boss.Chase();                               // ���������� ����
        boss.NavMeshRayCast(boss.target);           // ������ �������

        // ������ ������� ����
        if (Time.time - lastTargetChange > cooldownChange && Time.time - lastAttack > cooldownAttack)      // ���� �� ������
        {
            lastTargetChange = Time.time;
            boss.FindTarget();                                  // ����� ����
        }

        // ���������� ���� �����
        if (Time.time - lastRandom > randomCooldown)        // ���� ������ ��������� � �� ������
        {
            lastRandom = Time.time;                         // ����������� ����� �����
            
            if (boss.distanceToTarget > 3)
            {
                int random = Random.Range(1, 7);
                if (random < 6)
                {
                    boss.distanceToAttack = 6;
                    attackNumber = 2;               // ���� �����
                }
                if (random == 6)
                {
                    boss.distanceToAttack = 20;
                    attackNumber = 3;               // �����
                }
            }
            else
            {
                int random = Random.Range(1, 4);
                if (random < 2)
                {
                    boss.distanceToAttack = 20;
                    attackNumber = 4;               // �����
                }
                if (random == 2)
                {
                    boss.distanceToAttack = 6;      
                    attackNumber = 2;               // ���� �����
                }
                if (random == 3)
                {
                    boss.distanceToAttack = 20;
                    attackNumber = 3;               // �����
                }
            }
        }

        // ���� �� ������ ��������� - ������������
        if (!boss.readyToAttack)
            return;

        // ���� �� ������ - �������
        if (Time.time - lastAttack > cooldownAttack)              // ���� ������ ��������� � �� ������
        {
            lastAttack = Time.time;                         // ����������� ����� �����

            if (attackNumber == 2)
            {
                animator.SetTrigger("AttackRange");
            }
            if (attackNumber == 3)
            {
                animator.SetTrigger("AttackSpawn");
            }
            if (attackNumber == 4)
            {
                animator.SetTrigger("AttackExplousion");
            }
        }
    }

    void Attack(int number)
    {

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}
}
