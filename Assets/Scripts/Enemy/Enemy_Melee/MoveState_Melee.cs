
using UnityEngine;


public class MoveState_Melee : EnemyState
{
    private Enemy_Melee enemy;
    private Vector3 destination;
    public MoveState_Melee(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemyBase as Enemy_Melee;
    }

    public override void Enter()
    {
        base.Enter();
        destination = enemy.GetPatrolDestination();
        enemy.agent.SetDestination(destination);
       
    }
    public override void Update()
    {
        base.Update();
        enemy.FaceTarget(GetNextPathPoints());
        if(Vector3.Distance(enemy.transform.position, destination) <= enemy.agent.stoppingDistance + 0.1f)
            stateMachine.ChangeState(enemy.GetState<IdleState_Melee>());
    }
    public override void Exit()
    {
        base.Exit();
    }
   
}
