using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    public static UI Instance { get; private set; }
    public UI_InGame inGameUI { get; private set; }
    public UI_WeaponSelection weaponSelectionUI { get; private set; }
    public GameObject pauseUI;


    [SerializeField] private GameObject[] UI_Elements;

    private void Awake()
    {
        Instance = this;
        inGameUI = GetComponentInChildren<UI_InGame>(true);
        weaponSelectionUI = GetComponentInChildren<UI_WeaponSelection>(true);
    }
    private void Start() {
        AssignInputUI();
    }
    public void SwitchTo(GameObject uiToSwitchOn)
    {
        foreach (var go in UI_Elements)
        {
            go.SetActive(false);
        }
        uiToSwitchOn.SetActive(true);

    }
    public void SwitchToInGameUI()
    {
        SwitchTo(inGameUI.gameObject);
        GameManager.Instance.GameStart();
        ControlsManager.Instance.SwitchToCharactorControls();
    }
    public void StartTheGame() => SwitchToInGameUI();
    public void QuitTheGame() => Application.Quit();
    public void RestartTheGame() => GameManager.Instance.ReStartScene();

    public void PauseSwitch()
    {
        bool gamePause = pauseUI.activeSelf;
        if(gamePause)
        {
            SwitchTo(inGameUI.gameObject);

            ControlsManager.Instance.SwitchToCharactorControls();
            Time.timeScale = 1;
        }
        else
        {
            SwitchTo(pauseUI);
            ControlsManager.Instance.SwitchToUIControls();
            Time.timeScale = 0;
        }
    }
    private void AssignInputUI()
    {
        PlayerControls controls = GameManager.Instance.player.controls;
        controls.UI.UIPause.performed += ctx => PauseSwitch();
    }
}
