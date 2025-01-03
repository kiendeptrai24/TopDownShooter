using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleState_Range : EnemyState
{
    private Enemy_Range enemy;
    private float lastTimeShoot= -10;
    private int bulletsShot =0;

    private int bulletsPerAttack;
    private float weaponCooldown;
    public BattleState_Range(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemyBase as Enemy_Range;
    }
    public override void Enter()
    {
        base.Enter();
        bulletsPerAttack = enemy.weaponData.GetBulletsPerAttack();
        weaponCooldown = enemy.weaponData.GetWeaponCooldown();
        enemy.agent.isStopped = true;
        enemy.agent.velocity = Vector3.zero;
        enemy.visuals.EnableIK(true, true);
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
        enemy.visuals.EnableIK(false, false);

    }

    private void AttempToResetWeapon()
    {
        bulletsShot = 0;
        bulletsPerAttack = enemy.weaponData.GetBulletsPerAttack();
        weaponCooldown = enemy.weaponData.GetWeaponCooldown();
    }


    private bool WeaponOnCooldown() => Time.time > lastTimeShoot + weaponCooldown;
    private bool WeaponOutOfBullet() => bulletsShot >= bulletsPerAttack;
    private bool CanShoot() =>  Time.time > lastTimeShoot + 1 / enemy.weaponData.fireRate; //firerate per second

    private void Shoot()
    {
        enemy.FireSingleBullet();
        lastTimeShoot = Time.time;
        bulletsShot++;
    }

}
