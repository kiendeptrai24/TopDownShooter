using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    protected MeshRenderer mesh;
    protected Player_WeaponController weaponController;

    [SerializeField] private Material highlightMaterial;
    protected Material defaultMaterial;
    private void Start() {
        if(mesh == null)
            mesh = GetComponentInChildren<MeshRenderer>();
        defaultMaterial = mesh.sharedMaterial;
    }
    protected void UpdateMeshAndMaterial(MeshRenderer newMesh)
    {
        mesh = newMesh;
        defaultMaterial = newMesh.sharedMaterial;
    }
    public virtual void Interaction()
    {
        Debug.Log("Pickup: " + gameObject.name);
    }
    public void HighlightActive(bool active)
    {
        if(active)
            mesh.material = highlightMaterial;
        else
            mesh.material = defaultMaterial;
    }
    protected virtual void OnTriggerEnter(Collider other) {
        if(weaponController == null)
            weaponController = other.GetComponent<Player_WeaponController>();
            
        PlayerInteraction playerInteraction = other.GetComponent<PlayerInteraction>();
        if(playerInteraction == null)
            return;
        playerInteraction.GetInteractables().Add(this);
        playerInteraction.UpdateInteractable();
    }
    private void OnTriggerExit(Collider other) {
        PlayerInteraction playerInteraction = other.GetComponent<PlayerInteraction>();
        if(playerInteraction == null)
            return;
        playerInteraction.GetInteractables().Remove(this);
        playerInteraction.UpdateInteractable();
    }
}
