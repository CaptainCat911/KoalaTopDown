using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningBehNPC : StateMachineBehaviour
{
    NPC boss;                       // ������ �� ����

    // ����� ����
    float lastTargetFind;                   // ����� ���������� ������ ����
    float cooldownFind = 0.1f;              // ����������� ������ ����

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
        if (!boss.isAlive || boss.isNeutral)        
        {
            animator.SetTrigger("Idle");            // �������
            boss.chasing = false;                   // ��������� �������������
            return;
        }

        // ���� ��� ����
        if (!boss.target)           
        {
            // ���� ��������� - �������� ��������� �������
            if (boss.stayOnGround)
            {
                float distance = Vector2.Distance(boss.transform.position, boss.startPosition);         // ������� ��������� �� ��������� �������
                if (distance > 0.1f)
                {
                    boss.SetDestination(boss.startPosition);    // ��� � ��������� �������
                }
                else
                {
                    animator.SetTrigger("Idle");            // ������� � ����
                }
            }

            // ���� ��������� - ��������� � ����� ����������
            if (boss.goTo)
            {
                float distance = Vector2.Distance(boss.transform.position, boss.destinationPoint.position);         // ������� ��������� �� ��������� �������
                if (distance > 0.1f)
                {
                    boss.SetDestination(boss.destinationPoint.position);    // ��� � ��������� �������
                }
                // ���� �������� �� ����� ����������
                else
                {
                    boss.startPosition = boss.destinationPoint.position;    // ������������ ��������� ������� ��������� �����, �� ������� ��������
                    boss.goTo = false;              // ���� ����������
                    boss.stayOnGround = true;       // ����� �� �����
                    animator.SetTrigger("Idle");    // ������� � ����
                }
                
            }

            // ���� ����, ���� ����� � ��������� ������� ��� ����� ����������
            if (Time.time - lastTargetFind > cooldownFind)      // ���� �� ������
            {
                lastTargetFind = Time.time;
                boss.FindTarget();                              // ����� ����
            }
        }

        // ���� ����� ����
        if (boss.target)
        {
            boss.Chase();                                   // ���������� ����
            boss.chasing = true;
            if (boss.closeToTarget)                         // ���� �������� �� ����
            {
                animator.SetTrigger("ReadyAttack");         // ������� � ����������
                return;
            }
        }



        // ������ ������� ����
        if (Time.time - lastTargetChange > cooldownChange)      // ���� �� ������
        {
            lastTargetChange = Time.time;
            boss.FindTarget();                                  // ����� ����
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}
}
