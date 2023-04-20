using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleBehaviour : StateMachineBehaviour
{

    Boss boss;                       // ссылка на бота

    // Поиск цели
    float lastTargetFind;                   // время последнего поиска цели
    float cooldownFind = 0.5f;              // перезардяка поиска цели
    //float cooldownChangeTarget = 2f;        // перезарядка смена цели


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = animator.GetComponent<Boss>();       // ПОМЕНЯТЬ
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!boss.isAlive || boss.isNeutral)
        {
            return;
        }

        if (boss.currentHealth < boss.maxHealth / 3)
        {
            animator.SetTrigger("FindTarget");              // триггер
            return;
        }

        // Если нет цели
        if (!boss.target)
        {
            // если поведение - охранять стартовую позицию
            if (boss.stayOnGround)
            {
                float distance = Vector2.Distance(boss.transform.position, boss.startPosition);         // считаем дистанцию до стартовой позиции
                if (distance > 0.1f)
                {
                    animator.SetTrigger("Run");            // выходим в идле
                }
                else
                {
/*                    boss.agent.ResetPath();
                    if (boss.friendTarget)
                        boss.LookAt(boss.friendTarget);*/
                }
            }
        }

        if (boss.target)                                    // если есть цель
        {
            animator.SetTrigger("FindTarget");              // триггер
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
