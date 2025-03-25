using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

public class UI_Setting : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private float sliderMuliplier = 80;
    [Header("SFX Setting")]
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private TextMeshProUGUI sfxSliderText;
    [SerializeField] private string sfxParameter;
    [Header("BGM Setting")]
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private TextMeshProUGUI bgmSliderText;
    [SerializeField] private string bgmParameter;
    [Header("Friendly fire Setting")]
    [SerializeField] private GameObject turnOn;
    [SerializeField] private GameObject turnOff;
    private void Start() {
        OnFriendlyFireToggle();
    }

    public void SFXSliderValue(float value)
    {
        sfxSliderText.text = Mathf.RoundToInt(value * 100) + "%";
        value = Mathf.Clamp(value,.1f,1);
        float newValue = Mathf.Log10(value) * sliderMuliplier;
        audioMixer.SetFloat(sfxParameter,newValue);
    }
    public void BGMSliderValue(float value)
    {
        bgmSliderText.text = Mathf.RoundToInt(value * 100) + "%";
        value = Mathf.Clamp(value,.1f,1);
        float newValue = Mathf.Log10(value) * sliderMuliplier;
        Debug.Log(newValue);
        audioMixer.SetFloat(bgmParameter,newValue);
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
