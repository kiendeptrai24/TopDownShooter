
using UnityEngine;

public class BattleState_Range : EnemyState
{
    private Enemy_Range enemy;
    private float lastTimeShoot= -10;
    private int bulletsShot =0;

    private int bulletsPerAttack;
    private float weaponCooldown;
    private float coverCheckTimer=.5f;
    private bool firstTimeAttack = true;
    public BattleState_Range(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemyBase as Enemy_Range;
    }
    public override void Enter()
    {
        base.Enter();
        SetupValuesForFirstAttack();
        enemy.agent.isStopped = true;
        enemy.agent.velocity = Vector3.zero;
        enemy.visuals.EnableIK(true, true);
        stateTimer = enemy.attackDelay;
    }

    

    public override void Update()
    {
        base.Update();
        if (enemy.IsSeeingPlayer())
            enemy.FaceTarget(enemy.aim.position);
        
        if(MustAdvancePlayer())
            stateMachine.ChangeState(enemy.GetState<AdvancePlayerState_Range>());
            
        if(enemy.CanThrowGrenade())
            stateMachine.ChangeState(enemy.GetState<ThrowGrenadeState_Range>());
        ChangeCoverIfShould();
        
        if(stateTimer > 0)
            return;


        if (WeaponOutOfBullet())
        {
            if (enemy.IsUnstoppable() && UnstoppableWalkReady())
            {
                enemy.advanceDuration = weaponCooldown;
                stateMachine.ChangeState(enemy.GetState<AdvancePlayerState_Range>());
            }
            if (WeaponOnCooldown())
                AttempToResetWeapon();
            return;
        }
        if (CanShoot() && enemy.IsAimOnPlayer())
        {
            Shoot();
        }
    }

    public override void Exit()
    {
        base.Exit();
        enemy.visuals.EnableIK(false, false);

    }
    private bool MustAdvancePlayer()
    {
        if(enemy.IsUnstoppable())
            return false;
        return enemy.IsPlayerInAggressionRange() == false && ReadyToLeaveCover();
    }


    private void AttempToResetWeapon()
    {
        bulletsShot = 0;
        bulletsPerAttack = enemy.weaponData.GetBulletsPerAttack();
        weaponCooldown = enemy.weaponData.GetWeaponCooldown();
    }
    private bool UnstoppableWalkReady()
    {
        float distanceToPlayer = Vector3.Distance(enemy.transform.position, enemy.player.transform.position);
        bool outOffStoppingDistance = distanceToPlayer > enemy.advanceStoppingDistance;
        bool unstoppableWalkOnCooldown = 
            Time.time < enemy.weaponData.minWeaponCooldown + (enemy.GetState<AdvancePlayerState_Range>() as AdvancePlayerState_Range).LastTimeAdvanced;
        return outOffStoppingDistance && unstoppableWalkOnCooldown == false;
    }
    #region Weapon Region

    private bool ReadyToLeaveCover()
    {
        return Time.time > enemy.minCoverTime + (enemy.GetState<RunToCoverState_Range>() as RunToCoverState_Range).lastTimeToCover;
    }
    private void ChangeCoverIfShould()
    {
        if(enemy.coverPerk != CoverPerk.CanTakeAndChangeCover)
            return;

        coverCheckTimer -= Time.deltaTime;

        if(coverCheckTimer < 0)
        {
            coverCheckTimer = .5f; // checking cover each of 0.5 seconds
            if(ReadyToChangeCover() && ReadyToLeaveCover()) // if player is close or in clear sight
            {
                
                if (enemy.CanGetCover())
                    stateMachine.ChangeState(enemy.GetState<RunToCoverState_Range>());
            }
        }
    }
    private bool ReadyToChangeCover()
    {
        bool inDanger = IsPlayerInClearSight() || IsPlayerClose();
        
        bool advanceTimeIsOver = Time.time > 
        (enemy.GetState<AdvancePlayerState_Range>() as AdvancePlayerState_Range)
            .LastTimeAdvanced + enemy.advanceDuration;
        return inDanger && advanceTimeIsOver;
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
            if(hit.transform.root == enemy.player.root)
                return true;
        }
        return false;
    }
    private void SetupValuesForFirstAttack()
    {
        if (firstTimeAttack)
        {
            //note
            enemy.aggressionRange = enemy.advanceStoppingDistance + 2;

            firstTimeAttack = false;
            bulletsPerAttack = enemy.weaponData.GetBulletsPerAttack();
            weaponCooldown = enemy.weaponData.GetWeaponCooldown();
        }
    }
    #endregion

    #endregion

}
