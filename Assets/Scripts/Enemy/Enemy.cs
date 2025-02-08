using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class Enemy : MonoBehaviour
{
    public LayerMask whatIsAlly;
    public LayerMask WhatIsPlayer;
    public int healthPoint = 25;
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

    public Transform player { get; private set; }
    public Animator anim { get; private set; }

    public NavMeshAgent agent { get; private set; }


    public EnemyStateMachine stateMachine { get; private set; }
    public Enemy_Visuals visuals { get; private set; }

    public Ragdoll ragdoll { get; private set; }

    public HealthController health {get; private set;}
    protected virtual void Awake() 
    {

        player = GameObject.Find("Player").GetComponent<Transform>();
        stateMachine = new EnemyStateMachine();

        health = GetComponent<Enemy_Health>();
        ragdoll = GetComponent<Ragdoll>();
        visuals = GetComponent<Enemy_Visuals>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
    }
    protected virtual void Start()
    {
        InitialzePatrolPoints();
    }

    protected virtual void Update()
    {
        stateMachine.currentState.Update();
        if(ShouldEnterBattleMode())
            EnterBattleMode();
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
    public virtual void GetHit()
    {
        health.ReduceHealth();
        if(health.ShouldIde())
            Die();
    }
    public virtual void Die()
    {

    }
    public virtual void BulletImpact(Vector3 force, Vector3 hitPoint,Rigidbody rb)
    {
        if(health.ShouldIde())
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
        stateMachine.currentState.AbilityTrigger();
    }
    
    public void ActiveManualMovement(bool manualMovement) => this.manualMovement = manualMovement;
    public bool ManualMovementActive() => manualMovement;
    public void ActiveManualRotation(bool manualRotation) => this.manualRotation = manualRotation;

    public bool manualRotationActive() => manualRotation;

    
    public void AnimationTrigger() => stateMachine.currentState.AnimationTrigger();
    #endregion
    public bool IsPlayerInAggressionRange() => Vector3.Distance(transform.position, player.position) < aggressionRange;
    protected virtual void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position,aggressionRange);
    }

}
