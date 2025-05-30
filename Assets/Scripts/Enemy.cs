using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 2.0f;
    private GameObject _player;
    private PlayerController _playerController;
    
    // Start is called before the first frame update

    private void Awake()
    {
        _playerController = FindObjectOfType<PlayerController>();
        _player = _playerController.gameObject;

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        EnemyMovement();
    }

    public void EnemyMovement()
    {
        if (_playerController != null)
        {
            Vector2 newPosition = Vector2.MoveTowards(transform.position, _playerController.transform.position, _speed * Time.deltaTime);
            transform.position = newPosition;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Destroy(collision.gameObject);
            
        }
    }
}
