using UnityEngine;


[CreateAssetMenu(menuName = "Brains/ChaseAndAttackBrain")]
public class ChaseAndAttackBrain : Brain
{
    public override void Think(EnemyThinker thinker)
    {
        float distance = Vector3.Distance(thinker.botAI.transform.position, thinker.botAI.target.transform.position);       // считаем дистанцию до цели        
        if (thinker.botAI.targetVisible && distance < thinker.botAI.distanceToAttack)
        {         
            if (!thinker.botAI.closeToTarget)
            {
                thinker.botAI.agent.ResetPath();                                                              // сбрасываем путь            
                thinker.botAI.closeToTarget = true;                                                           // готов стрелять
            }
        }
        else 
        {
            thinker.botAI.agent.SetDestination(thinker.botAI.target.transform.position);                              // перемещаемся к цели
            if (thinker.botAI.closeToTarget)
                thinker.botAI.closeToTarget = false;                                                            // не готов стрелять                
        }

        //Debug.Log(range); 
        
    }
}