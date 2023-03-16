using _Core._10_Utils;
using _Core._6_Characters.Enemies.Boss;
using _Core._6_Characters.Enemies.Boss.AI;
using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAttack : EnemyAction
{
    /* run anim 
 * ist spieler rechts ? geh link vise versa
 * spawn platform
 * warte 2 sekunden (animationswechsel
 * dash los 
 * despawn platform */

    [SerializeField] private float preparatoionDuration;
    [SerializeField] private float buildUpTime;

    private Tween startPreparation;
    private Tween startBuildUp;

    private bool dashAttackFinished;

    public override void OnStart()
    {
        var position = TargetUtils.TargetIsToRight(gameObject , bossCombat.GetPlayer()) ? -20 : 20;
        bossEnemy.ChangeAnimationState(BossAnimatorState.BossRun);
        startPreparation = transform.DOMoveX(position, preparatoionDuration)
            .SetEase(Ease.Linear)
            .OnComplete(StartBuildUp);

            
    }

    public void StartBuildUp()
    {
        SpawnPlatform();
        bossEnemy.ChangeAnimationState(BossAnimatorState.BossDashBuildup);
        startBuildUp = DOVirtual.DelayedCall(buildUpTime, startDashing);

    }

    private void startDashing()
    {

    }

    private void SpawnPlatform()
    {

    }

    public override TaskStatus OnUpdate()
    {

        return dashAttackFinished ? TaskStatus.Success : TaskStatus.Running;    
    }
    public override void OnEnd()
    {
        startPreparation.Kill();
    }
}
