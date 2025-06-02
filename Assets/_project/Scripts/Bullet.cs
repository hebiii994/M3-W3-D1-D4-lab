using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] private float _speed = 1.0f;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _baseDamage = 5.0f;
    //[SerializeField] private float _damage = 5.0f;
    [SerializeField] private Enemy Enemy;

    [SerializeField] private AudioClip _arrowHit;
    private AudioSource _audioSource;
    public Vector2 Dir {  get; set; }

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.Log("Associare Audio hit freccia al prefab");
        }
    }
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
        if (_audioSource != null)
        {
            AudioSource.PlayClipAtPoint(_arrowHit, transform.position);
            Debug.Log("suono HIT");
        }



        if (collision.collider.CompareTag("Enemy"))
        {
            Enemy enemyComponent = collision.collider.GetComponent<Enemy>();
            if (enemyComponent != null)
            {
                float currentDamage = _baseDamage;
                if (PlayerShooterController.instance != null)
                {
                    currentDamage *= PlayerShooterController.instance.GetCurrentDamageMultiplier();
                }
                enemyComponent.TakeDamage(currentDamage);
            }
            Destroy(this.gameObject);
        }
        else if (collision.collider.CompareTag("Bush")) // O altri ostacoli
        {
            Destroy(this.gameObject);
        }
        
    }
}
