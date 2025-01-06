using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancePlayerState_Range : EnemyState
{
    Enemy_Range enemy;
    private Vector3 playerPos;
    public float LastTimeAdvanced { get; private set; }
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
        enemy.UpdateAimPosition();
        playerPos = enemy.player.position;
        enemy.agent.SetDestination(playerPos);
        enemy.FaceTarget(GetNextPathPoints());
        if(CanEnterBattleState())
            stateMachine.ChangeState(enemy.battleState);
    }
    override public void Exit()
    {
        base.Exit();
        LastTimeAdvanced = Time.time;
    }
    public bool CanEnterBattleState()
    {
        return Vector3.Distance(enemy.transform.position,playerPos) < enemy.advanceStoppingDistance
            && enemy.IsSeeingPlayer();
    }

}
