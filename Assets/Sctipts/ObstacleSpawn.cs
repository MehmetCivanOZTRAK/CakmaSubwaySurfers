using UnityEngine;
using System.Collections.Generic;

public class ObstacleSpawn : MonoBehaviour
{
    public static ObstacleSpawn instance;

    [Header("Obstacle Spawn Settings")]
    [SerializeField] private GameObject[] obstaclePrefabs;
    [SerializeField] private int _poolSize = 10;
    
    private Queue<GameObject> _pool = new Queue<GameObject>();

    private void Awake() {
        instance = this;
        InitializePool();
    }
    private void InitializePool()
    {
        for(int i = 0 ; i < _poolSize; i++)
        {
            GameObject prefab = obstaclePrefabs[Random.Range(0,obstaclePrefabs.Length)];
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            _pool.Enqueue(obj);
        }
    }
    public GameObject GetPooledObject()
    {
        if(_pool.Count == 0) return null;
        
        GameObject obj = _pool.Dequeue();
        obj.SetActive(true);
        return obj;
    }
    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(null);
        _pool.Enqueue(obj);
        
    }


}
