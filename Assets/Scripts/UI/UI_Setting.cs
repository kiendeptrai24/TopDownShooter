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
    private bool toggle;
    [SerializeField] private GameObject turnOn;
    [SerializeField] private GameObject turnOff;
    private void Start() {
        toggle = GameManager.Instance.friendlyFire;
        OnFriendlyFireToggle();
    }

    public void SFXSliderValue(float value)
    {
        sfxSliderText.text = Mathf.RoundToInt(value * 100) + "%";
        value = Mathf.Clamp(value,.1f,1);
        float newValue = Mathf.Log10(value) * sliderMuliplier;
        audioMixer.SetFloat(sfxParameter,newValue);
        PlayerPrefs.SetFloat("unitylaex",1);
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

        if(toggle == true)
        {
            turnOff.SetActive(false);
            turnOn.SetActive(true);
            GameManager.Instance.friendlyFire = true;
            toggle = false;
        }
        else
        {
            turnOn.SetActive(false);
            turnOff.SetActive(true);
            GameManager.Instance.friendlyFire = false;
            toggle = true;
            
        }

    }
    public void LoadSetting()
    {
        bool newfriendlyFire = PlayerPrefs.GetInt("FreindlyFire") == 1 ? true : false;

        GameManager.Instance.friendlyFire = newfriendlyFire;

        sfxSlider.value = PlayerPrefs.GetFloat(sfxParameter, .7f);
        bgmSlider.value = PlayerPrefs.GetFloat(bgmParameter, .7f);
    }
    void OnDisable()
    {
        bool friendlyFire = GameManager.Instance.friendlyFire;
        int friendlyFireInt = friendlyFire ? 1 : 0;
        PlayerPrefs.SetInt("FreindlyFire", friendlyFireInt);
        PlayerPrefs.SetFloat(sfxParameter,sfxSlider.value);
        PlayerPrefs.SetFloat(bgmParameter,bgmSlider.value);

    }
}
