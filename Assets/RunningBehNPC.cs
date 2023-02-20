using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningBehNPC : StateMachineBehaviour
{
    NPC boss;                       // ������ �� ����

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = animator.GetComponent<NPC>();       // ��������
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // ���� ��� ���� - ������������ � ����
        if (!boss.target)                           // ���� ���� ������� - 
        {
            animator.SetBool("Running", false);     // �������
            boss.chasing = false;                   // ��������� �������������
            return;
        }

        boss.Chase();                               // ���������� ����

        if (boss.closeToTarget)                     // ���� �������� �� ����
        {
            animator.SetBool("Running", false);     // �������
            return;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}
}
