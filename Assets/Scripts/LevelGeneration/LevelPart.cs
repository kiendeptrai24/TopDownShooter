using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelPart : MonoBehaviour
{
    [Header("Intersection check")]
    [SerializeField] private LayerMask intersectionLayer;
    [SerializeField] private Collider[] intersectionCheckColliders;
    [SerializeField] private Transform intersectionCheckParent;
    private Enemy[] enemyList;
    private void Awake() {
        enemyList = GetComponentsInChildren<Enemy>(true);
    }
    private void Start() {
        if(intersectionCheckColliders.Length <= 0)
        {
            intersectionCheckColliders = intersectionCheckParent.GetComponentsInChildren<Collider>();
        }
    }
    [ContextMenu("Set static to environment layer")]
    private void AdjustLayerForStaticObject()
    {
        foreach (Transform childTransform in transform.GetComponentsInChildren<Transform>(true))
        {
            if(childTransform.gameObject.isStatic)
            {
                childTransform.gameObject.layer = LayerMask.NameToLayer("Environment");
            }
        }
    }
    public bool IntersectionDetected()
    {
            Physics.SyncTransforms();
            foreach (var collider in intersectionCheckColliders)
            {
                Collider[] hitColliders = 
                    Physics.OverlapBox(collider.bounds.center,collider.bounds.extents,Quaternion.identity,intersectionLayer);

                foreach (var hit in hitColliders)
                {
                    IntersectionCheck intersectionCheck = hit.GetComponentInParent<IntersectionCheck>();

                    if(intersectionCheck != null && intersectionCheckParent != intersectionCheck.transform)
                        return true;
                }
            }
            return false;
    }
    public void SnapAndAlignPartTo(SnapPoint targetSnapPoint)
    {
        SnapPoint entrancePoint = GetEntrancePoint();

        AlignTo(entrancePoint, targetSnapPoint);/// IMPORTANT: ALignmnet should be before possition snapto
        SnapTo(entrancePoint, targetSnapPoint);
    }
    private void AlignTo(SnapPoint ownSnapPoint, SnapPoint targetSnapPoint)
    {
        var rotationOffset = 
            ownSnapPoint.transform.rotation.eulerAngles.y - transform.rotation.eulerAngles.y;
            transform.rotation = targetSnapPoint.transform.rotation;
        
        transform.Rotate(0,180,0);
        transform.Rotate(0,-rotationOffset,0);

    }
    private void SnapTo(SnapPoint ownSnapPoint, SnapPoint targetSnapPoint)
    {
        // calculate the offset between the level part's current possition
        // and it's own snap point's position. This offset represents the
        // distance and direction from the level part's pivot to it's snap point
        var offset = transform.position - ownSnapPoint.transform.position;

        // Determine the new possition for the level part. It's calculate by
        // adding the previously coputed offset to the target snap point's position
        // this effectively moves the level part so that its snap point aligns
        // with the target snap point's possition.
        var newPosition = targetSnapPoint.transform.position + offset;

        // update the level part's possiton to the newly calculated possition by using snap points.
        transform.position = newPosition;
    }
    public SnapPoint GetEntrancePoint() => GetSnapPointOfType(SnapPointType.Enter);
    public SnapPoint GetExitPoint() => GetSnapPointOfType(SnapPointType.Exit);

    private SnapPoint GetSnapPointOfType(SnapPointType pointType)
    {
        SnapPoint[] snapPoints = GetComponentsInChildren<SnapPoint>();
        List<SnapPoint> filteredSnapPoint = new List<SnapPoint>();

        // colect all snappoint of the spedified type
        foreach (SnapPoint snapPoint in snapPoints)
        {
            if(snapPoint.pointType == pointType)
                filteredSnapPoint.Add(snapPoint);
        }
        // if there aare matching snap points, 
        if(filteredSnapPoint.Count > 0)
        {
            int randomIndex = Random.Range(0, filteredSnapPoint.Count);
            return filteredSnapPoint[randomIndex];
        }
        return null;
    }
    public Enemy[] MyEnemies() => enemyList  == null ? new Enemy[0] : enemyList;

}
