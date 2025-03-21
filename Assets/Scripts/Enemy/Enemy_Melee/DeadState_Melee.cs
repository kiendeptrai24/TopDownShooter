

public class DeadState_Melee : EnemyState
{
    Enemy_Melee enemy;
    private bool interactionDisable;
    public DeadState_Melee(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemyBase as Enemy_Melee;
    }
    public override void Enter()
    {
        base.Enter();

        interactionDisable = false;

      

        stateTimer = 2.5f;
    }
    public override void Update()
    {
        base.Update();
        // uncomment if you want to disable interaction
        //DisableInteractionIfShould();
    }

    private void DisableInteractionIfShould()
    {
        if (stateTimer < 0 && interactionDisable == false)
        {
            interactionDisable = true;
            enemy.ragdoll.RagdollActive(false);
            enemy.ragdoll.CollidersActive(false);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
