


public class RecoveryState_Melee : EnemyState
{
    Enemy_Melee enemy;
    public RecoveryState_Melee(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemyBase as Enemy_Melee;
    }
    public override void Enter()
    {
        base.Enter();
        enemy.agent.isStopped = true;
        
        enemy.anim.SetFloat("RecoveryIndex", 0);
        
        if(enemy.PlayerInAttackRange())
            enemy.anim.SetFloat("RecoveryIndex", 1);
    }
    public override void Update()
    {
        base.Update();
        enemy.FaceTarget(enemy.player.position);

        if(triggerCalled)
        {
            if(enemy.CanThrowAxe())
                stateMachine.ChangeState(enemy.abilityState);
            else if(enemy.PlayerInAttackRange())
                stateMachine.ChangeState(enemy.attackState);
            else 
                stateMachine.ChangeState(enemy.chaseState);
        }
    }
    public override void Exit()
    {
        base.Exit();

    }
   

}   
