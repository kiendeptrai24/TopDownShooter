using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DancerState_Boss : EnemyState
{
    Enemy_Boss enemy;
    public DancerState_Boss(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = (Enemy_Boss)_enemyBase;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.agent.isStopped = true;
        enemy.agent.velocity = Vector3.zero;
    }
    public override void Update()
    {
        base.Update();
        if(triggerCalled)
            stateMachine.ChangeState(enemy.GetState<IdleState_Boss>());
        
    }
    public override void Exit()
    {
        base.Exit();
        enemy.anim.transform.localPosition = Vector3.zero;
    }
}
