using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
    public Player player;

    [Header("Settings")]

    public bool friendlyFire;
    [Space]
    public bool quickStart;

    private void Awake() {
        Instance = this;
        player = FindObjectOfType<Player>();
    }
    public void GameStart()
    {
        SetDefaultWeaponsForPlayer();
    }
    public void GameOver()
    {
        TimeManager.Instance.SlowMotionFor(1);
        UI.Instance.SwitchToGameOverUI();
        CameraManager.Instance.ChangeCameraDistance(6);
        ControlsManager.Instance.SwitchToUIControls();
    }
    public void GameCompleted()
    {
        UI.Instance.ShowVictoryScreenUI();
        ControlsManager.Instance.controls.Character.Disable();
        player.health.currentHealth += 99999;
    }
    private void SetDefaultWeaponsForPlayer()
    {
        List<Weapon_Data> defaultWeapons = UI.Instance.weaponSelectionUI.GetSelectedWeapons();

        player.weapon.SetDefaultWeapon(defaultWeapons);
    }
    public void ReStartScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
}
