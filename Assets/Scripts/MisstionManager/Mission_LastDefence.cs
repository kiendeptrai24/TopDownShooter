using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Defence Mission", menuName = "Missions/Defence - Mission")]
public class Mission_LastDefence : Mission
{
    public bool defenceBegun = false;
    [Header("Cooldown and Duration")]
    public float defenceDuration = 120;
    private float defenceTimer;
    public float waveColldown = 15;
    private float waveTimer;

    [Header("Respawn details")]

    public int amountOfRepawwnSpoints=2;
    public List<Transform> respawnPoints = new List<Transform>();
    private Vector3 defencePoint;
    [Space]
    public int enemiesPerWave;
    public GameObject[] possibleEnemies;
    private string defenceTimerText;
    private void OnEnable() {
        defenceBegun = false;
    }
    public override void StartMisstion()
    {
        defencePoint = FindObjectOfType<MissionEnd_Trigger>().transform.position;
        respawnPoints = new List<Transform>(ClosestPoints(amountOfRepawwnSpoints));
    }
    public override bool MissionCompleted()
    {
        if(defenceBegun == false)
        {
            StartDefenceEvent();
            return false;
        }
        return defenceTimer < 0;
    }
    private void StartDefenceEvent()
    {
        waveTimer = .5f;
        defenceTimer = defenceDuration;
        defenceBegun = true;
    }
    public override void UpdateMission()
    {
        if(defenceBegun == false)
            return;
        defenceTimer -= Time.deltaTime;
        waveTimer -= Time.deltaTime;
        if (waveTimer < 0)
        {
            CreateNewEnemies(enemiesPerWave);
            waveTimer = waveColldown;
        }
        defenceTimerText = System.TimeSpan.FromSeconds(defenceTimer).ToString("mm':'ss");
        Debug.Log(defenceTimerText);
    }
    private void CreateNewEnemies(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            int randomEnemyIndex = Random.Range(0, possibleEnemies.Length);
            int randomRespawnPointIndex = Random.Range(0, respawnPoints.Count);

            Transform randomRespawnPoint = respawnPoints[randomRespawnPointIndex];
            GameObject randomEnemy = possibleEnemies[randomEnemyIndex];
            
            randomEnemy.GetComponent<Enemy>().aggressionRange = 100;

            ObjectPool.Instance.GetObject(randomEnemy, randomRespawnPoint);

        }
    }
    private List<Transform> ClosestPoints(int amount)
    {
        List<Transform> closestPoints = new List<Transform>();
        List<MissionObject_EnemyRepawnPoint> allPoints = new List<MissionObject_EnemyRepawnPoint>(FindObjectsOfType<MissionObject_EnemyRepawnPoint>());

        while(closestPoints.Count < amount && allPoints.Count > 0)
        {
            float minDistance = Mathf.Infinity;
            MissionObject_EnemyRepawnPoint closestPoint = null;
            foreach (MissionObject_EnemyRepawnPoint point in allPoints)
            {
                float distance = Vector3.Distance(point.transform.position, defencePoint);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestPoint = point;
                }
            }
            // important to must remember if you delete it, this loop will never end
            if(closestPoint != null)
            {
                closestPoints.Add(closestPoint.transform);
                allPoints.Remove(closestPoint);
            }
        }

        return closestPoints;
    }
}
