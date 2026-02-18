using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    [Header("Object Pooling Settings")]
    [SerializeField] private GameObject objectprefabs;
    [SerializeField] private int PoolSize = 15;
    [SerializeField] private float _tileLength;

    [Header("References")]
    [SerializeField] private Transform _playerTransform;

    [Header("Spawn Settings")]
    [SerializeField] private float _spawnZ = -3.5f;
    [SerializeField] private int _amountOfTilesOnScreen = 15;

    public Queue<GameObject> objectPool = new Queue<GameObject>();
    
    private void Start()
    {
        _tileLength = objectprefabs.GetComponent<MeshRenderer>().bounds.size.z;
        
        for (int i = 0; i < PoolSize; i++)
        {
            GameObject obj = Instantiate(objectprefabs,Vector3.zero,objectprefabs.transform.rotation);
            var tileManager = obj.GetComponent<TileObstacleManager>();
            
           obj.SetActive(false);
            objectPool.Enqueue(obj);
        }
        for (int i = 0; i < _amountOfTilesOnScreen; i++)
        {
            GetPooledObject();
        }

    }
    private void Update() {
        if(_playerTransform.position.z - 18  > _spawnZ - (_amountOfTilesOnScreen * _tileLength))
        {
            GetPooledObject();
        }
    }
    
    public void GetPooledObject()
    {
        GameObject obj =objectPool.Dequeue();
        obj.transform.position = new Vector3(0,0,_spawnZ);
        _spawnZ += _tileLength;
        obj.SetActive(true);
        var tileManager = obj.GetComponent<TileObstacleManager>();
        if (tileManager != null)
        {
            tileManager.ActiveteTile();
        }
        objectPool.Enqueue(obj);
       
    }
}

