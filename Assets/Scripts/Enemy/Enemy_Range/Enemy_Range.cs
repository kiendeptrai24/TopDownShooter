using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
public enum CoverPerk{ Unavailable, CanTakeCover, CanTakeAndChangeCover}
public enum UnstoppablePerk{ Unavailable, Unstoppable}
public enum GrenadePerk{ Unavailable, CanThrowGrenade} 
public class Enemy_Range : Enemy
{
    [Header("Enemy Perks")]
    public Enemy_RangeWeaponType weaponType;
    public CoverPerk coverPerk;
    public UnstoppablePerk unstoppablePerk;
    public GrenadePerk grenadePerk;
    [Header("Grenade perk")]
    public int grenadeDamage;
    public GameObject grenadePrefab;
    public float impactPower;
    public float explosionTimer = .75f;
    public float timeToTarget = 1.2f;
    public float grenadeCooldown;
    public float LastTimeGrenadeThrown = -10;
    [SerializeField] private Transform grenadeStartPoint;
    [Header("Advance perk")]
    public float advanceSpeed;
    public float advanceStoppingDistance;
    public float advanceDuration = 2.5f;

    [Header("Cover system")]
    public float minCoverTime = 2f;
    public float safeDistance;
    public CoverPoint lastCover { get; private set; }
    public CoverPoint currentCover { get; private set; }


    [Header("Weapon details")]
    public float attackDelay;
    public Enemy_RangeWeaponData weaponData;
    [Space]
    public Transform gunPoint;
    public Transform WeaponHolder;
    public GameObject bulletPrefab;

    [Header("Aim details")]
    public float slowAim = 4f;
    public float fastAim = 20f;
    public Transform aim;
    public Transform playersBody;
    public LayerMask whatToIgnore;
    [SerializeField] List<Enemy_RangeWeaponData> avalibleWeaponData;

    protected override void Awake()
    {
        base.Awake();


        statesDirtionary = EnemyStateFactory.Instance.CreateStateByType(this,stateMachine,EnemyArchetypes.Range);
    }
    protected override void Start()
    {
        base.Start();
        playersBody = player.GetComponent<Player>().playerBody;
        aim.parent = null;
        InitializePerk();
        stateMachine.Initialize(GetState<IdleState_Range>());
        visuals.SetupLook();
        SetupWeapon();
        

    }
    protected override void Update()
    {
        base.Update();

    }
    public override void Die()
    {
        base.Die();
        if(stateMachine.GetCurrentState() != GetState<DeadState_Range>())
            stateMachine.ChangeState(GetState<DeadState_Range>());
    }
    public bool CanThrowGrenade()
    {
        if(grenadePerk == GrenadePerk.Unavailable)
            return false;
        if(Vector3.Distance(transform.position, player.position) < safeDistance)
            return false;
        if(Time.time > LastTimeGrenadeThrown + grenadeCooldown)
            return true;
        return false;
        
    }
    public void ThrowGrenade()
    {
        visuals.EnableGrenadeModel(false);
        LastTimeGrenadeThrown = Time.time;

        GameObject newGrenade = ObjectPool.Instance.GetObject(grenadePrefab,grenadeStartPoint);
        newGrenade.transform.position = grenadeStartPoint.transform.position;
        Enemy_Grenade newGrenadeScript = newGrenade.GetComponent<Enemy_Grenade>();
        if(stateMachine.GetCurrentState() == GetState<DeadState_Range>())
        {
            newGrenadeScript.SetupGrenade(whatIsAlly, transform.position, 1, explosionTimer,impactPower,grenadeDamage);
            return;
        }
        newGrenadeScript.SetupGrenade(whatIsAlly, player.position, timeToTarget, explosionTimer,impactPower,grenadeDamage);
    }
    protected override void InitializePerk()
    {
        if(weaponType == Enemy_RangeWeaponType.Random)
        {
            ChooseRandomWeaponType();
        }
        if (IsUnstoppable())
        {
            advanceSpeed = 1;
            anim.SetFloat("AdvanceAnimIndex",1); // walking slow
        }
    }

    private void ChooseRandomWeaponType()
    {
        List<Enemy_RangeWeaponType> validTypes = new List<Enemy_RangeWeaponType>();

        foreach (Enemy_RangeWeaponType value in Enum.GetValues(typeof(Enemy_RangeWeaponType)))
        {
            if (value != Enemy_RangeWeaponType.Random && value != Enemy_RangeWeaponType.Rifle)
                validTypes.Add(value);
        }
        int randomIndex = UnityEngine.Random.Range(0, validTypes.Count);
        weaponType = validTypes[randomIndex];
    }

    public override void EnterBattleMode()
    {
        if(inBattleMode)
            return;
        base.EnterBattleMode();

        if(CanGetCover())
            stateMachine.ChangeState(GetState<RunToCoverState_Range>());
        else
            stateMachine.ChangeState(GetState<BattleState_Range>());
        
    }
    #region Cover System

    public bool CanGetCover()
    {
        if(coverPerk == CoverPerk.Unavailable)
            return false;

        currentCover = AttempToFindCover()?.GetComponent<CoverPoint>();

        if(lastCover != currentCover && currentCover != null)
            return true;

        Debug.LogWarning("No cover found!");
        return false;
    }


    private Transform AttempToFindCover()
    {
        List<CoverPoint> collectedCoverPoints = new List<CoverPoint>();
        foreach (Cover cover in CollecNearByCovers())
        {
            collectedCoverPoints.AddRange(cover.GetValidCoverPoints(transform));
        }
        CoverPoint closestCoverPoint = null;
        float shortestDistance = float.MaxValue;

        foreach (CoverPoint coverPoint in collectedCoverPoints)
        {
            float currentDistance = Vector3.Distance(transform.position, coverPoint.transform.position);
            if(currentDistance < shortestDistance)
            {
                closestCoverPoint = coverPoint;
                shortestDistance = currentDistance;
            }
        }
        if(closestCoverPoint != null)
        {
            lastCover?.SetOccupied(false);
            lastCover = currentCover;
            currentCover = closestCoverPoint;
            currentCover.SetOccupied(true);
            return currentCover.transform; 
        }
        return null;
    } 

    private List<Cover> CollecNearByCovers()
    {
        float coverRadiusCheck =30f;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, coverRadiusCheck);
        List<Cover> collectedCovers = new List<Cover>();
        foreach (Collider collider in hitColliders)
        {
            Cover cover = collider.gameObject.GetComponent<Cover>();
            if(cover != null && collectedCovers.Contains(cover) ==  false)
                collectedCovers.Add(cover);
        }
        return collectedCovers;
    }    

    
    #endregion
    
    public void FireSingleBullet()
    {
        anim.SetTrigger("Shoot");
        Vector3  bulletsDirection = (aim.position - gunPoint.position).normalized;

        GameObject newBullet = ObjectPool.Instance.GetObject(bulletPrefab,gunPoint);
        newBullet.transform.position = gunPoint.position;
        newBullet.transform.rotation = Quaternion.LookRotation(gunPoint.forward);

        newBullet.GetComponent<Bullet>().BulletSetup(whatIsAlly,weaponData.bulletDamage);
        Rigidbody rbNewBullet = newBullet.GetComponent<Rigidbody>();

        
        rbNewBullet.mass = 20 / weaponData.bulletSpeed;
        rbNewBullet.velocity = weaponData.ApplyWeaponSpread(bulletsDirection) * weaponData.bulletSpeed;
    }
    private void SetupWeapon()
    {
        List<Enemy_RangeWeaponData> filteredData = new List<Enemy_RangeWeaponData>();

        foreach (var weaponData in avalibleWeaponData)
        {
            if(weaponData.weaponType == weaponType) 
                filteredData.Add(weaponData);
        }
        if(filteredData.Count > 0)
        {
            int random = UnityEngine.Random.Range(0, filteredData.Count);
            weaponData = filteredData[random];
        }
        else
            Debug.LogWarning("No avalible weapon data was found!");
        gunPoint = visuals.currentWeaponModel.GetComponent<Enemy_RangeWeaponModel>().gunPoint;
    }
    #region Enemy's aim region
        
    public void UpdateAimPosition()
    {

        float aimSpeed = IsAimOnPlayer() ? fastAim : slowAim;
        aim.position = Vector3.MoveTowards(aim.position, playersBody.position, aimSpeed * Time.deltaTime);
    }
    public bool IsAimOnPlayer()
    {
        float distanceAimToPlayer = Vector3.Distance(aim.position, player.position);
        return distanceAimToPlayer < 2;
    }
    public bool IsSeeingPlayer()
    {
        Vector3 enemyPosition = transform.position + Vector3.up;
        Vector3 direction = playersBody.position - enemyPosition;
        float distance = Vector3.Distance(transform.position, player.position);
        
        if(Physics.Raycast(enemyPosition, direction, out RaycastHit hit, distance, ~whatToIgnore))
        {
            if(hit.transform.root == player.root)
            {
                UpdateAimPosition();
                return true;
            }
        }
        return false;
    }
   
    #endregion
    
    public bool IsUnstoppable() => unstoppablePerk == UnstoppablePerk.Unstoppable;

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, advanceStoppingDistance);
    }
}
