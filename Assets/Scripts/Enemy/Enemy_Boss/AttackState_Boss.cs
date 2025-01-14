using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState_Boss : EnemyState
{
    Enemy_Boss enemy;
    public AttackState_Boss(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy  = (Enemy_Boss)_enemyBase;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.anim.SetFloat("AttackIndex",Random.Range(0,2));
        enemy.agent.isStopped = true;
        enemy.agent.velocity = Vector3.zero;
    }
    public override void Update()
    {
        base.Update();
        if(triggerCalled)
        {
            if(enemy.PlayerInAttackRange())
                stateMachine.ChangeState(enemy.idleState);
            else
                stateMachine.ChangeState(enemy.moveState);
        }
    }
    public override void Exit()
    {
        base.Exit();
    }
}
