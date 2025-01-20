using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAttackState_Boss : EnemyState
{
    Enemy_Boss enemy;
    private Vector3 lastPlayerPos;
    public float jumpAttackMovementSpeed;
    public JumpAttackState_Boss(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = (Enemy_Boss)_enemyBase;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.agent.isStopped = true;
        enemy.agent.velocity = Vector3.zero;
        lastPlayerPos = enemy.player.position;

        enemy.bossVisuals.PlayLandingZone(lastPlayerPos);
        enemy.bossVisuals.EnableWeaponTrail(true);
        float distanceToPlayer = Vector3.Distance(lastPlayerPos, enemy.transform.position);
        jumpAttackMovementSpeed = distanceToPlayer / enemy.travelTimeToTarget;
        enemy.FaceTarget(lastPlayerPos, 1000);
        if(enemy.bossWeaponType == BossWeaponType.Hummer)
        {
            enemy.agent.isStopped = false;
            enemy.agent.speed = enemy.runSpeed;
            enemy.agent.SetDestination(lastPlayerPos);
        }
    }
    public override void Update()
    {
        base.Update();
        Vector3 myPos = enemy.transform.position;
        enemy.agent.enabled = !enemy.ManualMovementActive();
        if(enemy.ManualMovementActive())
        {
            enemy.transform.position =
                Vector3.MoveTowards(myPos, lastPlayerPos, jumpAttackMovementSpeed * Time.deltaTime);

        }
        if(triggerCalled)
            stateMachine.ChangeState(enemy.moveState);
    }
    public override void Exit()
    {
        base.Exit();
        enemy.agent.enabled = true;
        enemy.SetJumpAttackOnCooldown();
        enemy.bossVisuals.EnableWeaponTrail(false);
    }
    public override void AbilityTrigger()
    {
        enemy.JumpImpact();
    }
}
