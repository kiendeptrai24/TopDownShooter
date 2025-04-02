using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



[CreateAssetMenu(fileName = "New Hunt Mission", menuName = "Missions/Hunt Mission")]
public class Mission_EnemyHunt : Mission
{
    public int amountToKill = 12;
    public EnemyType enemyType;
    private List<GameObject> enemiesGenerate = new List<GameObject>();
    
    private int killsToGo;
    public override void StartMisstion()
    {
        
        enemiesGenerate = FactoryEnemy.Instance.GetEnemies(enemyType,amountToKill);

        killsToGo = amountToKill;
        UpdateMissionUI();
        MissionObject_HuntTarget.OnTargetKilled += EliminateTarget;

    }
    public override bool MissionCompleted()
    {
        return killsToGo <= 0;
    }
    private void EliminateTarget()
    {
        killsToGo--;
        UpdateMissionUI();
        if(killsToGo <= 0)
        {
            UI.Instance.inGameUI.UpdateMissionInfo("get to the evacuation point");
            MissionObject_HuntTarget.OnTargetKilled -= EliminateTarget;
        }
    }
    private void UpdateMissionUI()
    {
        string missionText = $"Mission:  Eliminate {amountToKill} enemies with signal disruptor";
        string missionDetails = "Eliminate " + killsToGo + " " + enemyType.ToString() + "s";
        UI.Instance.inGameUI.UpdateMissionInfo(missionText, missionDetails);
    }

}
