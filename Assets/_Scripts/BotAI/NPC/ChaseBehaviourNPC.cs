using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCChaseBehaviour : StateMachineBehaviour
{
    NPC boss;                              // ссылка на бота
    public float cooldownAttack = 2f;       // перезарядка атаки   
    float lastAttack;                       // время последнего рандома  
    float randomCooldown = 1f;              // перезарядка рандома   
    float lastRandom;                       // время последнего рандома
    int attackNumber;                       // тип атаки

    // Смена цели
    float lastTargetChange;                 // время последнего поиска цели
    float cooldownChange = 4f;              // перезардяка поиска цели


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = animator.GetComponent<NPC>();       // ПОМЕНЯТЬ
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Если нет цели - возвращаемся в идле
        if (!boss.target)
        {
            animator.SetTrigger("Idle");            // триггер
            boss.chasing = false;                   // отключаем преследование
            return;
        }

        boss.Chase();                               // преследуем цель
        boss.NavMeshRayCast(boss.target);           // делаем рейкаст

        // Иногда сменяем цель
        if (Time.time - lastTargetChange > cooldownChange && Time.time - lastAttack > cooldownAttack)      // если кд готово
        {
            lastTargetChange = Time.time;
            boss.FindTarget();                                  // поиск цели
        }

        // Подготовка типа атаки
        if (Time.time - lastRandom > randomCooldown)        // если готовы атаковать и кд готово
        {
            lastRandom = Time.time;                         // присваиваем время атаки
            
            if (boss.distanceToTarget > 3)
            {
                int random = Random.Range(1, 7);
                if (random < 6)
                {
                    boss.distanceToAttack = 6;
                    attackNumber = 2;               // ренж атака
                }
                if (random == 6)
                {
                    boss.distanceToAttack = 20;
                    attackNumber = 3;               // спаун
                }
            }
            else
            {
                int random = Random.Range(1, 4);
                if (random < 2)
                {
                    boss.distanceToAttack = 20;
                    attackNumber = 4;               // взрыв
                }
                if (random == 2)
                {
                    boss.distanceToAttack = 6;      
                    attackNumber = 2;               // ренж атака
                }
                if (random == 3)
                {
                    boss.distanceToAttack = 20;
                    attackNumber = 3;               // спаун
                }
            }
        }

        // Если не готовы атаковать - возвращаемся
        if (!boss.readyToAttack)
            return;

        // Если всё готово - атакуем
        if (Time.time - lastAttack > cooldownAttack)              // если готовы атаковать и кд готово
        {
            lastAttack = Time.time;                         // присваиваем время атаки

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
    }

    void Attack(int number)
    {

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}
}
