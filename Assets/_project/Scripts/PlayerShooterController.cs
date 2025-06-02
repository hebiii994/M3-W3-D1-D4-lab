using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooterController : MonoBehaviour
{
    public static PlayerShooterController instance;

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

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.Log("Associare Audio lancio freccie a Gun");
        }
    }

    //gestione variabili sparo
    [SerializeField] private float _fireRate = 1.0f;
    [SerializeField] private float _fireRange = 6.0f;
    private float lastShotTime = 0f;

    //variabili prefab e GO
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private AudioClip _arrowFireSound;   
    private AudioSource _audioSource;
    public List<GameObject> _enemiesList;
    private GameObject _currentTarget;



    
    void Start()
    {

        _enemiesList = new List<GameObject>();
        
        if (_bulletPrefab == null)
        {
            Debug.Log("Non è stato assegnato un proiettile al player");
        }
    }
  

    // Update is called once per frame
    void Update()
    {


        if (_currentTarget == null)
        {
            _enemiesList.RemoveAll(item => item == null);
        }

        if (_currentTarget == null)
        {
            _currentTarget = FindNearestEnemy(_enemiesList);
            if (_currentTarget == null)
            {
                return;
            }
        }
        float distanceToCurrentTarget = Vector3.Distance(transform.position, _currentTarget.transform.position);
        if (distanceToCurrentTarget > _fireRange)
        {
            _currentTarget = null;
            return;
        }

        if (Time.time - lastShotTime > _fireRate)
        {
                Vector2 direction = (_currentTarget.transform.position - transform.position).normalized;
                lastShotTime = Time.time;
                Shoot(direction);

        }
        
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

    public void FireRateDown()
    {
        if (_fireRate <= 0.4f)
        {
            return;
        }
        _fireRate -= 0.1f;
    }
   
}
