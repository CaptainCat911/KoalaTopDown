using UnityEngine;


[CreateAssetMenu(menuName = "Brains/ChaseAndAttackBrain")]
public class ChaseAndAttackBrain : Brain
{
    public override void Think(EnemyThinker thinker)
    {
        float distance = Vector3.Distance(thinker.botAI.transform.position, thinker.botAI.target.transform.position);       // ������� ��������� �� ����        
        if (thinker.botAI.targetVisible && distance < thinker.botAI.distanceToAttack)
        {         
            if (!thinker.botAI.closeToTarget)
            {
                thinker.botAI.agent.ResetPath();                                                              // ���������� ����            
                thinker.botAI.closeToTarget = true;                                                           // ����� ��������
            }
        }
        else 
        {
            thinker.botAI.agent.SetDestination(thinker.botAI.target.transform.position);                              // ������������ � ����
            if (thinker.botAI.closeToTarget)
                thinker.botAI.closeToTarget = false;                                                            // �� ����� ��������                
        }

        //Debug.Log(range); 
        
    }
}