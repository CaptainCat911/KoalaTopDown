using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseBehaviourNPC : StateMachineBehaviour
{
    NPC boss;                               // ссылка на бота
    
    float lastAttack;                       // время последнего рандома    
    int attackNumber;                       // тип атаки
    //bool attackReady;                       // атака готова

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
        if (!boss.target || boss.isNeutral || !boss.isAlive)
        {
            boss.chasing = false;                   // отключаем преследование
            animator.SetTrigger("Run");             // триггер
            return;
        }

        // Если сейчас атакуем
        if (boss.attackingNow)
        {
            boss.agent.ResetPath();
            return;
        }

        // ТУТ ПЕРЕДЕЛАТЬ
        if (boss.lowHp && boss.mainBoss)
        {
            //boss.SayText("Тебе никогда не победить");
            boss.distanceToAttack = 30;
            attackNumber = 7;
            animator.SetFloat("AttackType", attackNumber);
            animator.SetTrigger("Attack");
            return;
        }

        boss.Chase();                               // преследуем цель
        boss.NavMeshRayCast(boss.target);           // делаем рейкаст

        // Иногда сменяем цель
        if (Time.time - lastTargetChange > cooldownChange && Time.time - lastAttack > boss.cooldownAttack)      // если кд готово
        {
            lastTargetChange = Time.time;
            boss.FindTarget();                                  // поиск цели
        }




        // Если не близко к цели - бежим
        if (!boss.targetInRange)
        {
            animator.SetTrigger("Run");
            return;
        }

        if (Time.time - lastAttack <= boss.cooldownAttack)
        {
            return;
        }
        // Подготовка типа атаки
        else
        {
            // Дальняя дистанция
            if (boss.distanceToTarget > 3)
            {
                if (ProbabilityCheck(boss.rangeAttackChance))    // шанс на ренж атаку
                {
                    boss.distanceToAttack = boss.rangeAttackDistance;
                    attackNumber = 3;               // ренж атака
                }
                if (ProbabilityCheck(boss.multiAttackChance))
                {
                    boss.distanceToAttack = 20;
                    attackNumber = 5;               // мультиренж атака
                }
            }
            // Ближняя дистанция
            else
            {
                if (ProbabilityCheck(boss.meleeAttackChance))
                {
                    boss.distanceToAttack = boss.meleeAttackDistance;
                    attackNumber = 1;               // удар
                }
                if (ProbabilityCheck(boss.explousionAttackChance))
                {
                    boss.distanceToAttack = 3;
                    attackNumber = 2;               // взрыв
                }
            }

            // Эти атаки для которых не важна дистанция
            if (ProbabilityCheck(boss.spawnAttackChance))
            {
                boss.distanceToAttack = 20;
                attackNumber = 4;               // спаун
            }
            if (ProbabilityCheck(boss.gravityChance))    // шанс на гравити атаку
            {
                boss.distanceToAttack = 10;
                attackNumber = 8;               // гравити атака
            }
            if (ProbabilityCheck(boss.laserChance))
            {
                boss.distanceToAttack = 20;
                attackNumber = 6;               // лазер атака
            }
            if (ProbabilityCheck(boss.teleportChance))
            {
                boss.distanceToAttack = 30;
                attackNumber = 9;               // телепорт
            }


            lastAttack = Time.time;                         // присваиваем время атаки
            animator.SetFloat("AttackType", attackNumber);
            animator.SetTrigger("Attack");

            boss.distanceToAttack = boss.defaultRangeToTarget;

            //attackReady = true;                 // готовы атаковать
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
}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

