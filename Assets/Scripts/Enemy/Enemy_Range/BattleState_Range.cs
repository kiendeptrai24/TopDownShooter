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
    private float coverCheckTimer=.5f;
    public BattleState_Range(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemyBase as Enemy_Range;
    }
    public override void Enter()
    {
        base.Enter();
        enemy.agent.isStopped = true;
        enemy.agent.velocity = Vector3.zero;
        bulletsPerAttack = enemy.weaponData.GetBulletsPerAttack();
        weaponCooldown = enemy.weaponData.GetWeaponCooldown();
        enemy.visuals.EnableIK(true, true);
    }
    public override void Update()
    {
        base.Update();
        if(enemy.IsPlayerInAggressionRange() == false)
            stateMachine.ChangeState(enemy.advancePlayerState);
        ChangeCoverIfShould();

        enemy.FaceTarget(enemy.player.position);
        if (WeaponOutOfBullet())
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

    private void ChangeCoverIfShould()
    {
        if(enemy.coverPerk != CoverPerk.CanTakeAndChangeCover)
            return;

        coverCheckTimer -= Time.deltaTime;

        if(coverCheckTimer < 0)
        {
            coverCheckTimer = .5f; // checking cover each of 0.5 seconds
            if(IsPlayerInClearSight() || IsPlayerClose()) // if player is close or in clear sight
            {
                
                if (enemy.CanGetCover())
                    stateMachine.ChangeState(enemy.runToCoverState);
            }
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

    #region Weapon Region
    private bool WeaponOnCooldown() => Time.time > lastTimeShoot + weaponCooldown;
    private bool WeaponOutOfBullet() => bulletsShot >= bulletsPerAttack;
    private bool CanShoot() =>  Time.time > lastTimeShoot + 1 / enemy.weaponData.fireRate; //firerate per second

    private void Shoot()
    {
        enemy.FireSingleBullet();
        lastTimeShoot = Time.time;
        bulletsShot++;
    }
    #region Cover system regoin
    
    private bool IsPlayerClose()
    {
        return Vector3.Distance(enemy.transform.position, enemy.player.transform.position) < enemy.safeDistance;
    }
    private bool IsPlayerInClearSight()
    {
        
        Vector3 directionToPlayer = enemy.player.transform.position - enemy.transform.position;
        Vector3 yOffset = new Vector3(0, .02f, 0);
        if(Physics.Raycast(enemy.transform.position + yOffset, directionToPlayer + yOffset, out RaycastHit hit))
        {
            return hit.collider.gameObject.GetComponentInParent<Player>() != null;
        }
        return false;
    }

    #endregion

    #endregion

}
