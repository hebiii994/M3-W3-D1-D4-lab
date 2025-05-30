using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooterController : MonoBehaviour
{
    [SerializeField] private float _fireRate = 0.5f;
    private float lastShotTime = 0f;
    [SerializeField] private float _fireRange = 10.0f;
    [SerializeField] private Bullet _bulletPrefab;
    
    public List<GameObject> _enemiesList;

    // Start is called before the first frame update
    void Start()
    {

        _enemiesList = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
        if (_enemiesList.Count > 0)
        {
            Debug.Log("Si comincia!");
        }
        if (_bulletPrefab == null)
        {
            Debug.Log("Non è stato assegnato un proiettile al player");
        }
    }

    // Update is called once per frame
    void Update()
    {
        GameObject enemiesToHit = FindNearestEnemy(_enemiesList);
        if (Time.time - lastShotTime > _fireRate)
        {
            if (enemiesToHit != null)
            {
                Vector2 direction = (enemiesToHit.transform.position - transform.position).normalized;
                lastShotTime = Time.time;
                Shoot(direction);
                
            }
        }
        
    }

    public GameObject FindNearestEnemy(List<GameObject> enemiesList)
    {
        // TO DO // 
        //tenere traccia dei nemici e ritornare il più vicino al giocatore, potrei passare un array o una lista di nemici
        // devo però restituire solo il più vicino quindi forse meglio passare solo 1 valore ma leggendo dalla lista inizialmente creavo un secondo array con i nemici in range

        GameObject enemiesInRange = null;
        Vector3 playerPosition = transform.position;

        foreach (GameObject enemy in enemiesList)
        {
            if (enemy == null)
            {
                enemiesList.Remove(enemy);
                continue;
            }

            // distanza con il nemico con .distance magari

            float distanceToEnemies = Vector3.Distance(playerPosition, enemy.transform.position);

            if (distanceToEnemies <= _fireRange)
            {
                enemiesInRange = enemy;
            }
            
        }
        return enemiesInRange;

        
    }

    public void Shoot(Vector2 direction)
    {
        // se c'è un nemico vicino deve creare un bullet e dare al suo RB velocità e direzione
        Bullet b = Instantiate(_bulletPrefab, transform.position, transform.rotation);
        b.Dir = direction;
    }
}
