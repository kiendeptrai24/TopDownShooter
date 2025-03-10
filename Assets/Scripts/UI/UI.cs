using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    public static UI Instance { get; private set; }
    public UI_InGame inGameUI { get; private set; }
    [SerializeField] private GameObject[] UIElements;

    private void Awake()
    {
        Instance = this;
        inGameUI = GetComponentInChildren<UI_InGame>(true);
    }
    public void SwitchTo(GameObject uiToSwitchOn)
    {
        foreach (var go in UIElements)
        {
            go.SetActive(false);
        }
        uiToSwitchOn.SetActive(true);

    }
    public void SwitchToInGameUI()
    {
        SwitchTo(inGameUI.gameObject);
    }
    public void QuitTheGame()
    {
        Application.Quit();
    }
}
