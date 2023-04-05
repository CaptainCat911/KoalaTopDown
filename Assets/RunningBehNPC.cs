using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningBehNPC : StateMachineBehaviour
{
    NPC boss;                       // ссылка на бота

    // —мена цели
    float lastTargetChange;                 // врем€ последнего поиска цели
    float cooldownChange = 4f;              // перезард€ка поиска цели

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = animator.GetComponent<NPC>();       // ѕќћ≈Ќя“№
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // ≈сли нет цели - возвращаемс€ в идле
        if (!boss.target || !boss.isAlive)          // если цель исчезла
        {
            animator.SetBool("Running", false);     // выходим
            boss.chasing = false;                   // отключаем преследование
            return;
        }

        boss.Chase();                               // преследуем цель

        if (boss.closeToTarget)                     // если добежали до цели
        {
            animator.SetBool("Running", false);     // выходим
            return;
        }

/*        // »ногда смен€ем цель
        if (Time.time - lastTargetChange > cooldownChange)      // если кд готово
        {
            lastTargetChange = Time.time;
            boss.FindTarget();                                  // поиск цели
        }*/
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}
}
