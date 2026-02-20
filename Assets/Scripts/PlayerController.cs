using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody _playerRigidbody;
    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private GameObject _gameOverPanel;
    [Header("Movement Settings")]
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _maxspeed = 40f;
    [SerializeField] private int _currentlane = 1;
    [SerializeField] private float _laneChangeSpeed = 8f;
    [SerializeField] private int _laneDistance = 3;
    [SerializeField] private float _acceleration = 0.1f;

    [Header("Jump Settings")]
    [SerializeField] private float _jumpForce = 10f;
    [SerializeField] private bool _isGrounded = true;

    [Header("Die Settings")]
    public static bool isGameOver = false;


    [Header("Swipe Settings")]
    [SerializeField] private float _swipeTreshHold = 50f;
    private Vector2 _startTouchPosition;
    private Vector2 _endTouchPosition;

    private void Start()
    {
        _playerRigidbody = GetComponent<Rigidbody>();
        _playerAnimator = GetComponent<Animator>();
    }
    private void Update()
    {
        if(isGameOver) return;
        if(_speed < _maxspeed)
        {
            _speed += _acceleration * Time.deltaTime;
        }
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
        CheckInput();
        SetMove();
    }

    private void CheckInput()
    {


        if ((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) && _currentlane > 0)
        {
            _currentlane--;
        }
        if ((Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) && _currentlane < 2)
        {
            _currentlane++;
        }
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
        {
            if (_isGrounded) SetJump();
        }

        if (Input.GetMouseButtonDown(0))
        {
            _startTouchPosition = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            _endTouchPosition = Input.mousePosition;
            AnalyizeSwipe();
        }

    }
    private void AnalyizeSwipe()
    {
        float distanceX = _endTouchPosition.x - _startTouchPosition.x;
        float distanceY = _endTouchPosition.y - _startTouchPosition.y;
        if (Mathf.Abs(distanceX) > Mathf.Abs(distanceY))
        {
            if (Mathf.Abs(distanceX) > _swipeTreshHold)
            {
                if (distanceX > 0 && _currentlane < 2) _currentlane++;
               
                else if (distanceX < 0 && _currentlane > 0) _currentlane--;
            }
        }
        else
        {
            if (Mathf.Abs(distanceY) > _swipeTreshHold)
            {
                if (distanceY > 0 && _isGrounded) SetJump();
            }
        }

    }

    private void SetMove()
    {
        Vector3 targetPosition = transform.position;
        targetPosition.x = (_currentlane - 1) * _laneDistance;
        transform.position = Vector3.Lerp(transform.position, targetPosition, _laneChangeSpeed * Time.deltaTime);
    }
    private void SetJump()
    {
        _playerRigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        _isGrounded = false;
        _playerAnimator.SetBool("canJump",true);

    }
    private void Die()
    {
        if(isGameOver) return;

        _speed = 0;
        _acceleration = 0;
        
        _playerAnimator.SetTrigger("isDeath");
        _playerAnimator.SetFloat("speed", 0);
        
        StartCoroutine(GameOver());
        Debug.Log("Game Over");
       
    }
    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(1.5f);
        _gameOverPanel.SetActive(true);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGrounded = true;
            _playerAnimator.SetBool("canJump", false);
        }
        if(collision.gameObject.CompareTag("Obstacle"))
        {
            
           Die();
           isGameOver = true;
        }
    }
}
