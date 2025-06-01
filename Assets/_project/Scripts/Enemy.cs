using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 1.7f;
    [SerializeField] private float health = 10f;
    [SerializeField] private PowerUp _powerUpPrefab;
    private GameObject _playerGameObject;
    [SerializeField][Range(0f, 1f)] private float _dropChance = 0.25f;


    // Start is called before the first frame update

    private void Awake()
    {
        _playerGameObject = GameObject.FindGameObjectWithTag("Player");

        if (_playerGameObject == null)
        {
            Debug.LogError("Nemico non riesce a trovare un oggetto con il tag 'Player' nella scena!");
        }
    }

    private void OnEnable()
    {
        if (PlayerShooterController.instance != null)
        {
            PlayerShooterController.instance.AddEnemyToList(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        if (PlayerShooterController.instance != null)
        {
            PlayerShooterController.instance.RemoveEnemyFromList(this.gameObject);
        }
        if (Random.Range(0f,1f) <= _dropChance )
        {
            PowerUp pw = Instantiate(_powerUpPrefab, transform.position,Quaternion.identity);
        }
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
        if (_playerGameObject != null)
        {
            Vector2 newPosition = Vector2.MoveTowards(transform.position, _playerGameObject.transform.position, _speed * Time.deltaTime);
            transform.position = newPosition;
        }
    }
    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        
        Destroy(this.gameObject);
        RoundManager.instance.totalEnemiesEliminatedInRound++;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Destroy(collision.gameObject);
            
        }
    }
}
