using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowGrenadeState_Range : EnemyState
{
    private Enemy_Range enemy;
    public bool finishedThrowingGrenaden { get; private set; } = true;
    public ThrowGrenadeState_Range(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = (Enemy_Range)_enemyBase;
    }

    public override void Enter()
    {
        base.Enter();
        finishedThrowingGrenaden = false;
        enemy.visuals.EnableWeaponModel(false);
        enemy.visuals.EnableIK(false, false);
        enemy.visuals.EnableSecondaryWeaponModel(true);
        enemy.visuals.EnableGrenadeModel(true);
    }
    public override void Update()
    {
        base.Update();
        Vector3 PlayerPosition = enemy.player.transform.position;   
        enemy.FaceTarget(PlayerPosition);
        enemy.aim.position = PlayerPosition;
        
        enemy.FaceTarget(enemy.aim.position);
        if(triggerCalled)
            stateMachine.ChangeState(enemy.battleState);
    }
    public override void Exit()
    {
        base.Exit();


    }
    override public void AbilityTrigger()
    {
        base.AbilityTrigger();
        finishedThrowingGrenaden = true;
        enemy.ThrowGrenade();
    }
}
