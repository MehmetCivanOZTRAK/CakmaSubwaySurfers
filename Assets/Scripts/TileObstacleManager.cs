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
        List<int> availableLines = new List<int>{0,1,2};

        int spawnCount = Random.Range(1,3);

        for(int i = 0; i < spawnCount; i++)
        {
            if(Random.value <= _obstacleSpawnChance)
            {
                int randomIndex = Random.Range(0,availableLines.Count);
                int laneId = availableLines[randomIndex];

                availableLines.RemoveAt(randomIndex);

                Transform selectedPoint = _obstacleSpawnPoints[laneId];
                if(ObstacleSpawn.instance!= null)
                {
                    GameObject obstacle = ObstacleSpawn.instance.GetPooledObject();

                    if(obstacle != null)
                    {
                        obstacle.transform.SetParent(null);
                        obstacle.transform.position = new Vector3(selectedPoint.position.x,1.1f,selectedPoint.position.z);
                        obstacle.transform.rotation = Quaternion.Euler(0,180,0);
                        

                        _activeObstacles.Add(obstacle);
                    }
                }
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
