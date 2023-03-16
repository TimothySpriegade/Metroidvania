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
    [SerializeField] private float dashDuration;

    private Tween startPreparation;
    private Tween startBuildUp;
    private Tween startAttack;
    private Tween finishAttack;

    private bool dashAttackFinished;

    public override void OnStart()
    {
        var position = TargetUtils.TargetIsToRight(gameObject , bossCombat.GetPlayer()) ? -20 : 20;
        var scale = transform.localScale;
        scale.x = TargetUtils.TargetIsToRight(gameObject, bossCombat.GetPlayer()) ? 1 : -1;

        transform.localScale = scale;
        bossEnemy.ChangeAnimationState(BossAnimatorState.BossRun);
        transform.localScale = scale;
       
        startPreparation = transform.DOMoveX(position, preparatoionDuration)
            .SetEase(Ease.Linear)
            .OnComplete(StartBuildUp);
    }

    public void StartBuildUp()
    {
        var scale = transform.localScale;
        scale.x = TargetUtils.TargetIsToRight(gameObject, bossCombat.GetPlayer()) ? -1 : 1;
        transform.localScale = scale;

        SpawnAndDespawnPlatform();
        bossEnemy.ChangeAnimationState(BossAnimatorState.BossDashBuildup);
        startBuildUp = DOVirtual.DelayedCall(buildUpTime, startDashing);

    }

    private void startDashing()
    {
        var position = gameObject.transform.position.x == 20 ? -20 : 20;   
        bossEnemy.ChangeAnimationState(BossAnimatorState.BossSwingAttack);
        startAttack = transform.DOMoveX(position, dashDuration)
            .SetEase(Ease.Linear)
            .OnComplete(FinishAttack);
    }

    private void FinishAttack()
    {
        SpawnAndDespawnPlatform();
        finishAttack = DOVirtual.DelayedCall(bossEnemy.ChangeAnimationState(BossAnimatorState.BossSwingAttackEnd), EndAttack());
    }

    private void EndAttack()
    {

    }

    private void SpawnAndDespawnPlatform()
    {

    }

    public override TaskStatus OnUpdate()
    {

        return dashAttackFinished ? TaskStatus.Success : TaskStatus.Running;    
    }
    public override void OnEnd()
    {
        dashAttackFinished = false;
        startPreparation.Kill();
        startBuildUp.Kill();
    }
}
