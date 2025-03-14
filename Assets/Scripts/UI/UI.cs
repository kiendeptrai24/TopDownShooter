using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public static UI Instance { get; private set; }
    public UI_InGame inGameUI { get; private set; }
    public UI_GameOver gameOverUI {get; private set;}
    public UI_WeaponSelection weaponSelectionUI { get; private set; }
    public GameObject pauseUI;


    [SerializeField] private GameObject[] UI_Elements;
    [Header("Fade Image")]
    [SerializeField] private Image fadeImage;

    private void Awake()
    {
        Instance = this;
        inGameUI = GetComponentInChildren<UI_InGame>(true);
        weaponSelectionUI = GetComponentInChildren<UI_WeaponSelection>(true);
        gameOverUI = GetComponentInChildren<UI_GameOver>(true);
    }
    private void Start() {
        AssignInputUI();
        StartCoroutine(ChangeImageAlpha(0,1.5f,null));
    }
    public void SwitchTo(GameObject uiToSwitchOn)
    {
        foreach (var go in UI_Elements)
        {
            go.SetActive(false);
        }
        uiToSwitchOn.SetActive(true);

    }
    public void SwitchToInGameUI() => StartCoroutine(StartGameSequence());
    public void SwitchToGameOverUI(string message = "GAME OVER!!!")
    {
        SwitchTo(gameOverUI.gameObject);
        gameOverUI.ShowGameOverMessage(message);
    }
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

    public void StartTheGame() => SwitchToInGameUI();
    public void QuitTheGame() => Application.Quit();
    public void RestartTheGame() => StartCoroutine(ChangeImageAlpha(1,1f,GameManager.Instance.ReStartScene));
    

    private IEnumerator StartGameSequence()
    {
        StartCoroutine(ChangeImageAlpha(1,1,null));
        Debug.Log("turn on");
        yield return new WaitForSeconds(1);
        SwitchTo(inGameUI.gameObject);
        GameManager.Instance.GameStart();
        ControlsManager.Instance.SwitchToCharactorControls();
        StartCoroutine(ChangeImageAlpha(0,1,null));
        Debug.Log("turn off");

    }
    private IEnumerator ChangeImageAlpha(float targetAlpha, float duration, Action onComplete)
    {
        float time = 0;
        Color currentColor = fadeImage.color;
        float startAlpha = currentColor.a;
        while(time < duration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha,targetAlpha,time/duration);

            fadeImage.color = new Color(currentColor.r,currentColor.g,currentColor.b,alpha);

            yield return null;
        }
        fadeImage.color = new Color(currentColor.r,currentColor.g,currentColor.b,targetAlpha);
        onComplete?.Invoke();
    }
    private void AssignInputUI()
    {
        PlayerControls controls = GameManager.Instance.player.controls;
        controls.UI.UIPause.performed += ctx => PauseSwitch();
    }
}
