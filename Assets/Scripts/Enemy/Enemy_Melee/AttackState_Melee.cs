using System.Collections.Generic;
using UnityEngine;

public class AttackState_Melee : EnemyState
{
    Enemy_Melee enemy;
    private Vector3 attackDirection;
    private float attackMoveSpeed;
    private const float MAX_ATTACK_DISTANCE = 50; 
    public AttackState_Melee(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemyBase as Enemy_Melee;
    }
    public override void Enter()
    {
        base.Enter();
        enemy.UpdateAttackData();
        enemy.visuals.EnableWeaponTrail(true);
        enemy.visuals.EnableWeaponModel(true);
        attackMoveSpeed = enemy.attackData.moveSpeed;
        enemy.anim.SetFloat("AttackAnimationSpeed", enemy.attackData.animationSpeed);
        enemy.anim.SetFloat("AttackIndex", enemy.attackData.attackIndex);
        enemy.anim.SetFloat("SlashAttackIndex", Random.Range(0, 6));
        enemy.agent.isStopped = true;
        enemy.agent.velocity = Vector3.zero;

        attackDirection = enemy.transform.position + (enemy.transform.forward * MAX_ATTACK_DISTANCE);

    }
    public override void Update()
    {
        base.Update();
        if(enemy.manualRotationActive())
        {
            enemy.FaceTarget(enemy.player.position);
            attackDirection = enemy.transform.position + (enemy.transform.forward * MAX_ATTACK_DISTANCE);
            
        }
        if(enemy.ManualMovementActive())
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, attackDirection, attackMoveSpeed * Time.deltaTime);
        Debug.Log(triggerCalled);
        if(triggerCalled)
        {
            if(enemy.PlayerInAttackRange())
                stateMachine.ChangeState(enemy.recoveryState);
            else
                stateMachine.ChangeState(enemy.chaseState);
        }
    }
    public override void Exit()
    {
        base.Exit();
        SetupNextAttack();
        enemy.visuals.EnableWeaponTrail(false);
    }

    private void SetupNextAttack()
    {
        int recoveryIndex = PlayerClose() ? 1 : 0;
        enemy.anim.SetFloat("RecoveryIndex", recoveryIndex);
        enemy.attackData = UpdateAttackData();
    }

    private bool PlayerClose() => Vector3.Distance(enemy.transform.position,enemy.player.position) <= 1;
    private AttackData_EnemyMelee UpdateAttackData()
    {
        List<AttackData_EnemyMelee> validAttacks = new List<AttackData_EnemyMelee>(enemy.attackList);


        if(PlayerClose())
        {
            validAttacks.RemoveAll(parameter => parameter.attackType == AttackType_Melee.Charge);
        }

        int random = Random.Range(0, validAttacks.Count);
            
        
        return validAttacks[random];
    }
}
