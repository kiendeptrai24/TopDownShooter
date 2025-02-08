
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    [SerializeField] private Transform ragdollParent;
    [SerializeField] private Collider[] ragdollColliders;
    [SerializeField] private Rigidbody[] ragdollRigidBodies;
    private void Awake() {
        ragdollColliders = gameObject.GetComponentsInChildren<Collider>();
        ragdollRigidBodies = gameObject.GetComponentsInChildren<Rigidbody>();

        RagdollActive(false);
    }
    public void RagdollActive(bool active)
    {
        foreach (Rigidbody rb in ragdollRigidBodies)
            rb.isKinematic = !active;
    }
    public void CollidersActive(bool active)
    {
        foreach (Collider cd in ragdollColliders)
            cd.enabled = active;
    }
}
