using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 2.0f;
    [SerializeField] private float _health = 10f;
    [SerializeField] private float _baseScale = 1.0f;
    [SerializeField] private PowerUp _powerUpPrefab;
    [SerializeField] private Rigidbody _rbEnemy;
    private GameObject _playerGameObject;
    [SerializeField][Range(0f, 1f)] private float _dropChance = 0.25f;
    [SerializeField] private int _damageToPlayer = 10;
    [SerializeField] private float _bounceForce = 5f;



    private float _currentHealth;
    private float _currentSpeed;
    private bool _isDead = false;

    // Start is called before the first frame update

    private void Awake()
    {
        Setup(_health, _speed, _baseScale);
        _playerGameObject = GameObject.FindGameObjectWithTag("Player");
        _rbEnemy = GetComponent<Rigidbody>();

        if (_playerGameObject == null)
        {
            Debug.LogError("Nemico non riesce a trovare un oggetto con il tag 'Player' nella scena!");
        }

        
    }

    public void Setup(float health, float speed, float scale)
    {
        _currentHealth = health;
        _currentSpeed = speed;
        transform.localScale = Vector3.one * scale;
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
        EnemyMovement(_currentSpeed);
    }

    public void EnemyMovement(float speed)
    {
        if (_playerGameObject != null)
        {
            Vector2 newPosition = Vector2.MoveTowards(transform.position, _playerGameObject.transform.position, speed * Time.deltaTime);
            transform.position = newPosition;
        }
    }
    public void TakeDamage(float damage)
    {
        if (_isDead)
        {
            return;
        }
        _health -= damage;
        //suono hit freccia
        

        if (_health <= 0 && !_isDead)
        {
            
            _isDead = true;
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
            if (collision.gameObject.TryGetComponent<PlayerLifeController>(out PlayerLifeController playerLife))
            {
                
                playerLife.TakeDamage(_damageToPlayer);
            }

            Vector2 bounceDirection = (transform.position - collision.transform.position).normalized;
            if (_rbEnemy != null)
            {
                _rbEnemy.AddForce(bounceDirection * _bounceForce, (ForceMode)ForceMode2D.Impulse);
            }

        }
    }
}
