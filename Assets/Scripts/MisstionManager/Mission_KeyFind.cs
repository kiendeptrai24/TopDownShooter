using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Key Mission", menuName = "Missions/Key mission")]
public class Mission_KeyFind : Mission
{
    [SerializeField] private GameObject key;
    private bool keyFound;
    public override void StartMisstion()
    {
        MissionObjectKey.OnKeyPickedUp += PickUpKey;
        UI.Instance.inGameUI.UpdateMissionInfo("Find a key-holder. Retrieve the key", "Find a key-holder and retrieve the key");
        Enemy enemy = LevelGenerator.instance.GetRandomEnemy();
        enemy.GetComponent<Enemy_DropController>()?.GiveKey(key);
        enemy.MakeEnemyVIP();
        // give key random enemy
        // levelup random enemy
    }
    public override bool MissionCompleted()
    {
        return keyFound;
    }
    private void PickUpKey()
    {
        keyFound = true;
        MissionObjectKey.OnKeyPickedUp -= PickUpKey;
        UI.Instance.inGameUI.UpdateMissionInfo("You've got a key!\nGet to the evacuation point.");
    }
}
