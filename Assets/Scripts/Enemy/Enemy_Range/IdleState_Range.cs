using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState_Range : EnemyState
{
    private Enemy_Range enemy;
    public IdleState_Range(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemyBase as Enemy_Range;
    }
    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.idleTimer;
    }
    public override void Update()
    {
        base.Update();
        if(stateTimer < 0)
            stateMachine.ChangeState(enemy.moveState);
    }
    public override void Exit()
    {
        base.Exit();
    }
}