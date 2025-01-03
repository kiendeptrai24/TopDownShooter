using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SubsystemsImplementation;

public class RunToCoverState_Range : EnemyState
{
    Enemy_Range enemy;
    private Vector3 destination;
    public RunToCoverState_Range(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemyBase as Enemy_Range;
    }

    public override void Enter()
    {
        base.Enter();
        destination = enemy.currentCover.transform.position;
        enemy.visuals.EnableIK(true,false);
        enemy.agent.isStopped = false;
        enemy.agent.speed = enemy.runSpeed;
        enemy.agent.SetDestination(destination);
    }
    public override void Update()
    {
        base.Update();
        enemy.FaceTarget(GetNextPathPoints());
        if(Vector3.Distance(enemy.transform.position, destination) < .5f)
            stateMachine.ChangeState(enemy.battleState);

    }
    public override void Exit()
    {
        base.Exit();
    }
}
