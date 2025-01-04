using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancePlayerState_Range : EnemyState
{
    Enemy_Range enemy;
    private Vector3 playerPos;
    public AdvancePlayerState_Range(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemyBase as Enemy_Range;
    }
    public override void Enter()
    {
        base.Enter();
        enemy.visuals.EnableIK(true, true);

        enemy.agent.isStopped = false;
        enemy.agent.speed = enemy.advanceSpeed;
        
    }
    public override void Update()
    {
        base.Update();
        playerPos = enemy.player.position;
        enemy.agent.SetDestination(playerPos);
        enemy.FaceTarget(GetNextPathPoints());
        if(Vector3.Distance(enemy.transform.position,playerPos) < enemy.advanceStoppingDistance)
            stateMachine.ChangeState(enemy.battleState);
    }
    override public void Exit()
    {
        base.Exit();

    }

}
