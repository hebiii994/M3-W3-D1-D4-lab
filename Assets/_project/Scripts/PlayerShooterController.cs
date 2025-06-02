using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooterController : MonoBehaviour
{
    public static PlayerShooterController instance;

    //gestione variabili sparo
    [SerializeField] private float _fireRate = 1.0f;
    [SerializeField] private float _fireRange = 6.0f;
    [SerializeField] private Transform _firePoint;
    private float _lastShotTime = -Mathf.Infinity;

    private float _baseFireRate; 
    private float _currentDamageMultiplier = 1.0f;
    private Coroutine _damageBoostCoroutine;
    private Coroutine _fireRateBoostCoroutine;

    //variabili prefab e GO
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private AudioClip _arrowFireSound;   
    private AudioSource _audioSource;
    public List<GameObject> _enemiesList;
    private GameObject _currentTarget;
    private PlayerController _playerControllerReference;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        _baseFireRate = _fireRate;
        _audioSource = GetComponent<AudioSource>();
        _playerControllerReference = GetComponent<PlayerController>();
        if (_playerControllerReference == null)
        {
            Debug.LogError("PlayerController non trovato da PlayerShooterController! Assicurati che entrambi gli script siano sullo stesso GameObject del Giocatore.");
        }

        if (_audioSource == null)
        {
            Debug.Log("Associare Audio lancio freccie a Gun");
        }
        if (_bulletPrefab == null)
        {
            Debug.LogError("Prefab del proiettile non assegnato a PlayerShooterController.");
        }

        if (_firePoint == null)
        {
            // Se non hai un firePoint specifico, usa la posizione del PlayerShooterController
            _firePoint = transform;
            Debug.LogWarning("FirePoint non assegnato, si userà la posizione del PlayerShooterController.");
        }
    }

    public float GetCurrentDamageMultiplier()
    {
        return _currentDamageMultiplier;
    }

    public void ApplyDamageBoost(float multiplier, float duration)
    {
        if (_damageBoostCoroutine != null)
        {
            StopCoroutine(_damageBoostCoroutine); 

            _currentDamageMultiplier /= PlayerPrefs.GetFloat("LastDamageMultiplier", 1f);
            if (_playerControllerReference != null) _playerControllerReference.SetDamageBoostVisual(false);
        }
        PlayerPrefs.SetFloat("LastDamageMultiplier", multiplier); 
        _damageBoostCoroutine = StartCoroutine(DamageBoostCoroutine(multiplier, duration));
    }

    private IEnumerator DamageBoostCoroutine(float multiplier, float duration)
    {
        _currentDamageMultiplier *= multiplier;
        if (_playerControllerReference != null)
        {
            _playerControllerReference.SetDamageBoostVisual(true); 
        }
        Debug.Log($"Damage boost applicato: x{_currentDamageMultiplier} per {duration}s.");
        yield return new WaitForSeconds(duration);
        _currentDamageMultiplier /= multiplier; 
        PlayerPrefs.DeleteKey("LastDamageMultiplier");
        if (_playerControllerReference != null)
        {
            _playerControllerReference.SetDamageBoostVisual(false); // Disattiva l'effetto rosso
        }
        Debug.Log("Damage boost scaduto.");
        _damageBoostCoroutine = null;
    }

    void Start()
    {

        _enemiesList = new List<GameObject>();
        
    }
  

    // Update is called once per frame
    void Update()
    {


        //if (_currentTarget == null)
        //{
        //    _enemiesList.RemoveAll(item => item == null);
        //}

        //if (_currentTarget == null)
        //{
        //    _currentTarget = FindNearestEnemy(_enemiesList);
        //    if (_currentTarget == null)
        //    {
        //        return;
        //    }
        //}
        //float distanceToCurrentTarget = Vector3.Distance(transform.position, _currentTarget.transform.position);
        //if (distanceToCurrentTarget > _fireRange)
        //{
        //    _currentTarget = null;
        //    return;
        //}

        //if (Time.time - _lastShotTime > _fireRate)
        //{
        //        Vector2 direction = (_currentTarget.transform.position - transform.position).normalized;
        //        _lastShotTime = Time.time;
        //        Shoot(direction);

        //}
        
    }

    public bool CanShoot()
    {
        return Time.time >= _lastShotTime + _fireRate;
    }

    public void ShootInDirection(Vector2 direction)
    {
        if (_bulletPrefab == null) return;

        Bullet b = Instantiate(_bulletPrefab, _firePoint.position, Quaternion.identity);
        b.Dir = direction;
        b.transform.up = direction;

        if (_arrowFireSound != null && _audioSource != null)
        {
            _audioSource.PlayOneShot(_arrowFireSound);
        }
        _lastShotTime = Time.time; 
    }

    public GameObject FindNearestEnemy(List<GameObject> enemiesList)
    {
        // TO DO // 
        //tenere traccia dei nemici e ritornare il più vicino al giocatore, potrei passare un array o una lista di nemici
        // devo però restituire solo il più vicino quindi forse meglio passare solo 1 valore ma leggendo dalla lista inizialmente creavo un secondo array con i nemici in range

        GameObject enemiesInRange = null;
        float minDistance = Mathf.Infinity;
        Vector3 playerPosition = transform.position;

        foreach (GameObject enemy in enemiesList)
        {
            if (enemy == null)
            {
                
                continue;
            }

            // distanza con il nemico con .distance magari

            float distanceToEnemies = Vector3.Distance(playerPosition, enemy.transform.position);

            if (distanceToEnemies <= _fireRange && distanceToEnemies < minDistance)
            {
                enemiesInRange = enemy;
            }
            
        }
        return enemiesInRange;

        
    }

    public void Shoot(Vector2 direction)
    {
        

        // se c'è un nemico vicino deve creare un bullet e dare al suo RB velocità e direzione
        Bullet b = Instantiate(_bulletPrefab, transform.position, Quaternion.identity);
        b.Dir = direction;
        b.transform.up = direction;

        if (_arrowFireSound != null)
        {
            _audioSource.PlayOneShot(_arrowFireSound);
        }
    }

    public void AddEnemyToList(GameObject enemy)
    {
        if (!_enemiesList.Contains(enemy))
        {
            _enemiesList.Add(enemy);
        }
    }
    public void RemoveEnemyFromList(GameObject enemy)
    {
        if (_enemiesList.Contains(enemy))
        {
            _enemiesList.Remove(enemy);
        }
    }

    public void ApplyFireRateBoost(float fireRateDecrease, float duration)
    {
        if (_fireRateBoostCoroutine != null)
        {
            StopCoroutine(_fireRateBoostCoroutine);
            _fireRate = _baseFireRate; 
        }
        _fireRateBoostCoroutine = StartCoroutine(FireRateBoostCoroutine(fireRateDecrease, duration));
    }

    private IEnumerator FireRateBoostCoroutine(float fireRateDecrease, float duration)
    {
        float originalFireRateForThisBoost = _fireRate; 
        _fireRate -= fireRateDecrease;
        if (_fireRate < 0.1f) _fireRate = 0.1f; 

        yield return new WaitForSeconds(duration);

        _fireRate = originalFireRateForThisBoost; 
        _fireRateBoostCoroutine = null;
    }

}
