using UnityEngine;

public class ControlsManager : MonoBehaviour
{
    public static ControlsManager Instance {get; private set;}
    public PlayerControls controls {get; private set;}
    private Player player;
    private void Awake() {
        if(Instance == null)
            Instance = this;
        else
            Destroy(Instance.gameObject);
        controls = new PlayerControls();
        player = GameManager.Instance.player;
    }
    private void Start() {
        SwitchToUIControls();
    }
    public void SwitchToCharactorControls()
    {
        controls.UI.Disable();
        controls.Car.Disable();
        controls.Character.Enable();
        player.SetControlsEnabledTo(true);
    }
    public void SwitchToUIControls()
    {
        controls.Character.Disable();
        controls.Car.Disable();
        controls.UI.Enable();
        player.SetControlsEnabledTo(false);
    }
    public void SwitchToCarControls()
    {
        controls.UI.Disable();
        controls.Character.Disable();
        controls.Car.Enable();
        player.SetControlsEnabledTo(false);

    }
}
