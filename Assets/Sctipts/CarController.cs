using UnityEngine;

public class CarController : MonoBehaviour
{
   [Header("Move Settings")]
   [SerializeField] private float _speed;


   private void Update() {
    if(PlayerController.isGameOver) return;
    transform.Translate(Vector3.forward * _speed * Time.deltaTime );
    
   }
}
