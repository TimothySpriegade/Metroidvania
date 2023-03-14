using _Core._6_Characters.Enemies.Boss.AI;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ExhaustionState : EnemyAction
{
    [SerializeField] private float exhaustionLength = 10;
    private float exhaustionTimer;

    public override TaskStatus OnUpdate()
    {
        exhaustionTimer = Time.time;
        if(exhaustionTimer > exhaustionLength) 
        {
            exhaustionTimer = 0;
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }
}
