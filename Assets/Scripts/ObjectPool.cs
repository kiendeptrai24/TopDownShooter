using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance { get; private set; }

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int poolSize =10;
    private Queue<GameObject> bulletPool= new();

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
        
        DontDestroyOnLoad(gameObject);
        CreateInitalPool();
    }
    public GameObject GetBullet()
    {
        if(bulletPool.Count == 0)
            CreateNewBullet();
        GameObject bulletToGet = bulletPool.Dequeue();
        bulletToGet.SetActive(true);
        bulletToGet.transform.parent = null;
        return bulletToGet;
    }
    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        bulletPool.Enqueue(bullet);
        bullet.transform.parent = transform;

    }
    private void CreateInitalPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            CreateNewBullet();
        }
    }

    private void CreateNewBullet()
    {
        GameObject newPool = Instantiate(bulletPrefab, transform);
        newPool.SetActive(false);
        bulletPool.Enqueue(newPool);
    }
}
