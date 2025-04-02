using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EnemyArchetypes {Melee, Range, Boss }
public class EnemyStateFactory 
{
    private static EnemyStateFactory _instance;
    public static EnemyStateFactory Instance => _instance ??= new EnemyStateFactory();

    private EnemyStateFactory() { } // Đảm bảo không thể tạo bằng new
    public Dictionary<Type, EnemyState> CreateStateByType(Enemy enemy, EnemyStateMachine stateMachine,EnemyArchetypes enemyArchetypes)
    {
        Dictionary<Type, EnemyState> statesDirtionary = new Dictionary<Type, EnemyState>();
        switch (enemyArchetypes)
        {
            case EnemyArchetypes.Melee:
                CreateMelee(enemy, stateMachine, statesDirtionary);
                break;
            case EnemyArchetypes.Range:
                CreateRange(enemy, stateMachine, statesDirtionary);
                break;
            case EnemyArchetypes.Boss:
                CreateBoss(enemy, stateMachine, statesDirtionary);
                break;
        }
        return statesDirtionary;
    }

    private static void CreateBoss(Enemy enemy, EnemyStateMachine stateMachine, Dictionary<Type, EnemyState> statesDirtionary)
    {
        statesDirtionary.Add(typeof(IdleState_Boss), new IdleState_Boss(enemy, stateMachine, "Idle"));
        statesDirtionary.Add(typeof(MoveState_Boss), new MoveState_Boss(enemy, stateMachine, "Move"));
        statesDirtionary.Add(typeof(AttackState_Boss), new AttackState_Boss(enemy, stateMachine, "Attack"));
        statesDirtionary.Add(typeof(JumpAttackState_Boss), new JumpAttackState_Boss(enemy, stateMachine, "JumpAttack"));
        statesDirtionary.Add(typeof(DancerState_Boss), new DancerState_Boss(enemy, stateMachine, "Dancer"));
        statesDirtionary.Add(typeof(AbilityState_Boss), new AbilityState_Boss(enemy, stateMachine, "Ability"));
        statesDirtionary.Add(typeof(DeadState_Boss), new DeadState_Boss(enemy, stateMachine, "Idle"));
    }

    private static void CreateRange(Enemy enemy, EnemyStateMachine stateMachine, Dictionary<Type, EnemyState> statesDirtionary)
    {
        statesDirtionary.Add(typeof(IdleState_Range), new IdleState_Range(enemy, stateMachine, "Idle"));
        statesDirtionary.Add(typeof(MoveState_Range), new MoveState_Range(enemy, stateMachine, "Move"));
        statesDirtionary.Add(typeof(BattleState_Range), new BattleState_Range(enemy, stateMachine, "Battle"));
        statesDirtionary.Add(typeof(RunToCoverState_Range), new RunToCoverState_Range(enemy, stateMachine, "Run"));
        statesDirtionary.Add(typeof(AdvancePlayerState_Range), new AdvancePlayerState_Range(enemy, stateMachine, "Advance"));
        statesDirtionary.Add(typeof(ThrowGrenadeState_Range), new ThrowGrenadeState_Range(enemy, stateMachine, "ThrowGrenade"));
        statesDirtionary.Add(typeof(DeadState_Range), new DeadState_Range(enemy, stateMachine, "Idle"));
    }

    private static void CreateMelee(Enemy enemy, EnemyStateMachine stateMachine, Dictionary<Type, EnemyState> statesDirtionary)
    {
        statesDirtionary.Add(typeof(IdleState_Melee), new IdleState_Melee(enemy, stateMachine, "Idle"));
        statesDirtionary.Add(typeof(MoveState_Melee), new MoveState_Melee(enemy, stateMachine, "Move"));
        statesDirtionary.Add(typeof(RecoveryState_Melee), new RecoveryState_Melee(enemy, stateMachine, "Recovery"));
        statesDirtionary.Add(typeof(ChaseState_Melee), new ChaseState_Melee(enemy, stateMachine, "Chase"));
        statesDirtionary.Add(typeof(AttackState_Melee), new AttackState_Melee(enemy, stateMachine, "Attack"));
        statesDirtionary.Add(typeof(DeadState_Melee), new DeadState_Melee(enemy, stateMachine, "Idle"));
        statesDirtionary.Add(typeof(AbilityState_Melee), new AbilityState_Melee(enemy, stateMachine, "AxeThrow"));
    }


}
