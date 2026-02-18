using System.Collections.Generic;
using UnityEngine;

public class PropPooler : MonoBehaviour
{
  public static PropPooler instance;
  [SerializeField] private List<GameObject> propPrefabs;
  [SerializeField] private int poolSize = 10;

  private Dictionary<string,Queue<GameObject>> _propPool = new Dictionary<string,Queue<GameObject>>();

  private void Awake()=> instance = this;

  private void Start() {
    foreach (var prefab in propPrefabs)
    {
        Queue<GameObject> objectPool = new Queue<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            objectPool.Enqueue(obj);
        }
        _propPool.Add(prefab.name, objectPool);
    }

  }
  private void Update() {
    GetRandomProps(new Vector3(Random.Range(-2f,2f),0,Random.Range(10f,20f)),Quaternion.identity);
  }
  public GameObject GetRandomProps(Vector3 position , Quaternion rotation)
    {
        string randomProps = propPrefabs[Random.Range(0,propPrefabs.Count)].name;
        GameObject obj = _propPool[randomProps].Dequeue();
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.SetActive(true);

        _propPool[randomProps].Enqueue(obj);
        return obj;
    }
}
