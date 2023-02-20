using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleBehaviourNPC : StateMachineBehaviour
{

    NPC boss;                       // ссылка на бота

    // ѕоиск цели
    float lastTargetFind;                   // врем€ последнего поиска цели
    float cooldownFind = 0.5f;              // перезард€ка поиска цели
    //float cooldownChangeTarget = 2f;        // перезар€дка смена цели


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = animator.GetComponent<NPC>();       // ѕќћ≈Ќя“№
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (boss.target)                                    // если есть цель
        {
            animator.SetTrigger("ChaseTarget");              // триггер
            boss.chasing = true;                            // преследование включено
        }

        if (Time.time - lastTargetFind > cooldownFind)      // если кд готово
        {
            lastTargetFind = Time.time;
            boss.FindTarget();                              // поиск цели
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
