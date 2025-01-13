using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState_Range : EnemyState
{
    private bool interactionDisable;
    Enemy_Range enemy;
    public DeadState_Range(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = (Enemy_Range)_enemyBase;
    }
    public override void Enter()
    {
        base.Enter();
        interactionDisable = false;

        enemy.anim.enabled = false;
        enemy.agent.isStopped = true;

        enemy.ragdoll.RagdollActive(true);

        stateTimer = 2.5f;
        if(enemy.throwGrenadeState.finishedThrowingGrenaden == false)
            enemy.ThrowGrenade();
    }
    public override void Update()
    {
        base.Update();
        DisableInteractionIfShould();
    }
    public override void Exit()
    {
        base.Exit();
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

}
