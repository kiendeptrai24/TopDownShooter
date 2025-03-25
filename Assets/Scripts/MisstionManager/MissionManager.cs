using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public static MissionManager Instance {get; private set;}
    public Mission currentMission;
    private void Awake() {
        Instance = this;
    }
    private void Update() {
        currentMission?.UpdateMission();
    }
    public void SetCurrrentMission(Mission newMission) {
        currentMission = newMission;
    }
    public void StartMission() => currentMission?.StartMisstion();
    public bool MissionCompleted() => currentMission.MissionCompleted();
}
