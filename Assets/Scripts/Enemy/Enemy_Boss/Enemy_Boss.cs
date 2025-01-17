using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class Enemy_Boss : Enemy
{
    [Header("Boss destails")]
    public float actionCooldown = 10;
    public float attackRange;
    [Header("Ability")]
    public ParticleSystem flamethrower;
    public float abilityCooldown;
    private float lastTimeUseAbility;
    public float flamethrowDuration;
    public bool flamethrowActive {get; private set;}
    [Header("Jump attack")]
    public float jumpAttackCooldown = 10; 
    private float lastTimeJumped;
    public float travelTimeToAttack = 1;
    public float minJumpDistanceRequired;
    [Space]
    [SerializeField] private LayerMask whatToIngore;
    public IdleState_Boss idleState {get; private set;}
    public MoveState_Boss moveState  {get; private set;}  
    public AttackState_Boss attackState {get; private set;}
    public JumpAttackState_Boss jumpAttackState {get; private set;}
    public DancerState_Boss dancerState {get; private set;}
    public AbilityState_Boss abilityState {get; private set;}


    protected override void Awake()
    {
        base.Awake();
        idleState = new IdleState_Boss(this,stateMachine,"Idle");
        moveState = new MoveState_Boss(this,stateMachine,"Move");
        attackState = new AttackState_Boss(this,stateMachine,"Attack");
        jumpAttackState = new JumpAttackState_Boss(this, stateMachine,"JumpAttack");
        dancerState = new DancerState_Boss(this,stateMachine,"Dancer");
        abilityState = new AbilityState_Boss(this, stateMachine, "Ability");
    }
    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
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
    }
    private void ChangeToDancerState(int index)
    {
        if(stateMachine.currentState == dancerState)
        {
            AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
            anim.Play(stateInfo.shortNameHash, 0, 0f);
        }
        anim.SetFloat("DancerIndex",index);
        stateMachine.ChangeState(dancerState);
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
    public bool CanDoAbility()
    {
        if(Time.time > lastTimeUseAbility + abilityCooldown + flamethrowDuration)
        {
            return true;
        }
        return false;
    }
    public void SetAbilityOnCooldown() => lastTimeUseAbility  = Time.time;
    public void SetJumpAttackOnCooldown() => lastTimeJumped = Time.time;
    public override void EnterBattleMode()
    {
        base.EnterBattleMode();
        stateMachine.ChangeState(moveState);
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
            if(hit.transform == player || hit.transform.parent == player)
                return true;
        }
        return false;
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
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, minJumpDistanceRequired);
    }
}
