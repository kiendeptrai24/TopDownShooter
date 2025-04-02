
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryEnemy : MonoBehaviour
{
    private static FactoryEnemy _instance;
    public static FactoryEnemy Instance 
    { 
        get 
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<FactoryEnemy>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject("FactoryEnemy");
                    _instance = obj.AddComponent<FactoryEnemy>();
                }
            }
            return _instance;
        }
    }

    private Transform point;
    public static event Action OnSpawnAreaUpdated;
    [SerializeField] private LayerMask whatIsGround;
    [Header("Enemies Settings")]
    public List<GameObject> enemiesMelee = new();
    public List<GameObject> enemiesRange = new();
    public List<GameObject> enemiesBoss = new();
    public List<GameObject> allEnemies = new();
    private List<BoxCollider> boxes = new();
    List<GameObject> enemiesGenerate = new();
    private void Awake()
    {
        point = GetComponentInChildren<Transform>();   
    }
    public void GetRepondArenaEnemy(BoxCollider[] boxColliders)
    {
        boxes.AddRange(boxColliders);
    }
    public Enemy GetRandomEnemy()
    {
        if(enemiesGenerate.Count <= 0)
        {
            GetEnemies(EnemyType.Boss,1);
        }
        int randomIndex = UnityEngine.Random.Range(0,enemiesGenerate.Count);
        return enemiesGenerate[randomIndex].GetComponent<Enemy>();
    }

    public List<GameObject> GetEnemies(EnemyType enemyType, int amountToKill)
    {
        OnSpawnAreaUpdated?.Invoke();
        
        List<GameObject> enemyList = null;

        switch (enemyType)
        {
            case EnemyType.Melee:
                enemyList = enemiesMelee;
                break;
            case EnemyType.Ranged:
                enemyList = enemiesRange;
                break;
            case EnemyType.Boss:
                enemyList = enemiesBoss;
                break;
            case EnemyType.Random:
                enemyList = enemiesBoss;
                break;
            default:
                Debug.LogError("Loại kẻ địch không hợp lệ!");
                return enemiesGenerate;
        }


        if (enemyList.Count == 0)
        {
            Debug.LogWarning($"Không có kẻ địch thuộc loại {enemyType}");
            return enemiesGenerate;
        }
        SetEnemiesPosition(amountToKill, enemiesGenerate, enemyList);

        return enemiesGenerate;
    }

    private void SetEnemiesPosition(int amountToKill, List<GameObject> enemiesGenerate, List<GameObject> enemyList)
    {
        for (int i = 0; i < amountToKill + 5; i++)
        {
            if (boxes.Count > 0)
            {
                SetEnemiesPosistion();
            }

            int randomIndex = UnityEngine.Random.Range(0, enemyList.Count);

            GameObject enemy = ObjectPool.Instance.GetObject(enemyList[randomIndex], point);
            enemy.AddComponent<MissionObject_HuntTarget>();
            enemiesGenerate.Add(enemy);
        }
    }

    private void SetEnemiesPosistion()
    {
        int randomIndexBox = UnityEngine.Random.Range(0, boxes.Count);
        point.position = GetRandomPositionInsideBox(boxes[randomIndexBox]);

        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, Vector3.down, out hitInfo, 10, whatIsGround))
        {
            point.transform.position = hitInfo.point;
        }
    }

    private Vector3 GetRandomPositionInsideBox(BoxCollider box)
    {
        Vector3 min = box.bounds.min; // Góc dưới bên trái của BoxCollider
        Vector3 max = box.bounds.max; // Góc trên bên phải của BoxCollider

        float x = UnityEngine.Random.Range(min.x, max.x);
        float y = UnityEngine.Random.Range(min.y, max.y);
        float z = UnityEngine.Random.Range(min.z, max.z);

        return new Vector3(x, y, z);
    }
}
