using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public static MissionManager instance {get; private set;}
    public Mission currentMission;
    private void Awake() {
        instance = this;
    }
    private void Update() {
        currentMission?.UpdateMission();
    }
    public void SetCurrrentMission(Mission newMission) {
        currentMission = newMission;
        StartMission();
    }
    private void StartMission() => currentMission.StartMisstion();
    public bool MissionCompleted() => currentMission.MissionCompleted();
}
