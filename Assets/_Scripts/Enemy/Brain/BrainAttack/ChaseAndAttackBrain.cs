using UnityEngine;


[CreateAssetMenu(menuName = "Brains/ChaseAndAttackBrain")]
public class ChaseAndAttackBrain : Brain
{
    public bool isRangeAttacking;
    public float rangeToAttack = 5;
    public float meleeToAttack = 1;
    float range;

    public override void Think(EnemyThinker thinker)
    {
        float distance = Vector3.Distance(thinker.botAI.transform.position, thinker.target.transform.position);       // ������� ��������� �� ����        

        if (isRangeAttacking)
            range = rangeToAttack;
        else
            range = meleeToAttack;

        if (thinker.botAI.targetVisible && distance < range)
        {         
            if (!thinker.botAI.readyToAttack)
            {
                thinker.botAI.agent.ResetPath();                                                              // ���������� ����            
                thinker.botAI.readyToAttack = true;                                                           // ����� ��������
                Debug.Log("Ready Attack");

                if (isRangeAttacking)
                {
                    thinker.botAI.hitBox.isRange = true;
                    thinker.botAI.distanceToAttack = range;
                }
                if (!isRangeAttacking)
                {
                    thinker.botAI.hitBox.isRange = false;
                    thinker.botAI.distanceToAttack = range;
                }
            }
        }
        else
        {
            thinker.botAI.agent.SetDestination(thinker.target.transform.position);                              // ������������ � ����
            if (thinker.botAI.readyToAttack)
                thinker.botAI.readyToAttack = false;                                                            // �� ����� ��������                
        }

        Debug.Log(range); 
    }
}