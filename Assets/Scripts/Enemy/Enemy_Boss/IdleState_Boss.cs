using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState_Boss : EnemyState
{
    private Enemy_Boss enemy;

    public IdleState_Boss(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = (Enemy_Boss)_enemyBase;
    }
    public override void Enter()
    {
        base.Enter();
        
        stateTimer = enemy.idleTimer;
    }
    public override void Update()
    {
        base.Update();
        if(enemy.inBattleMode && enemy.PlayerInAttackRange())
            stateMachine.ChangeState(enemy.attackState);
        if(stateTimer < 0)
            stateMachine.ChangeState(enemy.moveState);
    }
    public override void Exit()
    {
        base.Exit();
    }
}
