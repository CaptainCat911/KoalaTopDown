using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseBehaviour : StateMachineBehaviour
{
    Boss boss;                              // ������ �� ����

    //public float cooldownAttack;       // ����������� �����
    //float randomCooldown = 1f;              // ����������� �������    
    //float lastRandom;                       // ����� ���������� �������
    float lastAttack;                       // ����� ���������� �������  
    int attackNumber;                       // ��� �����
    bool attackReady;                       // ����� ������

    // ����� ����
    float lastTargetChange;                 // ����� ���������� ������ ����
    float cooldownChange = 4f;              // ����������� ������ ����


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = animator.GetComponent<Boss>();       // ��������
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // ���� ��� ���� - ������������ � ����
        if (!boss.target || !boss.isAlive)
        {
            animator.SetTrigger("Idle");            // �������
            boss.chasing = false;                   // ��������� �������������
            return;
        }

        // ���� ������ �������
        if (boss.attackingNow)
        {
            boss.agent.ResetPath();
            return;
        }

        if (boss.currentHealth < boss.maxHealth / 4)
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
            boss.FindTarget();                              // ����� ����
        }

        // ���������� ���� �����
        if (!attackReady && Time.time - lastAttack > boss.cooldownAttack)        
        {
            // ������� ���������
            if (boss.distanceToTarget > 3)
            {
                if (ProbabilityCheck(boss.rangeAttackChance))    // ���� �� ���� �����
                {
                    boss.distanceToAttack = 6;
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
                    boss.distanceToAttack = 3;
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

           attackReady = true;                 // ������ ���������
        }

        // ���� �� ������ ��������� - ������������
        if (!boss.closeToTarget || !attackReady)
            return;

        // ���� �� ������ - �������
        if (Time.time - lastAttack > boss.cooldownAttack)        // ���� ������ ��������� � �� ������
        {
            lastAttack = Time.time;                         // ����������� ����� �����

            animator.SetFloat("AttackType", attackNumber);
            animator.SetTrigger("Attack");

            attackReady = false;
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

/*    void Attack(int number)
    {

    }*/

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}
}
