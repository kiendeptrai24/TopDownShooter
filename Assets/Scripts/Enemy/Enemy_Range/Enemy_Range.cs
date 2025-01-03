using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy_Range : Enemy
{
    [Header("Cover system")]
    public bool canUseCovers = true;
    public CoverPoint lastCover { get; private set; }
    public CoverPoint currentCover { get; private set; }


    [Header("Weapon details")]
    public Enemy_RangeWeaponType weaponType;
    public Enemy_RangeWeaponData weaponData;
    [Space]
    public Transform gunPoint;
    public Transform WeaponHolder;
    public GameObject bulletPrefab;

    [SerializeField] List<Enemy_RangeWeaponData> avalibleWeaponData;
    #region State
    public IdleState_Range idleState { get; private set; }
    public MoveState_Range moveState { get; private set; }
    public BattleState_Range battleState { get; private set; }
    public RunToCoverState_Range runToCoverState { get; private set; }
        
    #endregion
    protected override void Awake()
    {
        base.Awake();
        idleState = new IdleState_Range(this, stateMachine,"Idle");
        moveState = new MoveState_Range(this, stateMachine,"Move");
        battleState = new BattleState_Range(this,stateMachine,"Battle");
        runToCoverState = new RunToCoverState_Range(this,stateMachine,"Run");
    }
    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
        visuals.SetupLook();
        SetupWeapon();

    }
    protected override void Update()
    {
        base.Update();

    }
    public override void EnterBattleMode()
    {
        if(inBattleMode)
            return;
        base.EnterBattleMode();

        if(CanGetCover())
            stateMachine.ChangeState(runToCoverState);
        else
            stateMachine.ChangeState(battleState);
        
    }
    public void FireSingleBullet()
    {
        anim.SetTrigger("Shoot");
        Vector3  bulletsDirection = (player.position + Vector3.up - gunPoint.position).normalized;

        GameObject newBullet = ObjectPool.Instance.GetObject(bulletPrefab);
        newBullet.transform.position = gunPoint.position;
        newBullet.transform.rotation = Quaternion.LookRotation(gunPoint.forward);

        newBullet.GetComponent<Enemy_Bullet>().BulletSetup();
        Rigidbody rbNewBullet = newBullet.GetComponent<Rigidbody>();

        
        rbNewBullet.mass = 20 / weaponData.bulletSpeed;
        rbNewBullet.velocity = weaponData.ApplyWeaponSpread(bulletsDirection) * weaponData.bulletSpeed;
    }
    #region Cover System

    public bool CanGetCover()
    {
        if(canUseCovers  == false)
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
            int random = Random.Range(0, filteredData.Count);
            weaponData = filteredData[random];
        }
        else
            Debug.LogWarning("No avalible weapon data was found!");
        gunPoint = visuals.currentWeaponModel.GetComponent<Enemy_RangeWeaponModel>().gunPoint;
    }
}
