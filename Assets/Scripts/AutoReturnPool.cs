using UnityEngine;

public class AutoReturnPool : MonoBehaviour
{
  [Header("Settings")]
  [SerializeField] private float _passDistance;

  private Transform _playerTransform;

  private void Start()
    {
        GameObject _playerobj = GameObject.FindGameObjectWithTag("Player");
        if(_playerobj != null)_playerTransform = _playerobj.transform;

    }

    private void Update()
    {
        if(_playerTransform == null) return;

        if(transform.position.z < _playerTransform.position.z - _passDistance)
        {
            if(ObstacleSpawn.instance != null)
            ObstacleSpawn.instance.ReturnToPool(gameObject);
        }
    }
}
