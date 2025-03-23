using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum EnemyType
{
    Melee,
    Ranged,
    Boss,
    Random
}

[RequireComponent(typeof(NavMeshAgent))]
public abstract class Enemy : MonoBehaviour
{
    public EnemyType enemyType;
    public LayerMask whatIsAlly;
    public LayerMask WhatIsPlayer;
    [Header("Idle data")]
    public float idleTimer =3;
    public float aggressionRange;
    [Header("Move data")]
    public float walkSpeed = 1.5f;
    public float runSpeed = 3f;
    public float turnSpeed;
    private bool manualMovement;
    private bool manualRotation;

    [SerializeField] private Transform[] patrolPoints;
    private Vector3[] patrolPointsPosition;
    private int currentPatrolIndex = 0;
    public bool inBattleMode { get; private set; }
    protected bool isMeleeAttackReady;

    public Transform player { get; private set; }
    public Animator anim { get; private set; }

    public NavMeshAgent agent { get; private set; }


    public EnemyStateMachine stateMachine { get; private set; }
    public Enemy_Visuals visuals { get; private set; }

    public Ragdoll ragdoll { get; private set; }

    public HealthController health {get; private set;}
    public Enemy_DropController dropController {get; private set;}  
    public AudioManager audioManager {get; private set;}
    protected Dictionary<Type, EnemyState> statesDirtionary = new Dictionary<Type, EnemyState>();

    protected virtual void Awake() 
    {

        player = GameObject.Find("Player").GetComponent<Transform>();
        stateMachine = new EnemyStateMachine();

        health = GetComponent<Enemy_Health>();
        ragdoll = GetComponent<Ragdoll>();
        visuals = GetComponent<Enemy_Visuals>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        dropController = GetComponent<Enemy_DropController>();
        
    }
    protected virtual void Start()
    {
        InitialzePatrolPoints();
        audioManager = AudioManager.Instance;
    }

    protected virtual void Update()
    {
        stateMachine.GetCurrentState().Update();
        if(ShouldEnterBattleMode())
            EnterBattleMode();
    }
    public EnemyState GetState<T>() where T : EnemyState
    {
        return statesDirtionary[typeof(T)];
    }
    public virtual void MakeEnemyVIP()
    {
        int additionalHealth = Mathf.RoundToInt(health.currentHealth * 1.5f);
        health.currentHealth += additionalHealth;

        transform.localScale = transform.localScale * 1.15f;
    }
    protected virtual void InitializePerk() 
    {
        
    }
    protected bool ShouldEnterBattleMode()
    {
        if(IsPlayerInAggressionRange() && !inBattleMode)
        {
            EnterBattleMode();
            return true;
        }
        return false;

    }
    public virtual void EnterBattleMode()
    {
        inBattleMode = true;
    }
    public virtual void GetHit(int damage)
    {
        health.ReduceHealth(damage);
        if(health.ShouldDie())
            Die();

    }
    public virtual void Die()
    {
        anim.enabled = false;
        agent.isStopped = true;
        agent.enabled = false;

        ragdoll.RagdollActive(true);

        dropController.DropItem();
        MissionObject_HuntTarget  huntTarget = GetComponent<MissionObject_HuntTarget>();
        huntTarget?.InvokeOntargetKilled();
    }
    public virtual void MeleeAttackCheck(Transform[] damagePoints, float attackCheckRadius,GameObject PrefabFX,int damage)
    {
        if(isMeleeAttackReady == false)
            return;

        foreach (Transform attackPoint in damagePoints)
        {
            Collider[] detectedHits = 
                Physics.OverlapSphere(attackPoint.position, attackCheckRadius, WhatIsPlayer);

            for (int i = 0; i < detectedHits.Length; i++)
            {
                // Takedamge
                IDamagable damagable = detectedHits[i].GetComponent<IDamagable>();

                if(damagable != null)
                {
                    damagable.TakeDamage(damage);
                    isMeleeAttackReady = false;
                    GameObject newAttackFx = ObjectPool.Instance.GetObject(PrefabFX,attackPoint);
                    ObjectPool.Instance.ReturnObject(newAttackFx,1);
                    return;
                }
            }
        }
    }
    public void EnableAttackCheck(bool enable) => isMeleeAttackReady = enable;

    public virtual void BulletImpact(Vector3 force, Vector3 hitPoint,Rigidbody rb)
    {
        if(health.ShouldDie())
            StartCoroutine(DeadImpactCoroutine(force, hitPoint, rb));
    }
    private IEnumerator DeadImpactCoroutine(Vector3 force, Vector3 hitPoint,Rigidbody rb)
    {
        yield return new WaitForSeconds(.1f);

        rb.AddForceAtPosition(force, hitPoint, ForceMode.Impulse);
    }
    #region Patrol logic
    public Vector3 GetPatrolDestination()
    {
        Vector3 destination = patrolPointsPosition[currentPatrolIndex++];
        if(currentPatrolIndex == patrolPointsPosition.Length)
            currentPatrolIndex = 0;
        return destination;
    }
    private void InitialzePatrolPoints()
    {
        patrolPointsPosition = new Vector3[patrolPoints.Length];
        for (int i = 0; i < patrolPoints.Length; i++)
        {
            patrolPointsPosition[i] = patrolPoints[i].position;
            patrolPoints[i].gameObject.SetActive(false);
        }
    }
    #endregion
    public void FaceTarget(Vector3 target,float turnSpeed = 0)
    {
        if(turnSpeed == 0)
            turnSpeed = this.turnSpeed;
        Quaternion targetQuaternion = Quaternion.LookRotation(target - transform.position);

        Vector3 currentEulerAngels = transform.rotation.eulerAngles;

        float yRotation = Mathf.LerpAngle(currentEulerAngels.y, targetQuaternion.eulerAngles.y,turnSpeed * Time.deltaTime);


        transform.rotation = Quaternion.Euler(currentEulerAngels.x, yRotation,currentEulerAngels.z);
    }
    
    
    #region Animation Event
    public virtual void AbilityTrigger()
    {
        stateMachine.GetCurrentState().AbilityTrigger();
    }
    
    public void ActiveManualMovement(bool manualMovement) => this.manualMovement = manualMovement;
    public bool ManualMovementActive() => manualMovement;
    public void ActiveManualRotation(bool manualRotation) => this.manualRotation = manualRotation;

    public bool manualRotationActive() => manualRotation;

    
    public void AnimationTrigger() => stateMachine.GetCurrentState().AnimationTrigger();
    #endregion
    public bool IsPlayerInAggressionRange() => Vector3.Distance(transform.position, player.position) < aggressionRange;
    protected virtual void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position,aggressionRange);
    }

}
