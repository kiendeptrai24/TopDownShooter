using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState_Boss : EnemyState
{
    Enemy_Boss enemy;
    public float lastTimeAttacked {get; private set;}
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
        stateTimer = 1f;
        enemy.bossVisuals.EnableWeaponTrail(true);
    }
    public override void Update()
    {
        base.Update();
        if(stateTimer > 0)
            enemy.FaceTarget(enemy.player.position,20);
        if(triggerCalled)
        {
            if(enemy.PlayerInAttackRange())
                stateMachine.ChangeState(enemy.GetState<IdleState_Boss>());
            else
                stateMachine.ChangeState(enemy.GetState<MoveState_Boss>());
        }
    }
    public override void Exit()
    {
        base.Exit();
        lastTimeAttacked = Time.time;
        enemy.bossVisuals.EnableWeaponTrail(false);
    }
}
