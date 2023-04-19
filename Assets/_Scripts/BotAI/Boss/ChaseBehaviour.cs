using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseBehaviour : StateMachineBehaviour
{
    Boss boss;                              // ������ �� ����
    public float cooldownAttack = 2f;       // ����������� �����

    [Header("����� ��������")]
    public int rangeAttackChance;           // ���� ���� �����
    public int multiAttackChance;           // .. �����������

    [Header("����� ������")]
    public int meleeAttackChance;
    public int explousionAttackChance;

    [Header("����� �����")]
    public int spawnAttackChance;           // .. ������
    public int gravityChance;               // .. ����������
    public int laserChance;                 // .. ������
    public int teleportChance;              // .. ���������

    float lastAttack;                       // ����� ���������� �������  
    //float randomCooldown = 1f;              // ����������� �������    
    //float lastRandom;                       // ����� ���������� �������
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

        if (boss.currentHealth < boss.maxHealth / 3)
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
        if (Time.time - lastTargetChange > cooldownChange && Time.time - lastAttack > cooldownAttack)      // ���� �� ������
        {
            lastTargetChange = Time.time;
            boss.FindTarget();                              // ����� ����
        }

        // ���������� ���� �����
        if (!attackReady && Time.time - lastAttack > cooldownAttack)        
        {
            // ������� ���������
            if (boss.distanceToTarget > 3)
            {
                if (ProbabilityCheck(rangeAttackChance))    // ���� �� ���� �����
                {
                    boss.distanceToAttack = 6;
                    attackNumber = 3;               // ���� �����
                }
                if (ProbabilityCheck(multiAttackChance))
                {
                    boss.distanceToAttack = 20;
                    attackNumber = 5;               // ���������� �����
                }
            }
            // ������� ���������
            else
            {
                if (ProbabilityCheck(meleeAttackChance))
                {
                    boss.distanceToAttack = 3;
                    attackNumber = 1;               // ����
                }
                if (ProbabilityCheck(explousionAttackChance))
                {
                    boss.distanceToAttack = 3;     
                    attackNumber = 2;               // �����
                }
            }

            // ��� ����� ��� ������� �� ����� ���������
            if (ProbabilityCheck(spawnAttackChance))
            {
                boss.distanceToAttack = 20;
                attackNumber = 4;               // �����
            }
            if (ProbabilityCheck(gravityChance))    // ���� �� ������� �����
            {
                boss.distanceToAttack = 10;
                attackNumber = 8;               // ������� �����
            }
            if (ProbabilityCheck(laserChance))
            {
                boss.distanceToAttack = 20;
                attackNumber = 6;               // ����� �����
            }
            if (ProbabilityCheck(teleportChance))
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
        if (Time.time - lastAttack > cooldownAttack)        // ���� ������ ��������� � �� ������
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
        if (random <= chance)
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
