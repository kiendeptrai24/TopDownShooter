using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.Lumin;
using UnityEngine.SceneManagement;

public class LevelGenerator : MonoBehaviour
{
    public static LevelGenerator Instance {get; private set;}
    private List<Enemy> enemyList;
    //navmesh
    [SerializeField] private NavMeshSurface navMeshSurface;

    [Space] 

    //level parts
    [SerializeField] private Transform lastLevelPart;
    [SerializeField] private List<Transform> levelParts;
    private List<Transform> currentLevelParts;
    private List<Transform> generatedLevelParts = new List<Transform>();

    // snap points
    [SerializeField] private SnapPoint nextSnapPoint;
    private SnapPoint defaultSnapPoint;

    //cooldown
    [Space]
    [SerializeField] private float generationCooldown;
    private float cooldownTimer;
    private bool generationOver = true;
    private bool generationFinish = false;
    private void Awake() {
        Instance = this;
    }
    void Start()
    {
        enemyList = new List<Enemy>();
        defaultSnapPoint = nextSnapPoint;
        
    }
    private void Update() {
        if(generationOver)
            return;
        cooldownTimer -= Time.deltaTime;
        if(cooldownTimer < 0)
        {
            if(currentLevelParts.Count > 0)
            {
                cooldownTimer = generationCooldown;
                GenerateNextLevelPart();
            }
            else if(generationOver == false)
            {
                FinishGeneration();
            }
        }
    }
    [ContextMenu("Restart generation")]
    public void InitializeGeneration()
    {
        if(generationFinish == true)
            return;
        nextSnapPoint = defaultSnapPoint;
        generationOver = false;
        currentLevelParts = new List<Transform>(levelParts);

        DestroyOldLevelPartsAndEnemies();
    }

    private void DestroyOldLevelPartsAndEnemies()
    {
        foreach (Enemy enemy in enemyList)
        {
            Destroy(enemy.gameObject);
        }
        foreach (Transform t in generatedLevelParts)
        {
            Destroy(t.gameObject);
        }
        generatedLevelParts = new List<Transform>();
        enemyList = new List<Enemy>();

    }

    private void FinishGeneration()
    {
        StartCoroutine(BuildNavMeshWithLoading());
    }
    private IEnumerator BuildNavMeshWithLoading()
    {
        generationOver = true;
        GenerateNextLevelPart();

        // Xây dựng NavMesh trong một frame khác để tránh lag
        yield return new WaitForSeconds(0.5f);

        navMeshSurface.BuildNavMesh();

        BuildEnemies();

        MissionManager.Instance.StartMission();
        // Hoàn thành, tắt UI
        UI.Instance.SwitchToInGameUI();
        generationFinish = true;
    }

    private void BuildEnemies()
    {
        foreach (Enemy enemy in enemyList)
        {
            enemy.transform.parent = null;
            enemy.gameObject.SetActive(true);
        }
    }

    [ContextMenu("Create next level part")]
    private void GenerateNextLevelPart()
    {
        Transform newPart = null;

        if(generationOver)
            newPart = Instantiate(lastLevelPart);
        else
            newPart = Instantiate(ChooseRandomPart());
            
        generatedLevelParts.Add(newPart);
        LevelPart levelPartScript = newPart.GetComponent<LevelPart>();

        levelPartScript.SnapAndAlignPartTo(nextSnapPoint);

        if(levelPartScript.IntersectionDetected())
        {
            Debug.LogWarning("Intersection between level parts");
            InitializeGeneration();
            return;
        }

        nextSnapPoint = levelPartScript.GetExitPoint();
        enemyList.AddRange(levelPartScript.MyEnemies());
        
    }
    private Transform ChooseRandomPart()
    {
        int randomIndex = Random.Range(0,currentLevelParts.Count);
        Transform choosePart = currentLevelParts[randomIndex];

        currentLevelParts.RemoveAt(randomIndex);
        return choosePart;
    }
    public Enemy GetRandomEnemy()
    {
        int randomIndex = Random.Range(0, enemyList.Count);
        return enemyList.Count <= 0 ? null : enemyList[randomIndex];
    }
    public List<Enemy> GetEnemyList() => enemyList == null ? new List<Enemy>() : enemyList;
}
