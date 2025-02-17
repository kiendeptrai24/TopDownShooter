using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private Transform lastLevelPart;
    [SerializeField] private List<Transform> levelParts;
    private List<Transform> currentLevelParts;
    private List<Transform> generatedLevelParts = new List<Transform>();
    [SerializeField] private SnapPoint nextSnapPoint;
    private SnapPoint defaultSnapPoint;
    [Space]
    [SerializeField] private float generationCooldown;
    private float cooldownTimer;
    private bool generationOver;

    void Start()
    {
        defaultSnapPoint = nextSnapPoint;
        InitializeGeneration();
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
    private void InitializeGeneration()
    {
        nextSnapPoint = defaultSnapPoint;
        generationOver = false;
        currentLevelParts = new List<Transform>(levelParts);

        foreach (Transform t in generatedLevelParts)
        {
            Destroy(t.gameObject);
        }
        generatedLevelParts = new List<Transform>();
    }

    private void FinishGeneration()
    {
        generationOver = true;
        GenerateNextLevelPart();

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
        
    }
    private Transform ChooseRandomPart()
    {
        int randomIndex = Random.Range(0,currentLevelParts.Count);
        Transform choosePart = currentLevelParts[randomIndex];

        currentLevelParts.RemoveAt(randomIndex);
        return choosePart;
    }
}
