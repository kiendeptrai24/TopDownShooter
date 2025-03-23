using System;
using System.Collections.Generic;
using UnityEngine;


public enum BossWeaponType{ Flamethrower, Hummer}
public class Enemy_Boss : Enemy
{
    public BossWeaponType bossWeaponType;
    [Header("Boss destails")]
    public float actionCooldown = 10;
    public float attackRange;
    [Header("Ability")]
    public float minAbilityDistance;
    public float abilityCooldown;
    private float lastTimeUseAbility;
    
    [Header("Flamethrower")]
    public int flameDamage;
    public float flameDamageCooldown;
    public ParticleSystem flamethrower;
    public float flamethrowDuration;
    public bool flamethrowActive {get; private set;}
    [Header("Hummer")]
    public int hummerAttiveDamage;
    public GameObject activationPrefab;
    [SerializeField] private float hummerRadius;


    [Header("Jump attack")]
    public int jumpAttackDamage;
    public float jumpAttackCooldown = 10; 
    private float lastTimeJumped;
    public float travelTimeToTarget = 1;
    public float minJumpDistanceRequired;
    [Space]
    public float impactRadius = 2.5f;
    public float impactPower =5;
    public Transform impactPoint;
    [SerializeField] private float upforceMultipler =10;
    [Space]
    [SerializeField] private LayerMask whatToIngore;

    [Header("Attack")]
    [SerializeField] private int meleeAttackDamage;
    [SerializeField] private Transform[] damagePoints;
    [SerializeField] private float attackCheckRadius;
    [SerializeField] private GameObject melleeAttackFx;

    public Enemy_BossVisuals bossVisuals {get; private set;}
    
    protected override void Awake()
    {
        base.Awake();

        statesDirtionary = EnemyStateFactory.Instance.CreateStateByType(this,stateMachine,EnemyArchetypes.Boss);
        bossVisuals = GetComponent<Enemy_BossVisuals>();
    }
    
    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(GetState<IdleState_Boss>());
    }
    protected override void Update()
    {
        base.Update();

        if(Input.GetKeyDown(KeyCode.Alpha6))
            ChangeToDancerState(0);
        else if(Input.GetKeyDown(KeyCode.Alpha7))
            ChangeToDancerState(1);
        else if(Input.GetKeyDown(KeyCode.Alpha8))
            ChangeToDancerState(2);
        else if(Input.GetKeyDown(KeyCode.Alpha9))
            ChangeToDancerState(3);
        if(ShouldEnterBattleMode())
            EnterBattleMode();
        
        MeleeAttackCheck(damagePoints,attackCheckRadius,melleeAttackFx,meleeAttackDamage);
    }
    

    private void ChangeToDancerState(int index)
    {
        if(stateMachine.GetCurrentState() == GetState<DancerState_Boss>())
        {
            AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
            anim.Play(stateInfo.shortNameHash, 0, 0f);
        }
        anim.SetFloat("DancerIndex",index);
        stateMachine.ChangeState(GetState<DancerState_Boss>());
    }

    public void ActivateFlamethrower(bool activate)
    {
        flamethrowActive = activate;
        if(!activate)
        {
            flamethrower.Stop();
            anim.SetTrigger("StopFlamethrower");
            return;
        }

        var mainModule = flamethrower.main;
        var extraModule = flamethrower.transform.GetChild(0).GetComponent<ParticleSystem>().main;

        mainModule.duration = flamethrowDuration;
        extraModule.duration = flamethrowDuration;

        flamethrower.Clear();
        flamethrower.Play();
    }
    public void ActivateHummer()
    {
        GameObject newActivation = ObjectPool.Instance.GetObject(activationPrefab, impactPoint);
        ObjectPool.Instance.ReturnObject(newActivation,1);

        MassDamage(damagePoints[0].position, hummerRadius,hummerAttiveDamage);
    }
    public bool CanDoAbility()
    {
        bool playerWithinDistance = Vector3.Distance(transform.position,player.position) < minAbilityDistance;
        if(playerWithinDistance == false)
            return false;
        if(Time.time > lastTimeUseAbility + abilityCooldown + flamethrowDuration)
        {
            return true;
        }
        return false;
    }
    public void JumpImpact()
    {
        Transform impactPoint = this.impactPoint;
        if(impactPoint == null)
            impactPoint = transform;
        MassDamage(impactPoint.position,impactRadius,jumpAttackDamage);
    }
    public void MassDamage(Vector3 impactPoint, float impactRadius,int damage)
    {
        HashSet<GameObject> uniqueEntities = new HashSet<GameObject>();
        Collider[] colliders = Physics.OverlapSphere(impactPoint, impactRadius, ~whatIsAlly);
        foreach (Collider hit in colliders)
        {
            IDamagable damagable = hit.GetComponent<IDamagable>();
            if (damagable != null)
            {
                GameObject rootEntity = hit.transform.root.gameObject;
                if (uniqueEntities.Add(rootEntity) == false)
                    continue;

                damagable.TakeDamage(damage);

            }
            ApplyPhycicalForceTo(impactPoint, impactRadius, hit);
        }

    }

    private void ApplyPhycicalForceTo(Vector3 impactPoint, float impactRadius, Collider hit)
    {
        Rigidbody rb = hit.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddExplosionForce(impactPower, impactPoint, impactRadius, upforceMultipler, ForceMode.Impulse);
        }
    }

    public void SetAbilityOnCooldown() => lastTimeUseAbility  = Time.time;
    public void SetJumpAttackOnCooldown() => lastTimeJumped = Time.time;
    public override void EnterBattleMode()
    {
        if(inBattleMode)
            return;
        base.EnterBattleMode();
        stateMachine.ChangeState(GetState<MoveState_Boss>());
    }
    public bool CanDoJumpAttack()
    {
        float distanceToPlayer = Vector3.Distance(player.position,transform.position);
        if(distanceToPlayer < minJumpDistanceRequired)
            return false;
        if(Time.time > lastTimeJumped + jumpAttackCooldown && IsPlayerInClearSight())
        {
            return true;
        }
        return false;
    }
    public bool IsPlayerInClearSight()
    {
        Vector3 myPos = transform.position + new Vector3(0, 1.5f, 0);
        Vector3 playerPos = player.position + Vector3.up;
        Vector3 directionToPlayer = (playerPos - myPos).normalized;
        if(Physics.Raycast(myPos, directionToPlayer, out RaycastHit hit, 100, ~whatToIngore))
        {
            if(hit.transform.root == player.root)
                return true;
        }
        return false;
    }
    public override void Die()
    {
        base.Die();
        if(stateMachine.GetCurrentState() != GetState<DeadState_Boss>())
            stateMachine.ChangeState(GetState<DeadState_Boss>());
    }
    public bool PlayerInAttackRange() => Vector3.Distance(transform.position, player.position) < attackRange;

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireSphere(transform.position,attackRange);
        if(player != null)
        {
            Vector3 myPos = transform.position + new Vector3(0, 1.5f, 0);
            Vector3 playerPos = player.position + Vector3.up;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(myPos,playerPos);
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,impactRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position,minAbilityDistance);
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, minJumpDistanceRequired);
        
        if(damagePoints.Length > 0)
        {
            foreach (var damagePoint in damagePoints)
            {
                Gizmos.DrawWireSphere(damagePoint.position, attackCheckRadius);
            }
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(damagePoints[0].position, hummerRadius);
        }
    }
}
