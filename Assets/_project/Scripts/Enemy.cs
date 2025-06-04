using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [System.Serializable]
    public class PowerUpDrop
    {
        public GameObject powerUpPrefab; 
        public float weight = 1.0f;      
    }

    [SerializeField] private float _speed = 1.5f;
    [SerializeField] private float _health = 3f;
    [SerializeField] private float _baseScale = 0.8f;
    [SerializeField] private Rigidbody2D _rbEnemy;
    private GameObject _playerGameObject;
    [SerializeField] private int _damageToPlayer = 10;
    [SerializeField] private float _bounceForce = 3f;

    [Header("Power-Up Drops")]
    [SerializeField] private List<PowerUpDrop> _powerUpDropTable;
    [SerializeField][Range(0f, 1f)] private float _overallDropChance = 0.25f;

   
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private Vector2 _lastMoveDirection = Vector2.down; 
    private Vector2 _currentMoveIntent = Vector2.zero;


    public float BaseHealthForScaling => _health;
    public float BaseSpeedForScaling => _speed;
    public float BaseScaleForScaling => _baseScale;


    private float _currentHealth;
    private float _currentSpeed;
    private bool _isDead = false;

    // Start is called before the first frame update

    private void Awake()
    {
        //Setup(_health, _speed, _baseScale);
        _playerGameObject = GameObject.FindGameObjectWithTag("Player");
        _rbEnemy = GetComponent<Rigidbody2D>();

        if (_playerGameObject == null)
        {
            Debug.LogError("Nemico non riesce a trovare un oggetto con il tag 'Player' nella scena!");
        }
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        if (_animator == null) Debug.LogError($"Animator mancante sul nemico {gameObject.name}!");
        if (_spriteRenderer == null) Debug.LogError($"SpriteRenderer mancante sul nemico {gameObject.name}!");

        
        if (_playerGameObject == null) _playerGameObject = GameObject.FindGameObjectWithTag("Player");
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
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_isDead)
        {
            
            _animator.SetBool("isMoving", false); 
            return;
        }
        _currentMoveIntent = Vector2.zero;

        if (_playerGameObject != null)
        {
            Vector2 directionToPlayer = (_playerGameObject.transform.position - transform.position);

           
            if (directionToPlayer.sqrMagnitude > 0.1f) 
            {
                _currentMoveIntent = directionToPlayer.normalized;
                _lastMoveDirection = _currentMoveIntent; 
            }
        }
        bool isActuallyMoving = _currentMoveIntent.sqrMagnitude > 0.01f;
        _animator.SetBool("isMoving", isActuallyMoving);

        
        Vector2 animationDirection = _lastMoveDirection;

        _animator.SetFloat("moveX", Mathf.Abs(animationDirection.x));
        _animator.SetFloat("moveY", animationDirection.y);

        
        if (animationDirection.x > 0.01f) 
        {
            _spriteRenderer.flipX = false;
        }
        else if (animationDirection.x < -0.01f) 
        {
            _spriteRenderer.flipX = true;
        }
    }

    private void FixedUpdate()
    {
        if (_isDead || _rbEnemy == null) return;

        if (_currentMoveIntent.sqrMagnitude > 0.01f)
        {
            _rbEnemy.velocity = _currentMoveIntent * _currentSpeed;
        }
        else
        {
            _rbEnemy.velocity = Vector2.zero; // Fermati se non c'è intenzione di movimento
        }
    }

    //public void EnemyMovement(float speed)
    //{
    //    if (_playerGameObject != null)
    //    {
    //        Vector2 newPosition = Vector2.MoveTowards(transform.position, _playerGameObject.transform.position, speed * Time.deltaTime);
    //        transform.position = newPosition;
    //    }
    //}
    public void TakeDamage(float damage)
    {
        if (_isDead)
        {
            return;
        }
        _currentHealth -= damage;
        //suono hit freccia
        

        if (_currentHealth <= 0 && !_isDead)
        {
            
            _isDead = true;
            Die();
        }
    }

    private void Die()
    {
        //RoundManager.instance.totalEnemiesEliminatedInRound++;
        if (RoundManager.instance != null)
        {
            RoundManager.instance.ReportEnemyKilled();
        }

        if (Random.Range(0f, 1f) <= _overallDropChance)
        {
            SpawnRandomPowerUp();
        }

        Destroy(this.gameObject);
    }
    private void SpawnRandomPowerUp()
    {
        if (_powerUpDropTable == null || _powerUpDropTable.Count == 0) return;

        float totalWeight = 0f;
        foreach (var drop in _powerUpDropTable)
        {
            totalWeight += drop.weight;
        }

        float randomValue = Random.Range(0, totalWeight);
        float cumulativeWeight = 0f;

        foreach (var drop in _powerUpDropTable)
        {
            cumulativeWeight += drop.weight;
            if (randomValue <= cumulativeWeight)
            {
                if (drop.powerUpPrefab != null)
                {
                    Instantiate(drop.powerUpPrefab, transform.position, Quaternion.identity);
                    Debug.Log($"PowerUp {drop.powerUpPrefab.name} spawnato da {gameObject.name}");
                }
                return; 
            }
        }
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
                _rbEnemy.AddForce(bounceDirection * _bounceForce, ForceMode2D.Impulse);
                Destroy(this.gameObject, 0.4f);
                RoundManager.instance.totalEnemiesEliminatedInRound++;
            }

        }
    }
}
