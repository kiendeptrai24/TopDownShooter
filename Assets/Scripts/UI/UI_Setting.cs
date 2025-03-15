using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

public class UI_Setting : MonoBehaviour
{
    [Header("SFX Setting")]
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private TextMeshProUGUI sfxSliderText;
    [Header("BGM Setting")]
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private TextMeshProUGUI bgmSliderText;
    [Header("Friendly fire Setting")]
    [SerializeField] private GameObject turnOn;
    [SerializeField] private GameObject turnOff;
    private void Start() {
        OnFriendlyFireToggle();
    }

    public void SFXSliderValue(float value)
    {
        sfxSliderText.text = Mathf.RoundToInt(value * 100) + "%";
    }
    public void BGMSliderValue(float value)
    {
        bgmSliderText.text = Mathf.RoundToInt(value * 100) + "%";
    }
    public void OnFriendlyFireToggle()
    {
        bool friendlyFire = GameManager.Instance.friendlyFire;
        GameManager.Instance.friendlyFire = !friendlyFire;
        if(friendlyFire == false)
        {
            turnOff.SetActive(false);
            turnOn.SetActive(true);
        }
        else
        {
            turnOn.SetActive(false);
            turnOff.SetActive(true);
        }

    }
}
