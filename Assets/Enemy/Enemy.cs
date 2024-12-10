using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Idle info")]
    public float idleTimer =3;
    [Header("Move info")]
    public float moveSpeed;
    [SerializeField] private Transform[] patrolPoints;
    private int currentPatrolIndex;

    public Animator amin { get; private set; }

    public NavMeshAgent agent { get; private set; }


    public EnemyStateMachine stateMachine { get; private set; }

    protected virtual void Awake() 
    {
        stateMachine = new EnemyStateMachine();


        agent = GetComponent<NavMeshAgent>();
        amin = GetComponentInChildren<Animator>();
    }
    protected virtual void Start()
    {
        InitialzePatrolPoints();
    }

    protected virtual void Update()
    {
        stateMachine.currentState.Update();
    }
    public Vector3 GetPatrolDestination()
    {
        Vector3 destination = patrolPoints[currentPatrolIndex].transform.position;
        currentPatrolIndex++;
        if(currentPatrolIndex >= patrolPoints.Length)
            currentPatrolIndex = 0;
        return destination;
    }

    private void InitialzePatrolPoints()
    {
        foreach (Transform t in patrolPoints)
        {
            t.parent = null;
        }
    }

}
