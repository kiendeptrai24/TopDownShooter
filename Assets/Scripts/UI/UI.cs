using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    public static UI Instance { get; private set; }
    public UI_InGame inGameUI { get; private set; }
    public UI_GameOver gameOverUI {get; private set;}
    public UI_WeaponSelection weaponSelectionUI { get; private set; }
    public GameObject pauseUI;


    [SerializeField] private GameObject[] UI_Elements;

    private void Awake()
    {
        Instance = this;
        inGameUI = GetComponentInChildren<UI_InGame>(true);
        weaponSelectionUI = GetComponentInChildren<UI_WeaponSelection>(true);
        gameOverUI = GetComponentInChildren<UI_GameOver>(true);
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
        Time.timeScale = 1;

    }
    public void SwitchToGameOverUI(string message = "GAME OVER!!!")
    {
        SwitchTo(gameOverUI.gameObject);
        gameOverUI.ShowGameOverMessage(message);
        
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
            TimeManager.Instance.ResumeTime();
        }
        else
        {
            SwitchTo(pauseUI);
            ControlsManager.Instance.SwitchToUIControls();
            TimeManager.Instance.PauseTime();
        }
    }
    private void AssignInputUI()
    {
        PlayerControls controls = GameManager.Instance.player.controls;
        controls.UI.UIPause.performed += ctx => PauseSwitch();
    }
}
