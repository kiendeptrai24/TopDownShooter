using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowGrenadeState_Range : EnemyState
{
    Enemy_Range enemy;
    public ThrowGrenadeState_Range(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = (Enemy_Range)_enemyBase;
    }

    public override void Enter()
    {
        base.Enter();
    }
    public override void Update()
    {
        base.Update();
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
        enemy.ThrowGrenade();
    }
}
