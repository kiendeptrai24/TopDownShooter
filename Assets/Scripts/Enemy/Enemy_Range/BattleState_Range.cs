using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleState_Range : EnemyState
{
    private Enemy_Range enemy;
    private float lastTimeShoot= -10;
    private int bulletsShot =0;
    public BattleState_Range(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemyBase as Enemy_Range;
    }
    public override void Enter()
    {
        base.Enter();
        enemy.agent.isStopped = true;
        enemy.agent.velocity = Vector3.zero;
        enemy.visuals.EnableIK(true);
    }
    public override void Update()
    {
        base.Update();

        enemy.FaceTarget(enemy.player.position);
        if(WeaponOutOfBullet())
        {
            if (WeaponOnCooldown())
                AttempToResetWeapon();
            return;
        }
        if (CanShoot())
        {
            Shoot();
        }
    }
    public override void Exit()
    {
        base.Exit();
        enemy.visuals.EnableIK(false);

    }

    private void AttempToResetWeapon() => bulletsShot = 0;


    private bool WeaponOnCooldown() => Time.time > lastTimeShoot + enemy.weaponCooldown;
    private bool CanShoot() =>  Time.time > lastTimeShoot + 1 / enemy.fireRate; //firerate per second
    private bool WeaponOutOfBullet() => bulletsShot >= enemy.bulletToShoot;

    private void Shoot()
    {
        enemy.FireSingleBullet();
        lastTimeShoot = Time.time;
        bulletsShot++;
    }

}
