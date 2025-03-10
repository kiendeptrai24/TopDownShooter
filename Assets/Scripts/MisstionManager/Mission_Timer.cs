using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Timer Mission", menuName = "Missions/Timer mission")]
public class Mission_Timer : Mission
{
    public float time;
    private float currentTime;
    public override void StartMisstion()
    {
        currentTime = time;
    }
    public override void UpdateMission()
    {
        currentTime -= Time.deltaTime;
        if (currentTime < 0)
            Debug.Log("gameOver");
        UpdateMissionUI();
    }

    private void UpdateMissionUI()
    {
        string timeText = System.TimeSpan.FromSeconds(currentTime).ToString("mm':'ss");
        string misstionText = "Get to the evacuation point before time runs out";
        string missionDetails = "Time left: " + timeText;
        UI.Instance.inGameUI.UpdateMissionInfo(misstionText, missionDetails);
    }

    public override bool MissionCompleted()
    {
        return currentTime > 0;
    }

}
