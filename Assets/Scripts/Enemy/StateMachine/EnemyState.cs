
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyState 
{
    protected Enemy enemyBase;
    protected EnemyStateMachine stateMachine;

    protected string animBoolName;
    protected float stateTimer;
    protected bool triggerCalled;


    public EnemyState(Enemy _enemyBase, EnemyStateMachine _stateMachine,string _animBoolName)
    {
        enemyBase = _enemyBase;
        stateMachine = _stateMachine;
        animBoolName = _animBoolName;
    }
    public virtual void Enter()
    {
        enemyBase.anim.SetBool(animBoolName,true);
        triggerCalled = false;
    }
    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }
    public virtual void Exit()
    {
        enemyBase.anim.SetBool(animBoolName,false);
    }
    public void AnimationTrigger() => triggerCalled = true;
    public virtual void AbilityTrigger()
    {

    }
    protected Vector3 GetNextPathPoints()
    {
        NavMeshAgent agent = enemyBase.agent;
        NavMeshPath path = agent.path;
        if(path.corners.Length < 2)
            return agent.destination;
        
        for (int i = 0; i < path.corners.Length; i++)
        {
            if(Vector3.Distance(agent.transform.position,path.corners[i]) < 1)
                return path.corners[i + 1];
        }
        return agent.destination;
    }
}
