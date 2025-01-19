using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class AbilityState_Boss : EnemyState
{
    Enemy_Boss enemy;
    public AbilityState_Boss(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = (Enemy_Boss)_enemyBase;
    }
    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.flamethrowDuration;
        enemy.agent.isStopped = true;
        enemy.agent.velocity = Vector3.zero;
    }
    public override void Update()
    {
        base.Update();
        enemy.FaceTarget(enemy.player.position);
        if(stateTimer < 0 && enemy.flamethrowActive)
            enemy.ActivateFlamethrower(false);
        if(triggerCalled)
            stateMachine.ChangeState(enemy.moveState);
    }
    public override void Exit()
    {
        base.Exit();
        enemy.SetAbilityOnCooldown();
        enemy.bossVisuals.ResetBatteries();
    }
    public override void AbilityTrigger()
    {
        base.AbilityTrigger();
        enemy.ActivateFlamethrower(true);
        enemy.bossVisuals.dischargeBatteries();
    }
}
