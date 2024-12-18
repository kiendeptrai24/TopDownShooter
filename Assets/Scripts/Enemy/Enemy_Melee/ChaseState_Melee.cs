using UnityEngine;

public class ChaseState_Melee : EnemyState
{
    private Enemy_Melee enemy;
    private float lastTimeUpdateDestination;

    public ChaseState_Melee(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemyBase as Enemy_Melee;
    }
    public override void Enter()
    {
        base.Enter();
        enemy.agent.isStopped = false;
        enemy.agent.speed = enemy.chaseSpeed;
    }
    public override void Update()
    {
        base.Update();
        
        if(enemy.PlayerInAttackRange())
            stateMachine.ChangeState(enemy.attackState);
            
        if(CanUpdateDestination())
        {
            enemy.agent.destination = enemy.player.transform.position;
        }

        enemy.FaceTarget(GetNextPathPoints());

        
    }
    public override void Exit()
    {
        base.Exit();
        enemy.agent.speed = enemy.moveSpeed;
    }
    private bool CanUpdateDestination()
    {
        if(Time.time > lastTimeUpdateDestination + 0.25f)
        {
            lastTimeUpdateDestination = Time.time;
            return true;
        }
        return false;
        
    }

}
