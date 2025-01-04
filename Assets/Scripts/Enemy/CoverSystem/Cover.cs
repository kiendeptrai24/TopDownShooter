using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Cover : MonoBehaviour
{
    private Transform playerTransform;
    [Header("Cover points")]
    [SerializeField] private GameObject coverPointPrefab;
    [SerializeField] private List<CoverPoint> coverPoints= new List<CoverPoint>();
    [SerializeField] private float xOffset =1;
    [SerializeField] private float yOffset =.2f;
    [SerializeField] private float zOffset = 1;

    private void Start() {
        GenerateCoverPoints();
        playerTransform = FindObjectOfType<Player>().transform;
    }

    private void GenerateCoverPoints()
    {
        Vector3[] localCoverPoints =
        {
            new Vector3(0, yOffset, zOffset), // front
            new Vector3(0, yOffset, -zOffset), // back
            new Vector3(xOffset, yOffset, 0), // right
            new Vector3(-xOffset, yOffset, 0) // left
        };

        foreach (Vector3 LocalPoint in localCoverPoints)
        {
            Vector3 worldPoint = transform.TransformPoint(LocalPoint);
            CoverPoint coverPoint = Instantiate(coverPointPrefab, worldPoint, Quaternion.identity, transform).GetComponent<CoverPoint>();
            coverPoints.Add(coverPoint);
        }
    }
    public List<CoverPoint> GetValidCoverPoints(Transform enemy)
    {
        List<CoverPoint> validCoverPoints = new List<CoverPoint>();
        foreach (CoverPoint coverPoint in coverPoints)
        {
            if(IsValidCoverPoint(coverPoint, enemy))
                validCoverPoints.Add(coverPoint);
        }
        return validCoverPoints;
    }
    private bool IsValidCoverPoint(CoverPoint coverPoint, Transform enemy)
    {
        if(coverPoint.occupied)
            return false;
            
        if(IsFurtherestFromPlayer(coverPoint) == false)
            return false;
            
        if(IsCoverBehindPlayer(coverPoint,enemy))
            return false;
            
        if(IsCoverCloseToPlayer(coverPoint))
            return false;
            
        if(IsCoverCloseToLastCover(coverPoint, enemy))
            return false;

        return true;
    }
    //checking between current point and furtherest point if they is equal to return true <Passing conditions>
    private bool IsFurtherestFromPlayer(CoverPoint coverPoint)
    {
        CoverPoint furtherestPoint = null;
        float furtherestDistance = 0;
        foreach (CoverPoint point in coverPoints)
        {
            float distance = Vector3.Distance(point.transform.position, playerTransform.position);
            if(distance > furtherestDistance)
            {
                furtherestDistance = distance;
                furtherestPoint = point;
            }
        }
        return furtherestPoint == coverPoint;
    }   

    // checking distance from player and enemy to coverPoint if player greater than enemy it will be false <Passing conditions>
    private bool IsCoverBehindPlayer(CoverPoint coverPoint, Transform enemy)
    {
        float distanceToPlayer = Vector3.Distance(coverPoint.transform.position, playerTransform.position);
        float distanceToEnemy = Vector3.Distance(coverPoint.transform.position, enemy.position);
        return distanceToPlayer < distanceToEnemy;
    }
    // checking distance from coverPoint to player if that is greater than 2 it will be false <Passing conditions>
    private bool IsCoverCloseToPlayer(CoverPoint coverPoint)
    {
        return Vector3.Distance(coverPoint.transform.position, playerTransform.position) < 2;
    }

    // cheking last point if it is null return false <Passing conditions> or
    // lastpoint is valid and distance of current point and last point greater than 3 return false <Passing conditions>
    private bool IsCoverCloseToLastCover(CoverPoint coverPoint,Transform enemy)
    {
        CoverPoint lastCover = enemy.GetComponent<Enemy_Range>().currentCover;
        return lastCover != null && Vector3.Distance(coverPoint.transform.position,lastCover.transform.position) < 3;   
    }
}
