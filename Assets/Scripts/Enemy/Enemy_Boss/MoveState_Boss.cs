using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState_Boss : EnemyState
{
    private Enemy_Boss enemy;
    private Vector3 destination;
    private float actionTimer;
    private float timeBeforeSpeedUp = 5;
    private bool isSpeedUpActivated;
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
        ResetSpeed();
        actionTimer = enemy.actionCooldown;
    }


    public override void Update()
    {
        base.Update();
        actionTimer -= Time.deltaTime;
        enemy.FaceTarget(GetNextPathPoints());

        
        if(enemy.inBattleMode)
        {
            if(ShouldSpeedUp())
                SpeedUp();

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

    private void SpeedUp()
    {
        enemy.agent.speed = enemy.runSpeed;
        enemy.anim.SetFloat("MoveAnimIndex", 1);
        enemy.anim.SetFloat("MoveAnimSppedMutiplier",1.5f);
        isSpeedUpActivated = true;
    }

    public override void Exit()
    {
        base.Exit();
    }
    private void ResetSpeed()
    {
        isSpeedUpActivated = false;
        enemy.anim.SetFloat("MoveAnimIndex", 0);
        enemy.anim.SetFloat("MoveAnimSppedMutiplier",1);
        enemy.agent.speed = enemy.walkSpeed;
    }
    private bool ShouldSpeedUp()
    {
        if(isSpeedUpActivated)
            return false;

        if(Time.time > enemy.attackState.lastTimeAttacked + timeBeforeSpeedUp)
            return true;
        return false;
    }
    private void PerformRandomAction()
    {
        actionTimer = enemy.actionCooldown;
        if(Random.Range(0,2) == 0)
        {
            TryAbility();
        }
        else
        {
            if(enemy.CanDoJumpAttack())
                stateMachine.ChangeState(enemy.jumpAttackState);
            else if(enemy.bossWeaponType == BossWeaponType.Hummer)
                TryAbility();
        }
    }

    private void TryAbility()
    {
        if (enemy.CanDoAbility())
            stateMachine.ChangeState(enemy.abilityState);
    }
}
