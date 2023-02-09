using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseBehaviour : StateMachineBehaviour
{
    Boss boss;                              // ссылка на бота
    public float randomCooldown = 2f;       // перезар€дка рандома   
    float lastRandom;                       // врем€ последнего рандома
    public float сooldown = 2f;       // перезар€дка рандома   
    float lastAttack;                       // врем€ последнего рандома  

    int attackNumber;                       // тип атаки


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = animator.GetComponent<Boss>();       // ѕќћ≈Ќя“№
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!boss.target)
        {
            animator.SetTrigger("Idle");            // триггер
            boss.chasing = false;                   // отключаем преследование
            return;
        }

        boss.Chase();                               // преследуем цель



        if (Time.time - lastRandom > randomCooldown)        // если готовы атаковать и кд готово
        {
            lastRandom = Time.time;                         // присваиваем врем€ атаки
            
            if (boss.distanceToTarget > 3)
            {
                int random = Random.Range(1, 4);
                if (random < 3)
                {
                    boss.distanceToAttack = 4;
                    attackNumber = 2;
                }
                if (random == 3)
                {
                    boss.distanceToAttack = 20;
                    attackNumber = 3;
                }
            }
            else
            {
                int random = Random.Range(1, 4);
                if (random < 3)
                {
                    boss.distanceToAttack = 3;
                    attackNumber = 4;
                }
                if (random == 3)
                {
                    boss.distanceToAttack = 4;
                    attackNumber = 2;
                }
            }

        }

        if (!boss.readyToAttack)
            return;

        if (Time.time - lastAttack > сooldown)              // если готовы атаковать и кд готово
        {
            lastAttack = Time.time;                         // присваиваем врем€ атаки


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


        /*            if (!switchType)
                    {
                        boss.botAIMeleeWeaponHolder.currentWeapon.attackClass = "2";
                        switchType = true;
                    }
                    if (switchType)
                    {
                        boss.botAIMeleeWeaponHolder.currentWeapon.attackClass = "3";
                        switchType = false;
                    }*/



    }

    void Attack(int number)
    {

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
