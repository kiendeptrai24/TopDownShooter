using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState_Boss : EnemyState
{
    Enemy_Boss enemy;
    private bool interactionDisable;
    public DeadState_Boss(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = (Enemy_Boss)_enemyBase;
    }
    public override void Enter()
    {
        base.Enter();
        enemy.abilityState.DisableFlameThrower();
        interactionDisable = false;
        stateTimer = 2.5f;
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
