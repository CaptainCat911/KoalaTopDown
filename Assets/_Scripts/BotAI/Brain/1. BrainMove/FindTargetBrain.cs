using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Brains/FindTargetBrain")]
public class FindTargetBrain : Brain
{
    public string tagTarget;

    public override void Think(EnemyThinker thinker)
    {
        GameObject target = GameObject.FindGameObjectWithTag(tagTarget);
        thinker.botAI.target = target;        
    }
}
