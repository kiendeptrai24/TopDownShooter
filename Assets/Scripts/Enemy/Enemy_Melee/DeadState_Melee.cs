

public class DeadState_Melee : EnemyState
{
    Enemy_Melee enemy;
    private Enemy_Ragdoll ragdoll;
    private bool interactionDisable;
    public DeadState_Melee(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemyBase as Enemy_Melee;
        ragdoll = enemy.GetComponent<Enemy_Ragdoll>();
    }
    public override void Enter()
    {
        base.Enter();

        interactionDisable = false;

        enemy.anim.enabled = false;
        enemy.agent.isStopped = true;

        ragdoll.RagdollActive(true);

        stateTimer = 2.5f;
    }
    public override void Update()
    {
        base.Update();
        // uncomment if you want to disable interaction
        DisableInteractionIfShould();
    }

    private void DisableInteractionIfShould()
    {
        if (stateTimer < 0 && interactionDisable == false)
        {
            interactionDisable = true;
            ragdoll.RagdollActive(false);
            ragdoll.CollidersActive(false);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
