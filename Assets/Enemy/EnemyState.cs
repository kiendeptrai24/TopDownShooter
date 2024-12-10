using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class EnemyState 
{
    protected Enemy enemyBase;
    protected EnemyStateMachine stateMachine;

    protected string animBoolName;
    protected float stateTimer;



    public EnemyState(Enemy _enemyBase, EnemyStateMachine _stateMachine,string _animBoolName)
    {
        enemyBase = _enemyBase;
        stateMachine = _stateMachine;
        animBoolName = _animBoolName;
    }
    public virtual void Enter()
    {
        enemyBase.amin.SetBool(animBoolName,true);
    }
    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
        Debug.Log(stateTimer);

    }
    public virtual void Exit()
    {
        enemyBase.amin.SetBool(animBoolName,false);
    }
}
