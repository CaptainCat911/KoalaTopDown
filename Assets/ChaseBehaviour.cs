using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseBehaviour : StateMachineBehaviour
{
    Boss boss;                              // ������ �� ����
    public float randomCooldown = 2f;       // ����������� �������   
    float lastRandom;                       // ����� ���������� �������
    public float �ooldown = 2f;       // ����������� �������   
    float lastAttack;                       // ����� ���������� �������  

    int attackNumber;                       // ��� �����


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = animator.GetComponent<Boss>();       // ��������
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!boss.target)
        {
            animator.SetTrigger("Idle");            // �������
            boss.chasing = false;                   // ��������� �������������
            return;
        }

        boss.Chase();                               // ���������� ����



        if (Time.time - lastRandom > randomCooldown)        // ���� ������ ��������� � �� ������
        {
            lastRandom = Time.time;                         // ����������� ����� �����
            
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

        if (Time.time - lastAttack > �ooldown)              // ���� ������ ��������� � �� ������
        {
            lastAttack = Time.time;                         // ����������� ����� �����


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
