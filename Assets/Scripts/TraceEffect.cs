using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TraceEffect : MonoBehaviour
{
    [SerializeField] protected LayerMask whatIsGround;
    [Range(0.001f, 0.3f)]
    [SerializeField] protected float checkRadius = 0.05f;
    [Range(-0.15f, 0.15f)]
    [SerializeField] protected float rayDistance = -0.05f;

    protected abstract void CheckTraces();
    private void Update()
    {
        CheckTraces();
    }

    protected bool IsTouchingGround(Vector3 position)
    {
        return Physics.CheckSphere(position + Vector3.down * rayDistance, checkRadius, whatIsGround);
    }

    protected void DrawGizmos(Vector3 position)
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(position + Vector3.down * rayDistance, checkRadius);
    }
    protected virtual void OnDrawGizmos()
    {
        
    }
}
