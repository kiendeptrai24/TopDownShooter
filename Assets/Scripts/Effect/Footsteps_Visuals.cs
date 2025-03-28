using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Footsteps_Visuals : TraceEffect
{
    [SerializeField] private TrailRenderer leftFoot;
    [SerializeField] private TrailRenderer rightFoot;
    protected override void CheckTraces()
    {
        leftFoot.emitting = IsTouchingGround(leftFoot.transform.position);
        rightFoot.emitting = IsTouchingGround(rightFoot.transform.position);
    }
    private void DrawFootstep()
    {
        DrawGizmos(leftFoot.transform.position);
        DrawGizmos(rightFoot.transform.position);
    }
    private void OnDrawGizmos()
    {
        DrawFootstep();
    }
}
