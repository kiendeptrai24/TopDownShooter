using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform playerBody;    
    public PlayerControls controls{get; private set;}
    public Player_Aim aim{get; private set;}
    public PlayerMovement movement{get; private set;}
    public Player_WeaponController weapon {get; private set;}
    public PlayerWeaponVisuals weaponVisuals{get; private set;}
    public PlayerInteraction interaction{get; private set;}
    public Player_Health health {get; private set;}
    public Ragdoll ragdoll {get; private set;}
    public Animator anim {get; private set;}
    public bool controlsEnable {get; private set;}

    private void Awake() {
        controls = new PlayerControls();
        aim = GetComponent<Player_Aim>();
        movement = GetComponent<PlayerMovement>();
        weapon = GetComponent<Player_WeaponController>();
        weaponVisuals = GetComponent<PlayerWeaponVisuals>();
        interaction = GetComponent<PlayerInteraction>();
        health = GetComponent<Player_Health>();
        ragdoll = GetComponent<Ragdoll>();
        anim = GetComponentInChildren<Animator>();
    }
    private void OnEnable() {
        controls.Enable();
        controls.Character.UIMissionToolTipSwitch.performed += ctx => UI.Instance.inGameUI.SwitchMissionTooltip();
        controls.Character.UIPause.performed += ctx => UI.Instance.PauseSwitch();
    }
    private void OnDisable() {
        controls.Disable();
    }
    public void SetControlsEnabledTo(bool enable) => controlsEnable = enable;
}
