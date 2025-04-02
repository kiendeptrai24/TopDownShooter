using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[Serializable]
public struct AttackData_EnemyMelee
{
    public int attackDamage;
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
    
    [Header("Enemy Setting")]
    public EnemyMelee_Type meleeType;
    public Enemy_MeleeWeaponType weaponType;
    [Header("Shield")]
    public int shieldDurability;
    public Transform shieldTranform;
    [Header("Dodge")]
    public float dodgeCooldown;
    private float lastTimeDodge;

    [Header("Axe throw ability")]
    public int axeDamage;
    public GameObject axePrefab;
    public float axeFlySpeed;
    public float aimTimer;
    public float axeThrowCooldown;
    public float lastTimeAxeThrown;
    public Transform axeStartPoint;

    [Header("Attack data")] 
    public AttackData_EnemyMelee attackData;
    public List<AttackData_EnemyMelee> attackList;
    public Enemy_WeaponModel currentWeapon;
    [Space]
    [SerializeField] private GameObject meleeAttackFX;
    public Enemy_MeleeSFX meleeSFX {get; private set;}

    // private Dictionary<Type, EnemyState> meleeStates = new Dictionary<Type, EnemyState>();
    protected override void Awake()
    {
        base.Awake();
        meleeSFX = GetComponent<Enemy_MeleeSFX>();

       statesDirtionary = EnemyStateFactory.Instance.CreateStateByType(this,stateMachine,EnemyArchetypes.Melee);

    }
    protected override void Start()
    {
        base.Start();
        
        ResetCooldown();

        stateMachine.Initialize(GetState<IdleState_Melee>());
        InitializePerk();

        visuals.SetupLook();
        UpdateAttackData();
    }
    protected override void Update()
    {
        base.Update();
        stateMachine.GetCurrentState().Update();
        MeleeAttackCheck(currentWeapon.damagePoints,currentWeapon.attackRadius,meleeAttackFX,attackData.attackDamage);
    }

    //Checking if the weapon attacks the player
   
    public override void EnterBattleMode()
    {
        if(inBattleMode)
            return;
        base.EnterBattleMode();
        stateMachine.ChangeState(GetState<RecoveryState_Melee>());
    }
    public override void GetHit(int damage)
    {
        base.GetHit(damage);
    }
    public override void Die()
    {
        base.Die();
        if(stateMachine.GetCurrentState() != GetState<DeadState_Melee>())
            stateMachine.ChangeState(GetState<DeadState_Melee>());
    }

    protected override void InitializePerk()
    {
        if(meleeType == EnemyMelee_Type.AxeThrow)
        {
            weaponType = Enemy_MeleeWeaponType.Throw;
        }
        if(meleeType == EnemyMelee_Type.Shield)
        {
            anim.SetFloat("ChaseIndex", 1);
            shieldTranform.gameObject.SetActive(true);
            weaponType = Enemy_MeleeWeaponType.OneHand;

        }
        if(meleeType == EnemyMelee_Type.Dodge)
        {
            weaponType = Enemy_MeleeWeaponType.Unarmed;
        }
    }
    public bool PlayerInAttackRange() => Vector3.Distance(transform.position, player.position) < attackData.attackRange;
    public void ActivateDodgeRoll()
    {
        if(meleeType != EnemyMelee_Type.Dodge)
            return;
        if(stateMachine.GetCurrentState() != GetState<ChaseState_Melee>())
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
        visuals.currentWeaponModel.gameObject.SetActive(false);

    }
    

    
    
    public void ThrowAxe()
    {
        GameObject newAxe = ObjectPool.Instance.GetObject(axePrefab,axeStartPoint);
        newAxe.GetComponent<EnemyAxe>().SetupAxe(axeFlySpeed,player,aimTimer,axeDamage);
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
    private void ResetCooldown()
    {
        lastTimeDodge -= dodgeCooldown;
        lastTimeAxeThrown -= axeThrowCooldown;
    }
    public void UpdateAttackData()
    {
        currentWeapon = visuals.currentWeaponModel.GetComponent<Enemy_WeaponModel>();

        if(currentWeapon.weaponData != null)
        {
            attackList = new List<AttackData_EnemyMelee>(currentWeapon.weaponData.attackData);
            turnSpeed = currentWeapon.weaponData.turnSpeed;
        }

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
