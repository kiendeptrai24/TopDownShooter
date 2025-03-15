using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance {get; private set;}
    [SerializeField] private float resumeRate = 3;
    [SerializeField] private float pauseRate = 1;
    
    private float timeAdjustRate;
    private float targetTimeScale = 1;

    private void Awake() {
        if(Instance == null)
            Instance = this;
        else
            Destroy(Instance.gameObject);
    }
    private void Update() {
        if(Input.GetKeyDown(KeyCode.Q))
            SlowMotionFor(2f);
        if(MathF.Abs(Time.timeScale - targetTimeScale) > .05f)
        {
            float adjustRate = Time.unscaledDeltaTime * timeAdjustRate;
            Time.timeScale = Mathf.Lerp(Time.timeScale, targetTimeScale, adjustRate);
        }
        else
            Time.timeScale = targetTimeScale;
    }
    public void PauseTime()
    {
        timeAdjustRate = pauseRate;
        targetTimeScale = 0;
    }
    public void ResumeTime()
    {
        timeAdjustRate = resumeRate;
        targetTimeScale = 1;
    }
    
    public void SlowMotionFor(float seconds) => StartCoroutine(SlowTimeCo(seconds));
    
    private IEnumerator SlowTimeCo(float seconds)
    {
        targetTimeScale = .6f;
        Time.timeScale = targetTimeScale;
        yield return new WaitForSecondsRealtime(seconds);
        ResumeTime();
    }
    public void ChangeTimer(float targetAlpha, float timeScale)
    {
        targetTimeScale = targetAlpha;
        Time.timeScale = timeScale;
    }


}
