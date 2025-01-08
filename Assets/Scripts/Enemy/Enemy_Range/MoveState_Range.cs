using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState_Range : EnemyState
{
    private Enemy_Range enemy;
    private Vector3 destination;
    public MoveState_Range(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemyBase as Enemy_Range;
    }
    public override void Enter()
    {
        base.Enter();
        enemy.agent.speed = enemy.walkSpeed;
        destination = enemy.GetPatrolDestination();
        enemy.agent.SetDestination(destination);
    }
    public override void Update()
    {
        base.Update();
        enemy.FaceTarget(GetNextPathPoints());
        if(Vector3.Distance(enemy.transform.position, destination) <= enemy.agent.stoppingDistance + 0.01f)
            stateMachine.ChangeState(enemy.idleState);
    }
    public override void Exit()
    {
        base.Exit();
    }
}
