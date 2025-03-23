
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource[] bgmList;
    [SerializeField] private bool playBgm;
    private int bgmIndex;
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
    private void Update() {
        if(playBgm == false && BgmIsPlaying())
            StopAllBgm();
        else if(bgmList[bgmIndex].isPlaying == false)
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


}
