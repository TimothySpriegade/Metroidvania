using _Core._6_Characters.Enemies.Boss.AI;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExhaustionState : EnemyAction
{
    private float exhaustionTimer;

    public override TaskStatus OnUpdate()
    {
        exhaustionTimer = Time.time;
        if(exhaustionTimer > 10f) 
        {
            exhaustionTimer = 0;
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }
}
