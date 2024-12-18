using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public struct AttackData
{
    public string attackName;
    public float attackRange;
    public float moveSpeed;
    public float attackIndex;
    [Range(1,2)]
    public float animationSpeed;
    public AttackType_Melee attackType;
}
public enum AttackType_Melee {Close, Charge}
public enum EnemyMelee_Type {Recular, Shield, Dodge, AxeThrow}
public class Enemy_Melee : Enemy
{
    #region States
    public IdleState_Melee idleState { get; private set; }
    public MoveState_Melee moveState { get; private set; }
    public RecoveryState_Melee recoveryState{ get; private set; }
    public ChaseState_Melee chaseState { get; private set; }
    public AttackState_Melee attackState{ get; private set; }
    public DeadState_Melee deadState { get; private set; }
    public AbilityState_Melee abilityState { get; private set; }
    #endregion
    
    [Header("Enemy Setting")]
    public EnemyMelee_Type meleeType;
    public Transform shieldTranform;
    public float dodgeCooldown;
    private float lastTimeDodge;

    [Header("Axe throw ability")]
    public GameObject axePrefab;
    public float axeFlySpeed;
    public float aimTimer;
    public float axeThrowCooldown;
    public float lastTimeAxeThrown;
    public Transform axeStartPoint;

    [Header("Attack data")]
    public AttackData attackData;
    public List<AttackData> attackList;

    [SerializeField] private Transform hiddenWeapon;
    [SerializeField] private Transform pullWeapon;

    protected override void Awake()
    {
        base.Awake();
        idleState = new IdleState_Melee(this,stateMachine,"Idle");
        moveState = new MoveState_Melee(this,stateMachine,"Move");
        recoveryState = new RecoveryState_Melee(this,stateMachine, "Recovery");
        chaseState = new ChaseState_Melee(this,stateMachine,"Chase");
        attackState = new AttackState_Melee(this, stateMachine, "Attack");
        deadState = new DeadState_Melee(this,stateMachine,"Idle");
        abilityState = new AbilityState_Melee(this,stateMachine,"AxeThrow");

    }
    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
        InitializeSpeciality();
    }
    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
        if(ShouldEnterBattleMode())
            EnterBattleMode();
    }
    public override void EnterBattleMode()
    {
        if(inBattleMode)
            return;
        base.EnterBattleMode();
        stateMachine.ChangeState(recoveryState);
    }
    public override void GetHit()
    {
        base.GetHit();
        if(healthPoint <= 0)
            stateMachine.ChangeState(deadState);
    }
    public void PulledWeapon()
    {
        hiddenWeapon.gameObject.SetActive(false);
        pullWeapon.gameObject.SetActive(true);

    }
    private void InitializeSpeciality()
    {
        if(meleeType == EnemyMelee_Type.Shield)
        {
            anim.SetFloat("ChaseIndex", 1);
            shieldTranform.gameObject.SetActive(true);
        }
    }
    public bool PlayerInAttackRange() => Vector3.Distance(transform.position, player.position) < attackData.attackRange;
    public void ActivateDodgeRoll()
    {
        if(meleeType != EnemyMelee_Type.Dodge)
            return;
        if(stateMachine.currentState != chaseState)
            return;
        if(Vector3.Distance(transform.position, player.position) < 2f)
            return;
        float dodgeAnimationDuration = GetAnimationClipDuration("Dodge roll");
        if(Time.time > lastTimeDodge +dodgeAnimationDuration + dodgeCooldown)
        {
            anim.SetTrigger("Dodge");
            lastTimeDodge = Time.time;
        }
    }
    public override void AbilityTrigger()
    {
        base.AbilityTrigger();
        pullWeapon.gameObject.SetActive(false);

    }
    public bool CanThrowAxe()
    {
        if(meleeType != EnemyMelee_Type.AxeThrow)
            return false;
        if(Time.time > lastTimeAxeThrown + axeThrowCooldown)
        {
            lastTimeAxeThrown = Time.time;
            return true;
        }
        
        return false;
        
    }
    private float GetAnimationClipDuration(string clipName)
    {
        AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if(clip.name == clipName)
                return clip.length;
        }
        Debug.Log(clipName + " animation not found!");
        return 0;
    }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position,attackData.attackRange);

    }

}
