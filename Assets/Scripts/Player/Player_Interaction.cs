using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private List<Interactable> interactables = new();
    private Interactable closestInteractable;

    private void Start() {
        Player player= GetComponent<Player>();
        player.controls.Character.Interaction.performed += context => InteracWithClosest();
    }
    public void InteracWithClosest()
    {
        closestInteractable?.Interaction();
        interactables.Remove(closestInteractable);

        UpdateInteractable();
    }
    public void UpdateInteractable()
    {
        closestInteractable?.HighlightActive(false);
        closestInteractable = null;
        float closestDistance = float.MaxValue;

        foreach (Interactable interactable in interactables)
        {
            float distance = Vector3.Distance(transform.position, interactable.transform.position);
            if(distance < closestDistance)
            {
                closestDistance = distance;
                closestInteractable = interactable;
            }
        }
        if(closestInteractable != null)
            closestInteractable.HighlightActive(true);
    }
    public List<Interactable> GetInteractables() => interactables; 
}
