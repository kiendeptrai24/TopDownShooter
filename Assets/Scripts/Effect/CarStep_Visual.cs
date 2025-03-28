using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CarStep_Visual : TraceEffect
{
    [SerializeField] private TrailRenderer[] carTrace;
    
    private void Start() {
        carTrace = GetComponentsInChildren<TrailRenderer>();
        SetTimeDuration(carTrace);
    }
    protected override void CheckTraces()
    {
        for (int i = 0; i < carTrace.Length; i++)
        {
            carTrace[i].emitting = IsTouchingGround(carTrace[i].transform.position);
        }
    }

    private void DrawCarTrace()
    {
        foreach (var trace in carTrace)
        {
            DrawGizmos(trace.transform.position);
        }
    }
    private void OnDrawGizmos()
    {
        DrawCarTrace();
    }
}
