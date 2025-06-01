using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] private float _speed = 1.0f;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _damage = 5.0f;
    [SerializeField] private Enemy Enemy;
    
    public Vector2 Dir {  get; set; }
    
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 5);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _rb.velocity = Dir * _speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            Enemy enemyComponent = collision.collider.GetComponent<Enemy>();
            enemyComponent.TakeDamage(_damage);
            Destroy(this.gameObject);
        }
        else if (collision.collider.CompareTag("Bush"))
        {
            Destroy(this.gameObject);
        }
    }
}
