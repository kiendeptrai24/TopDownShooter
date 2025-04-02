
using UnityEngine;

public class RespondArea : MonoBehaviour
{
    FactoryEnemy factoryEnemy;
    private BoxCollider[] boxes;
    private void Start() {
        factoryEnemy = FactoryEnemy.Instance;
        FactoryEnemy.OnSpawnAreaUpdated += GetBoxCollider;
    }

    public void GetBoxCollider()
    {
        boxes = GetComponentsInChildren<BoxCollider>();
        factoryEnemy.GetRepondArenaEnemy(boxes);
    }
    private void OnDestroy()
    {
        FactoryEnemy.OnSpawnAreaUpdated -= GetBoxCollider;
    }

}
