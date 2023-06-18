using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseBehaviourNPC : StateMachineBehaviour
{
    NPC boss;                               // ������ �� ����
    
    float lastAttack;                       // ����� ���������� �������    
    int attackNumber;                       // ��� �����
    //bool attackReady;                       // ����� ������

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
        if (!boss.target || boss.isNeutral || !boss.isAlive)
        {
            boss.chasing = false;                   // ��������� �������������
            animator.SetTrigger("Run");             // �������
            return;
        }

        // ���� ������ �������
        if (boss.attackingNow)
        {
            boss.agent.ResetPath();
            return;
        }

        // ��� ����������
        if (boss.lowHp && boss.mainBoss)
        {
            //boss.SayText("���� ������� �� ��������");
            boss.distanceToAttack = 30;
            attackNumber = 7;
            animator.SetFloat("AttackType", attackNumber);
            animator.SetTrigger("Attack");
            return;
        }

        boss.Chase();                               // ���������� ����
        boss.NavMeshRayCast(boss.target);           // ������ �������

        // ������ ������� ����
        if (Time.time - lastTargetChange > cooldownChange && Time.time - lastAttack > boss.cooldownAttack)      // ���� �� ������
        {
            lastTargetChange = Time.time;
            boss.FindTarget();                                  // ����� ����
        }




        // ���� �� ������ � ���� - �����
        if (!boss.targetInRange)
        {
            animator.SetTrigger("Run");
            return;
        }

        if (Time.time - lastAttack <= boss.cooldownAttack)
        {
            return;
        }
        // ���������� ���� �����
        else
        {
            // ������� ���������
            if (boss.distanceToTarget > 3)
            {
                if (ProbabilityCheck(boss.rangeAttackChance))    // ���� �� ���� �����
                {
                    boss.distanceToAttack = boss.rangeAttackDistance;
                    attackNumber = 3;               // ���� �����
                }
                if (ProbabilityCheck(boss.multiAttackChance))
                {
                    boss.distanceToAttack = 20;
                    attackNumber = 5;               // ���������� �����
                }
            }
            // ������� ���������
            else
            {
                if (ProbabilityCheck(boss.meleeAttackChance))
                {
                    boss.distanceToAttack = boss.meleeAttackDistance;
                    attackNumber = 1;               // ����
                }
                if (ProbabilityCheck(boss.explousionAttackChance))
                {
                    boss.distanceToAttack = 3;
                    attackNumber = 2;               // �����
                }
            }

            // ��� ����� ��� ������� �� ����� ���������
            if (ProbabilityCheck(boss.spawnAttackChance))
            {
                boss.distanceToAttack = 20;
                attackNumber = 4;               // �����
            }
            if (ProbabilityCheck(boss.gravityChance))    // ���� �� ������� �����
            {
                boss.distanceToAttack = 10;
                attackNumber = 8;               // ������� �����
            }
            if (ProbabilityCheck(boss.laserChance))
            {
                boss.distanceToAttack = 20;
                attackNumber = 6;               // ����� �����
            }
            if (ProbabilityCheck(boss.teleportChance))
            {
                boss.distanceToAttack = 30;
                attackNumber = 9;               // ��������
            }


            lastAttack = Time.time;                         // ����������� ����� �����
            animator.SetFloat("AttackType", attackNumber);
            animator.SetTrigger("Attack");

            boss.distanceToAttack = boss.defaultRangeToTarget;

            //attackReady = true;                 // ������ ���������
        }
    }

    bool ProbabilityCheck(int chance)
    {
        float random = Random.Range(0, 101);
        if (random < chance)
            return true;
        else
            return false;
    }
}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

