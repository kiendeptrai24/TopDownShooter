using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance { get; private set; }
    [SerializeField] private int poolSize =10;
    private Dictionary<GameObject, Queue<GameObject>> poolDictionary = new();

    [Header("To Initialize")]
    [SerializeField] private GameObject weaponPickup;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this; 
    }    
    private void Start() {
        InitializeNewPool(weaponPickup);
    }
    public GameObject GetObject(GameObject prefab)
    {
        if(poolDictionary.ContainsKey(prefab) == false)
            InitializeNewPool(prefab);
        
        if(poolDictionary[prefab].Count == 0)
            CreateNewObject(prefab);
    
        GameObject objectToGet = poolDictionary[prefab].Dequeue();
        objectToGet.SetActive(true);
        objectToGet.transform.parent = null;
        return objectToGet;
    }
    public void ReturnObject(GameObject objectToReturn,float delay = .001f) => StartCoroutine(DelayReturn(objectToReturn,delay)); 
    private IEnumerator DelayReturn(GameObject objectToReturn,float delay)
    {
        yield return new WaitForSeconds(delay);
        ReturnToPool(objectToReturn);
    }
    private void ReturnToPool(GameObject objectToReturn)
    {
        GameObject originalToReturn = objectToReturn.GetComponent<PooledObject>().originalPrefab; 

        objectToReturn.SetActive(false);
        objectToReturn.transform.parent = transform;
        poolDictionary[originalToReturn].Enqueue(objectToReturn);

    }
    private void InitializeNewPool(GameObject prefab)
    {
        poolDictionary[prefab] = new Queue<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            CreateNewObject(prefab);
        }
    }

    private void CreateNewObject(GameObject prefab)
    {
        GameObject newObject = Instantiate(prefab, transform);
        newObject.AddComponent<PooledObject>().originalPrefab = prefab;
        newObject.SetActive(false);
        poolDictionary[prefab].Enqueue(newObject);
    }
}
