using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningBehNPC : StateMachineBehaviour
{
    NPC boss;                       // ссылка на бота

    // ѕоиск цели
    float lastTargetFind;                   // врем€ последнего поиска цели
    float cooldownFind = 0.1f;              // перезард€ка поиска цели

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
        if (!boss.isAlive || boss.isNeutral)        
        {
            animator.SetTrigger("Idle");            // выходим
            boss.chasing = false;                   // отключаем преследование
            return;
        }

        // ≈сли нет цели
        if (!boss.target)           
        {
            // если поведение - охран€ть стартовую позицию
            if (boss.stayOnGround)
            {
                float distance = Vector2.Distance(boss.transform.position, boss.startPosition);         // считаем дистанцию до стартовой позиции
                if (distance > 0.1f)
                {
                    boss.SetDestination(boss.startPosition);    // идЄм к стартовой позиции
                }
                else
                {
                    animator.SetTrigger("Idle");            // выходим в идле
                }
            }

            // если поведение - двигатьс€ к точке назначени€
            if (boss.goTo)
            {
                float distance = Vector2.Distance(boss.transform.position, boss.destinationPoint.position);         // считаем дистанцию до стартовой позиции
                if (distance > 0.1f)
                {
                    boss.SetDestination(boss.destinationPoint.position);    // идЄм к стартовой позиции
                }
                // если добежали до точки назначени€
                else
                {
                    boss.startPosition = boss.destinationPoint.position;    // присваивааем стартовой позиции положение точки, до которой добежали
                    boss.goTo = false;              // го“у сбрасываем
                    boss.stayOnGround = true;       // стоим на земле
                    animator.SetTrigger("Idle");    // выходим в идле
                }
                
            }

            // »щем цель, пока бежим к стартовой позиции или точке назначени€
            if (Time.time - lastTargetFind > cooldownFind)      // если кд готово
            {
                lastTargetFind = Time.time;
                boss.FindTarget();                              // поиск цели
            }
        }

        // ≈сли нашли цель
        if (boss.target)
        {
            boss.Chase();                                   // преследуем цель
            boss.chasing = true;
            if (boss.closeToTarget)                         // если добежали до цели
            {
                animator.SetTrigger("ReadyAttack");         // выходим в реди“ујтак
                return;
            }
        }



        // »ногда смен€ем цель
        if (Time.time - lastTargetChange > cooldownChange)      // если кд готово
        {
            lastTargetChange = Time.time;
            boss.FindTarget();                                  // поиск цели
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}
}
