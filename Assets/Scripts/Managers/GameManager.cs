using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
    public Player player;

    [Header("Settings")]
    public bool friendlyFire;

    private void Awake() {
        Instance = this;
        player = FindObjectOfType<Player>();
    }
    public void SetDefaultWeapons()
    {
        List<Weapon_Data> defaultWeapons = UI.Instance.weaponSelectionUI.GetSelectedWeapons();

        player.weapon.SetDefaultWeapon(defaultWeapons);
    }
}
