using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Brains/PatrolBrain")]
public class PatrolBrain : Brain
{
    public override void Think(EnemyThinker thinker)
    {
        if (!thinker.botAI.agent.hasPath && thinker.patrolPoints.Length > 0)                    // ���� ��� ���� � ����� ��� �������������� ����������
        {
            thinker.botAI.SetDestination(thinker.patrolPoints[thinker.i].transform.position);   // ������������� �������
            thinker.i++;
            if (thinker.i >= thinker.patrolPoints.Length)
                thinker.i = 0;
        }
    }
}
