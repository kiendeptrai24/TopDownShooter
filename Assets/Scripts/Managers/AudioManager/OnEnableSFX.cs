using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnableSFX : MonoBehaviour
{
    [SerializeField] private AudioSource sfx; 
    [Range(0,1f)]
    [SerializeField] private float volume = .3f;
    [Range(0,.5f)]
    [SerializeField] private float minPitch = .5f;
    [Range(.5f,2)]
    [SerializeField] private float maxPitch = 1;
    void OnEnable()
    {
        PlaySFX();
    }
    private void PlaySFX()
    {
        if(sfx == null)
            return;
        float pitch = Random.Range(minPitch, maxPitch);
        sfx.pitch = pitch;
        sfx.volume = volume;
        sfx.Play();
    }
}
