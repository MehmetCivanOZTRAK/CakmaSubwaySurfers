using UnityEngine;
using System.Collections.Generic;


public class TileObstacleManager : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private Transform[] _obstacleSpawnPoints;

    [Header("Settings")]
    [Range(0f, 1f)]
    [SerializeField] private float _obstacleSpawnChance = 0.75f;

    private List<GameObject> _activeObstacles = new List<GameObject>();

    public void ActiveteTile()
    {
        ClearRandomObstacles();
        SpawnRandomObstacle();
    }
    public void DeactiveTile()
    {
        ClearRandomObstacles();
        gameObject.SetActive(false);
    }
    private void SpawnRandomObstacle()
    {
        int randomIndex = Random.Range(0, _obstacleSpawnPoints.Length);
        Transform spawnPoint = _obstacleSpawnPoints[randomIndex];

        if (Random.value < _obstacleSpawnChance)
        {
            GameObject obstacle = ObstacleSpawn.instance.GetPooledObject();
            if (obstacle != null)
            {
                obstacle.transform.SetParent(null); 
                obstacle.transform.position = spawnPoint.position;
                obstacle.transform.rotation = spawnPoint.rotation;
                obstacle.transform.SetParent(transform, true);

                obstacle.SetActive(true);
                _activeObstacles.Add(obstacle);
            }

        }
    }
    private void ClearRandomObstacles()
    {
        foreach (GameObject obstacle in _activeObstacles)
        {
            ObstacleSpawn.instance.ReturnToPool(obstacle);
        }
        _activeObstacles.Clear();
    }
}
