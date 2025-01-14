using UnityEngine;

public class AbilityState_Melee : EnemyState
{
    private Enemy_Melee enemy;
    private Vector3 moveDirection;
    private const float MAX_MOVE_DISTANCE = 20; 
    private float moveSpeed;

    public AbilityState_Melee(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemyBase as Enemy_Melee;
    }
    public override void Enter()
    {
        base.Enter();
        enemy.visuals.EnableWeaponModel(true);
        moveSpeed = .5f;
        moveDirection = enemy.transform.position + (enemy.transform.forward * MAX_MOVE_DISTANCE);

    }
    public override void Update()
    {
        base.Update();
        if(enemy.manualRotationActive())
        {
            enemy.FaceTarget(enemy.player.position);
            moveDirection = enemy.transform.position + (enemy.transform.forward * MAX_MOVE_DISTANCE);
            
        }
        if(enemy.ManualMovementActive())
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, moveDirection, moveSpeed * Time.deltaTime);
        if(triggerCalled)
            stateMachine.ChangeState(enemy.recoveryState);
    }
    public override void Exit()
    {
        base.Exit();
        enemy.anim.SetFloat("RecoveryIndex", 0);
    }
    public override void AbilityTrigger()
    {
        base.AbilityTrigger();
        enemy.ThrowAxe();
    }
}
