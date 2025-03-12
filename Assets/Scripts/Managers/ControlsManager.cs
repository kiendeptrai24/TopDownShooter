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
    }
    private void Start() {
        controls = GameManager.Instance.player.controls;
        player = GameManager.Instance.player;
        
        SwitchToUIControls();
    }
    public void SwitchToCharactorControls()
    {
        controls.UI.Disable();
        controls.Character.Enable();
        player.SetControlsEnabledTo(true);
    }
    public void SwitchToUIControls()
    {
        controls.Character.Disable();
        controls.UI.Enable();
        player.SetControlsEnabledTo(false);
    }
}
