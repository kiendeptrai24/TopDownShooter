
using System.Collections;
using System.Diagnostics.Tracing;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance {get; private set;}
    
    [SerializeField] private AudioSource[] bgmList;
    [SerializeField] private bool playBgm;
    private int bgmIndex;
    private void Awake() {
        if(Instance == false)
            Instance = this;
        else
            Destroy(Instance);
    }
    private void Start() {
        PlayBGM(1);
    }
    public void PlayBGM(int index)
    {
        if(index >= bgmList.Length)
            return;
        StopAllBgm();
        bgmIndex = index;
        bgmList[index].Play();
    }
    public void PlaySFX(AudioSource sfx, bool randomPitch = false, float minPitch = .8f, float maxPitch = 1.1f)
    {
        if(sfx == null)
            return;
        float pitch = Random.Range(minPitch,maxPitch);
        sfx.pitch = pitch;
        sfx.Play();
    }
    private void Update() {
        if(playBgm == false && BgmIsPlaying())
            StopAllBgm();
        if(playBgm && bgmList[bgmIndex].isPlaying == false)
            PlayRandomBGM();
    }
    public void StopAllBgm()
    {
        foreach (var bgm in bgmList)
        {
            bgm.Stop();
        }
    }
    [ContextMenu("Play random music")]
    public void PlayRandomBGM()
    {
        StopAllBgm();
        if(bgmList.Length <= 0)
            return;
        bgmIndex = Random.Range(0,bgmList.Length);
        PlayBGM(bgmIndex);
    }
    private bool BgmIsPlaying()
    {
        foreach (var bgm in bgmList)
        {
            if(bgm.isPlaying)
                return true;
        }
        return false;
    }
    public void SFXDelayAndFade(AudioSource source,bool play,float tartgetVolume,float delay = 0 ,float fadeDuration = 1)
    {
        StartCoroutine(SFXDelayAndFadeCo(source,play,tartgetVolume,delay,fadeDuration));
    }
    private IEnumerator SFXDelayAndFadeCo(AudioSource source,bool play, float tartgetVolume, float delay,float fadeDuration)
    {
        yield return new WaitForSeconds(delay);
        float startVolume = play ? 0 : source.volume;
        float endVolume = play ? tartgetVolume : 0;
        float elapsed = 0;
        if(play)
        {
            source.volume = 0;
            source.Play();
        }
        while(elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume,endVolume,elapsed/fadeDuration);
            yield return null;
        }
        source.volume = endVolume;
        if(play == false)
            source.Stop();

    }


}
