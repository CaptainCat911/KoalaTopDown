using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseBehaviour : StateMachineBehaviour
{
    Boss boss;                              // ссылка на бота
    public float cooldownAttack = 2f;       // перезарядка атаки

    [Header("Атака издалека")]
    public int rangeAttackChance;          // шанс ренже атаки

    [Header("Атака вблизи")]
    public int meleeAttackChance;
    public int explousionAttackChance;

    [Header("Общие атаки")]
    public int spawnAttackChance;           // .. спауна
    public int multiAttackChance;           // .. мультиатаки
    public int laserChance;                 // .. лазера


    float lastAttack;                       // время последнего рандома  
    //float randomCooldown = 1f;              // перезарядка рандома    
    //float lastRandom;                       // время последнего рандома
    int attackNumber;                       // тип атаки
    bool attackReady;                       // атака готова

    // Смена цели
    float lastTargetChange;                 // время последнего поиска цели
    float cooldownChange = 4f;              // перезардяка поиска цели


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = animator.GetComponent<Boss>();       // ПОМЕНЯТЬ
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Если нет цели - возвращаемся в идле
        if (!boss.target || !boss.isAlive)
        {
            animator.SetTrigger("Idle");            // триггер
            boss.chasing = false;                   // отключаем преследование
            return;
        }

        // Если сейчас атакуем
        if (boss.attackingNow)
        {
            boss.agent.ResetPath();
            return;
        }

        if (boss.currentHealth < boss.maxHealth / 3)
        {
            //boss.SayText("Тебе никогда не победить");
            boss.distanceToAttack = 20;
            animator.SetTrigger("TimeReverce");     // триггер            
            return;
        }

        boss.Chase();                               // преследуем цель
        boss.NavMeshRayCast(boss.target);           // делаем рейкаст

        // Иногда сменяем цель
        if (Time.time - lastTargetChange > cooldownChange && Time.time - lastAttack > cooldownAttack)      // если кд готово
        {
            lastTargetChange = Time.time;
            boss.FindTarget();                              // поиск цели
        }

        // Подготовка типа атаки
        if (!attackReady && Time.time - lastAttack > cooldownAttack)        
        {
            // Дальняя дистанция
            if (boss.distanceToTarget > 3)
            {
                if (ProbabilityCheck(rangeAttackChance))   // шанс на ренж атаку
                {
                    boss.distanceToAttack = 6;
                    attackNumber = 2;               // ренж атака
                }                
            }
            // Ближняя дистанция
            else
            {
                if (ProbabilityCheck(meleeAttackChance))
                {
                    boss.distanceToAttack = 3;
                    attackNumber = 1;               // удар
                }
                if (ProbabilityCheck(explousionAttackChance))
                {
                    boss.distanceToAttack = 3;     
                    attackNumber = 4;               // взрыв
                }
            }

            // Эти атаки для которых не важна дистанция
            if (ProbabilityCheck(spawnAttackChance))
            {
                boss.distanceToAttack = 20;
                attackNumber = 3;               // спаун
            }
            if (ProbabilityCheck(multiAttackChance))
            {
                boss.distanceToAttack = 20;
                attackNumber = 5;               // мультиренж атака
            }
            if (ProbabilityCheck(laserChance))
            {
                boss.distanceToAttack = 20;
                attackNumber = 6;               // лазер атака
            }

            attackReady = true;                 // готовы атаковать
        }

        // Если не готовы атаковать - возвращаемся
        if (!boss.closeToTarget || !attackReady)
            return;

        // Если всё готово - атакуем
        if (Time.time - lastAttack > cooldownAttack)        // если готовы атаковать и кд готово
        {
            lastAttack = Time.time;                         // присваиваем время атаки
            if (attackNumber == 1)
            {
                animator.SetTrigger("AttackMelee");
            }
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
            if (attackNumber == 5)
            {
                animator.SetTrigger("AttackMultiRange");
            }
            if (attackNumber == 6)
            {
                animator.SetTrigger("AttackLaser");
            }

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
