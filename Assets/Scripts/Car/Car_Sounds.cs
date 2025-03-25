using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Car_Sounds : MonoBehaviour
{
    private Car_Controller car;
    [SerializeField] private float engineVolume = .07f;
    [SerializeField] private AudioSource engineStart;
    [SerializeField] private AudioSource engineWork;
    [SerializeField] private AudioSource engineOff;

    private float minSpeed = 0;
    private float maxSpeed = 40;

    [Range(0,1)]
    public float minPitch = .75f;
    [Range(1,3)]
    public float maxPitch = 1.5f;
    private bool allowCarSounds;
    private void Start() {
        car = GetComponent<Car_Controller>();
        Invoke(nameof(AllowCarSound),1f);
        
    }
    public void ActivateCarSFX(bool activate)
    {
        if(allowCarSounds == false)
            return;
        if(activate)
        {
            engineStart.Play();
            AudioManager.Instance.SFXDelayAndFade(engineWork,true,engineVolume,1);
        }
        else{
            AudioManager.Instance.SFXDelayAndFade(engineWork,false,0f,.25f);
            engineOff.Play();
        }
    }
    private void Update() {
        UpdateEngineSound();
    }
    private void UpdateEngineSound()
    {
        float currentSpeed = car.speed;
        float pitch = Mathf.Lerp(minPitch, maxPitch, currentSpeed/maxSpeed);
        
        engineWork.pitch = pitch;
    }
    public void AllowCarSound() => allowCarSounds = true;



}
