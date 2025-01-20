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
        enemy.bossVisuals.EnableWeaponTrail(true);
    }
    public override void Update()
    {
        base.Update();
        enemy.FaceTarget(enemy.player.position);
        if(stateTimer < 0)
            DisableFlameThrower();
        if(triggerCalled)
            stateMachine.ChangeState(enemy.moveState);
    }
    public override void Exit()
    {
        base.Exit();
        enemy.SetAbilityOnCooldown();
        enemy.bossVisuals.ResetBatteries();
        enemy.bossVisuals.EnableWeaponTrail(false);
    }
    public void DisableFlameThrower()
    {
        if(enemy.bossWeaponType != BossWeaponType.Flamethrower)
            return;
        if(enemy.flamethrowActive == false)
            return;
        enemy.ActivateFlamethrower(false);
    }
    public override void AbilityTrigger()
    {
        base.AbilityTrigger();
        if(enemy.bossWeaponType == BossWeaponType.Flamethrower)
        {
            enemy.ActivateFlamethrower(true);
            enemy.bossVisuals.dischargeBatteries();

        }
        else if(enemy.bossWeaponType == BossWeaponType.Hummer){
            enemy.ActivateHummer();
        }
        
    }
}
