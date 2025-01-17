using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState_Boss : EnemyState
{
    private Enemy_Boss enemy;
    private Vector3 destination;
    private float actionTimer;
    public MoveState_Boss(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = (Enemy_Boss)_enemyBase;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.agent.isStopped = false;
        destination = enemy.GetPatrolDestination();
        enemy.agent.SetDestination(destination);
        enemy.agent.speed = enemy.walkSpeed;
        actionTimer = enemy.actionCooldown;
    }
    public override void Update()
    {
        base.Update();
        actionTimer -= Time.deltaTime;
        enemy.FaceTarget(GetNextPathPoints());

        
        if(enemy.inBattleMode)
        {
            Vector3 playerPos = enemy.player.position;
            
            enemy.agent.SetDestination(playerPos);

            if(actionTimer < 0)
                PerformRandomAction();

            if(enemy.PlayerInAttackRange())
            {
                stateMachine.ChangeState(enemy.attackState);
            }
        }
        else{
            if(Vector3.Distance(enemy.transform.position, destination) < .25f)
                stateMachine.ChangeState(enemy.idleState);

        }

    }
    public override void Exit()
    {
        base.Exit();
    }
    private void PerformRandomAction()
    {
        actionTimer = enemy.actionCooldown;
        if(Random.Range(0,2) == 0)
        {
            if(enemy.CanDoAbility())
                stateMachine.ChangeState(enemy.abilityState);
        }
        else{
            if(enemy.CanDoJumpAttack())
                stateMachine.ChangeState(enemy.jumpAttackState);
        }
    }
}
