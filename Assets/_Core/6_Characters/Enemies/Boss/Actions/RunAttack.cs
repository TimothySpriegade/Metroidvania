using _Core._6_Characters.Enemies.Boss.AI;
using _Core._10_Utils;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Core._6_Characters.Enemies.Boss;
using DG.Tweening;
using _Core._6_Characters.Enemies.Boss.Actions;

public class RunAttack : EnemyAction
{
    /* schaut zum spieler (intree)
     * checkt distanz
     * läuft bis distanz x
     * angriff
     * chill */
    [SerializeField] private float wantedDistanceToPlayer;
    [SerializeField] private float walkDuration;

    private Tween walkTween;
    private bool finishedAttack;


    public override void OnStart()
    {
        finishedAttack = false;
        var distanceToPlayer = TargetUtils.GetDistToTarget(gameObject, bossCombat.GetPlayer());
        var wantedPosition = distanceToPlayer - wantedDistanceToPlayer;
        wantedPosition = TargetUtils.TargetIsToRight(gameObject, bossCombat.GetPlayer()) ? wantedPosition : -wantedPosition;
        bossEnemy.ChangeAnimationState(BossAnimatorState.BossRun);
        walkTween = transform.DOMoveX(wantedPosition, walkDuration)
            .SetEase(Ease.Linear)
            .SetRelative()
            .OnComplete(Attack);
    }

    private void Attack()
    {
        bossEnemy.ChangeAnimationState(BossAnimatorState.BossSmallAttack);
        finishedAttack= true;
    }

    public override TaskStatus OnUpdate()
    {
        return finishedAttack ? TaskStatus.Success : TaskStatus.Running;
    }

    public override void OnEnd()
    {
        walkTween?.Kill();
    }


}
