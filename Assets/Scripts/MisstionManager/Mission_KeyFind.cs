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
        Debug.Log("I picked up a key");
    }
}
